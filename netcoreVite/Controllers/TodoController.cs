using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotVue.Data;
using DotVue.Models;

namespace DotVue.Controllers;

/// <summary>
/// Todo API控制器
/// 提供CRUD接口，供Vue组件调用
/// </summary>
[ApiController]
[Route("api/todos")] // 路由：api/todos
public class TodoController : ControllerBase
{
    private readonly TodoDbContext _context;

    public TodoController(TodoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取所有Todo任务
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        return await _context.Todos.ToListAsync();
    }

    /// <summary>
    /// 创建新任务
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        // 返回201 Created状态，包含新资源的URL
        return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
    }

    /// <summary>
    /// 更新任务状态
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(int id, TodoItem todo)
    {
        if (id != todo.Id)
        {
            return BadRequest(); // ID不匹配
        }

        _context.Entry(todo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound(); // 任务不存在
            }
            throw;
        }

        return NoContent(); // 更新成功
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.Todos.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // 辅助方法：检查任务是否存在
    private bool TodoItemExists(int id)
    {
        return _context.Todos.Any(e => e.Id == id);
    }
}