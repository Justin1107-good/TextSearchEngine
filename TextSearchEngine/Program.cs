
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

#region ��Ȩ���ܷ���ҳ��
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
#region ʹ��cookie���м�Ȩ


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(co => co.LoginPath = "/Account/Login");

#endregion
//������Ȩ���ԣ���ʽ1
//builder.Services.AddAuthorization(config =>
//{ 
//    //��һ�֣�ͨ���򵥷���������Ȩ����
//    config.AddPolicy("essayPolicy", p => p.RequireClaim(ClaimTypes.NameIdentifier, "8")); 
//});

//������Ȩ���ԣ���ʽ2
//builder.Services.AddAuthorization(config =>
//{ 
//    //�ڶ��֣���Ȩ����Ȩ�İ󶨣�Cookies��Ȩ���� �� ��Ȩ���Խ��а�
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
        Title = "�ӿ��ĵ�",
        Version = "v1",
        Description = $"HTTP API ",
        Contact = new OpenApiContact { Name = "����", Email = "wj11074@outlook.com", Url = new Uri("https://justin1107-good.github.io/dotnetResource/") },
        License = new OpenApiLicense { Name = "����" },
        TermsOfService = new Uri("https://github.com/IzyPro/WatchDog")
    });
    //   c.IncludeXmlComments(AppContext.BaseDirectory + "EFCore_SearchEngineDemo.xml");
});

builder.Services.AddDbContext<DataContext>(db =>
{
    db.UseSqlite("Data Source=jwText.db");
});// �������ݿ�������
builder.Services.AddSearchEngine<DataContext>(new LuceneIndexerOptions()
{
    Path = "lucene"
});// ����ע���������棬������������·��

#region  .NET ��ع���WatchDog ��һ��ʹ�� C# �����Ŀ�Դ��������ع��ߣ������Լ�¼�Ͳ鿴 ASP.Net Core Web �� WebApi ��ʵʱ��Ϣ���¼����쳣�� Http ������Ӧ�ȡ�https://github.com/IzyPro/WatchDog

builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = true; //�����Զ������־
    {
        //�ⲿ����ʱ��Щ����
        //��־��¼���ⲿ���ݿ�mysql
        //opt.SetExternalDbConnString = "server=localhost;user id=root;password=123456;database=WatchDog_log;SslMode=none;Charset=utf8;";
        //opt.SqlDriverOption = WatchDogSqlDriverEnum.PostgreSql;
    }


});
{
    ///ע�⵱��Ĭ�ϼƻ�ʱ�䡱����Ϊ��ÿ�ܡ�ʱ���븲���������ã�IsAutoClear = true
    ///
    //builder.Services.AddWatchDogServices(opt =>
    //{
    //    opt.IsAutoClear = true;
    //    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
    //});

}
#endregion
#region markdown ����Ԥ������Ҫ��Westwind.AspNetCore.Markdown��
//ע����� 
{
    builder.Services.AddMarkdown(x =>
    {
        x.AddMarkdownProcessingFolder("/posts/", "~/Pages/__MarkdownPageTemplate.cshtml");
       
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//ע��������ĵ�֧��
        //System.IO.File.ReadAllText("../Files/posts/", Encoding.GetEncoding("gb2312"));
        
    });
    builder.Services.AddMvc()
        .AddApplicationPart(typeof(MarkdownPageProcessorMiddleware).Assembly)
        ;

     
}
#endregion
//md�ļ���������
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

#region ���þ�̬��Դ����
//����Ŀ¼
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
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());//�����������ǲ���Ҫ��
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // ����·��
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); // Ĭ��·��

});
app.UseWatchDogExceptionLogger();
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "admin";

});
 
//�м���ܵ�MarkDown
app.UseMarkdown();
app.UseStaticFiles();
app.UseMvc();

app.MapRazorPages();

app.Run();
