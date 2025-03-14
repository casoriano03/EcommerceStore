<script setup>
import { ref, onMounted } from 'vue';
import { RouterLink, useRouter } from 'vue-router';
import { jwtDecode } from 'jwt-decode';
import router from '@/router';
import defaultUserIcon from '../components/icons/user-interface.png';
import loggedUserIcon from '../components/icons/man.png';


const totalAmount = ref(0);
const itemCount = ref(0);
const isLoggedIn = ref(false);
const isAdmin = ref(false);
const isUserLoggedIn = ref(false);

onMounted(()=>{
  const tokenPresent = sessionStorage.getItem("authToken");
  if (tokenPresent) {
    isLoggedIn.value = true;
    isUserLoggedIn.value = true
    const decodedPayload = getTokenPayLoad(tokenPresent);
  if (decodedPayload.role == "Admin" ) {
    isAdmin.value = true;
  }
}});

const getTokenPayLoad = (token)=>{
  if (token) {
    const decoded = jwtDecode(token);
    return decoded;
  }
    return null
}

const logout = ()=>{
  sessionStorage.removeItem("authToken")
  router.push("/")
  setTimeout(()=>{
    window.location.reload();
  }, 200);
}


</script>

<template>
  <div class="container-fluid navContainer">
    <header class="d-flex flex-wrap justify-content-center py-3 border-bottom">
      <div class="ms-3">
        <RouterLink to="/" class="d-flex align-items-center mb-3 mb-md-0 me-md-auto link-body-emphasis text-decoration-none">
        <img src="./icons/logo.png" height="60" alt="webshop logo" class="rounded-circle me-2">
        <span class="fs-4">The WebShop</span>
        </RouterLink>
      </div>
      
      <ul class="nav nav-pills m-auto">
        <li class="nav-item"><RouterLink to="/" class="nav-link text-dark navLink">Home</RouterLink></li>
        <li class="nav-item"><RouterLink to="/shop" class="nav-link text-dark navLink">Shop</RouterLink></li>
        <li class="nav-item"><a href="#" class="nav-link text-dark navLink">About</a></li>
        <li class="nav-item"><a href="#" class="nav-link text-dark navLink">Features</a></li>
      </ul>

      <ul class="nav nav-pills me-3">
        <li class="nav-item"><RouterLink to="/search" class="nav-link"><img src="./icons/research.png" alt="magnifying glass icon" height="18"></RouterLink></li>
        <li class="nav-item fs-5 pt-1">${{ totalAmount }}</li>
        <li class="nav-item"><a href="#" class="nav-link"><div class="border border-dark ps-2 pe-2 fs-6 mt-0 text-dark bg-light">{{ itemCount }}</div></a></li>
        <li class="nav-item">
          <div class="dropdown">
          <button class="btn dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
            <RouterLink to="/login" class="nav-link"><img :src="isUserLoggedIn?loggedUserIcon:defaultUserIcon" alt="user icon" height="20"></RouterLink>
          </button>
          <ul class="dropdown-menu text-center">
            <li><a v-if="!isLoggedIn" class="dropdown-item"><RouterLink to="/login">Login</RouterLink></a></li>
            <li><a v-if="isLoggedIn" class="dropdown-item"><RouterLink to="/profile">Profile</RouterLink></a></li>
            <li><a v-if="isAdmin" class="dropdown-item" >Admin</a></li>
            <li><a v-if="isLoggedIn" class="dropdown-item" @click="logout">Logout</a></li>
          </ul>
          </div>
        </li>       
      </ul>

      
    </header>
  </div>
</template>