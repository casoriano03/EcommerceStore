<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { RouterLink } from 'vue-router';
import Toasts from './Toasts.vue';
import { globalState } from '@/composables/useGlobalStore';
import { jwtDecode } from 'jwt-decode';

const props = defineProps({
  products:{
    type:Array,
    required:true
  },
});

const userId = ref(null);
const toastMessage = ref('');
const toastRef = ref(null);
const token = sessionStorage.getItem('authToken');
const items = ref(0);
const total = ref(0);

onMounted(()=>{
  const payload = getTokenPayLoad(token);
  if (payload) {
    userId.value = payload.nameid;
  };
});

const addCartItem = async(productId, productName) =>{
  try {

    if (!userId.value) {
      toastMessage.value = "User needs to login to add items to cart."
      toastRef.value.showToast();
    } else {

      const cartItem = {
      "userId":userId.value,
      "productId":productId,
      "quantity":1
      };

      const productExist = globalState.carts.find(c => c.product.id == productId);

      if (productExist) {
        const response = await axios.put(`https://localhost:7023/api/Cart/EditQuantityToCart?id=${productExist.id}&newQuantity=${productExist.quantity + cartItem.quantity}`);
        toastMessage.value = `Successfully added ${productName} to cart`;
        toastRef.value.showToast();
      } else {
        const response = await axios.post('https://localhost:7023/api/Cart/AddCartItem', cartItem);
        toastMessage.value = `Successfully added ${productName} to cart`;
        toastRef.value.showToast();
      };
    };

    const fetchCarts = await axios.get('https://localhost:7023/api/Cart/GetCartItemByUserId',{
      params:{id:userId.value}
    });

    globalState.carts=fetchCarts.data;

    fetchCarts.data.forEach(item => {
      items.value += item.quantity;
      total.value += item.quantity*item.product.price
    });

    globalState.itemCount = items.value;
    globalState.cartTotal = total.value;

    items.value=0;
    total.value=0;

  } catch (error) {
    console.log(error)
  }
};

const getTokenPayLoad = (token)=>{
  if (token) {
    const decoded = jwtDecode(token);
    return decoded;
  }
    return null
};
</script>

<template>
<div class="d-flex justify-content-center mb-5">
<div v-for="product in products" :key="product.id" class="card mx-2 my-2 text-center" style="width: 18rem;">
  <div class="card-body">
    <img :src="product.imageUrl" class="card-img-top" alt="...">
  </div>
  <div class="card-footer">
    <RouterLink :to="`/product/${product.id}`"><h5 class="card-title">{{ product.name }}</h5></RouterLink> 
  </div>
  <div class="card-footer">
    <p>{{ product.price }}</p>
  </div>
  <div class="card-footer">
    <button class="btn btn-outline-dark addToCart" @click="addCartItem(product.id, product.name)">Add to Cart</button>
  </div>
</div>
</div>
<Toasts ref="toastRef" :message="toastMessage"/>
</template>