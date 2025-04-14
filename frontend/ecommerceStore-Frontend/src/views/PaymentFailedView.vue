<script setup>
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

onMounted(()=>{
    setTimeout(()=>{
          router.push('/')
        }, 2000);
});
</script>

<template>
 <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-5">
                <div class="message-box _success text-center">
                     <img class="my-5" src="https://media2.giphy.com/media/v1.Y2lkPTc5MGI3NjExM3hidDl2cXg1YnF6MzY1Mmg1amI4c2wyYjFkMGlpdWc3dWoxcjBlZSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/JT7Td5xRqkvHQvTdEu/giphy.gif" style="width: 20rem;" alt="">
                    <h2 class="my-5"> Your payment failed.</h2>
                   <p class="lead my-5"> Please try again later. </p>      
                </div> 
            </div> 
        </div> 
 </div>
</template>