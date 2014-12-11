using System;
using LinqToTwitter;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;
using Project_Novi.Background;

namespace Twitter
{
    public class TwitterModule : IModule
    {
        public List<string> berichten = new List<string>();


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

            const string twitterAccountToDisplay = "WindesheimICT";

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
                string bericht = string.Format("\n\n {0} om {1}: \n\n-{2}", statusTweet.ScreenName, statusTweet.CreatedAt, statusTweet.Text);
                berichten.Add(bericht);
            }


        }

        public void Stop()
        {
            //berichten = null;
        }
    }
}
