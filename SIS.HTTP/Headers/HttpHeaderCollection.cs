namespace SIS.HTTP.Headers
{
    using SIS.HTTP.Common;
    using SIS.HTTP.Exceptions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }
        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNullOrEmpty(header.Key, nameof(header.Key));
            CoreValidator.ThrowIfNullOrEmpty(header.Value, nameof(header.Value));


            headers[header.Key] = header;

        }

        public bool ContainsHeader(string key)
                   => (headers.Any(x => x.Key == key));
        

        public HttpHeader GetHeader(string key)
        {
            if(this.ContainsHeader(key))
            {
                return headers[key];
            }

            throw new BadRequestException();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in headers)
            {
                sb.AppendLine(item.Value.ToString());
            }

            return sb.ToString(); 
        }
    }
}
