using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien
    {
        // UI search menu (đặt trong Designer file theo yêu cầu)
        private Label? lblTimMonMenu;
        private TextBox? txtTimMonMenu;

        /// <summary>
        /// Tạo UI ô tìm món trong khu vực menu top (cùng hàng Danh mục/Món).
        /// Có thể gọi từ InitializeComponent() hoặc gọi sau InitializeComponent() đều được.
        /// </summary>
        private void InitializeMenuMonSearchUI()
        {
            // Parent tốt nhất: cùng panel chứa cboDanhMuc/cboMon
            Control parent = cboDanhMuc?.Parent ?? cboMon?.Parent ?? this;

            // Nếu đã có sẵn control name "txtTimMon" thì chỉ gán lại reference
            var existTxt = parent.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTimMon");
            var existLbl = parent.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblTimMon");

            if (existTxt != null)
            {
                txtTimMonMenu = existTxt;
                lblTimMonMenu = existLbl; // có thể null nếu trước đó chưa tạo label
                if (lblTimMonMenu == null)
                {
                    lblTimMonMenu = new Label
                    {
                        Name = "lblTimMon",
                        AutoSize = true,
                        Text = "Tìm món:",
                        Font = new Font("Segoe UI", 9F),
                    };
                    parent.Controls.Add(lblTimMonMenu);
                    lblTimMonMenu.BringToFront();
                }

                ReflowMenuMonSearchUI();
                HookMenuMonSearchResizeOnce(parent);
                return;
            }

            // tránh add trùng theo field
            if (txtTimMonMenu != null && !txtTimMonMenu.IsDisposed) return;

            lblTimMonMenu = new Label
            {
                Name = "lblTimMon",
                AutoSize = true,
                Text = "Tìm món:",
                Font = new Font("Segoe UI", 9F),
            };

            txtTimMonMenu = new TextBox
            {
                Name = "txtTimMon",
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            parent.Controls.Add(lblTimMonMenu);
            parent.Controls.Add(txtTimMonMenu);

            lblTimMonMenu.BringToFront();
            txtTimMonMenu.BringToFront();

            ReflowMenuMonSearchUI();
            HookMenuMonSearchResizeOnce(parent);
        }

        private bool _menuMonSearchResizeHooked = false;

        private void HookMenuMonSearchResizeOnce(Control parent)
        {
            if (_menuMonSearchResizeHooked) return;
            _menuMonSearchResizeHooked = true;

            parent.Resize += (s, e) => ReflowMenuMonSearchUI();
        }

        /// <summary>
        /// Canh lại vị trí/width theo layout hiện tại.
        /// </summary>
        private void ReflowMenuMonSearchUI()
        {
            if (lblTimMonMenu == null || txtTimMonMenu == null) return;
            if (lblTimMonMenu.IsDisposed || txtTimMonMenu.IsDisposed) return;

            Control parent = txtTimMonMenu.Parent;

            int padL = 10;
            int padR = 12;

            // Y: nằm dưới hàng "Món"
            int baseBottom = 0;
            if (cboMon != null) baseBottom = cboMon.Bottom;
            else if (cboDanhMuc != null) baseBottom = cboDanhMuc.Bottom;

            int y = baseBottom + 10; // cách ra 10px

            // X: canh giống combo (thường là 90)
            int xLabel = padL;
            int xText = 90;

            lblTimMonMenu.Location = new Point(xLabel, y + 4);
            txtTimMonMenu.Location = new Point(xText, y);

            int w = parent.ClientSize.Width - xText - padR;
            if (w < 160) w = 160;
            txtTimMonMenu.Width = w;
            txtTimMonMenu.Height = cboMon?.Height ?? 27;
        }

        private TextBox? GetMenuSearchTextBox() => txtTimMonMenu;
        private string GetMenuSearchText() => txtTimMonMenu?.Text ?? "";
    }
}
