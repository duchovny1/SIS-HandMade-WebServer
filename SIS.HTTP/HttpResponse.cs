using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(StatusCode code, byte[] body)
        {
            this.HttpVersion = HttpVersion.Http11;
            this.StatusCode = code;
            this.Headers = new List<Header>();
            this.Content = body;

            if (body?.Length > 0)
            {
                this.Headers.Add(new Header("Content-Length", body.Length.ToString()));
            }
        }
        public HttpVersion HttpVersion { get; set; }

        public StatusCode StatusCode { get; set; }

        public IList<Header> Headers { get; set; }

        public byte[] Content { get; set; }

        public override string ToString()
        {
            StringBuilder responseAsString = new StringBuilder();
            var httpVersionAsString = this.HttpVersion switch
            {
                HttpVersion.Http10 => "HTTP/1.0",
                HttpVersion.Http11 => "HTTP/1.1",
                HttpVersion.Http20 => "HTTP/2.0",
                _ => "HTTP/1.1"
            };

            responseAsString.Append($"{httpVersionAsString} " +
                $"{(int)StatusCode} {this.StatusCode}" + HttpConstants.NewLine);


            foreach (var header in Headers)
            {
                responseAsString.Append(header.ToString() + HttpConstants.NewLine);
            }


            responseAsString.Append(HttpConstants.NewLine);

            return responseAsString.ToString();
        }
    }
}
