namespace DotVue.Models;

/// <summary>
/// Todo任务数据模型
/// 同时用于EF Core数据库映射和API数据传输
/// </summary>
public class TodoItem
{
    // 主键
    public int Id { get; set; }

    // 任务标题
    public string Title { get; set; } = string.Empty;

    // 任务内容
    public string Content { get; set; } = string.Empty;

    // 完成状态
    public bool IsCompleted { get; set; }

    // 创建时间（默认UTC时间）
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 修改时间
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}