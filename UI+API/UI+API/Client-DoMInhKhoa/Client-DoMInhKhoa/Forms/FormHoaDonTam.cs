using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormHoaDonTam : Form
    {
        private readonly int _donGoiId;

        private readonly ChiTietDonGoiService _ctDonGoiService = new();
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();

        private List<ThucAnDto> _dsThucAn = new();
        private List<ThucUongDto> _dsThucUong = new();

        private List<Row> _rows = new();
        private readonly CultureInfo _vi = CultureInfo.GetCultureInfo("vi-VN");

        private Label lblTitle = null!;
        private Label lblMa = null!;
        private Label lblNgayGio = null!;
        private DataGridView dgv = null!;
        private Label lblTong = null!;
        private Button btnIn = null!;
        private Button btnDong = null!;
        private PrintDocument printDocument1 = null!;

        private class Row
        {
            public string TenMon { get; set; } = "";
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien => DonGia * SoLuong;
        }

        public FormHoaDonTam(int donGoiId)
        {
            _donGoiId = donGoiId;

            BuildLayout();
            AttachEvents();

            StartPosition = FormStartPosition.CenterParent;
            Text = "Hóa đơn tạm (Pre-bill)";
            Width = 760;
            Height = 540;
        }

        private void BuildLayout()
        {
            lblTitle = new Label
            {
                Text = "HÓA ĐƠN TẠM (PRE-BILL)",
                Dock = DockStyle.Top,
                Height = 46,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var panelTop = new Panel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(10, 6, 10, 6) };

            lblMa = new Label
            {
                Text = "Đơn gọi: ",
                AutoSize = true,
                Location = new Point(10, 8),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblNgayGio = new Label
            {
                Text = "Thời gian: ",
                AutoSize = true,
                Location = new Point(10, 34),
                Font = new Font("Segoe UI", 10)
            };

            panelTop.Controls.Add(lblMa);
            panelTop.Controls.Add(lblNgayGio);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };

            var panelBottom = new Panel { Dock = DockStyle.Bottom, Height = 64 };

            lblTong = new Label
            {
                Text = "Tổng: 0 đ",
                AutoSize = true,
                Location = new Point(12, 20),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.RoyalBlue
            };

            btnIn = new Button
            {
                Text = "In tạm",
                Size = new Size(110, 34),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            btnDong = new Button
            {
                Text = "Đóng",
                Size = new Size(90, 34),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            panelBottom.Controls.Add(lblTong);
            panelBottom.Controls.Add(btnIn);
            panelBottom.Controls.Add(btnDong);

            panelBottom.Resize += (s, e) =>
            {
                btnDong.Location = new Point(panelBottom.Width - 100, 15);
                btnIn.Location = new Point(panelBottom.Width - 220, 15);
            };

            printDocument1 = new PrintDocument();

            Controls.Add(dgv);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Controls.Add(lblTitle);
        }

        private void AttachEvents()
        {
            Load += FormHoaDonTam_Load;
            btnDong.Click += (s, e) => Close();
            btnIn.Click += BtnIn_Click;
            printDocument1.PrintPage += PrintDocument1_PrintPage;
        }

        private async void FormHoaDonTam_Load(object? sender, EventArgs e)
        {
            lblMa.Text = $"Đơn gọi: #{_donGoiId}";
            lblNgayGio.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm}";

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _dsThucAn = await _thucAnService.GetAllAsync() ?? new List<ThucAnDto>();
            _dsThucUong = await _thucUongService.GetAllAsync() ?? new List<ThucUongDto>();

            var chiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiId) ?? new List<ChiTietDonGoiDto>();

            _rows = chiTiet.Select(ct =>
            {
                string ten = "Món";

                if (ct.ThucAnId.HasValue)
                    ten = _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";
                else if (ct.ThucUongId.HasValue)
                    ten = _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";

                return new Row
                {
                    TenMon = ten,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                };
            }).ToList();

            dgv.DataSource = null;
            dgv.DataSource = _rows;

            if (dgv.Columns[nameof(Row.TenMon)] != null) dgv.Columns[nameof(Row.TenMon)].HeaderText = "Tên món";
            if (dgv.Columns[nameof(Row.SoLuong)] != null) dgv.Columns[nameof(Row.SoLuong)].HeaderText = "SL";

            if (dgv.Columns[nameof(Row.DonGia)] != null)
            {
                var col = dgv.Columns[nameof(Row.DonGia)];
                col.HeaderText = "Đơn giá";
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.FormatProvider = _vi;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgv.Columns[nameof(Row.ThanhTien)] != null)
            {
                var col = dgv.Columns[nameof(Row.ThanhTien)];
                col.HeaderText = "Thành tiền";
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.FormatProvider = _vi;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            decimal tong = _rows.Sum(x => x.ThanhTien);
            lblTong.Text = $"Tổng: {tong.ToString("N0", _vi)} đ";
        }

        private void BtnIn_Click(object? sender, EventArgs e)
        {
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = printDocument1;
                preview.Width = 900;
                preview.Height = 700;
                preview.ShowDialog();
            }
        }

        private void PrintDocument1_PrintPage(object? sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            float y = 20;

            using var fontTitle = new Font("Arial", 14, FontStyle.Bold);
            using var fontBold = new Font("Arial", 10, FontStyle.Bold);
            using var font = new Font("Arial", 10);

            g.DrawString("QUÁN CAFE YENA", fontTitle, Brushes.Black, 20, y);
            y += 28;

            g.DrawString("HÓA ĐƠN TẠM (PRE-BILL)", fontBold, Brushes.Black, 20, y);
            y += 22;

            g.DrawString($"Đơn gọi: #{_donGoiId}", font, Brushes.Black, 20, y);
            y += 18;

            g.DrawString($"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm}", font, Brushes.Black, 20, y);
            y += 22;

            g.DrawString("Tên món", fontBold, Brushes.Black, 20, y);
            g.DrawString("SL", fontBold, Brushes.Black, 260, y);
            g.DrawString("Đơn giá", fontBold, Brushes.Black, 300, y);
            g.DrawString("Thành tiền", fontBold, Brushes.Black, 420, y);
            y += 18;

            foreach (var r in _rows)
            {
                g.DrawString(r.TenMon, font, Brushes.Black, 20, y);
                g.DrawString(r.SoLuong.ToString(), font, Brushes.Black, 260, y);
                g.DrawString(r.DonGia.ToString("N0", _vi), font, Brushes.Black, 300, y);
                g.DrawString(r.ThanhTien.ToString("N0", _vi), font, Brushes.Black, 420, y);
                y += 18;

                if (y > e.PageBounds.Height - 120)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            y += 16;
            var tong = _rows.Sum(x => x.ThanhTien);
            g.DrawString($"TỔNG: {tong.ToString("N0", _vi)} đ", fontTitle, Brushes.Black, 260, y);
            y += 28;

            g.DrawString("(*) Đây là hóa đơn tạm, CHƯA thanh toán.", font, Brushes.Black, 20, y);

            e.HasMorePages = false;
        }
    }
}
