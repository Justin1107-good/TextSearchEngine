
using Lucene.Net.Analysis.Bg;
using Masuit.LuceneEFCore.SearchEngine;
using Masuit.LuceneEFCore.SearchEngine.Extensions;
using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using Microsoft.OpenApi.Models;
using NuGet.Configuration;
using OpenAiClient4ChatGPT;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TextSearchEngine;
using WatchDog;
using WatchDog.src.Enums;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IOpenAiServices, OpenAiServices>();

#region 授权才能访问页面
builder.Services.AddMvcCore()
   .AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Account/index");
    options.Conventions.AuthorizePage("/Account/Add");
    options.Conventions.AuthorizePage("/Account/Update");
    options.Conventions.AllowAnonymousToPage("/Essays/Index");
    options.Conventions.AllowAnonymousToFolder("/Essays/About");
})
     .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
#endregion
#region 使用cookie进行鉴权


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(co => co.LoginPath = "/Account/Login");

#endregion
//增加授权策略：方式1
//builder.Services.AddAuthorization(config =>
//{ 
//    //第一种：通过简单方法增加授权策略
//    config.AddPolicy("essayPolicy", p => p.RequireClaim(ClaimTypes.NameIdentifier, "8")); 
//});

//增加授权策略：方式2
//builder.Services.AddAuthorization(config =>
//{ 
//    //第二种：鉴权和授权的绑定，Cookies鉴权方案 与 授权策略进行绑定
//    config.AddPolicy("ctmPolicy", x => x.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
//    .Requirements
//    .Add(new CtmAuthorizationHandler("8")));
//});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<HttpClient>();

builder.Services.AddScoped<IdentityUser>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "接口文档",
        Version = "v1",
        Description = $"HTTP API ",
        Contact = new OpenApiContact { Name = "晋王", Email = "wj11074@outlook.com", Url = new Uri("https://justin1107-good.github.io/dotnetResource/") },
        License = new OpenApiLicense { Name = "晋王" },
        TermsOfService = new Uri("https://github.com/IzyPro/WatchDog")
    });
    //   c.IncludeXmlComments(AppContext.BaseDirectory + "EFCore_SearchEngineDemo.xml");
});

builder.Services.AddDbContext<DataContext>(db =>
{
    db.UseSqlite("Data Source=jwText.db");
});// 配置数据库上下文
builder.Services.AddSearchEngine<DataContext>(new LuceneIndexerOptions()
{
    Path = "lucene"
});// 依赖注入搜索引擎，并配置索引库路径

#region  .NET 监控工具WatchDog 是一个使用 C# 开发的开源的轻量监控工具，它可以记录和查看 ASP.Net Core Web 和 WebApi 的实时消息、事件、异常、 Http 请求响应等。https://github.com/IzyPro/WatchDog

builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = true; //设置自动清除日志
    {
        //这部分暂时有些问题
        //日志记录到外部数据库mysql
        //opt.SetExternalDbConnString = "server=localhost;user id=root;password=123456;database=WatchDog_log;SslMode=none;Charset=utf8;";
        //opt.SqlDriverOption = WatchDogSqlDriverEnum.PostgreSql;
    }


});
{
    ///注意当“默认计划时间”设置为“每周”时，请覆盖如下设置：IsAutoClear = true
    ///
    //builder.Services.AddWatchDogServices(opt =>
    //{
    //    opt.IsAutoClear = true;
    //    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
    //});

}
#endregion
#region markdown 在线预览【需要包Westwind.AspNetCore.Markdown】
//注入组件 
{
    builder.Services.AddMarkdown(x =>
    {
        x.AddMarkdownProcessingFolder("/posts/", "~/Pages/__MarkdownPageTemplate.cshtml");
       
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//注册简体中文的支持
        //System.IO.File.ReadAllText("../Files/posts/", Encoding.GetEncoding("gb2312"));
        
    });
    builder.Services.AddMvc()
        .AddApplicationPart(typeof(MarkdownPageProcessorMiddleware).Assembly)
        ;

     
}
#endregion
//md文件乱码问题
//{ 
//  builder.Services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
//}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

#region 启用静态资源访问
//创建目录
var path = Path.Combine(app.Environment.WebRootPath, "Files/");
CommonFun.CreateDir(path);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = "/Files"
});
#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TextSearchEngine v1"));

app.UseAuthentication();
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());//这里它或许是不需要的
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // 属性路由
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); // 默认路由

});
app.UseWatchDogExceptionLogger();
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "admin";

});
 
//中间件管道MarkDown
app.UseMarkdown();
app.UseStaticFiles();
app.UseMvc();

app.MapRazorPages();

app.Run();
