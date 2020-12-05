using MaxVonGrafKftMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxVonGrafKftMobile.Utilties
{
    public static class Common
    {
        public static HomePage mMasterPage { get; set; }
        public static bool isRefresh { get; set; } = false;

        public static List<HomePageMasterMenuItem> homePageMasterMenuItems = new List<HomePageMasterMenuItem>();
    }
}