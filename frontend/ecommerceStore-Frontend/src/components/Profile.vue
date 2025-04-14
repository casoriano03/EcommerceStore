<script setup>
import { ref } from 'vue';
import { onMounted } from 'vue';
import { jwtDecode } from 'jwt-decode';

const token = sessionStorage.getItem("authToken");
const firstName = ref("");
const lastName = ref("");
const email = ref("");
const role = ref("");

onMounted(()=>{
    const payload = getTokenPayLoad(token);
    firstName.value = payload.given_name
    lastName.value = payload.family_name
    email.value = payload.email
    role.value = payload.role

    
})

const getTokenPayLoad = (token)=>{
  if (token) {
    const decoded = jwtDecode(token);
    return decoded;
  }
    return null
};

</script>

<template>
    <div class="px-4 py-5 my-5 text-center">
    <h1 class="display-5 fw-bold text-body-emphasis">{{firstName}} {{lastName}}</h1>
    <div class="col-lg-6 mx-auto">
      <p class="lead my-3">Email: {{ email }}</p>
      <p class="lead my-3">Role: {{ role }}</p>
    </div>
  </div>
</template>