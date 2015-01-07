using System;
using LinqToTwitter;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;
using Project_Novi.Background;
using System.Net;
using System.IO;
using System.Drawing;
using System.Xml;
using Project_Novi;

namespace Twitter
{
    public class TwitterModule : IModule
    {
        
        public List<Tweet> tweets1 = new List<Tweet>();
        public List<Tweet> tweets2 = new List<Tweet>();
        public List<Tweet> tweets3 = new List<Tweet>();
        public List<string> accounts = new List<string>();

        public Image usernameImage1;
        public Image usernameImage2;
        public Image usernameImage3;
        private Tweet tweet;
        WebClient wc = new WebClient();
        private const string Username1 = "1";
        private const string Username2 = "2";
        private const string Username3 = "3";
        public string twitterAccountToDisplay = GetUsernameTwitter("username" + Username2);


        public string Name
        {
            get { return "Twitter"; }
        }

        public string DisplayName
        {
            get { return "Twitter"; }
        }

        public void Initialize(IController controller)
        {
        }

        public static string GetUsernameTwitter(string usernameNumber)
        {
            //username1
            //username2
            //username3
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/Strings/{0}/String", usernameNumber));

            return nodeList[0].InnerText;
        }

        public void Start()
        {

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


            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList elemList = root.GetElementsByTagName("String");
            Console.WriteLine(elemList.Count);
            for (int i = 0; i < elemList.Count; i++)
            {
                accounts.Add(elemList[i].InnerXml);

                var statusTweets = from tweet in twitterContext.Status
                                   where tweet.Type == StatusType.User &&
                                         tweet.ScreenName == elemList[i].InnerXml &&
                                         tweet.IncludeContributorDetails == true &&
                                         tweet.Count == 3 &&
                                         tweet.IncludeEntities == true
                                   select tweet;

                foreach (var statusTweet in statusTweets)
                {
                    tweet = new Tweet(statusTweet.ScreenName, statusTweet.CreatedAt, statusTweet.Text);
                    if (i == 0)
                    {
                        tweets1.Add(tweet);
                    }
                    if (i == 1)
                    {
                        tweets2.Add(tweet);
                    }
                    if (i == 2)
                    {
                        tweets3.Add(tweet);
                    }
                    
                }

                var profilePicture = from tweet in twitterContext.User
                                     where tweet.Type == UserType.Show &&
                                     tweet.ScreenName == elemList[i].InnerXml
                                     select tweet.ProfileImageUrl;

                foreach (var pic in profilePicture)
                {
                    byte[] bytes = wc.DownloadData(pic);
                    MemoryStream ms = new MemoryStream(bytes);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                  
                    if (i == 0)
                    {
                        usernameImage1 = img;
                    }
                    if (i == 1)
                    {
                        usernameImage2 = img;
                    }
                    if (i == 2)
                    {
                        usernameImage3 = img;
                    }
                }
                
            }
        }
        public void Stop()
        {
            tweets1.Clear();
            tweets2.Clear();
            tweets3.Clear();
            accounts.Clear();
            
        }
    }
}
