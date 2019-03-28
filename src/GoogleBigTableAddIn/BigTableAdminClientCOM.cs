using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Cloud.Bigtable.Admin.V2;
using Google.Cloud.Bigtable.Common.V2;
using Google.Cloud.Bigtable.V2;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace GoogleBigTableAddIn
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class BigTableAdminClientCOM
    {
        private string ProjectId { get; set; }
        private string InstanceId { get; set; }
        private string TableName { get; set; }
        private string ColumnFamily { get; set; }
        private string CredentialPath { get; set; }

        /// <summary>
        /// Set Variables for All Operations
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="instanceId"></param>
        /// <param name="tableName"></param>
        /// <param name="columnFamily"></param>
        /// <param name="credentialPath"></param>
        /// <returns></returns>
        public bool SetVariables(string projectId, string instanceId, string tableName, string columnFamily, string credentialPath)
        {
            ProjectId = projectId;
            InstanceId = instanceId;
            TableName = tableName;
            CredentialPath = credentialPath;
            ColumnFamily = columnFamily;
            SetEnvironmentVariable();
            return true;
        }

        /// <summary>
        /// Check The Given Table is exist or not in Google Cloud Bigtable
        /// </summary>
        /// <returns></returns>
        public bool IsTableExists()
        {
            bool exists = false;
            try
            {
                BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();
                InstanceName instanceName = new InstanceName(ProjectId, InstanceId);
                var tables = bigtableTableAdminClient.ListTables(instanceName);
                exists = tables.Any(x => x.TableName.TableId == TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exists;
        }

        /// <summary>
        /// Create Table In Google Cloud Bigtable
        /// </summary>
        /// <returns></returns>
        public bool CreateTable()
        {
            bool created = false;
            try
            {
                var callSetting = CreateRetryCallSettings();
                var bigtableTableAdminClient = BigtableTableAdminClient.Create(settings: callSetting);
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
                     });
                created = true;
            }
            catch (RpcException ex)
            {
                if (ex.Status.StatusCode == StatusCode.AlreadyExists)
                {
                    created = true;
                }
                else
                    throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return created;
        }

        /// <summary>
        /// Insert Test Data into Google Cloud Bigtable
        /// </summary>
        /// <returns></returns>
        public bool InsertTestData(string columnName, string columnValue)
        {
            try
            {
                var bigtableClient = BigtableClient.Create();
                List<Mutation> Cols = new List<Mutation>();
                TableName _table = new TableName(ProjectId, InstanceId, TableName);

                var request = new MutateRowsRequest
                {
                    TableNameAsTableName = _table,
                };
                request.Entries.Add(Mutations.CreateEntry(Guid.NewGuid().ToString(),
                   Mutations.SetCell(ColumnFamily, columnName, columnValue)));
                bigtableClient.MutateRows(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Delete Table from Google Cloud Bigtable
        /// </summary>
        /// <returns></returns>
        public bool DeleteTable()
        {
            try
            {
                BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();
                bigtableTableAdminClient.DeleteTable(
                                new TableName(ProjectId, InstanceId, TableName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        private void SetEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);
        }

        private BigtableTableAdminSettings CreateRetryCallSettings()
        {
            var clientSettings = BigtableTableAdminSettings.GetDefault();
            var longTimeout = CallTiming.FromTimeout(TimeSpan.FromMinutes(2));
            clientSettings.CreateTableSettings = clientSettings.CreateTableSettings.WithCallTiming(longTimeout);
            return clientSettings;
        }
    }
}
