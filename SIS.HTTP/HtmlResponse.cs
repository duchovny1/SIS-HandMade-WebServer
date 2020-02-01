using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HtmlResponse : HttpResponse
    {
        public HtmlResponse(string html) 
            : base()
        {
            this.StatusCode = StatusCode.Ok;
            byte[] fileContext = Encoding.UTF8.GetBytes(html);
            this.Content = fileContext;
            this.Headers.Add(new Header("Content-type", "text/html"));
            this.Headers.Add(new Header("Content-Length", this.Content.Length.ToString()));
            
        }
    }
}
