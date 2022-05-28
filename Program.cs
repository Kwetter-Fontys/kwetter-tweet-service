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
    o.Authority = "https://keycloak.sebananasprod.nl/auth/realms/Kwetter";
    o.Audience = "account";
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


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<TweetContext>(options =>
    options.UseMySQL("server=localhost;port=3307;database=tweets;user=root;password=CEzmBKLB8?f5s!G7"),
            ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);
}
else
{
    builder.Services.AddDbContext<TweetContext>(options =>
    options.UseMySQL("server=38.242.248.109;port=3307;database=tweets;user=root;password=CEzmBKLB8?f5s!G7"),
            ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);
}
//Inject repo



builder.Services.AddHostedService<MessageReceiver>();
builder.Services.AddTransient<ITweetRepository, TweetRepository>();
builder.Services.AddTransient<ITweetService, TweetServiceClass>();
builder.Services.AddControllers();


//Initalize message broker as background service


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



app.Run();


public partial class Program { }