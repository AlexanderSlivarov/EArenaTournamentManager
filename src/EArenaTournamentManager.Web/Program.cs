using EArenaTournamentManager.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
