using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Modules
{
    class TextUtils
    {
        private static FontFamily font;
        public static Font getFont(int size)
        {
            if (font != null)
                return new Font(font, size, FontStyle.Regular);

            if (fontFamilyExists("Open Sans Light"))
                font = new FontFamily("Open Sans Light");
            else if (fontFamilyExists("Open Sans"))
                font = new FontFamily("Open Sans");
            else if (fontFamilyExists("Segoe UI Light"))
                font = new FontFamily("Segoe UI Light");
            else if (fontFamilyExists("Segoe UI"))
                font = new FontFamily("Segoe UI");
            else if (fontFamilyExists("Arial"))
                font = new FontFamily("Arial");
            else
                font = new FontFamily("Times New Roman");

            if (font != null)
                return new Font(font, size, FontStyle.Regular);
            return null;
        }

        private static bool fontFamilyExists(string font)
        {
            try
            {
                var _font = new FontFamily(font);
                return _font.IsStyleAvailable(FontStyle.Regular);
            }
            catch
            {
                return false;
            }
        }
    }
}
