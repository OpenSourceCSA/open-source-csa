using ES.Domain;
using Ezley.EncyrptionKeyStore;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ProjectionStore;
using Ezley.SnapshotStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ezley.ProjectionEngine
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
            services.AddControllers(); 
            
            SetupDI(services);
            
            services.AddHostedService<ProjectionBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

          //  app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        private void SetupDI(IServiceCollection services)
        {
            string endpointUri = Configuration["Azure:EndPointUri"]; 
            string database = Configuration["Azure:Database"]; 
            string authKey = Configuration[ "Azure:AuthKey"]; 
            string eventContainer = Configuration["Azure:EventContainer"];
            string keyContainer = Configuration["Azure:KeyContainer"];
            string viewContainer = Configuration["Azure:ViewContainer"];
            string snapshotContainer = Configuration["Azure:SnapshotContainer"];
             
            services.AddTransient<IEventStore, CosmosDBEventStore>(serviceProvider =>
                new CosmosDBEventStore(
                    new EventTypeResolver(), endpointUri, authKey, database, eventContainer));
            
            services.AddTransient<IKeyStore, CosmosDBKeyStore>(serviceProvider =>
                new CosmosDBKeyStore(
                    endpointUri, authKey, database, eventContainer));

            // Setup DI
            services.AddTransient<IRepository, Repository>();
        }
    }
}