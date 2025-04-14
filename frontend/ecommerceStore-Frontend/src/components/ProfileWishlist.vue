<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import ProductCard from './ProductCard.vue';

const products = ref([]);
const token = sessionStorage.getItem("authToken");
const userId = ref(null);

onMounted(async()=>{
    try {
        const payload = getTokenPayLoad(token);
        if (payload) {
            userId.value = parseInt(payload.nameid);
        }
       
        const response = await axios.get('https://localhost:7023/api/Wishlist/GetWishlistByUserId', {
            params:{userId:userId.value}
        });
        
        response.data.forEach(product => {
            products.value.push(product.product);
        });
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
    <div class="my-5">
        <h3>Wishlist:</h3>
    </div>
   <ProductCard :products="products" />
</template>