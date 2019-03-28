using System;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System.Linq;
using System.IO;

namespace GoogleBigTableAddIn
{
    public partial class CBTRibbon
    {
        private void CBTRibbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void btnSayHelloWorld_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.ThisAddIn.Application.ActiveSheet;
            worksheet.UsedRange.ClearContents();
            worksheet.Range["A1"].Value = BigTableAdminClientUtilityInstance().TestBigtableAddIn();
            worksheet.Columns.AutoFit();
        }

        private void btnAdd_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.ThisAddIn.Application.ActiveSheet;
            worksheet.UsedRange.ClearContents();
            if (!CheckCredentialPath(eb_crePath.Text, worksheet))
            {
                return;
            }
            bool Created = false;
            try
            {
                Created = BigTableAdminClientUtilityInstance().CreateTable();
                if (Created)
                    worksheet.Cells[1, 1] = "Table Created Successfully, Please Click on Insert Test Data to insert Data into Table.";
                else
                    worksheet.Cells[1, 1] = "Table Not Created";
                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("AlreadyExists"))
                {
                    worksheet.Cells[1, 1] = "Table Already Exists";
                }
                else
                    worksheet.Cells[1, 1] = ex.Message;
            }
        }

        private void btnDelete_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.ThisAddIn.Application.ActiveSheet;
            worksheet.UsedRange.ClearContents();
            if (!CheckCredentialPath(eb_crePath.Text, worksheet))
            {
                return;
            }
            bool Deleted = false;
            try
            {
                Deleted = BigTableAdminClientUtilityInstance().DeleteTable();
                if (Deleted)
                    worksheet.Cells[1, 1] = "Table Deleted Successfully";
                else
                    worksheet.Cells[1, 1] = "Table Not Deleted";
                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("NotFound"))
                    worksheet.Cells[1, 1] = "No Such Table Found";
                else
                    worksheet.Cells[1, 1] = ex.Message;
            }
        }

        private void btnCheck_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.ThisAddIn.Application.ActiveSheet;
            worksheet.UsedRange.ClearContents();
            if (!CheckCredentialPath(eb_crePath.Text, worksheet))
            {
                return;
            }
            bool Exists = false;
            try
            {
                Exists = BigTableAdminClientUtilityInstance().IsTableExists();
                if (Exists)
                    worksheet.Cells[1, 1] = "Table Exists";
                else
                    worksheet.Cells[1, 1] = "Table Does Not Exist";
                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                worksheet.Cells[1, 1] = ex.Message;
            }
        }

        private void btnDispalyData_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.ThisAddIn.Application.ActiveSheet;
            worksheet.UsedRange.ClearContents();

            if (!CheckCredentialPath(eb_crePath.Text, worksheet))
            {
                return;
            }
            try
            {
                var row = 1;
                var column = 1;
                var data = BigTableAdminClientUtilityInstance().GetAllRecords();
                if (data.ContainsKey("ErrorCode"))
                {
                    worksheet.Cells[1, 1] = data["ErrorCode"].FirstOrDefault();
                    return;
                }

                var columList = data["BigTableColumnList"];
                foreach (var acct in data)
                {
                    acct.Value.ToList().ForEach(cellData =>
                    {
                        worksheet.Cells[row, column] = cellData;
                        column = column + 1;
                    });
                    row++;
                    column = 1;
                }
                try
                {
                    Range range = worksheet.Range[$"A1:{GetExcelColumnName(columList.Count)}{data.Count}"];
                    worksheet.ListObjects.AddEx(XlListObjectSourceType.xlSrcRange, range, XlListObjectHasHeaders: XlYesNoGuess.xlYes).Name = "MyTableStyle";
                    worksheet.ListObjects.get_Item("MyTableStyle").TableStyle = "TableStyleLight9";
                    range.Columns.AutoFit();
                    range.Rows.AutoFit();
                }
                catch (Exception)
                {
                    //don't need to handle exception while setting style to sheet
                }
            }
            catch (Exception ex)
            {
                worksheet.Cells[1, 1] = ex.Message;
            }

        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private static bool CheckCredentialPath(string path, Worksheet worksheet)
        {
            var isFileExist = File.Exists(path);
            if (!isFileExist)
                worksheet.Cells[1, 1] = $"File does not exist  {path}";
            return isFileExist;
        }

        private void btnInsertTestData_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.ThisAddIn.Application.ActiveSheet;
            worksheet.UsedRange.ClearContents();
            try
            {
                BigTableAdminClientUtilityInstance().InsertTestDataToTable();
                worksheet.Cells[1, 1] = "Test Data Inserted Successfully, Please Click on Display Table Data to Show Table Data.";

            }
            catch (Exception ex)
            {
                worksheet.Cells[1, 1] = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
        }

        private BigTableAdminClientUtility BigTableAdminClientUtilityInstance()
        {
            return new BigTableAdminClientUtility(eb_ProjectId.Text, eb_instanceId.Text, eb_tableName.Text, eb_columnFamily.Text, eb_crePath.Text);
        }
    }
}
