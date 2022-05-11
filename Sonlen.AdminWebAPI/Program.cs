using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sonlen.AdminWebAPI.Data;
using Sonlen.AdminWebAPI.Extensions;
using Sonlen.AdminWebAPI.LogSetting;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;
using System.Text;

#region ConfigureServices

var builder = WebApplication.CreateBuilder(args);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
                builder.WithOrigins("https://localhost:44341",
                            "http://localhost:28134",
                            "https://localhost:7126",
                            "http://localhost:5126")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
            });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ILeaveRecordService, LeaveRecordService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<INoticeService, NoticeService>();
builder.Services.AddScoped<IPunchService, PunchService>();
builder.Services.AddScoped<IPunchRecordService, PunchRecordService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<IWorkOvertimeService, WorkOvertimeService>();
builder.Services.AddScoped<IWorkOvertimeRecordService, WorkOvertimeRecordService>();

#endregion


#region ConfigureApp

var app = builder.Build();

// api¨Ï¥Î°O¿ý
new LoggerFactory().AddLog4Net("log4Net.xml");
app.UseMiddleware<WebApiTrackerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});
app.ConfigureExceptionHandler(app.Logger);
app.Run();

#endregion

