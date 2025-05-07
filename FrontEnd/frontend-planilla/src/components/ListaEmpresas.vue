<template>
    <div class="container mt-5" style="padding-top: 2%;">
      <button class="btn btn-primary" @click="showModal">
        Agregar empresa
      </button>
      <ModalRegistrarEmpresa ref="modalRef" />
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import ModalRegistrarEmpresa from './modals/ModalRegistrarEmpresa.vue'
  const router = useRouter()
  
  const modalRef = ref(null)

  onMounted(async () => {
    const token = localStorage.getItem("jwtToken");
    
    if (!token) {
      alert('Tiene que iniciar sesión primero.');
      setTimeout(() => {
        router.push('/login');
      }, 2000);
    }
  })
  
  function showModal() {
    modalRef.value?.show()
  }
  </script>