using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace  Common
{
   public class ExcelHelper
    {
        public byte[] ToExcel(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style type=\"text/css\">");
            sb.Append("<!--");
            sb.Append(".text");
            sb.Append("{mso-style-parent:style0;");
            sb.Append("font-size:10.0pt;");
            sb.Append("font-family:\"Arial Unicode MS\", sans-serif;");
            sb.Append("mso-font-charset:0;");
            sb.Append(@"mso-number-format:\@;");
            sb.Append("text-align:center;");
            sb.Append("border:.5pt solid black;");
            sb.Append("white-space:normal;}");
            sb.Append("-->");
            sb.Append("</style>");
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            DataRow[] myRow = dt.Select();
            int i = 0;
            int cl = dt.Columns.Count;

            ////定义标题
            //sb.Append("<tr align=\"Center\" style=\"font-weight:bold;\">");
            //sb.Append("<td colspan=\"" + cl + "\" style =\" height : 25px; font-size: 20pt;\">" + DropEnterprise.SelectedItem + "</td>");
            //sb.Append("</tr>");

            ////定义中间标题
            //sb.Append("<tr align=\"Center\" style=\"font-weight:bold;\">");
            //sb.Append("<td >" + txtStart.Text + "</td>");
            //for (i = 0; i < cl - 2; i++)
            //{
            //    sb.Append("<td ></td>");
            //}
            //sb.Append("<td >" + DropEnterprise.SelectedItem + "</td>");
            //sb.Append("</tr>");

            //定义字段名
            sb.Append("<tr align=\"Center\" style=\"font-weight:bold;\">");
            for (i = 0; i < cl; i++)
            {
                if (i == (cl - 1))
                {
                    sb.Append("<td>" + dt.Columns[i].ColumnName.ToString() + "</td></tr>");
                }
                else
                {
                    //ls_item += dt.Columns[i].ColumnName.ToString() + "\t";
                    sb.Append("<td>" + dt.Columns[i].ColumnName.ToString() + "</td>");
                }
            }

            //定义数据
            foreach (DataRow row in myRow)
            {
                sb.Append("<tr align=\"Center\">");
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))
                    {
                        sb.Append("<td>" + row[i].ToString() + "</td></tr>");
                    }
                    else
                    {
                        //ls_item += dt.Columns[i].ColumnName.ToString() + "\t";
                        sb.Append("<td>" + row[i].ToString() + "</td>");
                    }
                }

            }
            sb.Append("</table>");

            byte[] fileContents = Encoding.Default.GetBytes(sb.ToString());

            return fileContents;
        }

        public string ExcelToDisk(DataTable dt)
        {
            string guid = Guid.NewGuid().ToString("n");

            string uploadPath = HttpContext.Current.Server.MapPath("/upload/export/") + DateTime.Now.ToString("yyyyMM");
            if (!System.IO.Directory.Exists(uploadPath))
            {
                System.IO.Directory.CreateDirectory(uploadPath);
            }

            string tarname = string.Format(@"{0}\{1}.xls", uploadPath,guid );

            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;

            // 为单元格添加样式   
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()]; 
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;//应用边界线 左边界线  
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;//应用边界线 上边界线  
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;//应用边界线 下边界线  

            int Colnum = dt.Columns.Count;//表格列数 
            int Rownum = dt.Rows.Count;//表格行数 

            // 为表头添加样式 
            Aspose.Cells.Style headStyle = workbook.Styles[workbook.Styles.Add()];
            headStyle.Font.IsBold = true;  
            headStyle.Font.Size = 11;

            //生成行 列名行 
            for (int i = 0; i < Colnum; i++)
            {
                cells[0, i].SetStyle(style); 
                cells[0, i].SetStyle(headStyle); 
                cells[0, i].PutValue(dt.Columns[i].ColumnName);
                cells[0, i].GetStyle().Pattern = BackgroundType.Solid;
                cells[0, i].GetStyle().ForegroundColor = System.Drawing.Color.CadetBlue;
            }

            //生成数据行 
            for (int i = 0; i < Rownum; i++)
            {
                cells.SetRowHeight(i, 25);
                for (int k = 0; k < Colnum; k++)
                {
                    cells[1 + i, k].SetStyle(style);
                    cells[1 + i, k].PutValue(dt.Rows[i][k].ToString());
                }
            }

            cells.SetRowHeight(Rownum, 25);

            for (int col = 0; col < Colnum; col++)
            {
                sheet.AutoFitColumn(col, 0, Colnum);
            }

            for (int col = 0; col < Colnum; col++)
            {
                cells.SetColumnWidthPixel(col, cells.GetColumnWidthPixel(col) + 30);
            }
             
            book.Save(tarname);

            return guid;
        }
    }
}
