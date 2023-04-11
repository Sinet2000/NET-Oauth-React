using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add this line to read the appsettings.json configuration
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.RegisterApplicationServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            // Set the Authority to the base URL of your authorization server or identity provider (IdP).
            // This value is used to discover the OpenID Connect configuration and validate tokens.
            options.Authority = configuration["Authentication:Authority"];

            // Set the Audience to the intended recipient of the access tokens issued by the authorization server.
            // This value is used by the resource server to ensure that the access tokens are meant for the correct API.
            options.Audience = configuration["Authentication:Audience"];

            // Configure the token validation parameters to ensure the security of your application.
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Validate the issuer (iss) claim of the access tokens.
                // This ensures that the tokens were issued by the expected authorization server.
                ValidateIssuer = true,

                // Validate the audience (aud) claim of the access tokens.
                // This ensures that the tokens are meant for the correct API.
                ValidateAudience = true,

                // Validate the lifetime (exp) claim of the access tokens.
                // This ensures that the tokens have not expired.
                ValidateLifetime = true,

                // Validate the signature of the access tokens.
                // This ensures that the tokens were signed by the expected authorization server and have not been tampered with.
                ValidateIssuerSigningKey = true,

                // If you need to validate the issuer, provide a valid list of issuers.
                // ValidIssuers = new[] { "https://your-valid-issuer1.com", "https://your-valid-issuer2.com" },

                // If the authorization server uses a custom signing key, you can set it here.
                // This ensures that the access tokens are signed with the expected key.
                // IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("your-custom-signing-key")),

                // Validate the token's signature algorithm against a list of allowed algorithms.
                // This ensures that the tokens are signed with a secure and supported algorithm.
                ValidateAlgorithm = true,
                ValidAlgorithms = new[] { SecurityAlgorithms.RsaSha256 },

                // Validate the token's 'nonce' claim to prevent replay attacks.
                // This requires sending a nonce value in the authorization request and storing it server-side.
                RequireNonce = true,

                // Set the clock skew to a lower value to minimize the time difference between the authorization server and the resource server.
                ClockSkew = TimeSpan.FromSeconds(30),
            };

            // Enable HTTPS metadata retrieval for the OpenID Connect configuration.
            // This ensures that the metadata is retrieved securely and prevents man-in-the-middle attacks.
            options.RequireHttpsMetadata = true;

            // Configure the event handlers to handle token validation failures and other authentication-related events.
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    // Log the exception and return a 401 Unauthorized response.
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    // Log the challenge and return a 401 Unauthorized response.
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
