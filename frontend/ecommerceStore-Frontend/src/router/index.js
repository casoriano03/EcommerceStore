import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue';
import ShopView from '@/views/ShopView.vue';
import LoginView from '@/components/Login.vue';
import RegisterView from '@/views/RegisterView.vue';
import ChangePasswordView from '@/views/ChangePasswordView.vue';
import ProflieView from '@/components/Profile.vue';
import SearchView from '@/views/SearchView.vue';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
      {
        path: '/',
        name: 'home',
        component: HomeView,
      },
      {
        path: '/shop',
        name: 'shop',
        component: ShopView,
      },
      {
        path: '/login',
        name: 'login',
        component: LoginView,
      },
      {
        path: '/register',
        name: 'register',
        component: RegisterView,
      },
      {
        path:'/changePassword',
        name:'changePassword',
        component: ChangePasswordView,
      },
      {
        path:'/profile',
        name:'profile',
        component: ProflieView,
      },
      {
        path:'/search',
        name:'search',
        component: SearchView,
      }
    ],
  })
  
  router.beforeEach((to, from, next) => {
    const isAuthenticated = sessionStorage.getItem("authToken");
  
    if (to.meta.requiresAuth && !isAuthenticated) {
      next("/login");
    } else {
      next();
    }
  });
  
  export default router