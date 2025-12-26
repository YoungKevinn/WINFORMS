using Client_DoMInhKhoa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class TaiKhoanService
    {
        
        private const string ChangePasswordEndpoint = "/api/Auth/change-password";

        public async Task DoiMatKhauAsync(string matKhauCu, string matKhauMoi)
        {
            var req = new DoiMatKhauRequest
            {
                MatKhauCu = matKhauCu,
                MatKhauMoi = matKhauMoi
            };

            // includeAuth = true để gửi kèm JWT token của admin
            await ApiClient.PostAsync<string>(
                ChangePasswordEndpoint,
                req,
                includeAuth: true
            );
        }
    }
}
