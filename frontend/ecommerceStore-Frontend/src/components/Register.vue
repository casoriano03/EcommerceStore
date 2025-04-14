<script setup>
import axios from 'axios';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import Toasts from './Toasts.vue';

const router = useRouter();
const confirmPassword = ref("");
const showPasswordWarning = ref(false);
const toastMessage = ref('Registration successful!');
const toastRef = ref(null);

const registerDetails = ref({
    firstName:"",
    lastName:"",
    email:"",
    password:""
})

const register = async() =>{
    try {
      if (registerDetails.value.password.toLocaleLowerCase()===confirmPassword.value.toLocaleLowerCase()) {
        const response = await axios.post("https://localhost:7023/api/Auth/register", registerDetails.value);
        toastRef.value.showToast();
        router.push("/login")
      } else {
        registerDetails.value.firstName = "";
        registerDetails.value.lastName = "";
        registerDetails.value.email = "";
        registerDetails.value.password = "";
        confirmPassword.value = "";
        showPasswordWarning.value = true;
      }
    } catch (error) {
        registerDetails.value.firstName = "";
        registerDetails.value.lastName = "";
        registerDetails.value.email = "";
        registerDetails.value.password = "";
        confirmPassword.value = "";
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
    <p v-if="showPasswordWarning" class="text-danger">Oops! It looks like the passwords don’t match. Please double-check and try again.</p>
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
    <div class="form-floating my-1">
      <input type="password" v-model="confirmPassword" class="form-control" id="floatingPassword" placeholder="Confirm Password">
      <label for="floatingPassword">Confirm Password</label>
    </div>
    <button class="btn btn-primary py-2 my-5" type="submit">Register</button>
  </form>
</div>
</body>
<Toasts ref="toastRef" :message="toastMessage" />
</template>