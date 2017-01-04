using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Xemio.Client
{
    public static class QueryString
    {
        public static string Build(IDictionary<string, string> query)
        {
            var queryItems = query
                .Select(f => $"{WebUtility.UrlEncode(f.Key)}={WebUtility.UrlEncode(f.Value)}")
                .ToList();

            if (queryItems.Any() == false)
                return string.Empty;

            return $"?{string.Join("&", queryItems)}";
        }
    }
}