# .Netcore with Vite and Vue components

This is a project template for building a web application using .Net Core as the backend and Vite with Vue.js for the frontend.

## Tech Stack

*   **Backend**: .Net Core, C#
*   **Frontend**: Vite, Vue.js, JavaScript
*   **Database**: SQLite (via Entity Framework Core)

## Project Structure

```
/
├── Controllers/      # .Net Core MVC controllers
├── Models/           # Data models and DbContext
├── Views/            # Razor views (.cshtml files)
├── vue/              # Vue.js frontend source code
│   ├── src/
│   │   ├── components/ # Vue components
│   │   ├── views/      # Vue views/pages
│   │   ├── App.vue     # Main Vue component
│   │   └── main.js     # Vue app entry point
│   ├── vite.config.js  # Vite configuration
│   └── package.json    # Frontend dependencies
├── wwwroot/          # Static assets (CSS, JS, images)
├── Program.cs        # .Net Core application entry point
└── DotVue.csproj     # Project file
```

## How to Update the Culomn in TodoItem

### add packages
```bash
# 安装EF Core核心包（提供ORM基础功能）
dotnet add package Microsoft.EntityFrameworkCore

# 安装SQLite数据库适配器
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

# 安装EF Core设计工具（提供迁移命令支持）
dotnet add package Microsoft.EntityFrameworkCore.Design
```
### update datebase after change the properties of TodoItem
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations list
```
## How to Run and Debug

### Backend (.Net Core)

1.  **Run the application**:
    ```bash
    dotnet run
    ```
2.  **Debug the application**:
    You can debug the application directly from your IDE (e.g., Visual Studio, Rider). Set breakpoints in your C# code and start a debugging session.

### Frontend (Vue.js)

1.  **Navigate to the `vue` directory**:
    ```bash
    cd vue
    ```
2.  **Install dependencies**:
    ```bash
    npm install
    ```
3.  **Run the development server**:
    ```bash
    npm run dev
    ```
    This will start the Vite development server with hot module replacement.

4.  **Build for production**:
    ```bash
    npm run build
    ```
    This will create a `dist` folder in the `vue` directory with the production-ready assets. These assets are then served by the .Net Core application.
