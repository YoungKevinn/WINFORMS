using Client_DoMInhKhoa.Forms;
using PdfSharp.Fonts;

namespace Client_DoMInhKhoa
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            ApplicationConfiguration.Initialize();
            Application.Run(new FormChonVaiTro());
        }
    }
}
