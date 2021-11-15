using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rsk.AspNetCore.Authentication.Saml2p;

namespace SP
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllersWithViews();

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = "cookie";
        options.DefaultChallengeScheme = "saml2";
      }).AddCookie("cookie")
        .AddSaml2p("saml2", options =>
        {
          options.Licensee = "DEMO";
          options.LicenseKey =
            "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjEtMTItMDlUMDE6MDA6MDAuNDk3MjU1NCswMDowMCIsImlhdCI6IjIwMjEtMTEtMDlUMDE6MDA6MDAiLCJvcmciOiJERU1PIiwiYXVkIjoyfQ==.bnTlss6aavv6Sc3GyOTjBYwH2itFum8JVQhC7lh+Y5KbLqzT4aOCck4QXbeyswFATLI8at70DBYILuMg1Qm9+mD4AlJJHbsXkLUmaEinyLsRrdXh0IQu9zn3S9+9TIsBWGxpYXDm/5EI0LFDYSQWo0ZkHTCI3RYXv1rwHgKqlRPqNDqAOaEG5st0+9BZPVmdneyJIYhgZoFrqAp2dJs29TY1dca65jhgOhbVANgz9xV6La+bGHhURPES7ei12ahKOClgDXORAU4mIBuaOKpzmFYnIL7f81v6mt7arHg/1waPC/ybArwCjAOHWviX/BFx/xvnRdyp7ei9tG0hPcR1qE3rvnhM66wco0eUEzjwAmgAytpB6Sy7DcC9yiweco8Wm6ywyC5h8HDXxLI0wBwsKuv2ww/k1p79ScJqhF7gTPeFMlwsjFzDb51Ts/mjXoCHc0XVNW0YEKEGAlBAUuLn9rhlAm90QrLzX0BN32rjPNNq9PUl69oQEcZ89DbmmmcVeZstPOHfIQzagOf2B8FEAyYzdMcVzCTHXffBG7DkKiSOrWC7fOvoIOj2e73EBrxVVUR2XT5tQy4gxNtGJQsJv9IvZu6Xl1biPthyClLDZtrUtxrvIanJZwymG75W5AsYrWkomWHfvm5eb2ICbgr21L5KRnYfkq6ODX+rpVGbPrk=";

          options.CallbackPath = "/signin-saml";
          options.SignInScheme = "cookie";

          options.ServiceProviderOptions = new SpOptions()
          {
            EntityId = "https://localhost:5002/saml"
          };

          options.IdentityProviderMetadataAddress = "https://login.microsoftonline.com/7f89cb48-aad5-4a15-ad82-69003700ff6c/federationmetadata/2007-06/federationmetadata.xml?appid=f1093ad8-f4c5-4090-a5f3-6d316e699c94";
          options.TimeComparisonTolerance = 120;
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}