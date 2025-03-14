<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const categories = ref([]);
onMounted ( async ()=>{
  try {
    const response = await axios.get('https://localhost:7023/api/Category/GetCategories');
    categories.value = response.data
  } catch (error) {
    console.log(error)
  }
});

</script>

<template>
<div class="container categoriesContainer py-5 px-5 my-5">
  <div class="text-center">
    <h2 class="my-5">Explore by Category – Find Exactly What You’re Looking For!</h2>
    <p class="lead categoriesText my-5 mx-auto">Dive into our carefully curated categories and discover a world of possibilities. Whether you’re searching for fashion, electronics, home essentials, or gifts, we’ve organized it all to make your shopping experience seamless. Browse with ease, find your favorites, and enjoy a personalized journey tailored just for you</p>
  </div>
<div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-3">
  <div v-for="category in categories" :key="category.id" class="col">
    <div class="card shadow-sm cardCategory">
      <img :src="category.imageLink" alt="" height="300" class="categoryImage">
      <h1 class="imageLabel">{{ category.name }}</h1>
    </div>
  </div>
</div>
</div>
</template>