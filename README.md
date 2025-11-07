# todolist_vuemvc
vue3+Net8 MVC，使用Razor 页面使用vue组件，MVC 管页面跳转，Vue 管组件内跳转
1. Vue 与 View 的挂载融合
  - Razor View 中定义 Vue 挂载节点：<div id="vue-container"></div>，作为 Vue 组件的容器。
  - Vue 入口文件需等待 DOM 加载完成后挂载：
// main.js
document.addEventListener('DOMContentLoaded', () => {
    createApp(App).mount('#vue-container');
});
2. 数据传递机制
  - 后端 → 前端：通过 Razor 语法注入初始数据到全局变量，Vue 初始化时读取：
<!-- View 中 -->
<script>
    window.initData = @Html.Raw(Json.Serialize(Model)); // 安全序列化后端模型
</script>
  - 前端 → 后端：Vue 通过 Axios 调用 MVC API（同域，无需跨域配置）。
3. 路由协同
  - 页面级跳转由 MVC 路由管理（如 @Url.Action("List", "Todo")）。
  - 组件内跳转由 Vue Router 管理，需配置 .NET 路由 fallback：
// Program.cs（确保 Vue 路由生效）
app.MapFallbackToFile("index.html");
4. 样式隔离
  - Vue 组件使用 <style scoped> 避免污染 Razor View 样式；View 中通过 ::v-deep 穿透修改组件样式（如需）。

## 问题思考
**1.Action方法中参数对象自动序列化问题**
1.模型绑定 Action 从Request自动获取参数
参考资料 https://learn.microsoft.com/zh-cn/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0#model-binding-simple-and-complex-types
用于指定数据从请求的不同位置（如 URL 路径、查询字符串、表单等）绑定，常见的有：
暂时无法在飞书文档外展示此内容
示例：混合使用多种绑定方式
csharp
[HttpPut("api/todos/{id:int}")] // 路由参数 id
public IActionResult UpdateTodo(
    [FromRoute] int id, // 从路由获取 id
    [FromBody] TodoItem todo, // 从请求体获取 todo 对象
    [FromHeader] string authorization, // 从请求头获取 Authorization
    [FromQuery] bool? forceUpdate // 从查询字符串获取 forceUpdate
)
{
    // 业务逻辑...
}
2.模型校验
参考 https://learn.microsoft.com/zh-cn/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
- ModelState.IsValid 的使用。
- 数据注解（[Required] 等）与自定义验证。
- 全局异常处理（Filter过滤器、中间件）。
3.模型校验问题
为何 UseExceptionHandler 无法捕获模型绑定错误？
ASP.NET Core 对模型绑定错误的处理逻辑是：
1. 模型绑定失败 → ModelState.IsValid = false。
2. 框架会检查 ModelState，若无效则直接返回 400 响应（不抛出异常）。
3. 只有异常被抛出 → UseExceptionHandler 不会触发（仅处理 “未处理的异常”）。
因此，UseExceptionHandler 和模型绑定错误是 两个独立的处理流程，需分别通过 “中间件” 和 “过滤器” 覆盖。
过滤器 vs 中间件：如何选择？
<img width="1007" height="385" alt="image" src="https://github.com/user-attachments/assets/d186608c-6146-41f3-ab3a-992339719e34" />
推荐方案
1. 优先使用过滤器：若只需处理 MVC 控制器的 ModelState 错误，且可能需要针对特定 Action/Controller 生效，过滤器是更简单的选择（直接访问 ModelState，执行时机更早）。
2. 中间件适合场景：若需在 MVC 管道外统一处理（如与其他中间件联动），或希望覆盖所有 MVC 请求（无需关心过滤器注册），可使用中间件。
