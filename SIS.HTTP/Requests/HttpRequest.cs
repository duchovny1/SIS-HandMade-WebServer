﻿namespace SIS.HTTP.Requests
{
    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Headers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
        }
        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }


        private bool IsValidRequest(string[] requestLine)
        {
            return false;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParams)
        {
            return false;
        }


        private void ParseRequest(string requestString)
        {
            string[] splitRequestString = requestString.
                Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None).ToArray();

            string[] requestLineParams = splitRequestString[0]
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            if (!this.IsValidRequest(requestLineParams))
            {
                throw new BadRequestException();
            }

            ParseRequestMethod(requestLineParams);
            ParseUrlMethod(requestLineParams);
            ParseRequestPath();

            ParseHeaders(splitRequestString.Skip(1).ToArray());
            //ParseCookies();

            ParseRequestParameters(splitRequestString[splitRequestString.Length - 1]);

        }

        private void ParseRequestParameters(string v)
        {
            throw new NotImplementedException();
        }

        private void ParseHeaders(IEnumerable<string> enumerable)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestPath()
        {
            throw new NotImplementedException();
        }

        private void ParseUrlMethod(string[] requestLineParams)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestMethod(string[] requestLineParams)
        {
            throw new NotImplementedException();
        }
    }
}
