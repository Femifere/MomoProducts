using Microsoft.EntityFrameworkCore;
using MomoProducts.Server.Interfaces.AuthData;
using MomoProducts.Server.Interfaces.Collections;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Interfaces.Remittance;
using MomoProducts.Server.Repositories.AuthData;
using MomoProducts.Server.Repositories.Collections;
using MomoProducts.Server.Repositories.Common;
using MomoProducts.Server.Repositories.Disbursements;
using MomoProducts.Server.Repositories.Remittance;
using MomoProducts.Server.Models;
using MomoProducts.Server.Interfaces.Disbursements;
using MomoProducts.Server.Services; // Add this line for AutoMapper

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories for dependency injection
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<IApiUserRepository, ApiUserRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPreApprovalRepository, PreApprovalRepository>();
builder.Services.AddScoped<IRequestToPayRepository, RequestToPayRepository>();
builder.Services.AddScoped<IRequestToWithdrawRepository, RequestToWithdrawRepository>();
builder.Services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
builder.Services.AddScoped<IMoneyRepository, MoneyRepository>();
builder.Services.AddScoped<IOauth2TokenRepository, Oauth2TokenRepository>();
builder.Services.AddScoped<IPayeeRepository, PayeeRepository>();
builder.Services.AddScoped<IPayerRepository, PayerRepository>();
builder.Services.AddScoped<IDepositRepository, DepositRepository>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<ICashTransferRepository, CashTransferRepository>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add controllers
builder.Services.AddControllers();

builder.Services.AddHttpClient();

// Add CORS policy to allow the React app to communicate with the backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Adjust the origin as needed
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the application
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MomoProducts API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
