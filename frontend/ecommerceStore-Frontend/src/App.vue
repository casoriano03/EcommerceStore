<script setup>
import { RouterView } from 'vue-router'
import HomeView from '@/views/HomeView.vue';
import Navbar from '@/components/Navbar.vue';
import Footer from '@/components/Footer.vue';
</script>

<template>
 <Navbar/>
 <RouterView/>
 <Footer/>
</template>
