<script setup>
import { ref } from 'vue';
import AdminProducts from '@/components/AdminProducts.vue';
import AdminSales from '@/components/AdminSales.vue';

const activeTab = ref('sales');


</script>

<template>
    <div class="container my-5">
        <h1>Admin</h1>

        <ul class="nav nav-tabs">
            <li class="nav-item">
                <button class="nav-link" :class="{ active: activeTab === 'sales' }" 
                @click="activeTab = 'sales'">Sales</button>
            </li>
            <li class="nav-item">
                <button class="nav-link" :class="{ active: activeTab === 'products' }" 
                @click="activeTab = 'products'">Products</button>
            </li>

        </ul>
        <div class="tab-content mt-4">
            <AdminSales v-if="activeTab === 'sales'" />
            <AdminProducts v-if="activeTab === 'products'" />
        </div>
    </div>
    
</template>