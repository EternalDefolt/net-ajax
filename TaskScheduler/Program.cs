var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<TaskScheduler.Services.ITaskRepository, TaskScheduler.Services.JsonTaskRepository>();
builder.Services.AddSingleton<TaskScheduler.Services.IIdempotencyStore, TaskScheduler.Services.InMemoryIdempotencyStore>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<TaskScheduler.Infrastructure.GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
