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

namespace Twitter
{
    public class TwitterModule : IModule
    {
        public List<string> berichten = new List<string>();
        public List<Image> pictures = new List<Image>();
        public List<Tweet> tweets = new List<Tweet>();

        private string twitteraccount = "windesheimICT";
        private Tweet tweet;


        public string Name
        {
            get { return "Twitter"; }
        }

        public void Initialize(IController controller)
        {
        }

        public void Start(){

            const string accessToken = "233587186-jNv7a0eqNBYlSwhYpmI8eg4OOkztPQ7YOrgMkRBf";
            const string accessTokenSecret = "j856eyllBbhNzClgwjJH7va1xqjxyLzl3qLHUd9guCqq0";
            const string consumerKey = "qM7PpwbKslZjnKEBUJ85sOUic";
            const string consumerSecret = "FlBXh2FTQrDqXTTge4vB7I1kmCHii67qF04BQ7I6z7zIWjrYDL";

            const string twitterAccountToDisplay = "windesheimICT";

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

            var statusTweets = from tweet in twitterContext.Status
                               where tweet.Type == StatusType.User &&
                                       tweet.ScreenName == twitterAccountToDisplay &&
                                       tweet.IncludeContributorDetails == true &&
                                       tweet.Count == 5 &&
                                       tweet.IncludeEntities == true
                               select tweet;



            foreach (var statusTweet in statusTweets)
            {
                string bericht = string.Format("@{3} {0} - {1}: \n\n{2}", statusTweet.ScreenName, statusTweet.CreatedAt, statusTweet.Text, twitteraccount);
                berichten.Add(bericht);

                tweet = new Tweet(statusTweet.ScreenName, statusTweet.CreatedAt, statusTweet.Text);

                tweets.Add(tweet);
                
       
                
            }


            var profilePicture = from tweet in twitterContext.User
                                 where tweet.Type == UserType.Show &&
                                        tweet.ScreenName == twitterAccountToDisplay
                                 select tweet.ProfileImageUrl;


            foreach (var pic in profilePicture)
            {
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(pic);
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                pictures.Add(img);
            }
        }

        public void Stop()
        {
        }
    }
}
