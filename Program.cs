using TweetService.DAL;
using TweetService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TweetService.Services;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Logging.ClearProviders();
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Information);
var logger = LoggerFactory.Create(config =>
{
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
}).CreateLogger("Program");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                        });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = Environment.GetEnvironmentVariable("Authority");
    o.Audience = Environment.GetEnvironmentVariable("Audience");
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            if (builder.Environment.IsDevelopment())
            {
                logger.LogWarning(c.Exception.ToString());
                return c.Response.WriteAsync(c.Exception.ToString());
            }
            logger.LogWarning("Invalid JWT token or unauthorized user.");
            return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#pragma warning disable CS8604 // Possible null reference argument.
builder.Services.AddDbContext<TweetContext>(options =>
options.UseMySQL(Environment.GetEnvironmentVariable("Database")),
        ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Transient);
#pragma warning restore CS8604 // Possible null reference argument.


//Inject repo
//Initalize message broker as background service
builder.Services.AddHostedService<MessageReceiver>();
builder.Services.AddTransient<ITweetRepository, TweetRepository>();
builder.Services.AddTransient<ITweetService, TweetServiceClass>();
builder.Services.AddControllers();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers().RequireCors(MyAllowSpecificOrigins);


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<TweetContext>();

    TweetInitializer.Initialize(context);
}

app.Run();
public partial class Program { }