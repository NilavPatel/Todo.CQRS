using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Todo.Application.CommandHanders;
using Todo.Application.EventHanders;
using Todo.Application.ReadModels;
using Todo.Contracts.Commands;
using Todo.Contracts.Events;
using Framework.Aggregate;
using Framework.Command;
using Framework.CommandBus;
using Framework.Event;
using Framework.EventStore;
using Framework.Repository;

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

            RegisterDatabases(services);
            RegisterRequiredServices(services);
            RegisterCommandHandlers(services);
            RegisterEventHandlers(services);
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

        public void RegisterDatabases(IServiceCollection services)
        {
            services.AddDbContext<EventStoreContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-11HQKNS\SQLExpress;Initial Catalog=EventStore;User Id=sa;Password=satest12@;"));
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-11HQKNS\SQLExpress;Initial Catalog=Todo;User Id=sa;Password=satest12@;"));
        }

        private void RegisterRequiredServices(IServiceCollection services)
        {
            services.AddScoped<ICommandBus, DefaultCommandBus>();
            services.AddScoped<IEventBus, DefaultEventBus>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        }

        private void RegisterCommandHandlers(IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<CreateTodoItem>, TodoItemCommandHandler>();
            services.AddScoped<ICommandHandler<MarkTodoItemAsComplete>, TodoItemCommandHandler>();
            services.AddScoped<ICommandHandler<MarkTodoItemAsUnComplete>, TodoItemCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoItemTitle>, TodoItemCommandHandler>();
        }

        private void RegisterEventHandlers(IServiceCollection services)
        {
            services.AddScoped<IEventHandler<TodoItemCreated>, TodoItemEventHandler>();
            services.AddScoped<IEventHandler<TodoItemMarkedAsComplete>, TodoItemEventHandler>();
            services.AddScoped<IEventHandler<TodoItemMarkedAsUnComplete>, TodoItemEventHandler>();
            services.AddScoped<IEventHandler<TodoItemTitleUpdated>, TodoItemEventHandler>();
        }
    }
}
