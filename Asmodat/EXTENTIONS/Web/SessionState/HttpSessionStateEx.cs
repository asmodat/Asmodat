using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;
using Asmodat.Debugging;
using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asmodat.Extentions.Web.SessionState
{
    
    public static class HttpSessionStateEx
    {
        /// <summary>
        /// Gets or creates key in session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Session"></param>
        /// <param name="key"></param>
        /// <param name="default_value"></param>
        /// <returns></returns>
        public static T GetValue<T>(this HttpSessionState Session, string key, T default_value)
        {
            if (Session == null)
                return default_value;

            try
            {
                if (Session[key] == null)
                {
                    Session[key] = default_value;
                    return default_value;
                }
                
                return (T)Session[key];
            }
            catch
            {
                return default_value;
            }
        }

        public static bool SetValue<T>(this HttpSessionState Session, string key, T value)
        {
            if (Session == null || key.IsNullOrEmpty())
                return false;

            try
            {
                Session[key] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}


/*
public static T GetValue<T>(this HttpSessionState Session, string key, T default_value)
        {
            if (Session == null || key.IsNullOrEmpty())
                return default_value;

            try
            {
                
                if (Session[key] == null)
                    return default_value;
                
                return (T)Session[key];
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return default_value;
            }
        }

*/
