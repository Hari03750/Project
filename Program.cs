using EventEaseApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server services.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// App services (registered as Scoped so state is per user "session/circuit").
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddSingleton<EventService>();      // shared event catalog across all users
builder.Services.AddScoped<AttendanceService>();    // per-user attendance interactions

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // Friendly error page in production instead of raw stack traces.
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
