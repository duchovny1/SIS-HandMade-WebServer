using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class FileResponse : HttpResponse
    {
        public FileResponse(byte[] fileContent, string contentType)
        {
            this.StatusCode = StatusCode.Ok;
            this.Content = fileContent;
            this.Headers.Add(new Header("Content-type", contentType));
            this.Headers.Add(new Header("Content-Length", this.Content.Length.ToString()));
        }
    }
}
