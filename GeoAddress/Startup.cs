using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


[assembly: OwinStartup(typeof(GeoAddress.Startup))]

namespace GeoAddress
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}
