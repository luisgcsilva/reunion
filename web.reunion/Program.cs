using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using web.reunion.Interfaces;
using web.reunion.Models;
using web.reunion.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IRoleService, RoleService>(client =>
{
    client.BaseAddress = new Uri("http://REPLACEWITHIP/api.reunion/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("apiClient", client =>
{
    client.BaseAddress = new Uri("http://REPLACEWITHIP/api.reunion/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddTransient<ISenderEmail, SmtpEmailSender>();

builder.Services.AddScoped<IMarcacaoService, MarcacaoService>();
builder.Services.AddScoped<ISalaService, SalaService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ILocalService, LocalService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAdminGroupService, AdminGroupService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
});

builder.Services.AddAuthorization(options =>
{

});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var serviceProvider = app.Services;
await ConfigureAuthorizationPolicies(serviceProvider);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task ConfigureAuthorizationPolicies(IServiceProvider services)
{
    var roleService = services.GetRequiredService<IRoleService>();
    var roles = await roleService.GetRolesAsync();

    var authorizationOptions = services.GetRequiredService<IOptions<AuthorizationOptions>>().Value;
    authorizationOptions.AddPolicy("Admins", policy =>
    {
        policy.RequireRole(roles.ToArray());
    });
}