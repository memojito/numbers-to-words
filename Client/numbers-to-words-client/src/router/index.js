import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '@/views/home-view.vue'; 
const routes = [
  {
    path: '/home',
    name: 'HomeView', 
    component: HomeView
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
