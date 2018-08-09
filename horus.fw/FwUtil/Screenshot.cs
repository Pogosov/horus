using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace horus.fw.FwUtil
{
    public class Screenshot
    {
        public static void CaptureScreen(string filePath)
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            using (var bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb))
            {
                using (var gfx = Graphics.FromImage(bmp))
                {
                    gfx.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                    bmp.Save(filePath);
                }
            }
        }
    }
}
