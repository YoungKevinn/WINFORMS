using Client_DoMInhKhoa.Forms;
using PdfSharp.Fonts;

namespace Client_DoMInhKhoa
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            ApplicationConfiguration.Initialize();
            Application.Run(new FormChonVaiTro());
        }
    }
}
