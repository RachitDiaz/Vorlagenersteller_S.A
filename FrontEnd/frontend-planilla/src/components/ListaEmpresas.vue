<template>
    <div class="container mt-5" style="padding-top: 2%;">
      <button class="btn btn-primary" @click="showModal">
        Agregar empresa
      </button>
      <ModalRegistrarEmpresa ref="modalRef" />
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import ModalRegistrarEmpresa from './modals/ModalRegistrarEmpresa.vue'
  
  const modalRef = ref(null)
  
  function showModal() {
    modalRef.value?.show()
  }
  </script>