using Sparkle.LinkedInNET;
using Sparkle.LinkedInNET.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LinkedInApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateAPI();
            GetAuthorizationUrl();

            ProfileReturn();
        }

        // http://davideguida.altervista.org/mvc-reading-linkedin-user-profile-data/
        // https://github.com/SparkleNetworks/LinkedInNET

        //public RedirectResult Profile()
        //{
        //    var redirectUrl = "http://mydomain/linkedin/profilereturn/"; 
        //    var url = GetAuthorizationUrl(redirectUrl); 
        //    return Redirect(url.ToString());
        //} 

        private static LinkedInApi CreateAPI()
        {
            var config = new LinkedInApiConfiguration("77ck1ulnz4e5yh", "olqES8j6K3uXJMNg"); 
            var api = new LinkedInApi(config); 
            return api;
        }

        private static Uri GetAuthorizationUrl()//string redirectUrl)
        {
            var api = CreateAPI();
            var scope = Sparkle.LinkedInNET.OAuth2.AuthorizationScope.ReadBasicProfile |
                Sparkle.LinkedInNET.OAuth2.AuthorizationScope.ReadEmailAddress |
                Sparkle.LinkedInNET.OAuth2.AuthorizationScope.ReadContactInfo;

            var state = Guid.NewGuid().ToString();

            var redirectUrl = "http://mywebsite/LinkedIn/OAuth2";

            var url = api.OAuth2.GetAuthorizationUrl(scope, state, redirectUrl);

            return url;
        } 

        private static Person ReadMyProfile(string code, string redirectUrl)
        {
            var api = CreateAPI(); 
            
            var userToken = api.OAuth2.GetAccessToken(code, redirectUrl);
            
            var user = new Sparkle.LinkedInNET.UserAuthorization(userToken.AccessToken); 
            
            var fieldSelector = FieldSelector.For<Person>().WithFirstName().WithLastName().WithEmailAddress();

            var profile = api.Profiles.GetMyProfile(user, null, fieldSelector); 
            
            return profile;
        }

        public void ProfileReturn(string code, string state)
        {
            var redirectUrl = "http://mydomain/linkedin/profilereturn/"; 
            var profile = ReadMyProfile(code, redirectUrl); 
            var jsonProfile = Newtonsoft.Json.JsonConvert.SerializeObject(profile); 
            //return Content(jsonProfile);
            Console.ReadKey();
        }

        //public RedirectResult Profile() { var redirectUrl = "http://mydomain/linkedin/profilereturn/"; var url = GetAuthorizationUrl(redirectUrl); return Redirect(url.ToString()); } - See more at: http://davideguida.altervista.org/mvc-reading-linkedin-user-profile-data/#sthash.t9L5zjy6.dpuf
    }
}
