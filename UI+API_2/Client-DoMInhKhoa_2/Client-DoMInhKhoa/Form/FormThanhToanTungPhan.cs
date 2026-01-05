using Client_DoMInhKhoa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormThanhToanTungPhan : Form
    {
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");
        private readonly List<RowVm> _rows = new();
        private bool _isFormatting = false;

        private int _donGoiId;
        private int _banId;

        private bool _hookedResize = false;

        public Image? QrImage { get; private set; }

        public class PayLine
        {
            public int ChiTietId { get; set; }
            public int PayQty { get; set; }
            public decimal DonGia { get; set; }
            public string TenMon { get; set; } = "";
        }

        private class RowVm
        {
            public int ChiTietId { get; set; }
            public string TenMon { get; set; } = "";
            public int SoLuong { get; set; }
            public int ThanhToan { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien => DonGia * ThanhToan;

            public void Inc() { if (ThanhToan < SoLuong) ThanhToan++; }
            public void Dec() { if (ThanhToan > 0) ThanhToan--; }
        }

        public List<PayLine> ResultLines { get; private set; } = new();
        public string PhuongThuc { get; private set; } = "Tiền mặt";
        public decimal KhachDua { get; private set; } = 0m;
        public decimal TienThoi { get; private set; } = 0m;

        // ✅ BẮT BUỘC: ctor rỗng để Designer mở được
        public FormThanhToanTungPhan()
        {
            InitializeComponent();

            // ✅ chặn design-time: không chạy logic runtime khi mở Designer
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            WireUiEvents();

            pnlBottom.Resize += (_, __) => LayoutBottomButtons();
            LayoutBottomButtons();

            RecalcTotal();
        }

        // ✅ ctor runtime bạn dùng
        public FormThanhToanTungPhan(List<(ChiTietDonGoiDto ct, string tenMon)> items, int donGoiId, int banId)
            : this()
        {
            LoadItems(items, donGoiId, banId);
        }

        // ✅ nạp dữ liệu runtime
        public void LoadItems(List<(ChiTietDonGoiDto ct, string tenMon)> items, int donGoiId, int banId)
        {
            _donGoiId = donGoiId;
            _banId = banId;

            _rows.Clear();
            if (items != null)
            {
                _rows.AddRange(items.Select(x => new RowVm
                {
                    ChiTietId = x.ct.Id,
                    TenMon = x.tenMon,
                    SoLuong = x.ct.SoLuong,
                    ThanhToan = 0,
                    DonGia = x.ct.DonGia
                }));
            }

            RenderList();
            UpdateCashFieldsVisibility();
            RecalcTotal();
        }

        private void WireUiEvents()
        {
            cboPhuongThuc.Items.Clear();
            cboPhuongThuc.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản" });
            cboPhuongThuc.SelectedIndex = 0;

            cboPhuongThuc.SelectedIndexChanged += (_, __) =>
            {
                UpdateCashFieldsVisibility();
                RecalcTotal();
            };

            txtKhachDua.KeyPress += (_, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            txtKhachDua.TextChanged += (_, __) =>
            {
                if (_isFormatting) return;

                if ((cboPhuongThuc.SelectedItem?.ToString() ?? "") != "Tiền mặt")
                {
                    RecalcTotal();
                    return;
                }

                try
                {
                    _isFormatting = true;
                    var v = ParseMoneyFromTextBox(txtKhachDua);
                    txtKhachDua.Text = v.ToString("N0", _vi);
                    txtKhachDua.SelectionStart = txtKhachDua.Text.Length;
                }
                finally { _isFormatting = false; }

                RecalcTotal();
            };

            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
            btnOK.Click += BtnOK_Click;
        }

        private void LayoutBottomButtons()
        {
            if (btnOK == null || btnCancel == null || pnlBottom == null) return;

            btnOK.Left = pnlBottom.ClientSize.Width - btnOK.Width - 12;
            btnCancel.Left = btnOK.Left - btnCancel.Width - 12;

            btnOK.Top = pnlBottom.ClientSize.Height - btnOK.Height - 12;
            btnCancel.Top = btnOK.Top;
        }

        private void UpdateCashFieldsVisibility()
        {
            bool isCash = (cboPhuongThuc.SelectedItem?.ToString() ?? "") == "Tiền mặt";
            txtKhachDua.Enabled = isCash;
            txtKhachDua.ReadOnly = !isCash;

            if (!isCash)
            {
                _isFormatting = true;
                txtKhachDua.Text = "0";
                _isFormatting = false;
            }
        }

        private void HookResizeOnce()
        {
            if (_hookedResize) return;
            _hookedResize = true;

            flp.SizeChanged += (_, __) =>
            {
                foreach (Control c in flp.Controls)
                {
                    if (c is Panel p)
                        p.Width = Math.Max(300, flp.ClientSize.Width - 30);
                }
            };
        }

        private void RenderList()
        {
            HookResizeOnce();

            flp.SuspendLayout();
            flp.Controls.Clear();

            int idx = 1;
            foreach (var r in _rows)
            {
                flp.Controls.Add(CreateRowCard(idx, r));
                idx++;
            }

            flp.ResumeLayout();

            // ép lại width sau khi add
            foreach (Control c in flp.Controls)
                if (c is Panel p) p.Width = Math.Max(300, flp.ClientSize.Width - 30);
        }

        private Panel CreateRowCard(int index, RowVm r)
        {
            var card = new Panel
            {
                Height = 72,
                Width = Math.Max(300, flp.ClientSize.Width - 30),
                Margin = new Padding(0, 0, 0, 8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTen = new Label
            {
                Text = $"{index}. {r.TenMon}",
                AutoSize = true,
                Location = new Point(12, 10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblNote = new Label
            {
                Text = "Thanh toán số lượng",
                AutoSize = true,
                Location = new Point(12, 38),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8, FontStyle.Italic)
            };

            var right = new Panel { Dock = DockStyle.Right, Width = 420 };

            var lblDonGia = new Label
            {
                Text = r.DonGia.ToString("N0", _vi),
                AutoSize = true,
                Location = new Point(10, 24),
                Font = new Font("Segoe UI", 9),
            };

            var qtyBox = new Panel
            {
                Location = new Point(160, 18),
                Size = new Size(130, 32),
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnMinus = new Button
            {
                Text = "-",
                Width = 34,
                Height = 28,
                Location = new Point(0, 1),
                FlatStyle = FlatStyle.Flat
            };
            btnMinus.FlatAppearance.BorderSize = 0;

            var lblQty = new Label
            {
                Text = $"{r.ThanhToan}/{r.SoLuong}",
                Width = 60,
                Height = 28,
                Location = new Point(35, 2),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnPlus = new Button
            {
                Text = "+",
                Width = 34,
                Height = 28,
                Location = new Point(94, 1),
                FlatStyle = FlatStyle.Flat
            };
            btnPlus.FlatAppearance.BorderSize = 0;

            var lblThanhTien = new Label
            {
                Text = r.ThanhTien.ToString("N0", _vi),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            void RefreshRow()
            {
                lblQty.Text = $"{r.ThanhToan}/{r.SoLuong}";
                lblThanhTien.Text = r.ThanhTien.ToString("N0", _vi);

                lblThanhTien.Left = right.Width - lblThanhTien.Width - 14;
                lblThanhTien.Top = 24;

                RecalcTotal();
            }

            btnMinus.Click += (_, __) => { r.Dec(); RefreshRow(); };
            btnPlus.Click += (_, __) => { r.Inc(); RefreshRow(); };

            qtyBox.Controls.Add(btnMinus);
            qtyBox.Controls.Add(lblQty);
            qtyBox.Controls.Add(btnPlus);

            right.Controls.Add(lblDonGia);
            right.Controls.Add(qtyBox);
            right.Controls.Add(lblThanhTien);

            card.Controls.Add(right);
            card.Controls.Add(lblTen);
            card.Controls.Add(lblNote);

            card.HandleCreated += (_, __) => RefreshRow();
            right.Resize += (_, __) => RefreshRow();

            return card;
        }

        private decimal GetTotalNeedPay() => _rows.Sum(x => x.DonGia * x.ThanhToan);

        private decimal ParseMoneyFromTextBox(TextBox tb)
        {
            var raw = (tb.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(raw)) return 0m;

            var digits = new string(raw.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(digits)) return 0m;

            if (!decimal.TryParse(digits, out var v)) return 0m;
            return v;
        }

        private void RecalcTotal()
        {
            var total = GetTotalNeedPay();
            lblTong.Text = $"Tổng cần trả: {total.ToString("N0", _vi)} đ";

            var method = cboPhuongThuc.SelectedItem?.ToString() ?? "Tiền mặt";
            if (method == "Tiền mặt")
            {
                var kd = ParseMoneyFromTextBox(txtKhachDua);
                var thoi = kd - total;
                if (thoi < 0) thoi = 0;
                lblTienThoi.Text = $"{thoi.ToString("N0", _vi)} đ";
            }
            else
            {
                lblTienThoi.Text = "0 đ";
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            var total = GetTotalNeedPay();
            if (total <= 0)
            {
                MessageBox.Show("Bạn chưa chọn món / số lượng để thanh toán.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var lines = _rows
                .Where(x => x.ThanhToan > 0)
                .Select(x => new PayLine
                {
                    ChiTietId = x.ChiTietId,
                    PayQty = x.ThanhToan,
                    DonGia = x.DonGia,
                    TenMon = x.TenMon
                })
                .ToList();

            var method = cboPhuongThuc.SelectedItem?.ToString() ?? "Tiền mặt";

            if (method == "Tiền mặt")
            {
                var kd = ParseMoneyFromTextBox(txtKhachDua);
                if (kd < total)
                {
                    MessageBox.Show("Tiền khách đưa chưa đủ.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                KhachDua = kd;
                TienThoi = kd - total;
            }
            else
            {
                KhachDua = 0;
                TienThoi = 0;

                using (var fqr = new FormThanhToanQR(total, _donGoiId, _banId))
                {
                    var dr = fqr.ShowDialog(this);
                    if (dr != DialogResult.OK || !fqr.DaNhanTien)
                    {
                        MessageBox.Show("Chưa xác nhận nhận tiền. Hủy thanh toán.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    QrImage = fqr.QrImage;
                }
            }

            ResultLines = lines;
            PhuongThuc = method;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void lblTienThoi_Click(object sender, EventArgs e)
        {

        }
    }
}
