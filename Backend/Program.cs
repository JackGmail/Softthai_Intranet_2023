using Backend.EF.ST_Intranet;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Back.Permission;
using Backend.Interfaces.Back.Permission.Menu;
using Backend.Interfaces.Permission.GroupUser;
using Backend.Interfaces.Permission.UserRole;
using Backend.Service;
using Backend.Services;
using Backend.Services.Back.Permission.GroupUser;
using Backend.Services.ISystemService;
using Backend.Services.Permission;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Backend.Service.UploadFile;
using Backend.Service.UploadFileSharePath;
using ST_API.Interfaces;
using System.Globalization;
using System.Text;
using Backend.Service.TypeleaveService;
using ST_API.Services.ISystemService;
using Backend.Services.ApproveLineService;
using Backend.Interfaces.Front.Project;
using ST_API.Service;
using Backend.Service.HomeService;
using Backend.Controllers;


#region builder
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

#region Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
#endregion

#region Database
builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<ST_IntranetEntity>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:STConnectionStrings"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                });
#endregion

#region Swagger
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
    x.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
     {

         {
            new OpenApiSecurityScheme
              {
                 Reference = new OpenApiReference
                 {
                   Type = ReferenceType.SecurityScheme,
                   Id = "bearerAuth"
                 }
            },new string[] {}
         }
     });
    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
#endregion

#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = Configuration["jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = Configuration["jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, // disable delay when token is expire delay timeout
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:SecretKey"]))
    };
});
#endregion

#region  UploadFile unlimit Size
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});
#endregion

#region  Json Options
builder.Services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
#endregion

#region Policy
builder.Services.AddCors(options => options.AddPolicy("corsPolicy", builder =>
{
    builder.WithOrigins("http://localhost:5000", "https://localhost:5000", "http://localhost:7239", "https://localhost:7239", "https://localhost:7214",
    "https://softthaiapp.com/intranetTest", "https://softthaiapp.com/IntranetTest_API",
    "https://softthaiapp.com/intranet", "https://softthaiapp.com/Intranet_API"
    )
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
}));
#endregion

#region Add interface Service Controller To Model
builder.Services.AddScoped<IAuthentication, AuthenticationSevice>();
builder.Services.AddScoped<INavigation, NavigationService>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IUploadToSharePathSevice, UploadToSharePathSevice>();
builder.Services.AddScoped<ITestToolsService, TestToolsService>();
builder.Services.AddScoped<IUserPermission, UserPermissionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IGroupUserService, GroupUserService>();
builder.Services.AddScoped<IMeetingRoomService, MeetingRoomService>();
builder.Services.AddScoped<IWFHService, WFHService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOTService, OTService>();
builder.Services.AddScoped<IWelfareService, WelfareService>();
builder.Services.AddScoped<IAllowanceService, AllowanceService>();
builder.Services.AddScoped<ITravelService, TravelService>();
builder.Services.AddScoped<IDailyTaskService, DailyTaskService>();
builder.Services.AddScoped<ITypeleaveService, TypeleaveService>();
builder.Services.AddScoped<ILineService, LineService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmailRequestService, EmailRequestService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IMasterProcessService, MasterProcessService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<ILeaveRightsService, LeaveRightsService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IWorkScheduleService, WorkScheduleService>();
builder.Services.AddScoped<IAdminBannerSevice, AdminBannerService>();
builder.Services.AddScoped<ITasksService, TasksService>();
#endregion

#region Memorybuilder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
#endregion

#region  Json Options
builder.Services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
builder.Services.AddSignalR().AddJsonProtocol(option =>
{
    option.PayloadSerializerOptions.WriteIndented = false;
});
#endregion

#endregion


#region app setting
var app = builder.Build();

#region Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Add("Expires", "0");
    await next(context);
});
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

app.UseHttpsRedirection();

var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString(); // For Cropper Image
var sOrigin = "https://localhost:7239.com"; // For Cropper Image
app.UseStaticFiles(new StaticFileOptions()
{
    //ServeUnknownFileTypes = true,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
             "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
    },
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),

});
app.UseRouting();
app.UseCors("corsPolicy");

// global error handler 
//app.UseMiddleware<LoggerManagerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(x => x.MapControllers());

app.MapControllerRoute(
 name: "default",
 pattern: "{controller}/{action=Index}/{id?}");

app.Run();
#endregion