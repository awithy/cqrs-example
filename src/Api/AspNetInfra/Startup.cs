﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Common;
using Api.Modules.EventBus;
using Api.Modules.EventStore;
using Api.Modules.Reservations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.AspNetInfra
{
    internal class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x =>
            {
                x.Conventions.Add(new FeatureConvention());
            });
            var config = new Configuration();
            services.AddSingleton<Configuration>(config);

            CommonModule.Register(services);
            EventBusModule.Regiser(services);
            EventStoreModule.Regiser(services);
            ReservationsModule.Register(services);
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<IInitializable> initializables)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            Task.WaitAll(initializables.Select(x => x.Initialize()).ToArray());
        }
    }
}