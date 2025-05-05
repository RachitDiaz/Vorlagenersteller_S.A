<template>
  <div class="container mt-5">
    <button class="btn btn-primary" @click="showModal">
      Agregar beneficio
    </button>
    <ModalAgregarBeneficios ref="modalRef" />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import ModalAgregarBeneficios from './modals/ModalAgregarBeneficios.vue'

const modalRef = ref(null)

function showModal() {
  modalRef.value?.show()
}
</script>
