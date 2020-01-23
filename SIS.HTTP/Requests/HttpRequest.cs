namespace SIS.HTTP.Requests
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

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParamets(requestBody);
        }

        private void ParseHeaders(IEnumerable<string> plainHeaders)
        {
            plainHeaders.Select(plainHeader => plainHeader
                .Split(new[]{':',' '}, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(headerKvp => this.Headers.AddHeader(
                    new HttpHeader(headerKvp[0], headerKvp[1])));
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        private void ParseUrlMethod(string[] requestLineParams)
        {
            this.Url = requestLineParams[1];
        }

        private void ParseRequestMethod(string[] requestLineParams)
        {
            bool parseResult = HttpRequestMethod.TryParse(requestLineParams[0], true,
                 out HttpRequestMethod result);

            if (!parseResult)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = result;
        }

        private void ParseRequestQueryParameters()
        {
            this.Url.Split('?')[1]
                .Split('&')
                .Select(queryParameter => queryParameter.Split('='))
                .ToList()
                .ForEach(queryParameterKvp => this.QueryData[queryParameterKvp[0]] =
                queryParameterKvp[1]);
        }

        private void ParseRequestFormDataParamets(string requestBody)
        {
            requestBody.Split('&')
                .Select(bodyParamer => bodyParamer.Split('='))
                .ToList()
                .ForEach(bodyParamerKvp => this.FormData[bodyParamerKvp[0]] =
                bodyParamerKvp[1]);
            //rb.Split(&).Select(x => x.Split(=)).ToList().ForEach(kvp => this.BodyList[key] == value)
        }
    }
}
