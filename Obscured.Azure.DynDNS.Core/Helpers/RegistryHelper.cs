using System;
using Microsoft.Win32;

namespace Obscured.Azure.DynDNS.Core.Helpers
{
    public class RegistryHelper
    {
        public static string RegPath = @"Software\";

        #region ApplicationGenerics
        public static void SaveValue(string company, string applicationName, string WindowName, string keyString, object value)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(company))
            {
                reg += company + "\\";
            }
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }

            reg += applicationName + "\\";
            var key = Registry.CurrentUser.CreateSubKey(reg + WindowName);
            if (key != null) key.SetValue(keyString, value.ToString());
        }

        public static string GetString(string company, string applicationName, string windowName, string keyString)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(company))
            {
                reg += company + "\\";
            }
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += applicationName + "\\";
            var key = Registry.CurrentUser.OpenSubKey(reg + windowName);
            return key != null ? key.GetValue(keyString).ToString() : "";
        }

        public static int GetInt(string company, string applicationName, string windowName, string keyString)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(company))
            {
                reg += company + "\\";
            }
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += applicationName + "\\";
            var key = Registry.CurrentUser.OpenSubKey(reg + windowName);
            if (key == null) return 0;

            var top = int.Parse(key.GetValue(keyString).ToString());
            return top;
        }

        public static double GetDouble(string company, string applicationName, string windowName, string keyString)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(company))
            {
                reg += company + "\\";
            }
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += applicationName + "\\";
            var key = Registry.CurrentUser.OpenSubKey(reg + windowName);
            if (key == null) return 0.0;

            var top = Double.Parse(key.GetValue(keyString).ToString());
            return top;
        }

        public static bool GetBoolean(string company, string applicationName, string windowName, string keyString)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(company))
            {
                reg += company + "\\";
            }
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            reg += applicationName + "\\";

            var key = Registry.CurrentUser.OpenSubKey(reg + windowName);
            if (key == null) return false;

            var top = bool.Parse(key.GetValue(keyString).ToString());
            return top;
        }
        #endregion ApplicationGenerics

        #region ApplicationWindowManagement
        public static void SaveStatus(string company, string applicationName, string windowName, double top, double left, double width, double height, bool maximized)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(company))
            {
                reg += company + "\\";
            }
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += applicationName + "\\";

            var key = Registry.CurrentUser.CreateSubKey(reg + windowName);
            if (key != null)
            {
                key.SetValue("Position_Top", top.ToString());
                key.SetValue("Position_Left", left.ToString());
                key.SetValue("Position_Width", width.ToString());
                key.SetValue("Position_Height", height.ToString());
                key.SetValue("Position_IsMaximized", maximized.ToString());
            }
        }

        public static double GetTop(string Company, string ApplicationName, string WindowName)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(Company))
            {
                reg += Company + "\\";
            }
            if (String.IsNullOrEmpty(ApplicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            reg += ApplicationName + "\\";

            var key = Registry.CurrentUser.OpenSubKey(reg + WindowName);
            if (key == null) return 0.0;
            
            var top = Double.Parse(key.GetValue("Position_Top").ToString());
            return top;
        }

        public static double GetLeft(string Company, string ApplicationName, string WindowName)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(Company))
            {
                reg += Company + "\\";
            }
            if (String.IsNullOrEmpty(ApplicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += ApplicationName + "\\";
            var key = Registry.CurrentUser.OpenSubKey(reg + WindowName);
            if (key == null) return 0.0;
            
            var top = Double.Parse(key.GetValue("Position_Left").ToString());
            return top;
        }
        public static double GetWidth(string Company, string ApplicationName, string WindowName)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(Company))
            {
                reg += Company + "\\";
            }
            if (String.IsNullOrEmpty(ApplicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += ApplicationName + "\\";
            var key = Registry.CurrentUser.OpenSubKey(reg + WindowName);
            if (key == null) return 500.0;
            
            var top = Double.Parse(key.GetValue("Position_Width").ToString());
            return top;
        }
        public static double GetHeight(string Company, string ApplicationName, string WindowName)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(Company))
            {
                reg += Company + "\\";
            }
            if (String.IsNullOrEmpty(ApplicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            
            reg += ApplicationName + "\\";
            var key = Registry.CurrentUser.OpenSubKey(reg + WindowName);
            if (key == null) return 300.0;
            
            var top = Double.Parse(key.GetValue("Position_Height").ToString());
            return top;
        }
        public static bool GetIsMaximized(string Company, string ApplicationName, string WindowName)
        {
            var reg = RegPath;
            if (!String.IsNullOrEmpty(Company))
            {
                reg += Company + "\\";
            }
            if (String.IsNullOrEmpty(ApplicationName))
            {
                throw new ArgumentNullException("ApplicationName cannot be null or left empty.");
            }
            reg += ApplicationName + "\\";

            var key = Registry.CurrentUser.OpenSubKey(reg + WindowName);
            if (key == null) return false;
            
            var top = bool.Parse(key.GetValue("Position_IsMaximized").ToString());
            return top;
        }
        #endregion ApplicationWindowManagement
    }
}