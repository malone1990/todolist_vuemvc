import { createRouter, createWebHistory } from 'vue-router';
import homeVIew from "../views/HomeVIew.vue";
import TodoDetailView from "../views/TodoDetailView.vue";
import About from "../components/About.vue";

const routes = [
  {
    path: '/',
    name: 'Home',
    component: homeVIew
  },
  {
    path: '/todo/:id',
    name: 'TodoDetail',
    component: TodoDetailView,
    props: true
  },
  {
    path: '/about',
    name: 'About',
    component: About
  }
]

const router = createRouter({
    history: createWebHistory(),
    routes
});

//显示用法
export default router;