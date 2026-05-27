using Microsoft.AspNetCore.Mvc;
using EArenaTournamentManager.Web.Middleware;
using EArenaTournamentManager.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("EArenaAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5086");
});

builder.Services.AddSession();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<TournamentService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<TeamMemberService>();
builder.Services.AddScoped<OrganizationStaffService>();
builder.Services.AddScoped<TournamentParticipantService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?code={0}");

app.UseSession();

app.UseMiddleware<UnauthorizedApiMiddleware>();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
