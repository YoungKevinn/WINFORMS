using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Helpers
{
    public static class PdfExporter
    {
        private static bool _inited;
        private static readonly object _lock = new();

        private static void EnsureInit()
        {
            if (_inited) return;
            lock (_lock)
            {
                if (_inited) return;

                // License config (Community) :contentReference[oaicite:1]{index=1}
                QuestPDF.Settings.License = LicenseType.Community;

                // giúp QuestPDF auto tìm font hệ thống (Windows Fonts) :contentReference[oaicite:2]{index=2}
                try
                {
                    QuestPDF.Settings.FontDiscoveryPaths.Add(
                        Environment.GetFolderPath(Environment.SpecialFolder.Fonts)
                    );
                }
                catch { /* ignore */ }

                _inited = true;
            }
        }

        public static void ExportDataGridViewToPdf(
            DataGridView dgv,
            string title,
            string sheetName,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            decimal? totalRevenue = null)
        {
            EnsureInit();

            if (dgv == null) throw new ArgumentNullException(nameof(dgv));

            if (dgv.Rows.Count == 0 || dgv.Columns.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

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

            var headers = cols.Select(c => c.HeaderText ?? c.Name).ToList();

            var rows = new List<List<string>>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                var line = new List<string>();
                foreach (var col in cols)
                {
                    var v = row.Cells[col.Name].Value;
                    line.Add(FormatCell(v));
                }
                rows.Add(line);
            }

            var doc = new GridPdfDocument(
                title: title,
                sheetName: sheetName,
                headers: headers,
                rows: rows,
                fromDate: fromDate,
                toDate: toDate,
                totalRevenue: totalRevenue
            );

            doc.GeneratePdf(sfd.FileName);

            MessageBox.Show("Xuất PDF thành công!", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string FormatCell(object v)
        {
            if (v == null) return "";
            if (v is DateTime dt) return dt.ToString("dd/MM/yyyy HH:mm:ss");
            return v.ToString() ?? "";
        }

        private class GridPdfDocument : IDocument
        {
            private readonly string _title;
            private readonly string _sheetName;
            private readonly List<string> _headers;
            private readonly List<List<string>> _rows;
            private readonly DateTime? _fromDate;
            private readonly DateTime? _toDate;
            private readonly decimal? _totalRevenue;

            public GridPdfDocument(
                string title,
                string sheetName,
                List<string> headers,
                List<List<string>> rows,
                DateTime? fromDate,
                DateTime? toDate,
                decimal? totalRevenue)
            {
                _title = title;
                _sheetName = sheetName;
                _headers = headers;
                _rows = rows;
                _fromDate = fromDate;
                _toDate = toDate;
                _totalRevenue = totalRevenue;
            }

            public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

            public void Compose(IDocumentContainer container)
            {
                container.Page(page =>
                {
                    // Landscape cho bảng rộng :contentReference[oaicite:3]{index=3}
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    // ưu tiên font phổ biến trên Windows (hỗ trợ tiếng Việt tốt)
                    page.DefaultTextStyle(x => x.FontFamily("Segoe UI", "Arial").FontSize(10));

                    page.Header().AlignCenter().Text(_title).FontSize(16).Bold();

                    page.Content().PaddingTop(10).Column(col =>
                    {
                        col.Spacing(8);

                        if (_fromDate.HasValue || _toDate.HasValue)
                        {
                            col.Item().Row(r =>
                            {
                                r.RelativeItem().Text($"Từ ngày: {_fromDate?.ToString("dd/MM/yyyy") ?? ""}");
                                r.RelativeItem().Text($"Đến ngày: {_toDate?.ToString("dd/MM/yyyy") ?? ""}");
                                r.RelativeItem().AlignRight().Text($"Sheet: {_sheetName}");
                            });
                        }

                        // Table :contentReference[oaicite:4]{index=4}
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < _headers.Count; i++)
                                    columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                foreach (var h in _headers)
                                {
                                    header.Cell().Element(CellHeaderStyle).Text(h);
                                }
                            });

                            foreach (var row in _rows)
                            {
                                for (int i = 0; i < _headers.Count; i++)
                                {
                                    var value = i < row.Count ? row[i] : "";
                                    table.Cell().Element(CellStyle).Text(value);
                                }
                            }
                        });

                        if (_totalRevenue.HasValue)
                        {
                            col.Item().AlignRight().Text($"Tổng doanh thu: {_totalRevenue.Value.ToString("#,##0", CultureInfo.GetCultureInfo("vi-VN"))}").Bold();
                        }
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Trang ");
                        t.CurrentPageNumber();
                        t.Span(" / ");
                        t.TotalPages();
                    });
                });
            }

            private static IContainer CellStyle(IContainer c)
            {
                return c.Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(4);
            }

            private static IContainer CellHeaderStyle(IContainer c)
            {
                return c.Background(Colors.Grey.Lighten3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(4)
                    .AlignCenter()
                    .AlignMiddle();
            }
        }
    }
}
