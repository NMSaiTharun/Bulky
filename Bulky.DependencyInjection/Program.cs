using BulkyBook.DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//New Service-every time request
builder.Services.AddTransient<ITransientGuidService,TransientGuidService>();

//New Service-once per request
builder.Services.AddScoped<IScopedGuidService, ScopedGuidService>();

//New Service-once per application lifetime
builder.Services.AddSingleton<ISingletonGuidService, SingletonGuidService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
