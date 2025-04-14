<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';
import Product from '@/components/Product.vue';


const route = useRoute();
const product = ref([]);


onMounted(async()=>{
    const productId = route.params.productId;
    const response = await axios.get('https://localhost:7023/api/Product/GetProductById', {
        params:{id:productId}
    });
    product.value = response.data;
});
</script>

<template>
   <Product :product="product"/>
</template>