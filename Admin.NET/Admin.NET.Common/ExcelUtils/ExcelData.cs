// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

//using Magicodes.ExporterAndImporter.Core;
//using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Common.ExcelCommon
{
    /// <summary>
    /// 获取Excel中的数据
    /// </summary>
    public static class ExcelData
    {

        private static string FileDir = "/File/ExcelTemp";

        //ExcelAppService.cs
        /// <summary>
        /// 解析excel数据
        /// </summary>
        /// <typeparam name="T">要解析的数据类型</typeparam>
        /// <param name="fileName">excel文件名称，不含路径</param>
        /// <returns></returns>
        internal async static Task<IEnumerable<T>> GetData<T>(string fileName) where T : class, new()
        {
            IImporter importer = new ExcelImporter();
            var fullpath = GetFullPath(fileName);
            var resultss = importer.GenerateTemplate<T>(fullpath);
            var result = await importer.Import<T>(fullpath);
            if (result.HasError)
            {
                var errFile = Path.GetFileNameWithoutExtension(fileName) + "_" + Path.GetExtension(fileName);
                //如果excel文件内容不符合要求（格式错误、必填数据未填、数据类型错误），则弹出错误提示并给出下载链接
                throw new Exception("导入错误");
            }
            return result.Data;
        }



        /// <summary>
        /// 获取文件全路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFullPath(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            var fullpath = Path.GetFullPath(Environment.CurrentDirectory + FileDir + "/" + fileName);
            return fullpath;
        }



        private static IWorkbook workbook = null;
        private static FileStream fs = null;
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="fileName">excel文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            var fullpath = GetFullPath(fileName);
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
                if (fullpath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fullpath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
                else if (fullpath.IndexOf(".csv") > 0)
                {

                    System.Text.Encoding encoding = GetType(fullpath); //Encoding.ASCII;//
                    DataTable dt = new DataTable();

                    System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

                    //记录每次读取的一行记录
                    string strLine = "";
                    //记录每行记录中的各字段内容
                    string[] aryLine = null;
                    string[] tableHead = null;
                    //标示列数
                    int columnCount = 0;
                    //标示是否是读取的第一行
                    bool IsFirst = true;
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (IsFirst == true)
                        {
                            strLine = strLine.Substring(0, strLine.Length - 1);
                            tableHead = strLine.Split(',');
                            IsFirst = false;
                            columnCount = tableHead.Length;
                            //创建列
                            for (int i = 0; i < columnCount; i++)
                            {
                                DataColumn dc = new DataColumn(tableHead[i]);
                                dt.Columns.Add(dc);
                            }
                        }
                        else
                        {
                            aryLine = strLine.Split(',');
                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < columnCount; j++)
                            {
                                dr[j] = aryLine[j];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (aryLine != null && aryLine.Length > 0)
                    {
                        dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                    }

                    sr.Close();
                    fs.Close();
                    return dt;
                }

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
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
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                        {
                            DataColumn column = new DataColumn(i.ToString());
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (j < 0)
                            {
                                continue;
                            }
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }




        public static async Task<ImportResult<T>> ExcelToEntityCollection<T>(string fileName, string sheetName, bool isFirstRowColumn) where T : class, new()
        {
            var fullpath = GetFullPath(fileName);

            try
            {
                IImporter Importer = new ExcelImporter();
                return await Importer.Import<T>(fullpath);

                //return import;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// <param name="FILE_NAME">文件路径</param>
        /// <returns>文件的编码类型</returns>

        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }/// 通过给定的文件流，判断文件的编码类型
         /// <param name="fs">文件流</param>
         /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }





    }
}
