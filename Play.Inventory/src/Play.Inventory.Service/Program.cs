using Play.Common.MongoDb;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Microsoft.Extensions.Logging;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var serviceProvider = builder.Services.BuildServiceProvider();

builder.Services.AddMongo()
                .AddMongoRepository<InventoryItem>("inventoryitems");

builder.Services.AddHttpClient<CatalogClient>(client => {
    client.BaseAddress = new Uri("https://localhost:7266");
})
// .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(  
//     5,
//     retryAttemt => TimeSpan.FromSeconds(Math.Pow(2, retryAttemt)),
//     onRetry: (outcome, timespan, retryAttemt) =>
//     {
//         serviceProvider.GetServices<ILogger<CatalogClient>>()?
//             .LogWarning($"Delay for {timespan.TotalSeconds} seconds, then making retry {retryAttemt}");
//     }
// ))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
