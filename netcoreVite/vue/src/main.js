import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import {useTodoStore} from "./store/todoStore.js";
import router from './router';

// 创建Pinia实例（状态管理）
const pinia = createPinia();

// 创建Vue实例
const app = createApp(App);

// 使用Pinia
app.use(pinia);

// 使用Vue Router
app.use(router);

// 挂载到Razor视图中的#todo-app节点
// 注意：需等待DOM加载完成
document.addEventListener('DOMContentLoaded', () => {
    const todoApp = document.getElementById('todo-app');
    if (todoApp) {
        // 从全局变量获取Razor注入的初始数据
        const initialData = window.__INITIAL_DATA__ || [];

        // 初始化Store数据
        const todoStore = useTodoStore();
        todoStore.init(initialData);

        // 挂载Vue组件
        app.mount(todoApp);
    }
});