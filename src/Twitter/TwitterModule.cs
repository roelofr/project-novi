using System;
using LinqToTwitter;
using System.Collections.Generic;
using System.Linq;
using Project_Novi.Api;
using System.Net;
using System.IO;
using System.Drawing;
using System.Xml;

namespace Twitter
{
    public class TwitterModule : IModule
    {
        public List<string> Accounts = new List<string>();
        public List<string> Hashtags = new List<string>();

        public List<Tweet> AllTweets = new List<Tweet>();

        public List<Tweet> User1Tweets = new List<Tweet>();
        public List<Tweet> User2Tweets = new List<Tweet>();
        public List<Tweet> User3Tweets = new List<Tweet>();

        public List<Tweet> Hashtag1Tweets = new List<Tweet>();
        public List<Tweet> Hashtag2Tweets = new List<Tweet>();
        public List<Tweet> Hashtag3Tweets = new List<Tweet>();

        public Image User1Image;
        public Image User2Image;
        public Image User3Image;

        public List<Image> Hashtag1Image = new List<Image>();
        public List<Image> Hashtag2Image = new List<Image>();
        public List<Image> Hashtag3Image = new List<Image>();

        public string TwitterAccountToDisplay = "All";

        public string Name
        {
            get { return "Twitter"; }
        }

        public Bitmap Icon
        {
            get
            {
                return Properties.Resources.bird;
            }
        }

        public string DisplayName
        {
            get { return "Twitter"; }
        }

        public bool Rotatable
        {
            get { return true; }
        }

        public void Initialize(IController controller)
        {
            controller.BackgroundUpdate += Update;
            Update();
        }

        public void Update()
        {
            try
            {
                var user1Tweets = new List<Tweet>();
                var user2Tweets = new List<Tweet>();
                var user3Tweets = new List<Tweet>();
                var hashtag1Tweets = new List<Tweet>();
                var hashtag2Tweets = new List<Tweet>();
                var hashtag3Tweets = new List<Tweet>();
                var accounts = new List<string>();
                var hashtags = new List<string>();

                const string accessToken = "2913538690-VtwNfPvdm17B16HmUwTMYbOUnXxxAXg3nJCPQG0";
                const string accessTokenSecret = "lRl45rfuVtwDNqiG0n0ioMOuwyKyvIqzOyZi3owczM43d";
                const string consumerKey = "HmvQgWj0nSthuP31zFV0dURCY";
                const string consumerSecret = "TejpuAZ51rmoVxizZxkL7pHvIGArCEKMYUv4xi1THvCKyGIYFE";

                var authorizer = new SingleUserAuthorizer
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = consumerKey,
                        ConsumerSecret = consumerSecret,
                        OAuthToken = accessToken,
                        OAuthTokenSecret = accessTokenSecret
                    }
                };
                var twitterContext = new TwitterContext(authorizer);

                // Perform local caching of images
                var pics = new Dictionary<string, Image>();
                Func<string, Image> getPic = (url =>
                {
                    if (pics.ContainsKey(url))
                    {
                        return pics[url];
                    }

                    var bytes = new WebClient().DownloadData(url);
                    var ms = new MemoryStream(bytes);
                    return Image.FromStream(ms);
                });

                var xmlDoc = new XmlDocument();
                xmlDoc.Load("TwitterSettings.xml");
                var root = xmlDoc.DocumentElement;

                // Usernames
                var usernameNodes = root.GetElementsByTagName("tag");

                for (int i = 0; i < usernameNodes.Count; i++)
                {
                    var name = usernameNodes[i].InnerXml;
                    accounts.Add(name);

                    var statusTweets = from tweet in twitterContext.Status
                                       where tweet.Type == StatusType.User &&
                                             tweet.ScreenName == name &&
                                             tweet.IncludeContributorDetails == true &&
                                             tweet.Count == 4 &&
                                             tweet.IncludeEntities == true
                                       select tweet;

                    foreach (var statusTweet in statusTweets)
                    {
                        var pic = statusTweet.User.ProfileImageUrl;
                        var img = getPic(pic);

                        var tweet = new Tweet(statusTweet.ScreenName, statusTweet.CreatedAt, statusTweet.Text, img, "@" + name);
                        if (i == 0)
                        {
                            user1Tweets.Add(tweet);
                            User1Image = img;
                        }
                        if (i == 1)
                        {
                            user2Tweets.Add(tweet);
                            User2Image = img;
                        }
                        if (i == 2)
                        {
                            user3Tweets.Add(tweet);
                            User3Image = img;
                        }
                    }
                }

                // Hashtags
                var hashtagNodes = root.GetElementsByTagName("type");

                for (int i = 0; i < hashtagNodes.Count; i++)
                {
                    var hashtag = hashtagNodes[i].InnerXml;
                    hashtags.Add(hashtag);

                    var hashtagtweet = from tweet in twitterContext.Search
                                       where tweet.Type == SearchType.Search &&
                                             tweet.Query == "#" + hashtag &&
                                             tweet.Count == 4 &&
                                             tweet.IncludeEntities == true
                                       select tweet;


                    foreach (var twit in hashtagtweet)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            var statusTweet = twit.Statuses[j];

                            var pic = statusTweet.User.ProfileImageUrl;
                            var img = getPic(pic);

                            var tweet = new Tweet(statusTweet.User.Name, statusTweet.CreatedAt, statusTweet.Text, img, "#" + hashtag);

                            if (i == 0)
                                hashtag1Tweets.Add(tweet);
                            if (i == 1)
                                hashtag2Tweets.Add(tweet);
                            if (i == 2)
                                hashtag3Tweets.Add(tweet);
                        }
                    }
                }

                // The allTweets is composed of the most recent tweets of all sources
                var allTweets = new List<Tweet>
                {
                    user1Tweets[0],
                    user2Tweets[0],
                    user3Tweets[0],
                    hashtag1Tweets[0],
                    hashtag2Tweets[0],
                    hashtag3Tweets[0]
                };
                allTweets.Sort((tweet1, tweet2) => tweet1.CreatedAt.CompareTo(tweet2.CreatedAt));
                allTweets.Reverse();

                // Update all module variables
                Accounts = accounts;
                Hashtags = hashtags;

                AllTweets = allTweets;
                User1Tweets = user1Tweets;
                User2Tweets = user2Tweets;
                User3Tweets = user3Tweets;

                Hashtag1Tweets = hashtag1Tweets;
                Hashtag2Tweets = hashtag2Tweets;
                Hashtag3Tweets = hashtag3Tweets;
            }
            catch { }
        }

        public static string GetUsernameTwitter(string usernameNumber)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/Strings/{0}/Username", usernameNumber));

            return nodeList[0].InnerText;
        }

        public void Start()
        {
            TwitterAccountToDisplay = "All";
        }

        public void Stop()
        {

        }
    }
}
