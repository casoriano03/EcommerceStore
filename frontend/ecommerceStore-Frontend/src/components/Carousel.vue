<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const products = ref([]);

onMounted(async()=>{
  try {
    const response = await axios.get('https://localhost:7023/api/Product/GetAllProducts')
    products.value = response.data
  } catch (error) {
    console.log(error)
  }
});

function truncateText(text){
  const maxLength = 100;
  if (text.length > maxLength) {
    return text.substring(0, maxLength) +"...";
  }
  return text
};
</script>

<template>
 <div id="carouselExample" class="carousel slide bg-#F6F0F0 px-5 py-5 text-center" data-bs-ride="carousel">
  <h2 class="my-5">New Arrivals, Endless Possibilities – Fresh Finds Await</h2>
  <p class="lead my-5 mx-auto carouselText">Stay ahead of the curve with our latest collection of new arrivals! From cutting-edge trends to timeless classics, we’ve handpicked the best just for you. Be the first to explore fresh styles, innovative gadgets, and exclusive deals. Refresh your wardrobe, upgrade your lifestyle, and make every day extraordinary with The Webshop’s newest additions!</p>

    <div class="carousel-inner">
      <div v-for="(chunk, index) in Math.ceil(products.length / 3)" 
        :key="index" 
        class="carousel-item"
        :class="{ active: index === 0 }">
        <div class="row justify-content-center">
          <div v-for="(product, i) in products.slice(index * 3, (index + 1) * 3)" :key="product.id" class="col-md-2">
            <div class="card text-center shadow-sm carouselCard">
              <div>
                <img :src="product.imageUrl" class="card-img-top" alt="Product Image" width="10" height="250">
              </div>
              <div class="card-body fs-6 cardBody">
                <h5 class="card-title">{{ product.name }}</h5>
                <!-- <p class="card-text">{{truncateText(product.description)}}</p> -->
              </div>
              <div class="card-footer">
                  <p class="fw-bold">NOK {{ product.price }}</p>
              </div>
              <div class="card-footer">
                  <a href="#" class="btn btn-outline-dark addToCart">Add to Cart</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Carousel Controls -->
    <button class="carousel-control-prev" type="button" data-bs-target="#carouselExample" data-bs-slide="prev">
      <span class="carousel-control-prev-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Previous</span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#carouselExample" data-bs-slide="next">
      <span class="carousel-control-next-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Next</span>
    </button>
  </div>
</template>