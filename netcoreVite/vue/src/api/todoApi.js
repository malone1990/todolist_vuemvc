import axios from 'axios';

// 创建axios实例，基础路径为当前域名（与.NET共享端口）
const api = axios.create({
    baseURL: '/', // 因为与.NET同端口，直接使用相对路径
    timeout: 5000 // 请求超时时间
});

/**
 * 获取所有Todo任务
 */
export const getTodos = () => {
    return api.get('/api/todos'); // 调用MVC API
};

/**
 * 添加新任务
 * @param {string} title - 任务标题
 * @param {string} content - 任务内容
 */
export const addTodo = (title, content = '') => {
    return api.post('/api/todos', {
        title,
        content,
        isCompleted: false
    });
};

/**
 * 更新任务状态
 * @param {number} id - 任务ID
 * @param {object} todo - 完整任务对象
 */
export const updateTodo = (id, todo) => {
    return api.put(`/api/todos/${id}`, todo);
};

/**
 * 删除任务
 * @param {number} id - 任务ID
 */
export const deleteTodo = (id) => {
    return api.delete(`/api/todos/${id}`);
};