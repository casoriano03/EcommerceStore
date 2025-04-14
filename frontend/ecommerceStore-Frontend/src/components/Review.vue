<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { useRoute } from 'vue-router';
import Toasts from './Toasts.vue';


const route = useRoute();
const productIdentifier = ref(null);
const token = sessionStorage.getItem("authToken");
const showReview = ref(false);
const userId = ref(null);
const userReviewInput = ref("");
const rating = ref(null);
const reviewArray = ref([]);
const reviewInput = ref({
    productId:null,
    userId:null,
    rating:null,
    comment:null,
});
const productRating = ref(null);
const reviewFirstName = ref("");
const toastMessage = ref('Thank you. Review added successfully!');
const toastRef = ref(null);

onMounted(async()=>{
    try {
        productIdentifier.value = route.params.productId;
        const payload = getTokenPayLoad(token);
        if (payload) {  
            reviewFirstName.value = payload.given_name;
            userId.value = parseInt(payload.nameid);
            showReview.value=true;
        }
        const response = await axios.get('https://localhost:7023/api/Review/GetReviewsByProductId', {
            params:{productId:productIdentifier.value}
        });
        reviewArray.value = response.data
        getProductRating();
        userRatedAlready();
    } catch (error) {
        console.log(error)
    }
});

const addReview = async()=>{
    reviewInput.value.productId=productIdentifier.value,
    reviewInput.value.userId=userId.value,
    reviewInput.value.rating=rating.value,
    reviewInput.value.comment=userReviewInput.value

    try {
        const response = await axios.post('https://localhost:7023/api/Review/CreateReview', reviewInput.value);
        const newReview = {
            ...reviewInput.value, 
            user: {firstName:reviewFirstName.value}, 
            created: new Date().toISOString()
        };
        reviewArray.value.push(newReview)
        userReviewInput.value = "";
        rating.value = null;
        toastRef.value.showToast();
    } catch (error) {
        console.log(error)
    }    
};

const getRating = (ratingInput)=>{
    rating.value = ratingInput;
};

const getTokenPayLoad = (token)=>{
  if (token) {
    const decoded = jwtDecode(token);
    return decoded;
  }
    return null
};

const getProductRating = ()=>{
    let sumRatings = 0;
    let reviewArrayLength = reviewArray.value.length;
    reviewArray.value.forEach(review => {
        sumRatings += review.rating;
    });

    let finalRating = (sumRatings/reviewArrayLength).toFixed(1);
    productRating.value=finalRating;
};

const userRatedAlready = ()=>{
    reviewArray.value.forEach(review => {
        if (review.userId === userId.value) {
            showReview.value = false;
        }
    });
};

</script>

<template>
     <div class="row">
            <div class="col mx-5 my-5">
                <h2 class="text-center">Real Experiences, Real Opinions – Hear It From Those Who Matter Most!</h2>
                <h3 v-if="productRating" class="mx-5 px-5 mt-5 my-3">Product Rating: ⭐ {{ productRating }}/5</h3>
                <div v-if="showReview" class="mx-5 px-5 border rounded addReviewContainer">
                    <h3 class="mt-5 my-3">Your Review:</h3>
                    <button v-for="num in 5" :key="num" type="button" :id="`button${ num }`"  class="btn my-3 mx-1" :class="{ 'btn-outline-dark': rating !== num, 'btn-dark active': rating === num }" @click="getRating(num)">{{ num }}</button>
                    <textarea v-model="userReviewInput" class="my-2 form-control w-100 h-150" placeholder="Write your review here" name="" id=""></textarea>
                    <br>
                    <button @click="addReview" class="btn btn-outline-dark my-1 mb-4">Post Review</button>
                </div>
                <div class="mx-5 my-5 px-5">
                    <div class="my-5">
                        <h3>Other People’s Reviews:</h3>
                    </div>
                    <div v-for="review in reviewArray" :key="review.id" class="border rounded my-2 px-3 py-3 reviewContainer">
                        <h5>Name: {{ review.user.firstName }}</h5>
                        <p>Rating: {{ review.rating }}</p>
                        <p>Date: {{ review.created }}</p>
                        <p class="lead fst-italic">"{{ review.comment }}"</p>
                    </div>
                    <div v-if="reviewArray.length===0" class="border rounded my-2 px-3 py-3 reviewContainer">
                        <h5>Product has no reviews.</h5>
                    </div>
                </div>
            </div>
        </div>
        <Toasts ref="toastRef" :message="toastMessage" />
</template>