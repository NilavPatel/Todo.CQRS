using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Framework.Registrar;
using Framework.BackgroundProcessor;
using Todo.Application.ReadModels;

namespace Todo.BackgroundProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Background App start");

            var serviceProvider = ConfigureServices(args);
            var backgroundProcessor = serviceProvider.GetService<IBackgroundEventProcessor>();
            backgroundProcessor.Start("Todo");

            System.Console.WriteLine("Press enter to stop");

            System.Console.ReadLine();

            System.Console.WriteLine("Background App end");
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
            services.AddEventStore(Configuration);
            services.RegisterIntegrationEventHandlers("Todo.Application");

            services.AddDbContext<TodoContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-11HQKNS\SQLExpress;Initial Catalog=Todo;User Id=sa;Password=satest12@;"));

            return services.BuildServiceProvider();
        }
    }
}
