using System;
using System.Drawing;

namespace Twitter
{
    public class Tweet
    {
        public string ScreenName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Text { get; private set; }
        public Image Image { get; private set; }
        public string Source { get; private set; }

        public Tweet(string screenname, DateTime createdAt, string text, Image image, string source)
        {
            ScreenName = screenname;
            CreatedAt = createdAt;
            Text = text;
            Image = image;
            Source = source;
        }
    }
}
