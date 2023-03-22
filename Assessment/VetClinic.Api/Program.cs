using VetClinic.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

Utilities.AddServices(builder);// Add services to the container.

var app = builder.Build();
Utilities.ConfigureRequestPipeline(app); // Configure the HTTP request pipeline.

Utilities.SeedDatabase(app); //Seed initial database

//// Add services to the container.
//Utilities.AddServices(builder);// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();



