using Microsoft.EntityFrameworkCore;
using ToDoApp.Services.Implementations;
using ToDoService.DAL.DbContexts;
using ToDoService.DAL.Repositories.Implementations;
using ToDoService.DAL.Repositories.Interfaces;
using ToDoService.NewFolder;
using ToDoService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer("name=DefaultConnection"));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ITaskServices, TaskServices>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Add services to the container.

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
app.UseMyMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
