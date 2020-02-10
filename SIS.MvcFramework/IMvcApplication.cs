using SIS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(IList<Route> routeTable);

        void ConfigureServices();
    }
}
