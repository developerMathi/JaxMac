using MaxVonGrafKftMobileModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxVonGrafKftMobile
{
    public class Constants
    {
        //public static int ClientId = 971;  //CSD live
        //public static int ClientId = 369;  // QA
        //public static int ClientId = 224;  // QA

        //public static int ClientId = 148;  //  staging sf-123
        //public static int ClientId = 321;
        public static int ClientId = 1028;//Jax Live

        //public static int ClientId = 1013;
        //public static int ClientId = 975;
        //public static int ClientId = 262;

        public static Admin admin = null;
        public static CutomerAuthContext cutomerAuthContext = null;
        public static CustomerReview customerDetails = null;
        public static List<int> countriesHasState = new List<int>() { 144, 121, 33, 34, 103, 198, 202, 69, 212, 2 };

        public static int LastMessageId = 0;  // QA
        // using for find home page or not to enable back key
        public static bool IsHome = false;
        public static bool IsRegisteredandNotLogin = false;
        public void setAdmin(Admin admi)
        {
            admin = admi;
        }

        public static string returnNavotarCardTypes(string type)
        {
            if (type.ToLower().Contains("visa"))
            {
                return "Visa";
            }

            else if (type.ToLower().Contains("master"))
            {
                return "Master";
            }
            else if (type.ToLower().Contains("discover"))
            {
                return "Discover";
            }
            else if (type.ToLower().Contains("express"))
            {
                return "American_Express";
            }
            else
            {
                return "Credit_Card";
            }
            
        }
    }
}
