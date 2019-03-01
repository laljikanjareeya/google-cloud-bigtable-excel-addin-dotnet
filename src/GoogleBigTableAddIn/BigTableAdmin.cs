using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Cloud.Bigtable.Admin.V2;
using Google.Cloud.Bigtable.Common.V2;
using Google.Cloud.Bigtable.V2;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GoogleBigTableAddIn
{
    public static class BigTableAdmin
    {
        public static string SayHelloWorld()
        {
            return "Big Table Add-In Ribbon Tested OK!";
        }

        private static bool CheckProperty(string projectId, string instanceId, string tableName)
        {
            return !string.IsNullOrWhiteSpace(projectId) && !string.IsNullOrWhiteSpace(instanceId) && !string.IsNullOrWhiteSpace(tableName);
        }

        private static bool CheckProperty(string projectId, string instanceId, string tableName, string columnFamily)
        {
            return CheckProperty(projectId, instanceId, tableName) && !string.IsNullOrWhiteSpace(columnFamily);
        }

        public static bool CheckExistanceTable(string projectId, string instanceId, string tableName, string credentialPath)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            if (!CheckProperty(projectId, instanceId, tableName))
            {
                throw new Exception("One Or More Properties Not Set");
            }
            bool exists = false;
            try
            {
                BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();
                InstanceName instanceName = new InstanceName(projectId, instanceId);
                var tables = bigtableTableAdminClient.ListTables(instanceName).ToList();
                var abc = tables.FirstOrDefault(x => x.TableName.TableId == tableName);
                exists = tables.Any(x => x.TableName.TableId == tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exists;
        }

        public static bool CreateTable(string projectId, string instanceId, string tableName, string columnFamily, string credentialPath)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

            if (!CheckProperty(projectId, instanceId, tableName, columnFamily))
            {
                throw new Exception("One Or More Properties Not Set");
            }

            if (CheckExistanceTable(projectId, instanceId, tableName, credentialPath))
            {
                throw new Exception("Table Already Exists");
            }

            bool Created = false;
            try
            {
                var callSetting = CreateRetryCallSettings(3);
                var bigtableTableAdminClient = BigtableTableAdminClient.Create();
                bigtableTableAdminClient.CreateTable(
                     new InstanceName(projectId, instanceId),
                     tableName, new Table
                     {
                         Granularity = Table.Types.TimestampGranularity.Millis,
                         ColumnFamilies =
                         {
                                    {
                                        columnFamily, new ColumnFamily
                                        {
                                            GcRule = new GcRule
                                            {
                                                MaxNumVersions = 1
                                            }
                                        }
                                    }
                         }
                     }, callSetting);
                Created = true;
            }
            catch (RpcException ex)
            {
                if (ex.Status.StatusCode == StatusCode.AlreadyExists)
                {
                    Created = true;
                }
                else
                    throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Created;
        }

        static CallSettings CreateRetryCallSettings(int tryCount)
        {
            var backoff = new BackoffSettings(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(5000), 2);
            return CallSettings.FromCallTiming(CallTiming.FromRetry(new RetrySettings(backoff, backoff, Expiration.None,
                (RpcException e) => e.Status.StatusCode != StatusCode.AlreadyExists && --tryCount > 0,
                RetrySettings.NoJitter)));
        }

        public static void AddTestDataToTable(string projectId, string instanceId, string tableName, string columnFamily, string credentialPath)
        {
            try
            {
                if (!CheckExistanceTable(projectId, instanceId, tableName, credentialPath))
                {
                    throw new Exception("No Such Table Found");
                }
                var bigtableClient = BigtableClient.Create();
                List<Mutation> Cols = new List<Mutation>();
                TableName _table = new TableName(projectId, instanceId, tableName);

                var request = new MutateRowsRequest
                {
                    TableNameAsTableName = _table,
                };
                request.Entries.Add(Mutations.CreateEntry(Guid.NewGuid().ToString(),
                   Mutations.SetCell(columnFamily, "TestColumnName", "Test Column Value")));
                bigtableClient.MutateRows(request);
            }
            catch (RpcException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteTable(string projectId, string instanceId, string tableName, string credentialPath)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            if (!CheckProperty(projectId, instanceId, tableName))
            {
                throw new Exception("One Or More Properties Not Set");
            }
            bool Deleted = false;
            try
            {
                BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();
                bigtableTableAdminClient.DeleteTable(
                                new TableName(projectId, instanceId, tableName));
                Deleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Deleted;
        }

        public static Dictionary<string, List<string>> GetAllRecords(string projectId, string instanceId, string tableName, string credentialPath)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            var bigtableClient = BigtableClient.Create();

            var recordList = new Dictionary<string, List<string>>();
            var columns = new List<string>();

            try
            {
                TableName tableNameClient = new TableName(projectId, instanceId, tableName);
                ReadRowsStream responseRead;
                responseRead = bigtableClient.ReadRows(tableNameClient);
                var response = responseRead.ToList().Result;
                if (response.Any())
                    columns.Add("RowKey");
                else
                    recordList.Add("ErrorCode", new List<string> { "No Record Found." });
                var dataListResult = new Dictionary<string, List<KeyValueViewModel>>();
                response.ForEach(row =>
                {
                    var keyValueList = new List<KeyValueViewModel>();
                    keyValueList.Add(new KeyValueViewModel { Key = "RowKey", Value = row.Key.ToStringUtf8() });
                    for (int familyIndex = 0; familyIndex < row.Families.Count; familyIndex++)
                    {
                        for (int columnIndex = 0; columnIndex < row.Families[familyIndex].Columns.Count; columnIndex++)
                        {
                            var columName = row.Families[familyIndex].Columns[columnIndex].Qualifier.ToStringUtf8().ToString();
                            if (!columns.Contains(columName))
                                columns.Add(columName);
                            var value = row.Families[familyIndex].Columns[columnIndex].Cells[0].Value.ToStringUtf8();
                            keyValueList.Add(new KeyValueViewModel { Key = columName, Value = value });
                        }
                    }
                    dataListResult.Add(row.Key.ToStringUtf8(), keyValueList);
                });
                var index = 0;
                recordList.Add("BigTableColumnList", columns);

                dataListResult.ToList().ForEach(data =>
                {
                    var array = new List<string>();
                    index = 0;
                    columns.ForEach(column =>
                    {
                        array.Add(data.Value.FirstOrDefault(c => c.Key == column)?.Value.ToString() ?? "");
                        index = index + 1;
                    });
                    recordList.Add(data.Key, array);
                });
                return recordList;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Table not found"))
                    recordList.Add("ErrorCode", new List<string> { "No Such Table Found." });
                else if (ex.InnerException.Message.Contains("NotFound"))
                    recordList.Add("ErrorCode", new List<string> { "Incorrect input details make sure you have enter correct information." });
                else
                    recordList.Add("ErrorCode", new List<string> { ex.InnerException == null ? ex.Message : ex.InnerException.Message });
                return recordList;
            }
        }
    }
}
