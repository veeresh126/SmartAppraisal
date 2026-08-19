using BL_SmartAppraisal.Interfaces;
using BL_SmartAppraisal.Services;
using BL_SmartAppraisal.Settings;

using DL_SmartAppraisal.Data;
using DL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Repositories;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// ADD SERVICES TO THE CONTAINER
// =====================================================

builder.Services.AddControllersWithViews();


// =====================================================
// DATABASE
// =====================================================

builder.Services.AddDbContext<SmartAppraisalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "SmartAppraisalConnection")));


// =====================================================
// EMAIL SETTINGS
// =====================================================

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(
        "EmailSettings"));


// =====================================================
// SESSION
// =====================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout =
        TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});


// =====================================================
// REPOSITORIES
// =====================================================

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    ICaseStudyRepository,
    CaseStudyRepository>();


// =====================================================
// SERVICES
// =====================================================

builder.Services.AddScoped<
    IUserService,
    UserService>();

builder.Services.AddScoped<
    IEmailService,
    EmailService>();

builder.Services.AddSingleton<
    IOtpService,
    OtpService>();

builder.Services.AddScoped<
    ICaseStudyService,
    CaseStudyService>();


// =====================================================
// SWAGGER
// =====================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// =====================================================
// BUILD APPLICATION
// =====================================================

var app = builder.Build();


// =====================================================
// HTTP REQUEST PIPELINE
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Smart Appraisal API v1");
    });
}
else
{
    app.UseExceptionHandler(
        "/Home/Error");

    app.UseHsts();
}


// =====================================================
// HTTPS
// =====================================================

app.UseHttpsRedirection();


// =====================================================
// STATIC FILES
// =====================================================

app.MapStaticAssets();


// =====================================================
// ROUTING
// =====================================================

app.UseRouting();


// =====================================================
// SESSION
// =====================================================

app.UseSession();


// =====================================================
// AUTHORIZATION
// =====================================================

app.UseAuthorization();


// =====================================================
// DEFAULT ROUTE
// =====================================================

app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Users}/{action=Index}/{id?}")
    .WithStaticAssets();


// =====================================================
// RUN
// =====================================================

app.Run();