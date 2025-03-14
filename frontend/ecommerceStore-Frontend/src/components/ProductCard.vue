<script setup>

const props = defineProps({
  products:{
    type:Array,
    required:true
  },
});

</script>

<template>
<div v-for="product in products" :key="product.id" class="card mx-2 my-2 text-center" style="width: 18rem;">
  <div class="card-body">
    <img :src="product.imageUrl" class="card-img-top" alt="...">
  </div>
  <div class="card-footer">
    <h5 class="card-title">{{ product.name }}</h5>
  </div>
  <div class="card-footer">
    <p>{{ product.price }}</p>
  </div>
  <div class="card-footer">
    <button>Add to Cart</button>
  </div>
</div>
</template>