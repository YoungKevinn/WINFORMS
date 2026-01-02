using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client_DoMInhKhoa.Models;

namespace Client_DoMInhKhoa.Services
{
    public class NhanVienService
    {
        public Task<List<NhanVienDto>> LayTatCaAsync()
            => ApiClient.GetAsync<List<NhanVienDto>>("/api/NhanVien", includeAuth: true);

        public Task<NhanVienDto> TaoAsync(string maNv, string hoTen, string vaiTro)
            => ApiClient.PostAsync<NhanVienDto>("/api/NhanVien", new
            {
                MaNhanVien = maNv,
                HoTen = hoTen,
                VaiTro = vaiTro
            }, includeAuth: true);

        public Task<string> SuaAsync(int id, string maNv, string hoTen, string vaiTro)
            => ApiClient.PutAsync<string>($"/api/NhanVien/{id}", new
            {
                MaNhanVien = maNv,
                HoTen = hoTen,
                VaiTro = vaiTro
            }, includeAuth: true);

        // Soft delete => API sẽ set TrangThai=2
        public Task XoaKhoaAsync(int id)
            => ApiClient.DeleteAsync($"/api/NhanVien/{id}", includeAuth: true);

        public Task<string> DatTrangThaiAsync(int id, int trangThai)
            => ApiClient.PutAsync<string>($"/api/NhanVien/{id}/status", new
            {
                TrangThai = trangThai
            }, includeAuth: true);

        public Task<string> ResetMatKhauAsync(int id, string matKhauMoi)
            => ApiClient.PutAsync<string>($"/api/NhanVien/{id}/set-password", new
            {
                MatKhauMoi = matKhauMoi
            }, includeAuth: true);
    }
}
