using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string newLocation)
        {
            this.Headers.Add(new Header("Location", newLocation));
            this.StatusCode = StatusCode.Found;

        }
    }
}
