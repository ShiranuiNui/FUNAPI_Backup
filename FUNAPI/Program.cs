using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// https://github.com/nicklasjepsen/ProfileApiSample/tree/master/ProfileApi.WebApi
// https://github.com/mithunvp/ContactsAPI/tree/master/src/ContactsApi

namespace FUNAPI
{
    public class Program
    {
        public static int Main(string[] args)
        {
            BuildWebHost(args).Run();
            return 0;
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:5000/")
            .UseKestrel()
            .Build();
    }
}