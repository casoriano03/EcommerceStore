<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';

const reviewArray = ref([]);
const userId = ref(null);
const token = sessionStorage.getItem("authToken");

onMounted(async()=>{
    try {
        const payload = getTokenPayLoad(token);
        if (payload) {
            userId.value = parseInt(payload.nameid);
        }
        const response = await axios.get('https://localhost:7023/api/Review/GetReviewsByUserId', {
            params:{userId:userId.value}
        });
        reviewArray.value=response.data
    } catch (error) {
        console.log(error)
    }
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
    <div>
        <div class="mx-5 my-5 px-5">
            <div class="my-5">
                <h3>Reviews Given:</h3>
            </div>
            <div v-for="review in reviewArray" :key="review.id" class="border rounded my-2 px-3 py-3 reviewContainer">
                <h5>Product: {{ review.product.name }}</h5>
                <p>Rating: {{ review.rating }} ⭐</p>
                <p>Date: {{ review.created }}</p>
                <p class="lead fst-italic">"{{ review.comment }}"</p>
            </div>
        </div>
    </div>
</template>