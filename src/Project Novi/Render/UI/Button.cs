using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Text;

namespace Project_Novi.Render.UI
{
    class Button : BasicButton
    {
        /// <summary>
        /// The text to show
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Creates a new button, the controller is required for click handling
        /// </summary>
        /// <param name="ctrl">The controller which fires Touch event we can hook into</param>
        /// <param name="text">The default text</param>
        public Button(IController ctrl, String text)
            : base(ctrl)
        {
            Text = text;
        }
        /// <summary>
        /// Returns the width and height of the current text using the given font size
        /// </summary>
        /// <returns></returns>
        protected Size calculateTextSize()
        {
            if (Text == null)
                return new Size(0, 0);

            var fontFamily = TextUtils.GetFont(TextSize);
            var fontValue = Text;

            return System.Windows.Forms.TextRenderer.MeasureText(Text, fontFamily);
        }
        /// <summary>
        /// Sizes the button to fit the given text.
        /// </summary>
        public void SizeToContentsX()
        {
            Size newSize = calculateTextSize();
            this.Size = new Size(newSize.Width, this.Size.Height);
        }
        /// <summary>
        /// Renders the text and button
        /// </summary>
        /// <param name="graphics"></param>
        public new void Render(Graphics graphics)
        {
            DrawBackground(graphics);
            DrawText(graphics, Text);
        }
    }
}
