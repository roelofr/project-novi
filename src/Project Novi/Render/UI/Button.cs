using Project_Novi.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Render.UI
{
    class Button : BasicButton
    {
        public String Text { get; set; }

        public Button(IController ctrl, String text)
            : base(ctrl)
        {
            Text = text;
        }
        protected int getFontSize()
        {
            return (int)Math.Floor(Size.Height * 72 / 96 * .7d);
        }
        protected Size getTextSize()
        {
            if (Text == null)
                return new Size(0, 0);

            var fontFamily = TextUtils.GetFont(getFontSize());
            var fontValue = Text;

            return System.Windows.Forms.TextRenderer.MeasureText(Text, fontFamily);
        }

        public void SizeToContentsX()
        {
            Size newSize = getTextSize();
            this.Size = new Size(newSize.Width, this.Size.Height);
        }
        public override void Render(Graphics graphics)
        {
            DrawBackground(graphics);
            DrawText(graphics, Text);
        }
    }
}
