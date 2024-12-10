
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

            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IAccountMapper, AccountMapper>();
            builder.Services.AddTransient<IPersonMapper, PersonMapper>();
            builder.Services.AddTransient<IAddressMapper, AddressMapper>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            { //Adds and configures Swagger generator services. SwaggerGen is a Swagger tool that generates Swagger documents for your API based on your controllers, models, and descriptions.
                opt.SwaggerDoc("v1", new OpenApiInfo //This line creates a Swagger document named "v1" with specified API information
                {
                    Version = "1.0", //The version of the API. This is useful when you have multiple versions of your API and want to generate separate Swagger documents for each.
                                       //Use Semantic Versioning: Follow Semantic Versioning (https://semver.org/) principles (MAJOR.MINOR.PATCH) to manage your API versions. This makes it clear to users how changes in versions might affect their use of your API.
                    Title = "Account registration", // Sets a human-readable title for the API documentation. This title is displayed in the Swagger UI as the name of the API.
                                                      //The title should clearly reflect the purpose of your API but remain succinct. It should be immediately clear to the developer what the API does or relates to.
                    Description = "Account registration",
                    //Provides a longer description of what the API does. In this case, it indicates that the API is for managing ToDo items, which helps developers understand the API's purpose at a glance.
                    //While the title is concise, the description allows for more detailed information about the API's purpose, its target audience, and key features or use cases.
                });
                var securitySchema = new OpenApiSecurityScheme //This object defines a security scheme that describes how the API is secured. In this case, it's configured for JWT authentication using the Bearer token scheme.
                {
                    Description = "JWT Authorization header is using Bearer scheme. \r\n\r\n" +
                               "Enter token. \r\n\r\n" +
                               "Example: \"d5f41g85d1f52a\"", // Provides information on how to use the security scheme, explaining that the JWT token should be provided in the Authorization header using the Bearer scheme, and gives an example token format.
                    Name = "Authorization", //The name of the header where the token should be supplied, which is "Authorization".
                    In = ParameterLocation.Header, // Specifies where the security scheme applies, in this case, in the Header of the request.
                    Type = SecuritySchemeType.Http, //The type of the security scheme, set to SecuritySchemeType.Http, indicating HTTP Authentication.
                    Scheme = "bearer", // Specifies the scheme used, in this case, "bearer", indicating Bearer Token authentication.
                    BearerFormat = "JWT", //Indicates the format of the token, here specified as "JWT" to denote that the bearer token is a JWT.
                    Reference = new OpenApiReference // Provides a reference object that allows the security scheme to be referenced within the Swagger document. It's identified by "Bearer".
                    {
                        Type = ReferenceType.SecurityScheme, //Specifies the type of the reference, which is a SecurityScheme.
                        Id = "Bearer", //Don't forget to set the same name "Bearer" as in AddSecurityDefinition
                    }
                };
                opt.AddSecurityDefinition("Bearer", securitySchema); //This method call adds the defined security scheme to the Swagger document, making it known that the API uses Bearer Token authentication.
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } }); //This adds a security requirement to the API documentation, specifying that the defined security scheme must be used. It effectively requires that the JWT token be provided for accessing the API, making it clear in the documentation that authentication is required.

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //is called to include XML comments in the Swagger UI. This makes the API documentation richer and more informative. The XML comments are generated by the compiler and contain descriptions of the API's endpoints, parameters, responses, etc.
                                                                                       //Don't forget to set xml tag <GenerateDocumentationFile> to true in .csproj file, or check the checkbox in project properties
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); //  This line creates the full path to the XML file. It combines the base directory of the app's context (where the application is running) with the filename of the XML documentation. This is where the XML documentation file is expected to be located at runtime.
                opt.IncludeXmlComments(xmlPath); //This instructs Swagger to include the XML comments found at xmlPath in the generated Swagger documentation. These XML comments come from the triple-slash comments you write above your action methods and parameters in your controllers, which are compiled into the XML file by enabling XML documentation file generation in your project settings.
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

            app.UseHttpsRedirection();

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
