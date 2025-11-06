import { defineStore } from 'pinia';
import { getTodos, addTodo, updateTodo, deleteTodo } from '../api/todoApi';

/**
 * Todo状态管理Store
 * 负责管理全局的Todo列表和操作方法
 */
export const useTodoStore = defineStore('todo', {
    // 状态数据
    state: () => ({
        items: [],       // Todo列表
        loading: false,  // 加载状态
        error: null      // 错误信息
    }),

    // 派生状态（计算属性）
    getters: {
        // 已完成任务数量
        completedCount: (state) => {
            return state.items.filter(item => item.isCompleted).length;
        }
    },

    // 动作方法（修改状态）
    actions: {
        /**
         * 初始化Todo列表（从Razor注入的初始数据）
         * @param {array} initialData - 初始数据
         */
        init(initialData) {
            this.items = initialData;
        },

        /**
         * 从API加载Todo列表
         */
        async fetchTodos() {
            this.loading = true;
            this.error = null;
            try {
                const response = await getTodos();
                this.items = response.data;
            } catch (err) {
                this.error = '加载任务失败';
                console.error(err);
            } finally {
                this.loading = false;
            }
        },

        /**
         * 添加新任务
         * @param {string} title - 任务标题
         * @param {string} content - 任务内容
         */
        async addNewTodo(title, content = '') {
            if (!title.trim()) return;
            try {
                const response = await addTodo(title, content);
                this.items.push(response.data);
            } catch (err) {
                this.error = '添加任务失败';
                console.error(err);
            }
        },

        /**
         * 更新任务
         * @param {number} id - 任务ID
         * @param {object} todo - 任务对象
         */
        async updateTodo(id, todo) {
            if (!todo.title.trim()) return;
            try {
                await updateTodo(id, todo);;
            } catch (err) {
                this.error = '更新任务失败';
                console.error(err);
                throw err; // 重新抛出错误以便调用者处理
            }
        },

        /**
         * 切换任务完成状态
         * @param {number} id - 任务ID
         */
        async toggleComplete(id) {
            const todo = this.items.find(item => item.id === id);
            if (!todo) return;

            // 先本地更新（优化体验）
            todo.isCompleted = !todo.isCompleted;
            try {
                await updateTodo(id, todo);
            } catch (err) {
                // 失败回滚
                todo.isCompleted = !todo.isCompleted;
                this.error = '更新任务失败';
                console.error(err);
            }
        },

        /**
         * 删除任务
         * @param {number} id - 任务ID
         */
        async removeTodo(id) {
            const index = this.items.findIndex(item => item.id === id);
            if (index === -1) return;

            // 先本地删除
            const deleted = this.items.splice(index, 1)[0];
            try {
                await deleteTodo(id);
            } catch (err) {
                // 失败回滚
                this.items.splice(index, 0, deleted);
                this.error = '删除任务失败';
                console.error(err);
            }
        }
    }
});