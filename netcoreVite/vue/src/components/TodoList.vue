<template>
  <div class="todo-container">
    <!-- 错误提示 -->
    <div v-if="error" class="error">{{ error }}</div>

    <!-- 加载状态 -->
    <div v-if="loading">加载中...</div>

    <!-- 主要内容区域 -->
    <div v-else>
      <!-- 添加任务输入框 -->
      <div class="add-todo">
        <input
            v-model="newTitle"
            @keyup.enter="handleAdd"
            placeholder="输入新任务..."
        />
        <button @click="handleAdd">添加</button>
      </div>

      <!-- 任务列表 -->
      <ul>
      <li v-for="item in items" :key="item.id" class="todo-item">
        <input
            type="checkbox"
            :checked="item.isCompleted"
            @change="toggleComplete(item.id)"
        />
        <span :class="{ completed: item.isCompleted }">{{ item.title }}</span>
        <button @click="editTodo(item)">编辑</button>
        <button @click="removeTodo(item.id)">删除</button>
      </li>
    </ul>

    <!-- 统计信息 -->
    <div class="stats">
      已完成：{{ completedCount }} / 总任务：{{ items.length }}
    </div>
  </div>
  </div>
</template>

<script setup>
// 导入 Vue 3 的组合式 API 函数
// ref 用于创建响应式变量，通常用于基本数据类型
// computed 用于创建计算属性，当依赖的响应式数据变化时，它会自动重新计算
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';

// 导入 Pinia store
// Pinia 是 Vue 的官方状态管理库，用于集中管理应用的共享状态
import { useTodoStore } from '../store/todoStore';

// --- 扩展提示 ---
// 组合式 API (Composition API) 是 Vue 3 的核心特性之一，它允许我们按逻辑功能组织代码，而不是按选项 (data, methods, etc.)。
// <script setup> 是一个语法糖，可以让我们更简洁地使用组合式 API。

// 获取 Store 实例
// useTodoStore() 会返回一个 Pinia store 实例，我们可以通过它来访问和操作状态
const store = useTodoStore();
const router = useRouter();

// --- 响应式状态 ---

// newTitle 是一个响应式变量，用于双向绑定输入框的值
// ref('') 创建了一个包含空字符串的响应式引用
const newTitle = ref('');

// --- 计算属性 ---

// 从 Store 中获取状态，并使用 computed 转换为计算属性
// 这样做的好处是，当 store 中的状态变化时，这些计算属性会自动更新，从而触发组件的重新渲染
const items = computed(() => store.items);
const loading = computed(() => store.loading);
const error = computed(() => store.error);
const completedCount = computed(() => store.completedCount);

// --- 事件处理函数 ---

// handleAdd 函数用于处理添加新任务的逻辑
const handleAdd = () => {
  // 调用 store 中的 action 来添加新任务
  store.addNewTodo(newTitle.value);
  // 添加后清空输入框
  newTitle.value = '';
};

// toggleComplete 函数用于切换任务的完成状态
const toggleComplete = (id) => store.toggleComplete(id);

// removeTodo 函数用于删除任务
const removeTodo = (id) => store.removeTodo(id);

// editTodo 函数用于编辑任务，跳转到编辑页面
const editTodo = (item) => {
  router.push(`/todo/${item.id}`);
};

// --- 扩展提示 ---
// 在 Vue 3 中，我们通常将业务逻辑（如 API 请求、数据处理）放在 Pinia store 中，
// 而组件则专注于渲染 UI 和响应用户交互。
// 这种关注点分离的模式使得代码更易于维护和测试。
</script>

<style>
.todo-container {
  max-width: 600px;
  margin: 20px auto;
  padding: 0 10px;
}

.add-todo {
  margin-bottom: 20px;
  display: flex;
  gap: 10px;
}

.add-todo input {
  flex: 1;
  padding: 8px;
}

.todo-item {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 5px 0;
  padding: 8px;
  border: 1px solid #eee;
  border-radius: 4px;
}

.todo-item .completed {
  text-decoration: line-through;
  color: #888;
}

.error {
  color: red;
  margin: 10px 0;
}

.stats {
  margin-top: 20px;
  color: #666;
}
</style>