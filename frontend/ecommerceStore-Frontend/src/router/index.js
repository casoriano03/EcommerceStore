import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue';
import ShopView from '@/views/ShopView.vue';
import LoginView from '@/components/Login.vue';
import RegisterView from '@/views/RegisterView.vue';
import ChangePasswordView from '@/views/ChangePasswordView.vue';
import ProfileView from '@/views/ProfileView.vue';
import SearchView from '@/views/SearchView.vue';
import ProductView from '@/views/ProductView.vue';
import CartView from '@/views/CartView.vue';
import ShippingView from '@/views/ShippingView.vue';
import PaymentSuccessView from '@/views/PaymentSuccessView.vue';
import PaymentFailedView from '@/views/PaymentFailedView.vue';
import AdminView from '@/views/AdminView.vue';


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
        component: ProfileView,
      },
      {
        path:'/search',
        name:'search',
        component: SearchView,
      },
      {
        path:'/product/:productId',
        name:'product',
        component: ProductView,
      },
      {
        path:'/cart',
        name:'cart',
        component: CartView,
      },
      {
        path:'/shipping',
        name:'order',
        component: ShippingView,
      },
      {
        path:'/paymentSuccess',
        name:'paymentSuccess',
        component: PaymentSuccessView,
      },
      {
        path:'/paymentFailed',
        name:'paymentFailed',
        component: PaymentFailedView,
      },
      {
        path:'/admin',
        name:'admin',
        component: AdminView,
      },

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