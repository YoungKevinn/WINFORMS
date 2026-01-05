using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormNhatKyHeThong : Form
    {
        private readonly AuditLogService _auditLogService = new AuditLogService();

        // Lưu danh sách hiện tại để sắp xếp
        private List<AuditLogDto> _currentData = new();

        public FormNhatKyHeThong()
        {
            InitializeComponent();
            InitGrid();

            // Bắt event click header cột để sort
            dgvAuditLog.ColumnHeaderMouseClick += dgvAuditLog_ColumnHeaderMouseClick;
        }

        private void InitGrid()
        {
            dgvAuditLog.AutoGenerateColumns = false;
            dgvAuditLog.Columns.Clear();

            // Thời gian
            var colTime = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.ThoiGian),
                HeaderText = "Thời gian",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" },
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            // Bảng
            var colTable = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.TenBang),
                HeaderText = "Bảng",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            // Id bản ghi
            var colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.IdBanGhi),
                HeaderText = "Id bản ghi",
                Width = 80,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            // Hành động
            var colAction = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.HanhDong),
                HeaderText = "Hành động",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            // Người thực hiện
            var colUser = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.NguoiThucHien),
                HeaderText = "Người thực hiện",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            // Giá trị cũ
            var colOld = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.GiaTriCu),
                HeaderText = "Giá trị cũ (JSON)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            // Giá trị mới
            var colNew = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(AuditLogDto.GiaTriMoi),
                HeaderText = "Giá trị mới (JSON)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dgvAuditLog.Columns.AddRange(
                colTime, colTable, colId, colAction, colUser, colOld, colNew);
        }

        private async void FormNhatKyHeThong_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var from = dtpFrom.Value.Date;
                var to = dtpTo.Value.Date;
                var keyword = txtKeyword.Text.Trim();

                var data = await _auditLogService.SearchAsync(keyword, from, to);

                // Nếu chỉ xem log đăng nhập
                if (chkOnlyLogin.Checked)
                {
                    data = data
                        .Where(x => string.Equals(
                            x.HanhDong,
                            "DangNhap",
                            StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                _currentData = data;

                dgvAuditLog.DataSource = null;
                dgvAuditLog.DataSource = _currentData;

                // clear sort glyph
                foreach (DataGridViewColumn col in dgvAuditLog.Columns)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nhật ký: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        // Khi tick / bỏ tick "Chỉ hiển thị log đăng nhập" -> reload lại dữ liệu
        private async void chkOnlyLogin_CheckedChanged(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void dgvAuditLog_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentData == null || _currentData.Count == 0) return;

            var column = dgvAuditLog.Columns[e.ColumnIndex];
            var prop = column.DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            bool ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;
            List<AuditLogDto> sorted;

            switch (prop)
            {
                case nameof(AuditLogDto.ThoiGian):
                    sorted = ascending
                        ? _currentData.OrderBy(x => x.ThoiGian).ToList()
                        : _currentData.OrderByDescending(x => x.ThoiGian).ToList();
                    break;
                case nameof(AuditLogDto.TenBang):
                    sorted = ascending
                        ? _currentData.OrderBy(x => x.TenBang).ToList()
                        : _currentData.OrderByDescending(x => x.TenBang).ToList();
                    break;
                case nameof(AuditLogDto.IdBanGhi):
                    sorted = ascending
                        ? _currentData.OrderBy(x => x.IdBanGhi).ToList()
                        : _currentData.OrderByDescending(x => x.IdBanGhi).ToList();
                    break;
                case nameof(AuditLogDto.HanhDong):
                    sorted = ascending
                        ? _currentData.OrderBy(x => x.HanhDong).ToList()
                        : _currentData.OrderByDescending(x => x.HanhDong).ToList();
                    break;
                case nameof(AuditLogDto.NguoiThucHien):
                    sorted = ascending
                        ? _currentData.OrderBy(x => x.NguoiThucHien).ToList()
                        : _currentData.OrderByDescending(x => x.NguoiThucHien).ToList();
                    break;
                default:
                    return;
            }

            _currentData = sorted;
            dgvAuditLog.DataSource = null;
            dgvAuditLog.DataSource = _currentData;

            foreach (DataGridViewColumn col in dgvAuditLog.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            column.HeaderCell.SortGlyphDirection =
                ascending ? SortOrder.Ascending : SortOrder.Descending;
        }
       
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
