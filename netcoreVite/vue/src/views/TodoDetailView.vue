<template>
  <div class="todo-detail">
    <h1>{{ isEditing ? '编辑任务' : '任务详情' }}</h1>

    <div v-if="loading">加载中...</div>

    <div v-else-if="error" class="error">{{ error }}</div>

    <form v-else-if="todo" @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="title">标题:</label>
        <input
          id="title"
          v-model="todo.title"
          type="text"
          required
          placeholder="输入任务标题"
        />
      </div>

      <div class="form-group">
        <label for="content">内容:</label>
        <textarea
          id="content"
          v-model="todo.content"
          rows="4"
          placeholder="输入任务内容"
        ></textarea>
      </div>

      <div class="form-group">
        <label>
          <input
            type="checkbox"
            v-model="todo.isCompleted"
          />
          已完成
        </label>
      </div>

      <div class="form-group">
        <label for="modifiedAt">修改时间:</label>
        <input
          id="modifiedAt"
          v-model="todo.modifiedAt"
          type="text"
          disabled
        />
      </div>

      <div class="form-actions">
        <button type="submit" :disabled="loading">
          {{ isEditing ? '更新任务' : '保存任务' }}
        </button>
        <button type="button" @click="goBack">取消</button>
      </div>
    </form>

    <div v-else-if="!isEditing">
      <p>未找到该任务</p>
      <button @click="goBack">返回列表</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useTodoStore } from '../store/todoStore';

const route = useRoute();
const router = useRouter();
const store = useTodoStore();

const todo = ref(null);
const loading = ref(false);
const error = ref(null);

// 判断是否为编辑模式（通过路由参数判断）
const isEditing = ref(false);

// 设置修改时间
const setModifiedTime = () => {
  const now = new Date();
  // 使用 toISOString() 来生成与 .NET Core 兼容的日期字符串
  todo.value.modifiedAt = now.toISOString();
};

// 从URL获取ID
const getIdFromRoute = () => {
  const id = parseInt(route.params.id);
  return isNaN(id) ? null : id;
};

// 从store获取指定ID的任务详情
const loadTodo = async (id) => {
  if (id) {
    loading.value = true;
    error.value = null;

    try {
      // 如果store中有该任务，则直接使用
      const existingTodo = store.items.find(item => item.id === id);
      if (existingTodo) {
        todo.value = JSON.parse(JSON.stringify(existingTodo)); // 深拷贝避免影响原始数据
        // 如果没有内容字段，初始化它
        if (!todo.value.content) {
          todo.value.content = '';
        }
        if (!todo.value.modifiedAt) {
          todo.value.modifiedAt = new Date().toISOString();
        }
      } else {
        // 尝试从API获取任务详情（如果需要的话）
        // 当前项目中暂时不需要
        error.value = '任务详情暂不可用';
      }
    } catch (err) {
      error.value = '加载任务详情失败';
      console.error(err);
    } finally {
      loading.value = false;
    }
  }
};

// 新增模式
const createNewTodo = () => {
  todo.value = {
    id: null,
    title: '',
    content: '',
    isCompleted: false,
    modifiedAt: new Date().toISOString()
  };
  isEditing.value = true;
};

// 表单提交
const handleSubmit = async () => {
  if (!todo.value.title.trim()) {
    error.value = '请输入任务标题';
    return;
  }

  loading.value = true;
  error.value = null;

  // 在提交前，最后一次更新修改时间
  setModifiedTime();

  try {
    if (isEditing.value) {
      // 更新现有任务
      if (todo.value.id) {
        await store.updateTodo(todo.value.id, todo.value);
        // 更新store中的数据
        const index = store.items.findIndex(item => item.id === todo.value.id);
        if (index !== -1) {
          store.items[index] = todo.value;
        }
      } else {
        // 新增任务
        const response = await store.addNewTodo(todo.value.title, todo.value.content);
        todo.value.id = response.data.id;
        // 添加到store
        store.items.push(response.data);
      }
    }

    // 成功后返回列表页面
    goBack();
  } catch (err) {
    error.value = '操作失败';
    console.error(err);
  } finally {
    loading.value = false;
  }
};

// 返回列表页
const goBack = () => {
  router.push('/');
};

// 页面加载时执行
onMounted(async () => {
  const id = getIdFromRoute();
  if (id) {
    // 编辑模式
    isEditing.value = true;
    await loadTodo(id);
  } else {
    // 新增模式
    createNewTodo();
  }
});

// 监听todo变化，及时设置修改时间
watch(() => todo.value, () => {
  if (todo.value && isEditing.value) {
    setModifiedTime();
  }
}, { deep: true });
</script>

<style scoped>
.todo-detail {
  max-width: 600px;
  margin: 20px auto;
  padding: 0 10px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

.form-group input[type="checkbox"] {
  width: auto;
}

.form-actions {
  margin-top: 20px;
}

.form-actions button {
  margin-right: 10px;
  padding: 8px 16px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.form-actions button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.error {
  color: red;
  margin: 10px 0;
}
</style>