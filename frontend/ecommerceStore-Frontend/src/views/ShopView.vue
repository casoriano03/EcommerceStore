<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import ProductCard from '@/components/ProductCard.vue';

const products = ref([]);
const filteredProducts = ref([]);

const filterProducts = (category)=>{
    filteredProducts.value = products.value.filter(p => p.category.name === category);
};

const resetFilter = ()=>{
    filteredProducts.value = products.value;
} 

onMounted(async()=>{
    try {
        const response = await axios.get('https://localhost:7023/api/Product/GetAllProducts');
        products.value=response.data;
        filteredProducts.value = response.data;
    } catch (error) {
        console.log(error)
    }
});
</script>

<template>
    <div>
  <div class="d-flex flex-column flex-shrink-0 p-3">
    <div class="fs-4">Categories</div>
    <hr>
    <div class="container text-center">
      <button class="btn bt-outline-dark" @click="resetFilter()">All Products</button>
      <button class="btn" @click="filterProducts('Electronics')"><img class="shopIcons" src="../components/icons/device.png" alt="">Electronics</button>
      <button class="btn" @click="filterProducts('Fashion')"><img class="shopIcons" src="../components/icons/brand.png" alt="">Fashion</button>
      <button class="btn" @click="filterProducts('Home & Kitchen')"><img class="shopIcons" src="../components/icons/small-appliance.png" alt="">Home & Kitchen</button>
      <button class="btn" @click="filterProducts('Sports & Outdoors')"><img class="shopIcons" src="../components/icons/physical.png" alt="">Sports & Outdoors</button>
      <button class="btn" @click="filterProducts('Toys & Games')"><img class="shopIcons" src="../components/icons/game-console.png" alt="">Toys & Games</button>
      <button class="btn" @click="filterProducts('Books')"><img class="shopIcons" src="../components/icons/book.png" alt="">Books</button>
    </div>
    <hr>
  </div>
</div>
    <div>
        <ProductCard :products="filteredProducts" />
    </div>
</template>