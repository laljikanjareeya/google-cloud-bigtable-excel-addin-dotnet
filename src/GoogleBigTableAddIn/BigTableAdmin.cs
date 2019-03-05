using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Cloud.Bigtable.Admin.V2;
using Google.Cloud.Bigtable.Common.V2;
using Google.Cloud.Bigtable.V2;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleBigTableAddIn
{
    public class BigTableAdmin
    {
        private string ProjectId { get; set; }
        private string InstanceId { get; set; }
        private string TableName { get; set; }
        private string ColumnFamily { get; set; }
        private string CredentialPath { get; set; }

        public BigTableAdmin(string projectId, string instanceId, string tableName, string columnFamily, string credentialPath)
        {
            ProjectId = projectId;
            InstanceId = instanceId;
            TableName = tableName;
            ColumnFamily = columnFamily;
            CredentialPath = credentialPath;

        }
        public string SayHelloWorld()
        {
            return "Big Table Add-In Ribbon Tested OK!";
        }

        private bool CheckProperty(bool isCreationTable = false)
        {
            return isCreationTable ? !string.IsNullOrWhiteSpace(ProjectId) && !string.IsNullOrWhiteSpace(InstanceId) &&
                !string.IsNullOrWhiteSpace(TableName) && !string.IsNullOrWhiteSpace(ColumnFamily) :
                !string.IsNullOrWhiteSpace(ProjectId) && !string.IsNullOrWhiteSpace(InstanceId) &&
                !string.IsNullOrWhiteSpace(TableName);
        }

        public bool CheckExistanceTable()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);
            if (!CheckProperty())
            {
                throw new Exception("One Or More Properties Not Set");
            }
            bool exists = false;
            try
            {
                BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();
                InstanceName instanceName = new InstanceName(ProjectId, InstanceId);
                var tables = bigtableTableAdminClient.ListTables(instanceName).ToList();
                var abc = tables.FirstOrDefault(x => x.TableName.TableId == TableName);
                exists = tables.Any(x => x.TableName.TableId == TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exists;
        }

        public bool CreateTable()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);

            if (!CheckProperty(true))
            {
                throw new Exception("One Or More Properties Not Set");
            }

            if (CheckExistanceTable())
            {
                throw new Exception("Table Already Exists");
            }

            bool Created = false;
            try
            {
                var callSetting = CreateRetryCallSettings(3);
                var bigtableTableAdminClient = BigtableTableAdminClient.Create();
                bigtableTableAdminClient.CreateTable(
                     new InstanceName(ProjectId, InstanceId),
                     TableName, new Table
                     {
                         Granularity = Table.Types.TimestampGranularity.Millis,
                         ColumnFamilies =
                         {
                                    {
                                        ColumnFamily, new ColumnFamily
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

        private CallSettings CreateRetryCallSettings(int tryCount)
        {
            var backoff = new BackoffSettings(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(5000), 2);
            return CallSettings.FromCallTiming(CallTiming.FromRetry(new RetrySettings(backoff, backoff, Expiration.None,
                (RpcException e) => e.Status.StatusCode != StatusCode.AlreadyExists && --tryCount > 0,
                RetrySettings.NoJitter)));
        }

        public void AddTestDataToTable()
        {
            try
            {
                if (!CheckExistanceTable())
                {
                    throw new Exception("No Such Table Found");
                }
                var bigtableClient = BigtableClient.Create();
                List<Mutation> Cols = new List<Mutation>();
                TableName _table = new TableName(ProjectId, InstanceId, TableName);

                var request = new MutateRowsRequest
                {
                    TableNameAsTableName = _table,
                };
                request.Entries.Add(Mutations.CreateEntry(Guid.NewGuid().ToString(),
                   Mutations.SetCell(ColumnFamily, "TestColumnName", "Test Column Value")));
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

        public bool DeleteTable()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);
            if (!CheckProperty())
            {
                throw new Exception("One Or More Properties Not Set");
            }
            bool Deleted = false;
            try
            {
                BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();
                bigtableTableAdminClient.DeleteTable(
                                new TableName(ProjectId, InstanceId, TableName));
                Deleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Deleted;
        }

        public Dictionary<string, List<string>> GetAllRecords()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);
            var bigtableClient = BigtableClient.Create();

            var recordList = new Dictionary<string, List<string>>();
            var columns = new List<string>();

            try
            {
                TableName tableNameClient = new TableName(ProjectId, InstanceId, TableName);
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
