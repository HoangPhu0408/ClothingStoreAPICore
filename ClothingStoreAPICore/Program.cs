using ClothingStoreAPICore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
/*builder.Services.AddDbContext<ClothingStoreContext>
    (options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("ClothingStore")));*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())); //set cứng
*/
builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("default", (option) =>
    {
        option.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});// thử
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder => builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});*/
builder.Services.AddDbContext<ClothingStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClothingStore"));
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("default");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
