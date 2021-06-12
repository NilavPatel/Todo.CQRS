using Microsoft.Extensions.DependencyInjection;
using Framework.Registrar;
using Todo.Application.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Framework.BackgroundProcessor;

namespace Todo.BackgroundProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices(args);
            var backgroundProcessor = serviceProvider.GetService<IBackgroundEventProcessor>();
            backgroundProcessor.Start();

            System.Console.ReadLine();
        }

        private static ServiceProvider ConfigureServices(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            // Register framework service dependecies and 
            // Add event store db context and 
            // Register command handlers, event handlers
            var services = new ServiceCollection();
            services.RegisterFrameworkServices();
            services.RegisterEventStore(Configuration);
            services.RegisterCommandHandlers("Todo.Application");
            services.RegisterIntegrationEventHandlers("Todo.Application");
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-11HQKNS\SQLExpress;Initial Catalog=Todo;User Id=sa;Password=satest12@;"));

            return services.BuildServiceProvider();
        }
    }
}
