using SIS.HTTP;
using SIS.MvcFramework;
using SIS2;
using SulsApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulsApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost.StartASync(new StartUp());
        }
    }
}
