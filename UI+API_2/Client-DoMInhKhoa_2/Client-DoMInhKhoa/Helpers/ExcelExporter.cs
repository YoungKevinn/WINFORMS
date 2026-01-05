using ClosedXML.Excel;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Helpers
{
    public static class ExcelExporter
    {
        public static void ExportDataGridViewToExcel(
            DataGridView dgv,
            string sheetName,
            string title,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            decimal? totalRevenue = null)
        {
            if (dgv == null) throw new ArgumentNullException(nameof(dgv));

            if (dgv.Rows.Count == 0 || dgv.Columns.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            // chỉ export cột visible (đúng thứ tự hiển thị)
            var cols = dgv.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            if (cols.Count == 0)
            {
                MessageBox.Show("Không có cột hiển thị để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(SafeSheetName(sheetName));

            int r = 1;

            // ===== Title =====
            ws.Cell(r, 1).Value = title;
            ws.Range(r, 1, r, cols.Count).Merge();
            ws.Row(r).Height = 24;
            ws.Cell(r, 1).Style.Font.Bold = true;
            ws.Cell(r, 1).Style.Font.FontSize = 14;
            ws.Cell(r, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r += 2;

            // ===== Meta =====
            if (fromDate.HasValue || toDate.HasValue)
            {
                ws.Cell(r, 1).Value = "Từ ngày:";
                ws.Cell(r, 2).Value = fromDate?.ToString("dd/MM/yyyy") ?? "";
                ws.Cell(r, 4).Value = "Đến ngày:";
                ws.Cell(r, 5).Value = toDate?.ToString("dd/MM/yyyy") ?? "";
                r += 2;
            }

            // ===== Header =====
            int headerRow = r;
            for (int i = 0; i < cols.Count; i++)
            {
                var c = cols[i];
                ws.Cell(headerRow, i + 1).Value = c.HeaderText ?? c.Name;
            }

            var headerRange = ws.Range(headerRow, 1, headerRow, cols.Count);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            r++;

            // ===== Data =====
            int startDataRow = r;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                for (int i = 0; i < cols.Count; i++)
                {
                    var col = cols[i];
                    var val = row.Cells[col.Name].Value;
                    var cell = ws.Cell(r, i + 1);

                    if (val == null)
                    {
                        cell.Value = "";
                    }
                    else if (val is DateTime dt)
                    {
                        cell.Value = dt;
                        cell.Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";
                    }
                    else if (TryToDecimal(val, out var dec))
                    {
                        cell.Value = dec;
                        cell.Style.NumberFormat.Format = "#,##0";
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                    else if (int.TryParse(val.ToString(), out var iv))
                    {
                        cell.Value = iv;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                    else
                    {
                        cell.Value = val.ToString();
                    }
                }

                r++;
            }

            int endDataRow = r - 1;

            if (endDataRow >= startDataRow)
            {
                var dataRange = ws.Range(startDataRow, 1, endDataRow, cols.Count);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            // ===== Total (optional) =====
            if (totalRevenue.HasValue)
            {
                r += 1;
                ws.Cell(r, cols.Count - 1).Value = "Tổng doanh thu:";
                ws.Cell(r, cols.Count).Value = totalRevenue.Value;

                ws.Cell(r, cols.Count - 1).Style.Font.Bold = true;
                ws.Cell(r, cols.Count).Style.Font.Bold = true;
                ws.Cell(r, cols.Count).Style.NumberFormat.Format = "#,##0";
                ws.Cell(r, cols.Count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(headerRow);

            wb.SaveAs(sfd.FileName);

            MessageBox.Show("Xuất Excel thành công!", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string SafeSheetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Sheet1";
            var invalid = new[] { ':', '\\', '/', '?', '*', '[', ']' };
            foreach (var ch in invalid) name = name.Replace(ch, '_');
            return name.Length > 31 ? name.Substring(0, 31) : name;
        }

        private static bool TryToDecimal(object val, out decimal result)
        {
            result = 0;
            var s = val.ToString()?.Trim();
            if (string.IsNullOrEmpty(s)) return false;

            return decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                || decimal.TryParse(s, NumberStyles.Any, new CultureInfo("vi-VN"), out result);
        }
    }
}
