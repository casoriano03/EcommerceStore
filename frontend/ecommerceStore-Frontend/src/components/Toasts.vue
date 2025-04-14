<script setup>
const props = defineProps({
  message:{
    type:String,
    required:true
  },
  autoHide: {
    type: Boolean,
    default: true
  },
  delay: {
    type: Number,
    default: 3000 
  },
});

const showToast = ()=>{
    const toastLiveExample = document.getElementById('liveToast');
    if (toastLiveExample) {
        const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample);
        toastBootstrap.show();
    }
};

defineExpose({ showToast });
</script>

<template>
<div class="toast-container position-fixed bottom-0 end-0 p-3">
    <div id="liveToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
        <div class="toast-header">
            <img src="./icons/logo.png" class="rounded me-2" alt="..." style="width: 1rem;">
            <strong class="me-auto">The Webshop</strong>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    <div class="toast-body">
      {{message}}
    </div>
  </div>
</div>
</template>