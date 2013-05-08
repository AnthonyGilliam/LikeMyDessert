using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Global.Utilities.Interfaces;

namespace Global.Utilities
{
	public class HttpHelper : IHttpHelper
	{
		#region IHttpHelper Members

		public string GetFullPath(string  virtualPath)
		{
			var fullPath = HttpContext.Current.Server.MapPath(virtualPath);
			return fullPath;
		}

        public bool ExistsInSession(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

		public void AddToAppSession(string key, object obj)
		{
		    HttpContext.Current.Session[key] = obj;
		}

		public T GetFromAppSession<T>(string key)
		{
            var obj = (T)HttpContext.Current.Session[key];

		    return obj;
		}

		public void AddToCurrentRequestItems(string key, object obj)
		{
			throw new NotImplementedException();
		}

		public T GetFromCurrentRequestItems<T>(string key)
		{
			throw new NotImplementedException();
		}

		#endregion
    }
}
