using MonoTouch.Foundation;
using MyCompany.Visitors.Client;

namespace MyCompany.Visitors.Client
{
    using MyCompany.Visitors.Client;
    using System;

    /// <summary>
    /// Class to store application settings across a user session.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Auth uri storage key.
        /// </summary>
        public const string STORAGEKEY_AUTHURI = "authUri";
        /// <summary>
        /// api uri storage key.
        /// </summary>
        public const string STORAGEKEY_APIURI = "apiUri";
        /// <summary>
        /// test mode storage key.
        /// </summary>
        public const string STORAGEKEY_TESTMODE = "testMode";
        /// <summary>
        /// security token storage key.
        /// </summary>
        public const string STORAGEKEY_SECURITYTOKEN = "securityToken";
        /// <summary>
        /// security token expiration date storage key.
        /// </summary>
        public const string STORAGEKEY_SECURITYTOKEN_EXPIRATION = "securityTokenExpirationDateTime";
        /// <summary>
        /// Width of snapped view
        /// </summary>
        public const int SNAPPED_WIDTH = 500;

        /// <summary>
        /// Transition between two apps to snapped.
        /// </summary>
        public const int MEDIUM_SNAPPED_WIDTH = 800;

        /// <summary>
        /// Uri for authetication.
        /// </summary>
        public static string AuthenticationUri 
        {
            get
            {
                string authUri;
                //if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(STORAGEKEY_AUTHURI))
                //{
                    authUri = "https://login.windows.net/mycompanydemo.onmicrosoft.com";
                //    ApplicationData.Current.LocalSettings.Values[STORAGEKEY_AUTHURI] = authUri;
                //}
                //else
                //{
                //    authUri = ApplicationData.Current.LocalSettings.Values[STORAGEKEY_AUTHURI].ToString();
                //}
                return authUri;
            }
        }

        /// <summary>
        /// API uri for authentication
        /// </summary>
        public static Uri ApiUri 
        {
            get
            {
				string apiUri = NSUserDefaults.StandardUserDefaults.StringForKey("Server_IP_Adress") ?? "https://mycompanyvisitors.azurewebsites.net/";
	            //apiUri = "https://mycompanyvisitors.azurewebsites.net/";
                if (!apiUri.EndsWith("/"))
                    apiUri = string.Format("{0}/", apiUri);

                return new Uri(apiUri);
            }
        }

        /// <summary>
        /// Reply Uri for authentication process
        /// </summary>
        public static string ReplyUri
        {
            get
            {
                return "http://localhost:31330/";
            }
        }

        /// <summary>
        /// LoginHint
        /// </summary>
        public static string LoginHint
        {
            get
            {
                return "scottha@mycompanydemos.com";
            }
        }

        /// <summary>
        /// Client Id for authentication process
        /// </summary>
        public static string ClientId
        {
            get
            {
                return "ec5bff30-bc31-4de4-8b04-c812a8b27e53";
            }
        }

        /// <summary>
        /// Test mode.
        /// </summary>
        public static bool TestMode
        {
            get
            {
                //if (ApplicationData.Current.LocalSettings.Values.ContainsKey(STORAGEKEY_TESTMODE))
                //{
                //    bool isTest = false;
                //    bool.TryParse(ApplicationData.Current.LocalSettings.Values[STORAGEKEY_TESTMODE].ToString(), out isTest);
                //    return isTest;
                //}
                
                return true;
            }
            set
            {
                //ApplicationData.Current.LocalSettings.Values[STORAGEKEY_TESTMODE] = value.ToString();
            }
        }

        /// <summary>
        /// Obtained token from authentication.
        /// </summary>
        public static string SecurityToken { get; set; }

        /// <summary>
        /// SecurityToken Expiration DateTime
        /// </summary>
        public static DateTimeOffset SecurityTokenExpirationDateTime
        {
            get
            {
                //if (ApplicationData.Current.LocalSettings.Values.ContainsKey(STORAGEKEY_SECURITYTOKEN_EXPIRATION))
                //{
                //    return (DateTimeOffset)ApplicationData.Current.LocalSettings.Values[STORAGEKEY_SECURITYTOKEN_EXPIRATION];
                //}

                return DateTime.MinValue;
            }
            set
            {
                //ApplicationData.Current.LocalSettings.Values[STORAGEKEY_SECURITYTOKEN_EXPIRATION] = value;
            }
        }

        /// <summary>
        /// Authenticated employee information.
        /// </summary>
        public static Employee EmployeeInformation { get; set; }

        /// <summary>
        /// Remove the key and its value from the local settings.
        /// </summary>
        /// <param name="keyToDelete"></param>
        public static void RemoveKeyValue(string keyToDelete)
        {
            //if (ApplicationData.Current.LocalSettings.Values.ContainsKey(keyToDelete))
            //{
            //    ApplicationData.Current.LocalSettings.Values.Remove(keyToDelete);
            //}   
        }

        /// <summary>
        /// update the value of the given key in the local settings.
        /// </summary>
        /// <param name="keyToUpdate"></param>
        /// <param name="value"></param>
        public static void UpdateKeyValue(string keyToUpdate, string value)
        {
            //ApplicationData.Current.LocalSettings.Values[keyToUpdate] = value;
        }
    }
}
