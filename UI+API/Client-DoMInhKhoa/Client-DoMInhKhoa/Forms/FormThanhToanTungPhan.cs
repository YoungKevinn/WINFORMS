using Client_DoMInhKhoa.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormThanhToanTungPhan : Form
    {
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");

        private FlowLayoutPanel flp = null!;

        // Bottom UI
        private Label lblTong = null!;
        private ComboBox cboPhuongThuc = null!;
        private TextBox txtKhachDua = null!;
        private Label lblTienThoi = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        private readonly List<RowVm> _rows = new();
        private bool _isFormatting = false;

        private readonly int _donGoiId;
        private readonly int _banId;

        // ✅ để bill lấy ảnh QR (nếu chuyển khoản)
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

            public void Inc()
            {
                if (ThanhToan < SoLuong) ThanhToan++;
            }

            public void Dec()
            {
                if (ThanhToan > 0) ThanhToan--;
            }
        }

        public List<PayLine> ResultLines { get; private set; } = new();
        public string PhuongThuc { get; private set; } = "Tiền mặt";
        public decimal KhachDua { get; private set; } = 0m;
        public decimal TienThoi { get; private set; } = 0m;

        public FormThanhToanTungPhan(List<(ChiTietDonGoiDto ct, string tenMon)> items, int donGoiId, int banId)
        {
            _donGoiId = donGoiId;
            _banId = banId;

            Text = "Thanh toán từng phần";
            StartPosition = FormStartPosition.CenterParent;

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 560);
            MinimumSize = new Size(980, 560);

            Font = new Font("Segoe UI", 10);
            BackColor = Color.White;

            _rows.AddRange(items.Select(x => new RowVm
            {
                ChiTietId = x.ct.Id,
                TenMon = x.tenMon,
                SoLuong = x.ct.SoLuong,
                ThanhToan = 0,
                DonGia = x.ct.DonGia
            }));

            BuildUI();
            RenderList();
            RecalcTotal();
        }

        private void BuildUI()
        {
            flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(12, 10, 12, 10)
            };

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300f));

            var panelLeft = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            lblTong = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(6, 18),
                Text = "Tổng cần trả: 0 đ"
            };
            panelLeft.Controls.Add(lblTong);

            var panelMid = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };

            var lblPT = new Label { AutoSize = true, Text = "Phương thức:", Location = new Point(6, 10) };
            cboPhuongThuc = new ComboBox
            {
                Location = new Point(120, 6),
                Width = 240,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboPhuongThuc.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản" });
            cboPhuongThuc.SelectedIndex = 0;
            cboPhuongThuc.SelectedIndexChanged += (s, e) =>
            {
                UpdateCashFieldsVisibility();
                RecalcTotal();
            };

            var lblKD = new Label { AutoSize = true, Text = "Khách đưa:", Location = new Point(6, 46) };
            txtKhachDua = new TextBox
            {
                Location = new Point(120, 42),
                Width = 240,
                Text = "0"
            };

            txtKhachDua.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            txtKhachDua.TextChanged += (s, e) =>
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
                finally
                {
                    _isFormatting = false;
                }

                RecalcTotal();
            };

            var lblTT = new Label { AutoSize = true, Text = "Tiền thối:", Location = new Point(6, 82) };
            lblTienThoi = new Label
            {
                AutoSize = true,
                Text = "0 đ",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(120, 82)
            };

            panelMid.Controls.Add(lblPT);
            panelMid.Controls.Add(cboPhuongThuc);
            panelMid.Controls.Add(lblKD);
            panelMid.Controls.Add(txtKhachDua);
            panelMid.Controls.Add(lblTT);
            panelMid.Controls.Add(lblTienThoi);

            var panelRight = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0), Padding = new Padding(0) };

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Anchor = AnchorStyles.Right
            };

            btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(110, 36),
                Margin = new Padding(0, 0, 12, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            btnOK = new Button
            {
                Text = "Xác nhận",
                Size = new Size(130, 36),
                Margin = new Padding(0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnOK.Click += BtnOK_Click;

            buttons.Controls.Add(btnCancel);
            buttons.Controls.Add(btnOK);

            panelRight.Controls.Add(buttons);

            panelRight.Resize += (s, e) =>
            {
                buttons.Left = panelRight.Width - buttons.Width - 6;
                buttons.Top = (panelRight.Height - buttons.Height) / 2;
            };

            table.Controls.Add(panelLeft, 0, 0);
            table.Controls.Add(panelMid, 1, 0);
            table.Controls.Add(panelRight, 2, 0);

            bottom.Controls.Add(table);

            Controls.Add(flp);
            Controls.Add(bottom);

            UpdateCashFieldsVisibility();
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

        private void RenderList()
        {
            flp.SuspendLayout();
            flp.Controls.Clear();

            int idx = 1;
            foreach (var r in _rows)
            {
                flp.Controls.Add(CreateRowCard(idx, r));
                idx++;
            }

            flp.ResumeLayout();
        }

        private Panel CreateRowCard(int index, RowVm r)
        {
            var card = new Panel
            {
                Height = 72,
                Width = flp.ClientSize.Width - 30,
                Margin = new Padding(0, 0, 0, 8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            flp.SizeChanged += (s, e) => { card.Width = flp.ClientSize.Width - 30; };

            var lblTen = new Label
            {
                Text = $"{index}. {r.TenMon}",
                AutoSize = true,
                Location = new Point(12, 10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblNote = new Label
            {
                Text = "Ghi chú/Món thêm",
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

            btnMinus.Click += (s, e) => { r.Dec(); RefreshRow(); };
            btnPlus.Click += (s, e) => { r.Inc(); RefreshRow(); };

            qtyBox.Controls.Add(btnMinus);
            qtyBox.Controls.Add(lblQty);
            qtyBox.Controls.Add(btnPlus);

            right.Resize += (s, e) => RefreshRow();

            right.Controls.Add(lblDonGia);
            right.Controls.Add(qtyBox);
            right.Controls.Add(lblThanhTien);

            card.Controls.Add(right);
            card.Controls.Add(lblTen);
            card.Controls.Add(lblNote);

            card.HandleCreated += (s, e) => RefreshRow();

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

                // ✅ CHỈ HIỆN QR KHI CHUYỂN KHOẢN
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
    }
}
