using Microsoft.AspNetCore.WebUtilities;
using OngProject.Core.Interfaces.IServices;
using System;

namespace OngProject.Core.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public string GetPage(string route, int? page)
        {
            if (page.HasValue)
            {
                var endpointUri = new Uri(string.Concat(_baseUri, route));
                var modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "page", page.ToString());

                return new Uri(modifiedUri).ToString();
            }

            return null;
        }
    }
}
