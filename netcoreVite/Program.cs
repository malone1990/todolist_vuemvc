using DotVue.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. 注册MVC服务（包含Views和API控制器）
builder.Services.AddControllersWithViews(options => {
        // 提高优先级（按 Order 顺序执行）
        options.Filters.Add<AutoValidateModelStateFilter>(order: int.MinValue + 100); //针对全局做Filter ModelState error，灵活的需要在ControllerhuoAction设置[ServiceFilter(typeof(ModelStateValidationFilter))]
    })
    .AddJsonOptions(options =>
    {
        // 配置JSON序列化选项，启用详细错误信息
        options.JsonSerializerOptions.WriteIndented = true;
        // options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter()); // 针对全局所有的时间字段做转换
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// 2. 注册Swagger（API文档，便于调试）
// builder.Services.AddSwaggerGen();

// 3. 注册数据库上下文（使用SQLite）
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoDb")));

var app = builder.Build();

// 全局异常处理中间件
// 能捕获的异常：
// 路由中间件（UseRouting）及后续中间件（如 UseEndpoints、MVC 管道）抛出的异常。
// 静态文件中间件（UseStaticFiles）、认证中间件（UseAuthentication）等在它之后注册的中间件异常。
// 优势：覆盖范围最广，能捕获 MVC 管道内外的绝大多数异常（如路由匹配失败、Action 执行异常等）。
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (errorFeature != null)
        {
            // 在控制台输出详细的错误信息
            // 将异常信息记录到控制台
            // 输出请求体内容（如果有）
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body);
                var bodyContent = await reader.ReadToEndAsync();
                Console.WriteLine($"请求体内容: {bodyContent}");
                context.Request.Body.Position = 0;
            }
            // 返回标准化的错误响应
            await context.Response.WriteAsync($"{{\"error\": \"Internal Server Error\", \"message\": \"{errorFeature.Error.Message}\"}}");
        }
    });
});

// 迁移数据库（确保表存在）
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    dbContext.Database.EnsureCreated(); // 自动创建数据库和表
}
// 5. 启用静态文件服务（允许访问wwwroot中的Vue编译文件）
app.UseStaticFiles();

// 6. 启用路由
app.UseRouting();

// 7. 配置MVC路由规则
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
