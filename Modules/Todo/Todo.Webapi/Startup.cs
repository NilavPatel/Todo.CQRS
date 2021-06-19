using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Todo.Application.ReadModels;
using Framework.Registrar;

namespace Todo.Webapi
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo.Webapi", Version = "v1" });
            });

            // Register framework service dependecies and 
            // Add event store db context and 
            // Register command handlers, event handlers
            services.RegisterFrameworkServices();
            services.AddEventStore(Configuration);
            services.RegisterCommandHandlers("Todo.Application");
            services.RegisterDomainEventHandlers("Todo.Application");

            services.AddDbContext<TodoContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-11HQKNS\SQLExpress;Initial Catalog=Todo;User Id=sa;Password=satest12@;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo.Webapi v1"));
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
