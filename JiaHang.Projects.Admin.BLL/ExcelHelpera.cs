using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace JiaHang.Projects.Admin.BLL
{
    public class ExcelHelpera
    {
        /// <summary>
        /// 将Excel单表转为Datatable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileType"></param>
        /// <param name="strMsg"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDatatable(Stream stream, string fileType, out string strMsg, string sheetName = null)
        {
            strMsg = "";
            DataTable dt = new DataTable();
            ISheet sheet = null;
            IWorkbook workbook = null;
            try
            {
                #region 判断excel版本
                //2007以上版本excel
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                //2007以下版本excel
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    throw new Exception("传入的不是Excel文件！");
                }
                #endregion
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(3);
                    int cellCount = firstRow.LastCellNum;
                    for (int i = firstRow.FirstCellNum+1; i < cellCount; i++)
                    {
                        ICell cell = firstRow.GetCell(i);
                        
                        if (cell != null)
                        {
                            //列名
                            string cellValue = cell.StringCellValue.Trim();
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                DataColumn dataColumn = new DataColumn(cellValue);
                                dt.Columns.Add(dataColumn);
                            }
                        }
                    }
                    for (int i = 0; i < 11; i++)
                    {
                        DataColumn dataColumn = new DataColumn(null);
                        dt.Columns.Add(dataColumn);
                    }
                    DataRow dataRow = null;
                    //遍历行
                    for (int j = sheet.FirstRowNum + 6; j <= sheet.LastRowNum; j++)
                    {
                        IRow row = sheet.GetRow(j);
                        
                        dataRow = dt.NewRow();
                        if (row == null || row.FirstCellNum < 0)
                        {
                            continue;
                        }
                        //遍历列
                        for (int i = row.FirstCellNum; i < cellCount-1; i++)
                        {
                            ICell cellData = row.GetCell(i+1);

                            if (cellData != null)
                            {
                                //判断是否为数字型，必须加这个判断不然下面的日期判断会异常
                                if (cellData.CellType == CellType.Numeric)
                                {
                                    //判断是否日期类型
                                    if (DateUtil.IsCellDateFormatted(cellData))
                                    {
                                        dataRow[i] = cellData.DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[i] = cellData.ToString().Trim();
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        dataRow[i] = cellData.ToString().Trim();
                                    }
                                    catch (Exception ex)
                                    {

                                        workbook = new HSSFWorkbook(stream);
                                        strMsg = ex.Message;
                                    }
                                    
                                }
                            }
                            else
                            {
                                dataRow[i] = string.Empty;
                            }
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
                else
                {
                    throw new Exception("没有获取到Excel中的数据表！");
                }
            }
            catch (Exception ex)
            {
                workbook = new HSSFWorkbook(stream);
                strMsg = ex.Message;
            }
            return dt;
        }
    }
}
