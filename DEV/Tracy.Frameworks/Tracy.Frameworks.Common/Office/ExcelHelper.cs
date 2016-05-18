using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

namespace Tracy.Frameworks.Common.Office
{
    ///// <summary>
    ///// 描述:Excel导入导出操作
    ///// 作者:鲁宁
    ///// 时间:2013/10/17 9:44:53
    ///// </summary>
    //public class ExcelHelper
    //{
    //    #region 常量

    //    private const string InvalidColumnFlag = "columns_+="; //无效头的字符串

    //    private const int MaxSheetRows2007 = 1048576; //行的最大值

    //    private const string Office2003Excel = "application/vnd.ms-excel"; //office2003 excel 格式

    //    private const string Office2007Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; //office2007 excel格式

    //    private const string DefaultSheetName = "sheet"; //默认的sheet名称

    //    private const string DefaultTable = "TableName"; //默认的TableName

    //    private const string SPLIT_ObliqueLine = "/"; //分隔符斜线 /

    //    private const string SPLIT_VerticalLine = "|"; //分隔符竖线 |

    //    #endregion

    //    /// <summary>
    //    /// 导出Excel
    //    /// </summary>
    //    /// <param name="context">当前请求上下文</param>
    //    /// <param name="dtSource">数据源</param>
    //    /// <param name="fileName">生成的文件名</param>
    //    /// <returns>true成功,false失败</returns>
    //    public static bool Export(HttpContext context, DataTable dtSource, string fileName)
    //    {
    //        try
    //        {
    //            fileName = String.IsNullOrEmpty(fileName) ? DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx" : fileName;

    //            string sheetName = String.IsNullOrEmpty(dtSource.TableName) ? DefaultTable + "1" : dtSource.TableName;

    //            using (ExcelPackage pck = new ExcelPackage())
    //            {
    //                WriteSheets(dtSource, pck, sheetName);
    //                context.Response.Clear();
    //                context.Response.ContentType = Office2007Excel;
    //                context.Response.ContentEncoding = Encoding.UTF8;
    //                context.Response.Charset = "";
    //                context.Response.AppendHeader(
    //                    "Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
    //                context.Response.BinaryWrite(pck.GetAsByteArray());
    //                context.Response.Flush();
    //                //context.Response.End(); //避免ThreadAbortException异常
    //                context.ApplicationInstance.CompleteRequest();
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            //LogHelper.Error("export excel occur error", ex);
    //        }
    //        return false;
    //    }

    //    public static bool ExportWithColumnName(HttpContext context, DataTable dtSource, string fileName)
    //    {
    //        IDictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
    //        fileName = String.IsNullOrEmpty(fileName) ? DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx" : fileName;
    //        string sheetName = String.IsNullOrEmpty(dtSource.TableName) ? DefaultTable + "1" : dtSource.TableName;
    //        dic.Add(sheetName, dtSource);

    //        return ExportToExcelByWeb(context,dic,fileName);
    //    }

    //    /// <summary>
    //    /// 导入Excel
    //    /// </summary>
    //    /// <param name="filePath">文件在服务端路径</param>
    //    /// <returns>返回DataTable</returns>
    //    public static DataTable Import(string filePath)
    //    {
    //        DataTable dt = new DataTable();

    //        try
    //        {
    //            using (ExcelPackage pck = new ExcelPackage(new System.IO.FileInfo(filePath)))
    //            {
    //                var sheet = pck.Workbook.Worksheets.First();
    //                if (sheet == null) return null;

    //                foreach (var cell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
    //                {
    //                    dt.Columns.Add(cell.Value.ToString());
    //                }
    //                var rows = sheet.Dimension.End.Row;
    //                for (var i = 2; i <= rows; i++)
    //                {
    //                    var row = sheet.Cells[i, 1, i, sheet.Dimension.End.Column];
    //                    dt.Rows.Add(row.Select(cell => cell.Value).ToArray());
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            dt = null;
    //            //LogHelper.Error("import excel occur error", ex);
    //        }
    //        return dt;
    //    }

    //    #region Private method

    //    private static bool ExportToExcelByWeb(HttpContext context, IDictionary<string, DataTable> dic, string fileName)
    //    {
    //        int col_filter = 0;
    //        string[] strcolmerg;
    //        string[] strcolumns; // columns split
    //        bool checkmutule = false;
    //        string strsheet = DefaultSheetName;
    //        int sheeti = 1;

    //        try
    //        {
    //            using (ExcelPackage pck = new ExcelPackage())
    //            {
    //                for (int j = pck.Workbook.Worksheets.Count; j >= 1; j--)
    //                {
    //                    pck.Workbook.Worksheets.Delete(j);
    //                }

    //                foreach (var kp in dic)
    //                {
    //                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(kp.Key);
    //                    ws.Cells.Style.ShrinkToFit = true;

    //                    DataTable dtexport = (DataTable)kp.Value;
    //                    col_filter = 0;  //复位

    //                    if (fileName == "")
    //                        fileName = dtexport.TableName;


    //                    if (fileName == "")
    //                        fileName = strsheet + sheeti.ToString();

    //                    sheeti++;

    //                    #region check mutil columns
    //                    foreach (DataColumn dc in dtexport.Columns)
    //                    {
    //                        if (dc.Caption.IndexOf(SPLIT_ObliqueLine) >= 0)
    //                        {
    //                            checkmutule = true;
    //                            break;
    //                        }
    //                    }
    //                    #endregion
    //                    if (dtexport.Rows.Count > 0)
    //                    {
    //                        #region  row  > 0
    //                        for (int j = 0; j <= dtexport.Rows.Count - 1; j++)
    //                        {
    //                            for (int i = 0; i <= dtexport.Columns.Count - 1; i++)
    //                            {
    //                                try
    //                                {
    //                                    if (!checkmutule)
    //                                    {
    //                                        #region  single columlns
    //                                        strcolumns = dtexport.Columns[i].Caption.ToString().Split(SPLIT_VerticalLine.ToCharArray());

    //                                        if (j == 0)
    //                                        {
    //                                            if (strcolumns.Length >= 2)
    //                                            {
    //                                                if (Convert.ToInt16(strcolumns[1].ToString().Trim()) >= 0)  //valid columns caption
    //                                                    ws.Cells[1, 1 + Convert.ToInt16(strcolumns[1])].Value = strcolumns[0];  //write the columns caption
    //                                            }
    //                                        }

    //                                        if (strcolumns.Length >= 2)
    //                                        {
    //                                            if (Convert.ToInt16(strcolumns[1].ToString().Trim()) >= 0)  //valid columns
    //                                                ws.Cells[2 + j, 1 + Convert.ToInt16(strcolumns[1])].Value = dtexport.Rows[j][i].ToString();  //write the columns caption
    //                                        }
    //                                        #endregion
    //                                    }
    //                                    else
    //                                    {
    //                                        #region  multi columns
    //                                        strcolumns = dtexport.Columns[i].Caption.ToString().Split(SPLIT_VerticalLine.ToCharArray());

    //                                        if (strcolumns[0].IndexOf(InvalidColumnFlag) >= 0)
    //                                        {

    //                                            if (j == 0)
    //                                                col_filter++;
    //                                            continue;
    //                                        }

    //                                        if (j == 0)
    //                                        {
    //                                            #region  wirte the column head
    //                                            if (strcolumns.Length >= 2)
    //                                            {
    //                                                strcolmerg = strcolumns[0].Split(SPLIT_ObliqueLine.ToCharArray());

    //                                                if (strcolmerg.Length > 1)
    //                                                {
    //                                                    if (Convert.ToInt16(strcolumns[1]) == 0)
    //                                                    {
    //                                                        ws.Cells[1, i + 1 - col_filter, 1, i + 2 - col_filter].Merge = true;   // merge
    //                                                        ws.Cells[1, i + 1 - col_filter, 1, i + 2 - col_filter].Value = strcolmerg[0]; // write cell
    //                                                    }
    //                                                    ws.Cells[2, i + 1 - col_filter].Value = strcolmerg[1]; // write cell
    //                                                }
    //                                                else
    //                                                {
    //                                                    ws.Cells[1, i + 1 - col_filter, 2, i + 1 - col_filter].Merge = true;  // merge
    //                                                    ws.Cells[1, i + 1 - col_filter, 2, i + 1 - col_filter].Value = strcolmerg[0]; // write cell
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                ws.Cells[1, i + 1 - col_filter, 2, i + 1 - col_filter].Merge = true;// merge
    //                                                ws.Cells[1, i + 1 - col_filter, 2, i + 2 - col_filter].Value = strcolumns[0]; // write cell
    //                                            }
    //                                            #endregion
    //                                        }
    //                                        ws.Cells[3 + j, 1 + i - col_filter].Value = dtexport.Rows[j][i].ToString();  // wirte the record
    //                                        #endregion
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    //LogHelper.Error("export excel occur error", ex);
    //                                }
    //                            }
    //                        }
    //                        #endregion
    //                    }
    //                    else
    //                    {
    //                        #region   row = 0
    //                        for (int i = 0; i <= dtexport.Columns.Count - 1; i++)
    //                        {
    //                            try
    //                            {
    //                                strcolumns = dtexport.Columns[i].Caption.ToString().Split(SPLIT_VerticalLine.ToCharArray());

    //                                if (strcolumns[0].IndexOf(InvalidColumnFlag) >= 0)
    //                                {
    //                                    if (i == 0)
    //                                        col_filter++;
    //                                    continue;
    //                                }
    //                                if (!checkmutule)
    //                                {
    //                                    if (strcolumns.Length >= 2)
    //                                    {
    //                                        if (Convert.ToInt16(strcolumns[1].ToString().Trim()) >= 0)  //valid the columns
    //                                            ws.Cells[1, 1 + Convert.ToInt16(strcolumns[1])].Value = strcolumns[0];
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    if (strcolumns.Length >= 2)
    //                                    {
    //                                        strcolmerg = strcolumns[0].Split(SPLIT_ObliqueLine.ToCharArray());

    //                                        if (strcolmerg.Length > 1)
    //                                        {
    //                                            if (Convert.ToInt16(strcolumns[1]) == 0)
    //                                            {
    //                                                ws.Cells[1, i + 1 - col_filter, 1, i + 2 - col_filter].Merge = true;
    //                                                ws.Cells[1, i + 1 - col_filter, 1, i + 2 - col_filter].Value = strcolmerg[0]; // write the columns
    //                                            }
    //                                            ws.Cells[2, i + 1 - col_filter].Value = strcolmerg[1]; // write the columns
    //                                        }
    //                                        else
    //                                        {

    //                                            ws.Cells[1, i + 1 - col_filter, 2, i + 1 - col_filter].Merge = true;
    //                                            ws.Cells[1, i + 1 - col_filter, 2, i + 1 - col_filter].Value = strcolmerg[0]; //  write the columns
    //                                        }

    //                                    }
    //                                    else
    //                                    {
    //                                        ws.Cells[1, i + 1 - col_filter, 2, i + 1 - col_filter].Merge = true;
    //                                        ws.Cells[1, i + 1 - col_filter, 2, i + 2 - col_filter].Value = strcolumns[0]; // write the columns
    //                                    }
    //                                }
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                //LogHelper.Error("export excel occur error", ex);
    //                            }
    //                        }
    //                        #endregion
    //                    }
    //                }

    //                string strfilename_excel = HttpUtility.UrlEncode(fileName);
    //                //if (StringHelper.GetExtensionName(strfilename_excel, '.').ToLower() != "xlsx")
    //                //{
    //                //    strfilename_excel = HttpUtility.UrlEncode(fileName) + ".xlsx";
    //                //}

    //                //Write it back to the client
    //                var data = pck.GetAsByteArray();
    //                context.Response.Clear();
    //                context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
    //                context.Response.ContentType = Office2007Excel;
    //                context.Response.AddHeader("content-disposition", "attachment;  filename=" + strfilename_excel);
    //                context.Response.AddHeader("Content-Length", data.Length.ToString());
    //                context.Response.BinaryWrite(data);
    //                context.Response.Flush();
    //                context.ApplicationInstance.CompleteRequest();
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            //LogHelper.Error("export excel occur error",ex);
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// 创建每个Sheet
    //    /// </summary>
    //    /// <param name="dtSource"></param>
    //    /// <param name="pck"></param>
    //    /// <param name="sheetName"></param>
    //    private static void WriteSheets(DataTable dtSource, ExcelPackage pck, string sheetName)
    //    {
    //        const int max = MaxSheetRows2007 - 1;
    //        var rows = dtSource.Rows.Count;
    //        var sheetCount = (rows % max == 0) ? rows / max : rows / max + 1; //如果超过最大行数则新建一个sheet
    //        for (int sheetNo = 0; sheetNo < sheetCount; sheetNo++)
    //        {
    //            WriteSheet(
    //                dtSource,
    //                pck,
    //                (sheetNo == 0) ? sheetName : sheetName + "_" + sheetNo,
    //                sheetNo * max,
    //                (sheetNo + 1) * max < rows ? (sheetNo + 1) * max - 1 : rows - 1
    //                );
    //        }
    //    }

    //    /// <summary>
    //    /// 创建具体的sheet
    //    /// </summary>
    //    /// <param name="table"></param>
    //    /// <param name="excel"></param>
    //    /// <param name="sheetName"></param>
    //    /// <param name="startRowIndex"></param>
    //    /// <param name="endRowIndex"></param>
    //    private static void WriteSheet(DataTable dtSource, ExcelPackage pck, string sheetName, int startRowIndex, int endRowIndex)
    //    {
    //        var sheet = CreateSheet(pck, sheetName);
    //        var i = 1;
    //        foreach (DataColumn col in dtSource.Columns)
    //        {
    //            sheet.Cells[1, i].Value = col.ColumnName;
    //            i++;
    //        }
    //        var columnCount = dtSource.Columns.Count;
    //        i = 2;

    //        sheet.Cells[1, 1, dtSource.Rows.Count + 1, dtSource.Columns.Count].Style.Border.Left.Style = ExcelBorderStyle.Thin;
    //        sheet.Cells[1, 1, dtSource.Rows.Count + 1, dtSource.Columns.Count].Style.Border.Right.Style = ExcelBorderStyle.Thin;
    //        sheet.Cells[1, 1, dtSource.Rows.Count + 1, dtSource.Columns.Count].Style.Border.Top.Style = ExcelBorderStyle.Thin;
    //        sheet.Cells[1, 1, dtSource.Rows.Count + 1, dtSource.Columns.Count].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
    //        sheet.Cells.Style.Font.Name = "正楷";
    //        sheet.Cells.Style.ShrinkToFit = true;

    //        for (var m = startRowIndex; m <= endRowIndex; m++)
    //        {
    //            var row = dtSource.Rows[m];
    //            for (var j = 1; j <= columnCount; j++)
    //            {
    //                sheet.Cells[i, j].Value = row[j - 1].ToString();
    //            }
    //            i++;
    //        }
    //    }

    //    private static ExcelWorksheet CreateSheet(ExcelPackage pck, string sheetName)
    //    {
    //        foreach (ExcelWorksheet sheet in pck.Workbook.Worksheets)
    //        {
    //            if (String.Compare(sheet.Name, sheetName, StringComparison.OrdinalIgnoreCase) == 0)
    //            {
    //                return sheet;
    //            }
    //        }
    //        return pck.Workbook.Worksheets.Add(sheetName);
    //    }

    //    #endregion
    //}
}
