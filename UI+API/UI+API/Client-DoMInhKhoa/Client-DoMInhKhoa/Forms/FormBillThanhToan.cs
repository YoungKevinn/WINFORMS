using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBillThanhToan : Form
    {
        private readonly CultureInfo _vi = CultureInfo.GetCultureInfo("vi-VN");

        // Input
        private readonly string _tenQuan;
        private readonly string _diaChi;
        private readonly string _soDienThoai;
        private readonly string _tenNhanVien;
        private readonly string _tenBan;
        private readonly string _maHoaDon;
        private readonly DateTime _thoiGian;
        private readonly List<BillLine> _lines;
        private readonly string _phuongThuc;
        private readonly decimal _khachDua;
        private readonly decimal _tienThoi;
        public decimal TongTien { get; }

        // UI
        private Label lblHeader = null!;
        private Label lblInfo = null!;
        private DataGridView dgv = null!;
        private Label lblTong = null!;
        private Label lblPay = null!;
        private Button btnIn = null!;
        private Button btnDong = null!;
        private PrintDocument printDoc = null!;

        public class BillLine
        {
            public string TenMon { get; set; } = "";
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien => DonGia * SoLuong;
        }

        // Constructor (có diaChi, soDienThoai để khỏi lỗi named parameter)
        public FormBillThanhToan(
            List<BillLine> lines,
            string tenNhanVien,
            string tenBan,
            decimal tongTien,
            string phuongThuc = "Tiền mặt",
            decimal khachDua = 0m,
            decimal tienThoi = 0m,
            string tenQuan = "QUÁN CAFE",
            string diaChi = "",
            string soDienThoai = "",
            string maHoaDon = "",
            DateTime? thoiGian = null)
        {
            InitializeComponent(); // Designer có thì gọi, không sao

            _lines = lines ?? new List<BillLine>();
            _tenNhanVien = tenNhanVien ?? "";
            _tenBan = tenBan ?? "";
            _phuongThuc = phuongThuc ?? "Tiền mặt";
            _khachDua = khachDua;
            _tienThoi = tienThoi;

            _tenQuan = string.IsNullOrWhiteSpace(tenQuan) ? "QUÁN" : tenQuan;
            _diaChi = diaChi ?? "";
            _soDienThoai = soDienThoai ?? "";
            _maHoaDon = maHoaDon ?? "";
            _thoiGian = thoiGian ?? DateTime.Now;

            TongTien = tongTien;

            BuildUI();
            BindGrid();
        }

        private void BuildUI()
        {
            Text = "Bill thanh toán";
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10);
            BackColor = Color.White;
            ClientSize = new Size(820, 520);
            MinimumSize = new Size(820, 520);

            lblHeader = new Label
            {
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Text = _tenQuan
            };

            lblInfo = new Label
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(10, 0, 10, 0),
                Font = new Font("Segoe UI", 10),
                Text = BuildInfoText()
            };

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            dgv.AutoGenerateColumns = false;
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BillLine.TenMon),
                HeaderText = "Tên món",
                FillWeight = 45
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BillLine.SoLuong),
                HeaderText = "SL",
                FillWeight = 10
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BillLine.DonGia),
                HeaderText = "Đơn giá",
                FillWeight = 20,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, FormatProvider = _vi }
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BillLine.ThanhTien),
                HeaderText = "Thành tiền",
                FillWeight = 25,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, FormatProvider = _vi }
            });

            var bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 90,
                BackColor = Color.WhiteSmoke
            };

            lblTong = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(12, 12),
                Text = $"Tổng tiền: {TongTien.ToString("N0", _vi)} đ"
            };

            lblPay = new Label
            {
                AutoSize = true,
                Location = new Point(12, 44),
                Text = BuildPayText()
            };

            btnDong = new Button
            {
                Text = "Đóng",
                Size = new Size(110, 36),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            btnIn = new Button
            {
                Text = "In bill",
                Size = new Size(110, 36),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            bottom.Controls.Add(lblTong);
            bottom.Controls.Add(lblPay);
            bottom.Controls.Add(btnIn);
            bottom.Controls.Add(btnDong);

            bottom.Resize += (s, e) =>
            {
                btnDong.Left = bottom.Width - btnDong.Width - 12;
                btnDong.Top = 24;

                btnIn.Left = btnDong.Left - btnIn.Width - 10;
                btnIn.Top = 24;
            };

            btnDong.Click += (s, e) => Close();
            btnIn.Click += BtnIn_Click;

            printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;

            Controls.Clear();
            Controls.Add(dgv);
            Controls.Add(bottom);
            Controls.Add(lblInfo);
            Controls.Add(lblHeader);
        }

        private string BuildInfoText()
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(_diaChi)) parts.Add(_diaChi);
            if (!string.IsNullOrWhiteSpace(_soDienThoai)) parts.Add("SĐT: " + _soDienThoai);

            var line1 = string.Join(" | ", parts);
            var line2 = $"Bàn: {_tenBan}    NV: {_tenNhanVien}";
            var line3 = $"Thời gian: {_thoiGian:dd/MM/yyyy HH:mm}";
            var line4 = string.IsNullOrWhiteSpace(_maHoaDon) ? "" : $"Mã HĐ: {_maHoaDon}";

            return string.Join(Environment.NewLine,
                new[] { line1, line2, line3, line4 }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private string BuildPayText()
        {
            if ((_phuongThuc ?? "") == "Tiền mặt")
            {
                return $"Phương thức: {_phuongThuc} | Khách đưa: {_khachDua.ToString("N0", _vi)} | Tiền thối: {_tienThoi.ToString("N0", _vi)}";
            }
            return $"Phương thức: {_phuongThuc}";
        }

        private void BindGrid()
        {
            dgv.DataSource = null;
            dgv.DataSource = _lines.ToList();
        }

        private void BtnIn_Click(object? sender, EventArgs e)
        {
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = printDoc;
                preview.Width = 1000;
                preview.Height = 700;
                preview.ShowDialog();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            float y = 20;

            using var fontTitle = new Font("Arial", 14, FontStyle.Bold);
            using var font = new Font("Arial", 10);
            using var fontBold = new Font("Arial", 10, FontStyle.Bold);

            g.DrawString(_tenQuan, fontTitle, Brushes.Black, 20, y);
            y += 26;

            if (!string.IsNullOrWhiteSpace(_diaChi))
            {
                g.DrawString(_diaChi, font, Brushes.Black, 20, y);
                y += 18;
            }
            if (!string.IsNullOrWhiteSpace(_soDienThoai))
            {
                g.DrawString("SĐT: " + _soDienThoai, font, Brushes.Black, 20, y);
                y += 18;
            }

            g.DrawString($"Bàn: {_tenBan}    NV: {_tenNhanVien}", font, Brushes.Black, 20, y);
            y += 18;

            g.DrawString($"Thời gian: {_thoiGian:dd/MM/yyyy HH:mm}", font, Brushes.Black, 20, y);
            y += 18;

            if (!string.IsNullOrWhiteSpace(_maHoaDon))
            {
                g.DrawString($"Mã HĐ: {_maHoaDon}", font, Brushes.Black, 20, y);
                y += 18;
            }

            y += 8;
            g.DrawString("Tên món", fontBold, Brushes.Black, 20, y);
            g.DrawString("SL", fontBold, Brushes.Black, 280, y);
            g.DrawString("Đơn giá", fontBold, Brushes.Black, 330, y);
            g.DrawString("Thành tiền", fontBold, Brushes.Black, 430, y);
            y += 18;

            foreach (var c in _lines)
            {
                g.DrawString(c.TenMon, font, Brushes.Black, 20, y);
                g.DrawString(c.SoLuong.ToString(), font, Brushes.Black, 280, y);
                g.DrawString(c.DonGia.ToString("N0", _vi), font, Brushes.Black, 330, y);
                g.DrawString(c.ThanhTien.ToString("N0", _vi), font, Brushes.Black, 430, y);
                y += 18;
            }

            y += 12;
            g.DrawString($"Tổng: {TongTien.ToString("N0", _vi)} đ", fontTitle, Brushes.Black, 280, y);
            y += 24;

            g.DrawString(BuildPayText(), fontBold, Brushes.Black, 20, y);
        }
    }
}
