<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';

const ordersArray = ref([]);
const userId = ref(null);
const token = sessionStorage.getItem("authToken");

onMounted(async()=>{
    try {
        const payload = getTokenPayLoad(token);
        if (payload) {
            userId.value = parseInt(payload.nameid);
        };

        const orders = await axios.get(`https://localhost:7023/api/Order/GetOrdersByUserId?userId=${userId.value}`);
        ordersArray.value = orders.data;
        console.log(ordersArray.value)
    } catch (error) {
        console.log(error)
    }
});

const getTokenPayLoad = (token)=>{
  if (token) {
    const decoded = jwtDecode(token);
    return decoded;
  }
    return null
};
</script>
<template>
    <div class="container">
        <div class="row text-center">
            <div class="col">Date</div>
            <div class="col">Order Number</div>
            <div class="col">Status</div>
            <div class="col">Total</div>
        </div>
        <hr>
        <div class="row text-center" v-for="order in ordersArray" :key="order">
            <div class="col">{{ order.orderDate.substring(0,10) }}</div>
            <div class="col">{{ order.id }}</div>
            <div class="col">{{ order.status }}</div>
            <div class="col">{{ order.total.toFixed(2) }}</div>
            <hr>
        </div>
    </div>
</template>