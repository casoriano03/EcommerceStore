<script setup>
import axios from 'axios';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const registerDetails = ref({
    firstName:"",
    lastName:"",
    email:"",
    password:""
})

const register = async() =>{
    try {
        const response = await axios.post("https://localhost:7023/api/Auth/register", registerDetails.value);
        router.push("/login")
    } catch (error) {
        console.log(error)
    }
}

</script>

<template>
     <body class="container d-flex align-items-center py-5">
<div class="form-signin w-25 m-auto">
  <form @submit.prevent="register">
    <div class="text-center">
        <img class="mb-4 rounded-circle" src="./icons/logo.png" alt="" width="100" height="100">
    </div>

    <h1 class="h3 mb-3 fw-normal">Register</h1>
    <div class="form-floating my-1">
      <input type="text" v-model="registerDetails.firstName" class="form-control" id="floatingInput" placeholder="name@example.com">
      <label for="floatingInput">First Name</label>
    </div>
    <div class="form-floating my-1">
      <input type="text" v-model="registerDetails.lastName" class="form-control" id="floatingInput" placeholder="name@example.com">
      <label for="floatingInput">Last Name</label>
    </div>
    <div class="form-floating my-1">
      <input type="email" v-model="registerDetails.email" class="form-control" id="floatingInput" placeholder="name@example.com">
      <label for="floatingInput">Email address</label>
    </div>
    <div class="form-floating my-1">
      <input type="password" v-model="registerDetails.password" class="form-control" id="floatingPassword" placeholder="Password">
      <label for="floatingPassword">Password</label>
    </div>
    <button class="btn btn-primary py-2 my-5" type="submit">Register</button>
  </form>
</div>
</body>
</template>