<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import ProductModal from './ProductModal.vue';
import Toasts from './Toasts.vue';

const products = ref([]);
const filteredProducts = ref([]);
const searchInput = ref(null);
const toastMessage = ref('');
const toastRef = ref(null);
const modalRef = ref(null);




onMounted(async()=>{
    try {
        const fetchProducts = await axios.get('https://localhost:7023/api/Product/GetAllProducts');
        products.value = fetchProducts.data;
    } catch (error) {
        console.log(error)
    }
    
});

const searchProduct = ()=>{
    filteredProducts.value = products.value.filter(p =>
        p.name.toLowerCase().includes(searchInput.value.toLowerCase())
    );
};

const addProduct =()=>{
  modalRef.value.nullProductValues();
  modalRef.value.showModal('Add');
};

const editProduct =(product)=>{
  modalRef.value.showModal('Edit', product)
};

const deleteProduct = async(productId)=>{
  try {
    const deleteSuccessfull = await axios.delete(`https://localhost:7023/api/Product/DeleteProduct?id=${productId}`);
    toastMessage.value = deleteSuccessfull.data;
    toastRef.value.showToast();
    const product = products.value.findIndex(p => p.id == productId);
    products.value.splice(product,1)
  } catch (error) {
    console.log(error)
  }
};

</script>

<template>
    <div class="container">
      <div class="row my-5">
        <label for="searchInput">Search product here: <input type="text" v-model="searchInput" @input="searchProduct" name="searchInput" id="searchInput" style="width: 30%;"></label>
        <button class="btn btn-primary my-3" style="width: 10rem;" @click="addProduct()">Add Product</button>
      </div>
      <div class="row border" v-for="product in (searchInput==null?products:filteredProducts)" :key="product">
        <div class="col">
            <img :src="product.imageUrl" alt="image" style="width: 10rem;">
        </div>
        <div class="col">
            <h5>{{ product.name }}</h5>
            <p>Description: {{ product.description }} </p>
            <p>Price: ${{ product.price }}</p>
            <p>Inventory: {{ product.inventory }}</p>
        </div>
        <div class="col d-flex align-items-center justify-content-center justify-items-center">
            <button class="btn btn-outline-secondary" @click="editProduct(product)">✏️</button>
            <button class="btn btn-outline-danger" @click="deleteProduct(product.id)">🗑️</button>
        </div>
      </div>
    </div>
<Toasts ref="toastRef" :message="toastMessage"/>
<ProductModal ref="modalRef"/>
</template>