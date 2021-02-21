using DAL.Redis.DBFactory;
using DAL.Redis.Implementations;

using Logic.DALAbstractions;
using Logic.MessageProcessors;
using Logic.Options;
using Logic.PrintAbstractions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.IO;
using System.Reflection;

using TimePrinter.HostedServices;
using TimePrinter.Printers;

using Options = Logic.Options;

namespace TimePrinter
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
            ConfigureCustomServices(services);

            services.AddHostedService<MessagePrinterHostedService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimePrinter", Version = "v1" });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            ConfigureOptions(services);
            ConfigureDAL(services);
            ConfigurePrinters(services);
            ConfigureMessageProcessors(services);
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            RedisOptions redisOpts = Configuration.GetSection("Redis").Get<RedisOptions>();
            services.AddSingleton(redisOpts);

            Options.HostOptions hostOpts = Configuration.GetSection("Host").Get<Options.HostOptions>();
            services.AddSingleton(hostOpts);

            PrintJobOptions printJobOpts = Configuration.GetSection("PrintJob").Get<PrintJobOptions>();
            services.AddSingleton(printJobOpts);
        }

        private static void ConfigureDAL(IServiceCollection services)
        {
            services.AddSingleton<RedisEntitiesNames>()
                .AddSingleton<RedisDBFactory>()
                .AddSingleton<IMessageCleaner, MessageCleaner>()
                .AddSingleton<IMessageReader, MessageReader>()
                .AddSingleton<IMessageWriter, MessageWriter>();
        }

        private static void ConfigurePrinters(IServiceCollection services)
        {
            services.AddSingleton<IMessagePrinter, MessagePrinter>();
        }

        private static void ConfigureMessageProcessors(IServiceCollection services)
        {
            services.AddSingleton<MessageSaver>()
                .AddSingleton<PrintJob>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimePrinter v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
