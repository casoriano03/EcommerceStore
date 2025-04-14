<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';

const token = sessionStorage.getItem('authToken');
const userId = ref(null);
const userFirstName = ref(null);
const userLastName = ref(null);
const userEmail = ref(null);
const userAddress = ref(null);
const userZipCode = ref(null);
const userCity = ref(null);
const userPhone = ref(null);
const products = ref([]);
const total = ref(null);
const orderId = ref(null);

const shipping = ref({
    orderId: null,
    name: null,
    address: null,
    zipCode: null,
    city: null,
    phone: null
});

onMounted(async()=>{
    const payload = getTokenPayLoad(token);
    if (payload) {
        userId.value = parseInt(payload.nameid);
        userFirstName.value = payload.given_name;
        userLastName.value = payload.family_name;
        userEmail.value = payload.email;
    }

    const userOrder = await axios.get(`https://localhost:7023/api/Cart/GetCartItemByUserId?id=${userId.value}`);
    products.value = userOrder.data;
    products.value.forEach(product => {
        total.value += product.quantity*product.product.price
    });
});

const toPayment = async()=>{
    shipping.value.name = `${userFirstName.value} ${userLastName.value}`;
    shipping.value.address = userAddress.value;
    shipping.value.zipCode = userZipCode.value;
    shipping.value.city = userCity.value;
    shipping.value.phone = userPhone.value;

    sessionStorage.setItem('shippingDetails', JSON.stringify(shipping.value));
  
    const paymentResult = await axios.post(`https://localhost:7023/api/Stripe/create-checkout-session?userId=${userId.value}`);
    window.location.href = paymentResult.data.sessionUrl;
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
    <div class="container">
        <RouterLink to="/shop"><button class="btn btn-outline-dark my-5 cartButton">Continue Shopping</button> </RouterLink>
        <h1 class="text-center">Shipping Details</h1>
        <div class="row">
            <h3 class="mt-5 mb-3">User Details</h3>
            <p>Name: {{ userFirstName }} {{ userLastName }}</p>
            <p>Email: {{ userEmail }}</p>
            <label for="address" class="form-label">Address: </label>
            <input type="text" v-model="userAddress" class="form-control" id="address">
            <label for="zipCode" class="form-label">Zip Code: </label>
            <input type="text" v-model="userZipCode" class="form-control" id="zipCode">
            <label for="city" class="form-label">City: </label>
            <input type="text" v-model="userCity" class="form-control" id="city">
            <label for="phone" class="form-label">Phone: </label>
            <input type="text" v-model="userPhone" class="form-control" id="phone">
        </div>
        <div class="row text-center my-5">
            <div class="col ">Product Name</div>
            <div class="col ">Quantity</div>
            <div class="col ">Price</div>
            <div class="col ">Total</div>
        </div>
        <div v-for="cart in products" :key="cart" class="row my-5">
            <div class="col border-top "><div class="d-flex"><div class="mx-2"><img :src="cart.product.imageUrl" style="width: 3rem;" alt=""></div><div>{{ cart.product.name }}</div></div></div>
            <div class="col border-top text-center">{{ cart.quantity }}</div>
            <div class="col border-top text-center">{{ cart.product.price }}</div>
            <div class="col border-top text-center">{{ cart.product.price*cart.quantity }}</div>
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
            <div class="col text-center"><button class="btn btn-outline-dark align-end cartButton" @click="toPayment">To Payment</button></div>
        </div>
    </div>
</template>