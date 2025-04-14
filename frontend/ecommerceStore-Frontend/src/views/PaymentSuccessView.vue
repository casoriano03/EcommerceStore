<script setup>
import axios from 'axios';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { jwtDecode } from 'jwt-decode';


const router = useRouter();
const userId = ref(null);
const carts = ref([]);
const token = sessionStorage.getItem('authToken');
const shippingDetails = sessionStorage.getItem('shippingDetails')
const totalAmount = ref(null);
const order = ref({
    userId:null,
    total:null
})

onMounted(async()=>{
    try {
        const payload = getTokenPayLoad(token);
        if (payload) {
            userId.value = parseInt(payload.nameid);
        };

        const fetchCarts = await axios.get(`https://localhost:7023/api/Cart/GetCartItemByUserId?id=${userId.value}`);
        carts.value = fetchCarts.data;
        carts.value.forEach(cart => {
            totalAmount.value += cart.quantity * cart.product.price;
        });

        order.value.total = totalAmount.value;
        order.value.userId = userId.value;
        const createOrder = await axios.post('https://localhost:7023/api/Order/CreateOrder', order.value);
        const getOrders = await axios.get(`https://localhost:7023/api/Order/GetOrdersByUserId?userId=${userId.value}`);
        console.log(getOrders.data)
        const getOrderId = getOrders.data.find(o => o.total == totalAmount.value)
        const parsedShippingDetails = JSON.parse(shippingDetails)
        parsedShippingDetails.orderId = getOrderId.id;
        const shippingOrder = await axios.post('https://localhost:7023/api/Shipping/CreateShippingOrder', parsedShippingDetails);

        fetchCarts.data.forEach(cart => {
            
        });
        
    } catch (error) {
        console.log(error)
    }
    setTimeout(()=>{
        router.push('/').then(() => {
        window.location.reload();
        });
    }, 2000);
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
 <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-5">
                <div class="message-box _success text-center">
                     <img src="https://media4.giphy.com/media/v1.Y2lkPTc5MGI3NjExenh4MWE3Z2E2cTUwazdoNG9xOXE2NWpqaWx1Mno2MmV5NmVnYXp1aiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/bRpa6DFERjTR9roQcQ/giphy.gif" style="width: 20rem;" alt="" class="my-5">
                    <h2 class="my-5"> Your payment was successful </h2>
                   <p class="lead my-5"> Thank you for your payment. We will be in contact with more details shortly </p>      
                </div> 
            </div> 
        </div> 
 </div>
</template>