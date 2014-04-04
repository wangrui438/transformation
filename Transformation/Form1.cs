using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transformation
{
    public partial class Form1 : Form
    {
        private DataTable _bodyDataTable;
        private DataTable _headDataTable;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtfileName.Text = this.openFileDialog1.FileName;
            }
            
        }

        private void btnTf_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtfileName.Text))
            {
                txtRead(this.txtfileName.Text.Trim());
            }
            else
            {
                MessageBox.Show("请选择要转换的文件！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }


        private void txtRead(string fileName)
        {
            FileStream fileStream = new FileStream(fileName,FileMode.Open,FileAccess.Read);
            StreamReader reader = new StreamReader(fileStream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);//从开始读取 
            string str = reader.ReadLine();
            
            while(str!=null)  
             {
                CreateDataTable(str);
                str = reader.ReadLine();
             }
            reader.Close();
            fileStream.Close();

            if (_bodyDataTable != null && _bodyDataTable.Rows.Count > 1)
            {
                int Xmax = (int)_bodyDataTable.Compute("max(X)", "");
                int Ymax = (int)_bodyDataTable.Compute("max(Y)", "");

                DataTable dt = new DataTable();
                dt.Columns.Add("RowId", Type.GetType("System.String"));
                for (int i = 0; i <= Xmax; i++)
                {
                    dt.Columns.Add(i.ToString(), Type.GetType("System.String"));
                }
                for (int j = 0; j <= Ymax; j++)
                {
                    DataRow row = dt.NewRow();
                    row["RowId"] = j;
                    dt.Rows.Add(row);
                }

                foreach (DataRow item in _bodyDataTable.Rows)
                {
                    string X = item["X"].ToString();
                    string Y = item["Y"].ToString();
                    DataRow[] rows = dt.Select("RowId='" + Y + "'");
                    rows[0]["" + X + ""] = item["Value"].ToString();

                }

                ExportToTxt(dt);

                _bodyDataTable = null;
            }
            else
            {
                MessageBox.Show("选择的文件不符合转换要求！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtfileName.Text = "";
            }
        }

        private void CreateDataTable(string strRow)
        {
            if (!string.IsNullOrEmpty(strRow))
            {
                if (Regex.Match(strRow, "^[X]{1}[0-9]*[Y]{1}[0-9|\t|-]*[Z]{1}").Success)
                {
                    int xIndex = strRow.IndexOf('X') + 1;
                    int yIndex = strRow.IndexOf('Y');
                    int zIndex = strRow.IndexOf('Z');
                    //X
                    string xStr = strRow.Substring(xIndex, yIndex - xIndex).Trim();
                    //Y
                    string yStr = strRow.Substring(yIndex+1, zIndex - yIndex-1).Replace("-","").Trim();
                    //Value
                    string valueStr = strRow.Substring(strRow.Length - 2, 2).Trim();

                    if (_bodyDataTable == null || _bodyDataTable.Rows.Count == 0)
                    {
                        _bodyDataTable = initDataTable("body");
                    }

                    DataRow row;
                    row = _bodyDataTable.NewRow();
                    row["X"] = xStr;
                    row["Y"] = yStr;
                    row["Value"] = valueStr;
                    _bodyDataTable.Rows.Add(row);
                }
                else
                {
                    if (!Regex.Match(strRow, "^[X]{1}[0-9]*[Y]{1}[0-9|\t|-]*").Success)
                    {
                        if (_headDataTable == null || _headDataTable.Rows.Count == 0)
                        {
                            _headDataTable = initDataTable("head");
                        }
                        
                        DataRow row;
                        row = _headDataTable.NewRow();
                        row["HD"] = strRow;
                        _headDataTable.Rows.Add(row);
                    }
                }
            }
        }


        private DataTable initDataTable(string strType)
        {
            DataTable table = new DataTable();
            if (strType.Equals("body"))
            {
                table.Columns.Add("X", Type.GetType("System.Int32"));
                table.Columns.Add("Y", Type.GetType("System.Int32"));
                table.Columns.Add("Value", Type.GetType("System.Int32"));
            }
            if(strType.Equals("head"))
            {
                table.Columns.Add("HD", Type.GetType("System.String"));
            }
            
            return table;
        }


        /// <summary>
        /// 导出到文本文件
        /// </summary>
        /// <param name="_dtExport">需要导出的Datatable</param>
        private void ExportToTxt(DataTable _dtExport)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请选择文件存放路径";
            sfd.FileName = "导出数据";
            sfd.Filter = "文本文档(*.txt)|*.txt";
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string FileName = sfd.FileName.ToLower();
            if (!FileName.Contains(".txt"))
            {
                FileName += ".txt";
            }
            if (FileName.Substring(FileName.LastIndexOf(Path.DirectorySeparatorChar)).Length <= 5)
            {
                MessageBox.Show("文件保存失败，文件名不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                DateTime start = DateTime.Now;

                using (StreamWriter _objWriter = new StreamWriter(FileName, false, System.Text.Encoding.UTF8))
                {
                    //处理文件头
                    IDictionary<string, string> headDic = new Dictionary<string, string>();
                    headDic.Add("DEVICE", "DEVICE:");
                    headDic.Add("LOT", "LOT:");
                    headDic.Add("WAFER", "WAFER:");
                    headDic.Add("FNLOC", "FNLOC:");
                    headDic.Add("ROWCT", "ROWCT:");
                    headDic.Add("COLCT", "COLCT:");
                    headDic.Add("BCEQU", "BCEQU:");
                    headDic.Add("REFPX", "REFPX:");
                    headDic.Add("REFPY", "REFPY:");
                    headDic.Add("DUTMS", "DUTMS:");
                    headDic.Add("XDIES", "XDIES:");
                    headDic.Add("YDIES", "YDIES:");

                    string[] keys = new string[headDic.Count];
                    headDic.Keys.CopyTo(keys, 0);

                    foreach (DataRow dr in _headDataTable.Rows)
                    {
                        string strHead = dr["HD"].ToString().Trim();

                        foreach (string key in keys)
                        {
                            if (Regex.IsMatch(strHead,"^"+key+"{0}"))
                            {
                                //string strVal = Regex.IsMatch(strHead.Substring(key.Length).Trim(), "^[0-9]*") ? strHead.Substring(key.Length).Trim() : "";
                                headDic[key] = headDic[key] + strHead.Substring(key.Length).Trim();
                                //headDic[key] = headDic[key] + strVal;
                            }
                        }
                    }

                    foreach (KeyValuePair<string,string > dicItem in headDic)
                    {
                        _objWriter.WriteLine(dicItem.Value);
                    }

                    
                    //处理坐标图形
                    int rowCount = 0;
                    
                    for (int dc = 0; dc < _dtExport.Columns.Count; dc++)
                    {
                        bool isRemoveColumn = true;
                        foreach (DataRow row in _dtExport.Rows)
                        {
                            if (row[_dtExport.Columns[dc]].ToString() != "")
                            {
                                isRemoveColumn = false;
                            }
                        }
                        if (isRemoveColumn)
                        {
                            _dtExport.Columns.Remove(_dtExport.Columns[dc]);
                            continue;
                        }
                    }

                    for (int i = 0; i < _dtExport.Rows.Count; i++)
                    {
                        rowCount++;
                        int count = 0;
                        bool isRemoveRow = true;
                        foreach (DataColumn item in _dtExport.Columns)
                        {
                            if (item.ColumnName != "RowId" && _dtExport.Rows[i][item.ColumnName].ToString() != "")
                            {
                                count++;
                                isRemoveRow = false;
                            }
                        }

                        if (isRemoveRow)
                        {
                            _dtExport.Rows.Remove(_dtExport.Rows[i]);
                            continue;
                        }

                        int num = 0;
                        StringBuilder sb = new StringBuilder();
                        foreach (DataColumn col in _dtExport.Columns)
                        {
                            string column = _dtExport.Rows[i][col.ColumnName].ToString();

                            if (col.ColumnName == "RowId")
                            {
                                column = "RowData:";
                                //column = count.ToString();
                            }
                            else
                            {

                                if (column != "")
                                {
                                    switch (column)
                                    {
                                        case "56":
                                        case "57":
                                        case "58":
                                            column = "033";
                                            num++;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    if (num > 0 && num < count)
                                    {
                                        column = "001";
                                    }
                                    else
                                    {
                                        column = "___";
                                    }
                                }

                            }
                            string strTail=column.Equals("RowData:") ? "" :" ";
                            sb.Append(column + strTail);
                        }

                        if (rowCount != _dtExport.Rows.Count)
                        {
                            _objWriter.WriteLine(sb.ToString().Trim());
                        }
                        else
                        {
                            _objWriter.Write(sb.ToString().Trim());
                        }

                    }


                   
                }


               
                DateTime end = DateTime.Now;
                TimeSpan ts = end - start;
                MessageBox.Show("文件保存成功，用时" + ts.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            
        }
    }
}
