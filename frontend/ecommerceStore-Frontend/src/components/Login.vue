<script setup>
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const loginDetails = ref({
    email:"",
    password:""
});

const router = useRouter();

const login = async()=>{
    try {
        const response = await axios.post('https://localhost:7023/api/Auth/login', loginDetails.value)
        sessionStorage.setItem("authToken", response.data)
        router.push("/")
        setTimeout(()=>{
          window.location.reload();
        }, 200);
    } catch (error) {
        console.log(error)
    }
};

const confirmChangePass = async()=>{
    const confirmation = prompt("Confirm change password? Enter email address");
    if (confirmation) {
        const response = await axios.get("https://localhost:7023/api/Auth/Random_Password", {
            params: {userEmailInput:confirmation}
        });
        console.log(response.data)
        router.push("/changePassword")
    }
};

</script>

<template>
    <body class="container d-flex align-items-center py-5">
<div class="form-signin w-25 m-auto">
  <form @submit.prevent="login">
    <div class="text-center">
        <img class="mb-4 rounded-circle" src="./icons/logo.png" alt="" width="100" height="100">
    </div>

    <h1 class="h3 mb-3 fw-normal">Please sign in</h1>

    <div class="form-floating my-1">
      <input type="email" v-model="loginDetails.email" class="form-control" id="floatingInput" placeholder="name@example.com">
      <label for="floatingInput">Email address</label>
    </div>
    <div class="form-floating my-1">
      <input type="password" v-model="loginDetails.password" class="form-control" id="floatingPassword" placeholder="Password">
      <label for="floatingPassword">Password</label>
    </div>
    <div class="my-3">
      <p class="btn" @click="confirmChangePass">Forgot Password? Click here!</p>
    </div>

    <div class="my-3">
        <p>Don't have an account yet?</p>
        <RouterLink to="/register">Register</RouterLink>
    </div>
    <button class="btn btn-primary py-2" type="submit">Sign in</button>
  </form>
</div>
</body>
</template>