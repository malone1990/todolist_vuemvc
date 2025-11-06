
using Microsoft.EntityFrameworkCore;
using DotVue.Models;

namespace DotVue.Data;

/// <summary>
/// EF Core数据库上下文
/// 负责与SQLite数据库交互
/// </summary>
public class TodoDbContext : DbContext
{
    // 构造函数注入配置
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    // TodoItem表的映射
    public DbSet<TodoItem> Todos => Set<TodoItem>();
}