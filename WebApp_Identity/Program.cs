using Microsoft.AspNetCore.Authorization;
using WebApp_Identity.Authorization;
using WebApp_Identity.Common;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(AppConstant.AUTHENTICATION_SCHEME)
    .AddCookie(AppConstant.AUTHENTICATION_SCHEME, options =>
    {
        options.Cookie.Name = AppConstant.AUTHENTICATION_SCHEME;
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBelongToHRDepartment",
        policy => policy.RequireClaim("department", "HR"));

    options.AddPolicy("AdminOnly",
        policy => policy.RequireClaim("admin"));

    options.AddPolicy("HRManagerOnly", policy => policy
        .RequireClaim("department", "HR")
        .RequireClaim("manager")
        .Requirements.Add(new HRManagerProbationRequirement(3)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
