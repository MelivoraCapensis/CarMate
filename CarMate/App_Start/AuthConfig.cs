using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using CarMate.Models;
using CarMate.Code;

namespace CarMate
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // следует обновить сайт. Дополнительные сведения: http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");


            //https://developers.facebook.com/
            OAuthWebSecurity.RegisterFacebookClient(
                appId: "814701851943328",
                appSecret: "36bba673fcaea0f6e30a51152dc20353");

            /*
             * Login: CarMate@ukr.net
             * Password: CarMate123456
             * Birthday: 01.01.1989 (Male)
             * 
             * Email: CarMate@ukr.net
             * Password: CarMate123456
             * 
             */


            OAuthWebSecurity.RegisterTwitterClient(
               consumerKey: "zCObyj7ydgkh8zsMfqMha3VOG",
               consumerSecret: "ZaxD0iGZYguKO4g5GVpRzufTc4zbgIThqCat2RMtBJRw5JBXFF");

            //https://vk.com/apps
            OAuthWebSecurity.RegisterClient(
                client: new VKontakteAuthenticationClient(
                    "4880283",
                    "3uPXyI93563gZ8e51ojA"),
                    displayName: "ВКонтакте",
                    extraData: null
                );

            //https://console.developers.google.com/project
            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
