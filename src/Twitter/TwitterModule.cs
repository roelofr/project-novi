﻿using System;
using LinqToTwitter;
using System.Collections.Generic;
using System.Linq;
using Project_Novi.Api;
using System.Net;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Threading;
using System.Windows.Forms;

namespace Twitter
{
    public class TwitterModule : IModule
    {

        public List<Tweet> tweets1 = new List<Tweet>();
        public List<Tweet> tweets2 = new List<Tweet>();
        public List<Tweet> tweets3 = new List<Tweet>();
        public List<string> accounts = new List<string>();

        public List<Tweet> hashtags1 = new List<Tweet>();
        public List<Tweet> hashtags2 = new List<Tweet>();
        public List<Tweet> hashtags3 = new List<Tweet>();
        public List<string> hashtags = new List<string>();

        public Image usernameImage1;
        public Image usernameImage2;
        public Image usernameImage3;

        WebClient wc = new WebClient();

        public List<Image> hashtagImage1 = new List<Image>();
        public List<Image> hashtagImage2 = new List<Image>();
        public List<Image> hashtagImage3 = new List<Image>();

        private Tweet tweet;

        public string twitterAccountToDisplay = GetUsernameTwitter("username1");

        public string Name
        {
            get { return "Twitter"; }
        }

        public Bitmap Icon
        {
            get
            {
                return Properties.Resources.twittericon;
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
                tweets1.Clear();
                tweets2.Clear();
                tweets3.Clear();
                accounts.Clear();
                hashtags.Clear();
                hashtagImage1.Clear();
                hashtagImage2.Clear();
                hashtagImage3.Clear();

                hashtags1.Clear();
                hashtags2.Clear();
                hashtags3.Clear();
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
                XmlNodeList elemList = root.GetElementsByTagName("tag");
                Console.WriteLine(elemList.Count);
                for (int i = 0; i < elemList.Count; i++)
                {
                    accounts.Add(elemList[i].InnerXml);

                    var statusTweets = from tweet in twitterContext.Status
                        where tweet.Type == StatusType.User &&
                              tweet.ScreenName == elemList[i].InnerXml &&
                              tweet.IncludeContributorDetails == true &&
                              tweet.Count == 4 &&
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
                        byte[] bytes = new WebClient().DownloadData(pic);
                        MemoryStream ms = new MemoryStream(bytes);
                        Image img = Image.FromStream(ms);

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
                XmlNodeList elemList2 = root.GetElementsByTagName("type");
                Console.WriteLine(elemList2.Count);
                for (int i = 0; i < elemList2.Count; i++)
                {
                    hashtags.Add(elemList2[i].InnerXml);
                    var hashtagtweet = from tweet in twitterContext.Search
                        where tweet.Type == SearchType.Search &&
                              tweet.Query == "#" + elemList2[i].InnerXml &&
                              tweet.Count == 4 &&
                              tweet.IncludeEntities == true
                        select tweet;


                    foreach (var twit in hashtagtweet)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            tweet = new Tweet(twit.Statuses[j].User.Name, twit.Statuses[j].CreatedAt,
                                twit.Statuses[j].Text);

                            if (i == 0)
                            {
                                hashtags1.Add(tweet);
                            }
                            if (i == 1)
                            {
                                hashtags2.Add(tweet);
                            }
                            if (i == 2)
                            {
                                hashtags3.Add(tweet);
                            }
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            byte[] bytes = wc.DownloadData(twit.Statuses[j].User.ProfileImageUrl);
                            MemoryStream ms = new MemoryStream(bytes);
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                            if (i == 0)
                            {
                                hashtagImage1.Add(img);
                            }
                            if (i == 1)
                            {
                                hashtagImage2.Add(img);
                            }
                            if (i == 2)
                            {
                                hashtagImage3.Add(img);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public static string GetUsernameTwitter(string usernameNumber)
        {
            //username1
            //username2
            //username3
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/Strings/{0}/Username", usernameNumber));

            return nodeList[0].InnerText;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
