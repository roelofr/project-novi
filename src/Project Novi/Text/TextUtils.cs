using System.Drawing;

namespace Project_Novi.Text
{
    public static class TextUtils
    {
        private static FontFamily _font;
        public static Font GetFont(int size)
        {
            if (_font != null)
                return new Font(_font, size, FontStyle.Regular);

            if (FontFamilyExists("Open Sans Light"))
                _font = new FontFamily("Open Sans Light");
            else if (FontFamilyExists("Open Sans"))
                _font = new FontFamily("Open Sans");
            else if (FontFamilyExists("Segoe UI Light"))
                _font = new FontFamily("Segoe UI Light");
            else if (FontFamilyExists("Segoe UI"))
                _font = new FontFamily("Segoe UI");
            else if (FontFamilyExists("Arial"))
                _font = new FontFamily("Arial");
            else
                _font = new FontFamily("Times New Roman");

            return _font != null ? new Font(_font, size, FontStyle.Regular) : null;
        }

        private static bool FontFamilyExists(string font)
        {
            try
            {
                var fontFamily = new FontFamily(font);
                return fontFamily.IsStyleAvailable(FontStyle.Regular);
            }
            catch
            {
                return false;
            }
        }
    }
}
