<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import Review from './Review.vue';
import Toasts from './Toasts.vue';
import { globalState } from '@/composables/useGlobalStore';
import { jwtDecode } from 'jwt-decode';

const props = defineProps({
    product: {
        type:Object,
        required:true,
    }
});

const token = sessionStorage.getItem('authToken');
const items = ref(0);
const total = ref(0);
const isWishlisted = ref(false);
const userId = ref(null);
const quantity = ref(1);
const wishlistedProducts = ref([]);
const wishlistId = ref(null);
const showWarning = ref(false);
const toastMessage = ref('');
const toastRef = ref(null);
const addWishList = ref({
    userId:null,
    productId:null
});
const cartItem = ref({
            userId:null,
            productId: null,
            quantity: null
        });

onMounted(async()=>{
    const payload = getTokenPayLoad(token);
    if (payload) {
        userId.value = payload.nameid;

        const productWishlisted = await axios.get('https://localhost:7023/api/Wishlist/GetWishlistByUserId', {
            params:{
                userId:userId.value
            }
        });
        wishlistedProducts.value = productWishlisted.data;
    };
        wishlistedProducts.value.forEach(wishlist => {
            if (wishlist.productId === props.product.id) {
                isWishlisted.value = true;
                wishlistId.value = wishlist.id;
            }
        });
});

const addToWishList = async()=>{
    if (!userId.value) {
        showWarning.value=true;  
    } else {

        isWishlisted.value = !isWishlisted.value;
        addWishList.value.userId = parseInt(userId.value);
        addWishList.value.productId = props.product.id;
   
        try {
            if (isWishlisted.value===true) {
                const response = await axios.post('https://localhost:7023/api/Wishlist/AddProductToWishlist', addWishList.value);
                wishlistId.value = response.data.id
                toastMessage.value = `Successfully added ${props.product.name} to wishlist.`;
                toastRef.value.showToast(); 
                };
            if (isWishlisted.value===false) {
                const response = await axios.delete(`https://localhost:7023/api/Wishlist/RemoveProductFromWishlist?id=${wishlistId.value}`);
                toastMessage.value = `Successfully removed ${props.product.name} from wishlist.`;
                toastRef.value.showToast();
                };
        } catch (error) {
            console.log(error)
        };
    };
};

const addItemToCart = async()=>{
    try {
        cartItem.value.userId = userId.value;
        cartItem.value.productId = props.product.id;
        cartItem.value.quantity = quantity.value;

        if (!cartItem.value.userId) {
            toastMessage.value = "User needs to login to add items to cart.";
            toastRef.value.showToast();
        } else {

            const productExist = globalState.carts.find(c => c.product.id == props.product.id);
            
            if (productExist) {
                const response = await axios.put(`https://localhost:7023/api/Cart/EditQuantityToCart?id=${productExist.id}&newQuantity=${productExist.quantity + quantity.value}`);
                toastMessage.value = `Successfully added ${props.product.name} to cart`;
                toastRef.value.showToast();
            } else {
                const response = await axios.post('https://localhost:7023/api/Cart/AddCartItem', cartItem.value);
                toastMessage.value = `Successfully added ${props.product.name} to cart`;
                toastRef.value.showToast();
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
        };  
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
     <div>
        <div class="container row">
            <div class="col text-start d-flex align-items-center jusitfy-content-center mx-5">
                <div class="mx-5 my-5 px-5 border rounded productDetailsContainer">
                    <h1 class="my-5">{{ props.product.name }}</h1>
                    <p class="lead my-5">{{ props.product.description }}</p>
                    <h3 class="my-5">$ {{ props.product.price }}</h3>
                    <p v-if="showWarning" class="text-danger">Login to add this product to your wishlist</p>
                    <p class="fs-3" @click="addToWishList">{{ isWishlisted?"❤️":"🤍" }}</p>
                    <label for="productQuantityInput">Quantity:</label>
                    <br>
                    <input v-model="quantity" class="form-label productQuantityInput" type="number" name="productQuantityInput" id="">
                    <div>
                        <button class="btn btn-outline-dark my-5 productButton" @click="addItemToCart()">Add to Cart</button>
                    </div>
                    
                </div>
            </div>
            <div class="col my-5 text-center">
                <img :src="product.imageUrl" alt="" class="productImage">
            </div>
        </div>  
    </div>
    <Toasts ref="toastRef" :message="toastMessage"/>
    <Review/>
</template>