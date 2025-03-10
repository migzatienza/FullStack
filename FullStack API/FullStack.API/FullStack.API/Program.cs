using FullStack.API.DAO;
using FullStack.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
    ContentRootPath = System.IO.Directory.GetCurrentDirectory(),
    WebRootPath = "wwwroot",
    EnvironmentName = Environments.Development // Change to your environment
});

builder.WebHost.UseUrls("http://*:5000", "https://*:5001", "https://*:44345"); // Listen on both HTTP and HTTPS

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<TokenService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// For BearerToken
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set to true in production
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidIssuer = jwtSettings["Issuer"],
            ClockSkew = TimeSpan.Zero
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<FullStackDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("FullStackConnectionString"));
// });
builder.Services.AddScoped<EmployeeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS with the specified policy
//app.UseCors("AllowSpecificOrigin");
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
