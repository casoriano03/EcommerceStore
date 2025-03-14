<script setup>
</script>

<template>
 <div class="container col-xxl-8 px-4 py-5">
    <div class="row flex-lg-row-reverse align-items-center g-5 py-5">
      <div class="col-10 col-sm-8 col-lg-6">
        <img src="./icons/logo.png" class="d-block mx-lg-auto img-fluid" alt="Bootstrap Themes" width="700" height="500" loading="lazy">
      </div>
      <div class="col-lg-6">
        <h1 class="display-5 fw-bold text-body-emphasis lh-1 mb-3">Discover, Shop, Delight – Your One-Stop Destination</h1>
        <p class="lead">From trendy fashion to must-have gadgets, we bring you the best products at unbeatable prices. Shop with ease, enjoy fast delivery, and experience customer service that truly cares. Your perfect purchase is just a click away!</p>
        <div class="d-grid gap-2 d-md-flex justify-content-md-start">
          <RouterLink to="/shop" type="button" class="btn btn-outline-dark btn-lg px-4 btnToShop">To Shop</RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>