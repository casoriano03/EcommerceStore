<script setup>
import { ref, defineEmits } from 'vue';
import axios from 'axios';
import Toasts from './Toasts.vue';

const props = defineProps({
    product: {
        type:Object,
    },
});

const modalFunction = ref(null);
const productName = ref(null);
const productDescription = ref(null);
const productPrice = ref(null);
const productInventory = ref(null);
const productImageUrl = ref(null);
const productCategory = ref(null);
const productId = ref(null);
const toastMessage = ref('');
const toastRef = ref(null);
const newProduct = ref({
  name:null,
  description:null,
  price:null,
  inventory:null,
  imageUrl:null,
  categoryId:null
});
const isInputNull = ref(false);

const addProduct = async()=>{
  isInputNull.value = false;
  productValues();
  nullCheck();
  if (isInputNull.value === false) {
    try {
    const addingProduct = await axios.post('https://localhost:7023/api/Product/AddProduct', newProduct.value);
    nullProductValues();
    hideModal();
    window.location.reload();
    } catch (error) {
      console.log(error)
    } 
  } else {
    return
  }
};

const editProduct = async()=>{
  productValues();
  nullCheck();
  try {
    const editingProduct = await axios.put(`https://localhost:7023/api/Product/UpdateProduct?id=${productId.value}`, newProduct.value);
  } catch (error) {
    console.log(error)
  }
  hideModal();
  window.location.reload();
};


const productValues = ()=>{
  newProduct.value.name = productName.value;
  newProduct.value.description = productDescription.value;
  newProduct.value.price = productPrice.value;
  newProduct.value.inventory = productInventory.value;
  newProduct.value.imageUrl = productImageUrl.value;
  newProduct.value.categoryId = parseInt(productCategory.value);
};

const nullProductValues = ()=>{
  productName.value = null;
  productDescription.value = null;
  productPrice.value = null;
  productInventory.value = null;
  productImageUrl.value = null;
  productCategory.value = null;
};

const nullCheck = ()=>{
  if (newProduct.value.name === null) return isInputNull.value = true;
  if (newProduct.value.description === null) return isInputNull.value = true;
  if (newProduct.value.price === null) return isInputNull.value = true;
  if (newProduct.value.inventory === null) return isInputNull.value = true;
  if (newProduct.value.imageUrl === null) return isInputNull.value = true;
  if (newProduct.value.categoryId === null) return isInputNull.value = true;
};

const showModal = (modalFunc, product)=>{
  if (product) {
    modalFunction.value = modalFunc;
    productId.value = product.id;
    productName.value = product.name;
    productDescription.value = product.description;
    productPrice.value = product.price;
    productInventory.value = product.inventory;
    productImageUrl.value = product.imageUrl;
    productCategory.value = product.categoryId;
  } else {
    modalFunction.value = modalFunc;
  }
  modalFunction.value = modalFunc;
  const modal = document.getElementById('exampleModal');
  const modalInstance = bootstrap.Modal.getOrCreateInstance(modal);
  if (modalInstance) {
    modalInstance.show();
  }
};

const hideModal = ()=>{
  const modal = document.getElementById('exampleModal');
  const modalInstance = bootstrap.Modal.getOrCreateInstance(modal);
  if (modalInstance) {
    modalInstance.hide();
  }
};

defineExpose({hideModal,showModal,nullProductValues});
</script>

<template>
  <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h1 class="modal-title fs-5" id="exampleModalLabel">{{ modalFunction }} Product</h1>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
          <p class="text-danger" v-if="isInputNull">Please fill upp all up all input fields.</p>
          <div class="row my-1">
          <div class="col">Product Name: </div>
          <div class="col"><input type="text" v-model="productName" placeholder="Enter product name" required></div>
        </div>
        <div class="row my-1">
          <div class="col">Product Description: </div>
          <div class="col"><textarea type="text" v-model="productDescription" placeholder="Enter product description" style="width: 11.8rem;" required></textarea></div>
        </div>
        <div class="row my-1">
          <div class="col">Product Price: </div>
          <div class="col"><input type="number" v-model="productPrice" placeholder="Enter product price" required></div>
        </div>
        <div class="row my-1">
          <div class="col">Product Inventory: </div>
          <div class="col"><input type="number" v-model="productInventory" placeholder="Enter product inventory" required></div>
        </div>
        <div class="row my-1">
          <div class="col">Product Image Url: </div>
          <div class="col"><input type="text" v-model="productImageUrl" placeholder="Enter product image url" required></div>
        </div>
        <div class="row my-1">
          <div class="col">Product Category: </div>
          <div class="col">
            <select name="cars" id="cars" v-model="productCategory" style="width: 11.8rem;" required>
              <option value="2">Electronics</option>
              <option value="3">Fashion</option>
              <option value="4">Home & Kitchen</option>
              <option value="5">Sports & Outdoors</option>
              <option value="6">Toys & Games</option>
              <option value="7">Books</option>
            </select>
          </div>
        </div>  
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" @click="(modalFunction === 'Add' ? addProduct : editProduct)()">{{ modalFunction }} Product</button>
      </div>
    </div>
  </div>
</div>
<Toasts ref="toastRef" :message="toastMessage"/>
</template>