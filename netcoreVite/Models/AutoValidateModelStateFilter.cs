using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotVue.Models;

public class AutoValidateModelStateFilter : IActionFilter, IOrderedFilter
{
    // 确保在模型绑定后执行（IActionFilter 默认在模型绑定后）
    private readonly ILogger<AutoValidateModelStateFilter> _logger;

    // 确保在模型绑定后、其他业务逻辑前执行（优先级较高）
    public int Order => int.MinValue + 100;

    public AutoValidateModelStateFilter(ILogger<AutoValidateModelStateFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // 1. 检查 ModelState 是否有效
        if (!context.ModelState.IsValid)
        {
            // 2. 提取错误信息
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // 3. 记录日志
            _logger.LogWarning(
                "模型验证失败。路径：{Path}，方法：{Method}，错误：{Errors}",
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Method,
                string.Join("; ", errors)
            );

            // 4. 返回自定义错误响应（捕获错误）
            context.Result = new BadRequestObjectResult(new
            {
                StatusCode = 400,
                Message = "请求数据格式错误",
                Errors = errors
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
