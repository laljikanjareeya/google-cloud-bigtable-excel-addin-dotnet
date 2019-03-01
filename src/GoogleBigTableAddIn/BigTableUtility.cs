using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Google.Cloud.Bigtable.Common.V2;
using Google.Cloud.Bigtable.V2;
using Scripting;


namespace GoogleBigTableAddIn
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class BigTableUtilityCOM : IBigTableUtilityCOM
    {
        public Dictionary GetAllRecords(string projectId, string instanceId, string tableName, string credentialPath, bool isFilterApplied, string rowCount)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            var bigtableClient = BigtableClient.Create();

            Dictionary recordList = new Dictionary();
            var columns = new List<string>();

            try
            {
                columns.Add("RowKey");
                TableName tableNameClient = new TableName(projectId, instanceId, tableName);
                ReadRowsStream responseRead;
                if (isFilterApplied)
                    responseRead = bigtableClient.ReadRows(tableNameClient, rowsLimit: Convert.ToInt64(rowCount));
                else
                    responseRead = bigtableClient.ReadRows(tableNameClient);

                var response = responseRead.ToList()?.Result;
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
                var columnArray = new string[columns.Count];
                var index = 0;
                columns.ForEach(column =>
                {
                    columnArray.SetValue(column, index);
                    index = index + 1;
                });
                recordList.Add("BigTableColumnList", columnArray);
                recordList.Add("BigTableColumnCount", columns.Count);
                dataListResult.ToList().ForEach(data =>
                {
                    var array = new string[columns.Count];
                    index = 0;
                    columns.ForEach(column =>
                    {
                        array.SetValue(data.Value.FirstOrDefault(c => c.Key == column)?.Value.ToString() ?? "", index);
                        index = index + 1;
                    });
                    recordList.Add(data.Key, array);
                });
                return recordList;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Table not found"))
                    recordList.Add("ErrorCode", new string[1] { "No Such Table Found" });
                else if (ex.InnerException.Message.Contains("NotFound"))
                    recordList.Add("ErrorCode", new string[1] { "Incorrect input details make sure you have enter correct information." });
                else
                    recordList.Add("ErrorCode", new string[1] { ex.Message });
                return recordList;
            }
        }
    }
}
