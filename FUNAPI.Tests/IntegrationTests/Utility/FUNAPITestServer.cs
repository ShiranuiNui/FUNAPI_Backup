using System;
using FUNAPI.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace FUNAPI.Tests
{
    public class FUNAPITestServer : IDisposable
    {
        private readonly TestServer server;
        public LecturesTestContext Context { get; private set; }
        public FUNAPITestServer()
        {
            this.Context = new LecturesTestContext();
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(service => service.AddDbContext<LecturesTestContext>()));
        }
        public RequestBuilder CreateRequest(string path)
        {
            return this.server.CreateRequest(path);
        }
        public void Dispose()
        {
            this.server.Dispose();
        }
    }
}