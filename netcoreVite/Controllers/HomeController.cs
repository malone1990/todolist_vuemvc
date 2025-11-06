using System.Diagnostics;
using DotVue.Data;
using Microsoft.AspNetCore.Mvc;
using DotVue.Models;
using Microsoft.EntityFrameworkCore;

namespace DotVue.Controllers;

/// <summary>
/// 首页控制器
/// 负责展示混合Razor+Vue的页面
/// </summary>
public class HomeController : Controller
{
    private readonly TodoDbContext _context;

    // 注入数据库上下文
    public HomeController(TodoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 首页视图（Razor+Vue混合）
    /// </summary>
    public async Task<IActionResult> Index()
    {
        // 从数据库获取初始Todo列表（传递给Razor视图）
        var todos = await _context.Todos.ToListAsync();
        return View(todos); // 传递数据到Views/Home/Index.cshtml
    }
}
