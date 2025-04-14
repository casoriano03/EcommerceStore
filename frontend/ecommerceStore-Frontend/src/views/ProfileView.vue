<script setup>
import { ref } from 'vue';
import Profile from '@/components/Profile.vue';
import ProfileReview from '@/components/ProfileReview.vue';
import ProfileWishlist from '@/components/ProfileWishlist.vue';
import ProfileOrders from '@/components/ProfileOrders.vue';

const activeTab = ref('profile');

</script>

<template>
      <div class="container my-5">
    <h1>My Profile</h1>

    <!-- Tabs Navigation -->
    <ul class="nav nav-tabs">
      <li class="nav-item">
        <button class="nav-link" :class="{ active: activeTab === 'profile' }" 
        @click="activeTab = 'profile'">Profile Info</button>
      </li>
      <li class="nav-item">
        <button class="nav-link" :class="{ active: activeTab === 'reviews' }"
        @click="activeTab = 'reviews'">My Reviews</button>
      </li>
      <li class="nav-item">
        <button class="nav-link" :class="{ active: activeTab === 'wishlist' }"
        @click="activeTab = 'wishlist'">Wishlist</button>
      </li>
      <li class="nav-item">
        <button class="nav-link" :class="{ active: activeTab === 'orders' }"
        @click="activeTab = 'orders'">Orders</button>
      </li>
    </ul>

    <!-- Tab Content -->
    <div class="tab-content mt-4">
      <Profile v-if="activeTab === 'profile'" />
      <ProfileReview v-if="activeTab === 'reviews'" />
      <ProfileWishlist v-if="activeTab === 'wishlist'" />
      <ProfileOrders v-if="activeTab === 'orders'" />
    </div>
  </div>
  
    
</template>