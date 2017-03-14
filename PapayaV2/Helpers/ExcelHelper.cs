using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using PapayaX2.Database;

namespace PapayaX2.Helpers
{
    public class ExcelHelper
    {
        private PapayaEntities db = new PapayaEntities();

        public class ImportEventArgs : EventArgs
        {
            public string Message { get; set; }

            public ImportEventArgs(string _message)
            {
                this.Message = _message;
            }
        }

        public delegate void ImportEventHandler(object sender, ImportEventArgs ea);
        public event ImportEventHandler RowProcessed;
        public event ImportEventHandler RowInserted;
        public event ImportEventHandler RowWarning;
        public event ImportEventHandler RowError;

        protected void RaiseRowProcessed()
        {
            var handler = this.RowProcessed;
            if (handler != null)
                handler(null, new ImportEventArgs(""));
        }

        protected void RaiseRowInserted(string message)
        {
            var handler = this.RowInserted;
            if (handler != null)
                handler(null, new ImportEventArgs(message));
        }

        protected void RaiseRowWarning(string message)
        {
            var handler = this.RowWarning;
            if (handler != null)
                handler(null, new ImportEventArgs(message));
        }

        protected void RaiseRowError(string message)
        {
            var handler = this.RowError;
            if (handler != null)
                handler(null, new ImportEventArgs(message));
        }

        public void Import(ArrayList data, String tableName, String[] columns, List<String> skipColumns = null, List<String> skipRows = null)
        {
            if (skipColumns == null)
            {
                skipColumns = new List<String>();
            }
            if (skipRows == null)
            {
                skipRows = new List<String>();
            }
            try
            {
                SqlConnection conn = new SqlConnection(((EntityConnection)db.Database.Connection).StoreConnection.ConnectionString);
                conn.Open();
                String sql = "INSERT INTO " + tableName + " (";
                String columnList = "";
                String paramList = "";
                int ctr = 1;

                foreach (String col in columns)
                {
                    columnList += col + ", ";
                    paramList += "@p" + ctr + ", ";
                    ctr++;
                }

                columnList = columnList.Substring(0, columnList.Length - 2);
                paramList = paramList.Substring(0, paramList.Length - 2);
                sql += columnList + ") VALUES (" + paramList + ")";

                //CallableStatement proc = conn.prepareCall(sql);
                int rowIdx = 1;
                foreach (ArrayList row in data)
                {
                    if (!skipRows.Contains(rowIdx.ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            int colIdx = 1;
                            ctr = 1;
                            foreach (String cell in row)
                            {
                                if (!skipColumns.Contains(ctr.ToString()))
                                {
                                    cmd.Parameters.AddWithValue("@p" + colIdx, cell);
                                    colIdx++;
                                }
                                ctr++;
                            }
                            try
                            {
                                cmd.ExecuteNonQuery();
                                this.RaiseRowInserted("Row : #" + rowIdx + ") <b style=\"color:red\">[INSERTED]</b>");
                                //proc.execute();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("duplicate"))
                                    this.RaiseRowWarning("Row : #" + rowIdx + ") <b style=\"color:red\">[DUPLICATED]</b>");
                                else
                                    this.RaiseRowError("Row : #" + rowIdx + ") <b style=\"color:red\">[ERROR]:</b> " + ex.Message);
                                //System.Console.WriteLine(e.Message);
                                //System.out.println("ERROR INSERTING ROW: " + row);
                                //e.printStackTrace();
                            }
                        }
                    }

                    this.RaiseRowProcessed();
                    rowIdx++;
                }
                conn.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        private ArrayList xlsParser(String filePath, String[] xlsRowExtension = null, String sheet = "Sheet1")
        {
            ArrayList ret = new ArrayList();
            try
            {
                IWorkbook wb = new HSSFWorkbook(new FileStream(filePath, FileMode.Open));
                HSSFSheet sht = (HSSFSheet)wb.GetSheet(sheet);
                int rowCount = sht.PhysicalNumberOfRows;
                int colCount = sht.GetRow(0).PhysicalNumberOfCells;
                for (int i = 0; i < rowCount; i++)
                {
                    ArrayList row = new ArrayList();
                    for (int j = 0; j < colCount; j++)
                    {
                        string col = (sht.GetRow(i).GetCell(j) != null ? sht.GetRow(i).GetCell(j).ToString() : "");
                        row.Add(col);
                    }
                    if (xlsRowExtension != null)
                    {
                        foreach (String cellExt in xlsRowExtension)
                        {
                            row.Add(cellExt);
                        }
                    }
                    ret.Add(row);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return ret;
        }

        private ArrayList csvParser(String filePath, String[] csvRowExtension = null)
        {
            ArrayList ret = new ArrayList();
            try
            {
                String[] lines = File.ReadAllLines(filePath);
                foreach (String l in lines)
                {
                    String line = l;
                    if (line.Length > 0)
                    {
                        if (csvRowExtension != null)
                        {
                            foreach (String ext in csvRowExtension)
                            {
                                line += "," + ext;
                            }
                        }
                        ArrayList row = new ArrayList();
                        String[] csvRow = line.Split(new char[] { ',' });
                        foreach (String cell in csvRow)
                        {
                            row.Add(cell);
                        }
                        ret.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return ret;
        }

        public ArrayList Parser(string filePath, String[] rowExtension = null, String sheet = "Sheet1")
        {
            ArrayList ret = new ArrayList();

            switch (Path.GetExtension(filePath).ToLower())
            {
                case ".csv":
                    ret = csvParser(filePath, rowExtension);
                    break;
                case ".xls":
                    ret = xlsParser(filePath, rowExtension, sheet);
                    break;
            }

            return ret;
        }

        public void Export(String filename, ArrayList data, int startRow, int startColumn, String templateSheet = "Sheet1", List<int> numericColumns = null)
        {
            if (numericColumns == null)
            {
                numericColumns = new List<int>();
            }

            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Create))
                {
                    HSSFWorkbook templateWorkbook = new HSSFWorkbook();

                    HSSFSheet sheet = (HSSFSheet)templateWorkbook.CreateSheet(templateSheet);

                    int numeric;
                    for (int i = 0; i < data.Count; i++)
                    {
                        ArrayList row = (ArrayList)data[i];
                        IRow xlsRow = sheet.CreateRow(i + startRow);

                        for (int j = 0; j < row.Count; j++)
                        {
                            if (numericColumns.Contains(j))
                            {
                                xlsRow.CreateCell(j + startColumn).SetCellType(CellType.NUMERIC);

                                if (Int32.TryParse(row[j].ToString(), out numeric))
                                    xlsRow.CreateCell(j + startColumn).SetCellValue(numeric);
                                else
                                    xlsRow.CreateCell(j + startColumn).SetCellValue(row[j].ToString());
                            }
                            else
                            {
                                xlsRow.CreateCell(j + startColumn).SetCellValue(row[j].ToString());
                            }
                        }
                    }

                    templateWorkbook.Write(fs);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public byte[] GetExcelBytes(ArrayList data, int startRow, int startColumn, List<int> numericColumns = null)
        {
            if (numericColumns == null)
            {
                numericColumns = new List<int>();
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    HSSFWorkbook templateWorkbook = new HSSFWorkbook();

                    HSSFSheet sheet = (HSSFSheet)templateWorkbook.CreateSheet("Sheet1");

                    int numeric;
                    for (int i = 0; i < data.Count; i++)
                    {
                        ArrayList row = (ArrayList)data[i];
                        IRow xlsRow = sheet.CreateRow(i + startRow);

                        for (int j = 0; j < row.Count; j++)
                        {
                            if (numericColumns.Contains(j))
                            {
                                xlsRow.CreateCell(j + startColumn).SetCellType(CellType.NUMERIC);

                                if (Int32.TryParse(row[j].ToString(), out numeric))
                                    xlsRow.CreateCell(j + startColumn).SetCellValue(numeric);
                                else
                                    xlsRow.CreateCell(j + startColumn).SetCellValue(row[j].ToString());
                            }
                            else
                            {
                                xlsRow.CreateCell(j + startColumn).SetCellValue(row[j].ToString());
                            }
                        }
                    }

                    templateWorkbook.Write(stream);

                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Method to get all sheet names in the opened xls file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The list of names</returns>
        public List<string> GetSheets(String filePath)
        {
            List<string> list = new List<string>();

            IWorkbook wb = new HSSFWorkbook(new FileStream(filePath, FileMode.Open));

            foreach (ISheet sheet in wb)
            {
                list.Add(sheet.SheetName);
            }

            return list;
        }
    }
}