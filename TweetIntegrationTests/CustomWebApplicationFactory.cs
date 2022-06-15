using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TweetService.DAL;
using Microsoft.EntityFrameworkCore.InMemory;

namespace TweetIntegrationTests
{
    public class CustomWebApplicationFactory<Program>
        : WebApplicationFactory<Program> where Program : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("RabbitMQHost", "38.242.248.109");
            Environment.SetEnvironmentVariable("RabbitMQUsername", "guest");
            Environment.SetEnvironmentVariable("RabbitMQPassword", "pi4snc7kpg#77Q#F");
            Environment.SetEnvironmentVariable("RabbitMQQueueName", "deleteTweets");
            Environment.SetEnvironmentVariable("Audience", "account");
            Environment.SetEnvironmentVariable("Authority", "https://keycloak.sebananasprod.nl/auth/realms/Kwetter");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<TweetContext>));

                services.Remove(descriptor);

                services.AddDbContext<TweetContext>(options =>
                
                    options.UseInMemoryDatabase("InMemoryDbForTesting"),
                    ServiceLifetime.Transient,
                    optionsLifetime: ServiceLifetime.Transient);

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TweetContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<Program>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        TweetInitializer.Initialize(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test tweets. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
