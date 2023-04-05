using VetClinic.Api.Helpers;

#region Add services to the container
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
Utilities.AddServices(builder);

var app = builder.Build();
#endregion

#region Configure the HTTP request pipeline And Seed Database
// Configure the HTTP request pipeline
Utilities.ConfigureRequestPipeline(app);

// Seed database
Utilities.SeedDatabase(app);
#endregion

app.Run();



