using API.Models; // Namespace chứa các Entity (DonGoi, ThanhToan...)
using API.DTOs;   // Namespace chứa DTO (ThanhToanCreateUpdateDto)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public ThanhToanController(CafeDbContext context)
        {
            _context = context;
        }

        // =============================================================
        // 1. LẤY LỊCH SỬ THANH TOÁN CỦA 1 ĐƠN (GET)
        // GET: api/ThanhToan?donGoiId=123
        // =============================================================
        [HttpGet]
        public async Task<IActionResult> GetLichSuThanhToan([FromQuery] int donGoiId)
        {
            var lichSu = await _context.ThanhToans
                .Where(x => x.DonGoiId == donGoiId)
                .OrderByDescending(x => x.ThanhToanLuc)
                .ToListAsync();

            return Ok(lichSu);
        }

        // =============================================================
        // 2. TẠO THANH TOÁN MỚI (POST)
        // POST: api/ThanhToan
        // Logic: Ghi nhận tiền -> Tính toán -> Cập nhật trạng thái Đơn & Bàn
        // =============================================================
        [HttpPost]
        public async Task<IActionResult> TaoThanhToan([FromBody] ThanhToanCreateUpdateDto dto)
        {
            // Dùng Transaction để đảm bảo an toàn dữ liệu (sai là rollback hết)
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // A. Ghi nhận thanh toán mới vào bảng ThanhToan
                var tt = new ThanhToan
                {
                    DonGoiId = dto.DonGoiId,
                    SoTien = dto.SoTien,
                    PhuongThuc = dto.PhuongThuc,
                    ThanhToanLuc = DateTime.Now
                };
                _context.ThanhToans.Add(tt);
                await _context.SaveChangesAsync();

                // B. Tính toán lại tài chính
                // 1. Tổng tiền phải trả của đơn (Giá món * Số lượng)
                var tongTienDonHang = await _context.ChiTietDonGois
                    .Where(ct => ct.DonGoiId == dto.DonGoiId)
                    .SumAsync(ct => ct.SoLuong * ct.DonGia);

                // 2. Tổng tiền ĐÃ TRẢ (bao gồm cả lần này)
                var daTra = await _context.ThanhToans
                    .Where(t => t.DonGoiId == dto.DonGoiId)
                    .SumAsync(t => t.SoTien);

                // C. Cập nhật trạng thái Đơn gọi & Bàn
                var donGoi = await _context.DonGois.FindAsync(dto.DonGoiId);

                string trangThaiTraVe = "PARTIAL"; // Mặc định là trả 1 phần

                if (donGoi != null)
                {
                    // Nếu trả Đủ hoặc Dư -> Đóng đơn, Trả bàn
                    if (daTra >= tongTienDonHang)
                    {
                        // 1. Cập nhật Đơn gọi -> Đã thanh toán (1)
                        donGoi.TrangThai = 1;
                        donGoi.DongLuc = DateTime.Now;

                        // 2. Cập nhật Bàn -> Trống (0)
                        var ban = await _context.Bans.FindAsync(donGoi.BanId);
                        if (ban != null)
                        {
                            ban.TrangThai = 0; // Trạng thái 0 = Trống
                        }

                        trangThaiTraVe = "FULL";
                    }
                    else
                    {
                        // Nếu chưa đủ -> Giữ nguyên trạng thái Đơn (0 - Đang phục vụ)
                        // Bàn vẫn đỏ, Đơn vẫn mở để khách trả tiếp lần sau
                        donGoi.TrangThai = 0;
                        trangThaiTraVe = "PARTIAL";
                    }

                    await _context.SaveChangesAsync();
                }

                // D. Hoàn tất Transaction
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Ghi nhận thanh toán thành công",
                    TrangThai = trangThaiTraVe,
                    DaTra = daTra,
                    TongTien = tongTienDonHang,
                    ConLai = (tongTienDonHang - daTra) > 0 ? (tongTienDonHang - daTra) : 0
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest("Lỗi xử lý thanh toán: " + ex.Message);
            }
        }
    }
}