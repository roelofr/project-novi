using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    public class Tweet
    {
        public string ScreenName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Text { get; private set; }

        public Tweet(string screenname, DateTime createdAt, string text)
        {
            this.ScreenName = screenname;
            this.CreatedAt = createdAt;
            this.Text = text;
        }
    }
}
