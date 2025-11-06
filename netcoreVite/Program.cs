using DotVue.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. 注册MVC服务（包含Views和API控制器）
builder.Services.AddControllersWithViews();

// 2. 注册Swagger（API文档，便于调试）
// builder.Services.AddSwaggerGen();

// 3. 注册数据库上下文（使用SQLite）
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoDb")));

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     // app.UseExceptionHandler("/Home/Error");
//     // // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     // app.UseHsts();
//     app.UseSwagger();
//     app.UseSwaggerUI(); // 启用Swagger界面
// }
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
