<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'vue-router';
import Toasts from '@/components/Toasts.vue';

const router = useRouter();
const filteredCart = ref([]);
const token = sessionStorage.getItem('authToken');
const userId = ref(null);
const toastRef = ref(null);
const toastMessage = ref('');
const total = ref(0);
const items = ref(0);

onMounted(async()=>{
    try {
        const payload = getTokenPayLoad(token);
    if (payload) {
        userId.value = payload.nameid;
        const fetchCarts = await axios.get('https://localhost:7023/api/Cart/GetCartItemByUserId',{
            params:{id:userId.value}
        });

        fetchCarts.data.forEach(item => {
            items.value += item.quantity;
            total.value += item.quantity*item.product.price
            filteredCart.value.push(item)          
        });
    };
    } catch (error) {
        console.log(error)
    }
});

const changeCartQuantity = async(newQuantity, cart)=>{
    try {
        if (newQuantity < 1) {
            toastMessage.value = "Quantity cannot be less than 1";
            toastRef.value.showToast();
            return;
        }

        if (newQuantity > cart.product.inventory) {
            toastMessage.value = `Limited Stocks. Quantity cannot be more than ${cart.product.inventory}.`;
            toastRef.value.showToast();
            return;
        }
        const changeCartQuantity = await axios.put(`https://localhost:7023/api/Cart/EditQuantityToCart?id=${cart.id}&newQuantity=${newQuantity}`);   
        window.location.reload();
    } catch (error) {
        console.log(error)
    }
};

const deleteCart =async(cartId, productName)=>{
    console.log(cartId)
    try {
        const deleteCart = await axios.delete(`https://localhost:7023/api/Cart/DeleteCartItem?id=${cartId}`)
    } catch (error) {
        console.log(error)
    }
    toastMessage.value = `Successfully deleted ${productName} from cart.`;
    toastRef.value.showToast();
    window.location.reload();
};

const checkOut = async()=>{
    router.push('/shipping')
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
    <div class="container my-5">
        <RouterLink to="/shop"><button class="btn btn-outline-dark mb-5 cartButton">Continue Shopping</button> </RouterLink>
        <h3 class="text-center my-5">Your Cart ({{ items}})</h3>
        <h3 v-if="filteredCart.length===0">No items in the cart</h3>
        <div class="row text-center my-5">
            <div class="col ">Product Name</div>
            <div class="col ">Quantity</div>
            <div class="col ">Price</div>
            <div class="col ">Total</div>
        </div>
        <div v-for="cart in filteredCart" :key="cart" class="row my-5">
            <div class="col border-top "><div class="d-flex"><div class="mx-2"><img :src="cart.product.imageUrl" style="width: 3rem;" alt=""></div><div>{{ cart.product.name }}</div></div></div>
            <div class="col border-top text-center"><input  @change="changeCartQuantity($event.target.value, cart)" :value="cart.quantity" type="number" class="text-center"></div>
            <div class="col border-top text-center">{{ cart.product.price }}</div>
            <div class="col border-top text-end"><p class="text-center">{{ cart.product.price*cart.quantity }} </p><button @click="deleteCart(cart.id, cart.product.name)" class="btn btn-outline-secondary ms-5">x</button></div>
            
        </div>
        <div class="row my-5">
            <div class="col"></div>
            <div class="col"></div>
            <div class="col"></div>
            <div class="col">Grand Total: $ {{ total }} </div>
        </div>
        <div class="row my-5">
            <div class="col"></div>
            <div class="col"></div>
            <div class="col"></div>
            <div class="col text-center"><button class="btn btn-outline-dark align-end cartButton" @click="checkOut">Checkout</button></div>
        </div>
    </div>
    <Toasts ref="toastRef" :message="toastMessage"/>
</template>