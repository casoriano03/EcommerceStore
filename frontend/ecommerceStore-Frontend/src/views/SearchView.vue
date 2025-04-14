<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import ProductCard from '@/components/ProductCard.vue';

const searchString = ref("");
const productsData = ref([]);
const filteredProducts = ref([]);


onMounted(async()=>{
    try {
        const response = await axios.get('https://localhost:7023/api/Product/GetAllProducts');
        productsData.value = response.data
    } catch (error) {
        console.log(error)
    }
});

const searchData = () =>{
    filteredProducts.value = productsData.value.filter(p =>
        p.name.toLowerCase().includes(searchString.value.toLowerCase())
    );
}

</script>

<template>
  <div class="px-4 py-5 my-5 text-center">
    <h1 class="display-5 fw-bold text-body-emphasis">Find What Matters – Fast, Accurate, and Effortless.</h1>
    <div class="col-lg-6 mx-auto">
      <p class="lead mb-4">Discover the power of precision with our advanced search solutions. Whether you're looking for data, products, or information, our intuitive search tools deliver lightning-fast results tailored to your needs. Say goodbye to endless scrolling and hello to seamless, accurate searches that save you time and energy. Experience the future of search – where speed meets simplicity.</p>
      <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
        <label for="ControlInput1" class="form-label">Product Name</label>
        <input type="text" @input="searchData" v-model="searchString" class="form-control" id="ControlInput1">
      </div>
    </div>
  </div>
  <div>
    <div class="row mx-5 my-5">
        <ProductCard v-model:products="filteredProducts" />
    </div>
  </div>
</template>