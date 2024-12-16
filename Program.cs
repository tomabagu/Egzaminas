
using Egzaminas.Interfaces;
using Egzaminas.Mappers;
using Egzaminas.Repository;
using Egzaminas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Egzaminas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<ILoginService, LoginService>();
            builder.Services.AddTransient<IPersonMapper, PersonMapper>();
            builder.Services.AddTransient<IAddressMapper, AddressMapper>();

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Account management", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var configuration = builder.Configuration;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sfdhsdfh5hsfdr8thbcs5h5h5xdg5dstg5sxc2v1zsdrf49w8sae7t6zd1b3srt4yuh6f1nb32xcv1h68a47er6t51asd31sdf65h4rt6h1dfhb35"))
                    };
                });

            //Add DbContext DefaultConnection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AccountsDbContext>(opt => opt.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); //This middleware automatically redirects HTTP requests to HTTPS, ensuring that all communications between the client and server are encrypted and secure.

            app.UseCors(builder => //This method is called to add the CORS middleware to the application's request processing pipeline. 
            {
                builder
                .AllowAnyOrigin() //This method call configures the CORS policy to allow requests from any origin. In a production environment, it's generally recommended to be more specific about which origins are allowed to ensure the security of your web application.
                .AllowAnyMethod() //This allows the CORS policy to accept requests made with any HTTP method (such as GET, POST, PUT, DELETE, etc.). This is useful for a RESTful API that needs to support a wide range of actions on resources.
                .AllowAnyHeader(); //his configures the CORS policy to allow any headers in the requests. Headers are often used in requests to carry information about the content type, authentication, etc. Allowing any header supports a wide range of requests that might include custom or standard headers.
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
