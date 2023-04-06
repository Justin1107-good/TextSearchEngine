using Microsoft.AspNetCore.Mvc;

namespace TextSearchEngine.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class UrlHelperExtensions
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="urlHelper"></param>
    /// <param name="localUrl"></param>
    /// <returns></returns>
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                return urlHelper.Page("/Index");
            }

            return localUrl;
        }
    }
}
