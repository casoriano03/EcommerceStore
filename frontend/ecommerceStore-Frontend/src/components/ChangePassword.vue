<script setup>
import axios from 'axios';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const changePassDetails = ref({
    email:"",
    oldPassword:"",
    newPassword:"",
});

const changePassword = async()=>{
    const response = await axios.put("https://localhost:7023/api/Auth/Change_Password", changePassDetails.value)
    router.push("/login");
}

</script>

<template>
     <body class="container d-flex align-items-center py-5">
<div class="form-signin w-25 m-auto">
  <form @submit.prevent="changePassword">
    <div class="text-center">
        <img class="mb-4 rounded-circle" src="./icons/logo.png" alt="" width="100" height="100">
    </div>
    <div class="text-center">
        <h1 class="h3 mb-3 fw-normal">Change Password</h1>
        <p>Please check your email for a random generated password</p>
    </div>

    <div class="form-floating my-1">
      <input type="email" v-model="changePassDetails.email" class="form-control" id="floatingInput" placeholder="name@example.com">
      <label for="floatingInput">Email address</label>
    </div>
    <div class="form-floating my-1">
      <input type="password" v-model="changePassDetails.oldPassword" class="form-control" id="floatingPassword" placeholder="Password">
      <label for="floatingPassword">Generated Password</label>
    </div>
    <div class="form-floating my-1">
      <input type="password" v-model="changePassDetails.newPassword" class="form-control" id="floatingPassword" placeholder="Password">
      <label for="floatingPassword">New Password</label>
    </div>
    <button class="btn btn-primary py-2 my-5" type="submit">Change Password</button>
  </form>
</div>
</body>
</template>