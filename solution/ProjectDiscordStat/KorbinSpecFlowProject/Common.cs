    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace KorbinSpecFlowProject
    {
        // Sitewide definitions and useful methods
        public class Common
        {
            public const string BaseUrl = "https://localhost:7228";
            // Page names that everyone should use
            public const string HomePageName = "Home";
            public const string AccountPageName = "Account";
            public const string ContactUsPageName = "Contact";
            public const string PrivacyPageName = "Privacy";
            public const string AllServersPageName = "AllServers";
            public const string ServerGrowthPageName = "ServerGrowth";
        public const string ForumPageName = "Forum";

        // File to store browser cookies in
        public const string CookieFile = "C:\\Users\\Korbin Cardoza\\Desktop\\School\\cookies.txt";

            // A handy way to look these up
            public static readonly Dictionary<string, string> Paths = new()
            {
                { HomePageName, "/" },
                { AccountPageName, "/Account/Account" },
                { ContactUsPageName, "/Home/Contact" },
                { PrivacyPageName, "/Home/Privacy" },
                { AllServersPageName, "/Home/AllServers" },
                { ServerGrowthPageName, "/Account/ServerGrowth" },
                { ForumPageName, "/Forum/Forum" },
            };

            public static string PathFor(string pathName) => Paths[pathName];
            public static string UrlFor(string pathName) => BaseUrl + Paths[pathName];
        }
    }
