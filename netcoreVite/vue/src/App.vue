<template>
  <div id="app-container">
    <nav>
      <router-link to="/">任务列表</router-link>
      <router-link to="/about">关于</router-link>
    </nav>

    <!-- 使用 router-view 渲染路由匹配的组件 -->
    <router-view></router-view>

  </div>
</template>

<script setup>
// 可以在这里添加全局的路由相关的逻辑
</script>

<style scoped>
#app-container {
  padding: 20px;
}

nav {
  margin-bottom: 20px;
  display: flex;
  gap: 10px;
}

nav a {
  padding: 8px 16px;
  text-decoration: none;
  color: #333;
  border: 1px solid #ccc;
  border-radius: 4px;
}

nav a.router-link-active {
  background-color: #007bff;
  color: white;
}
</style>