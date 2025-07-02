<template>
  <div class="modal fade" id="myModal"
    tabindex="-1" aria-labelledby="modalLabel"
    aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Eliminar empresa</h5>
          <button type="button" class="btn-close"
            data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div v-if="mensajeError" class="alert alert-danger" role="alert">
          {{ mensajeError }}
        </div>
        <div class="modal-body">
          ¿Estás seguro que deseas eliminar la empresa?
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary"
            data-bs-dismiss="modal">
            Cancelar
          </button>

          <button type="button" class="btn btn-danger"
            @click="eliminarEmpleado">
            Eliminar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import {ref, onMounted, defineExpose} from 'vue';
import axios from 'axios';
import Modal from 'bootstrap/js/dist/modal';
import {backendURL} from '../../config/config.js';

const token = localStorage.getItem('jwtToken');
const modalRef = ref(null);
let modalInstance = null;
const mensajeError = ref('');

onMounted(() => {
  modalInstance = new Modal(modalRef.value);
});

function show() {
  modalInstance?.show();
}

/**
 * Function para enviar info a backend y modificar el beneficio
 */
function eliminarEmpleado() {
  axios.delete(`${backendURL}Empresa`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  }).then(() => {
    localStorage.removeItem('jwtToken');
    window.location.href = '/login';
  }).catch((error) => {
    if (error.response && error.response.data) {
      mensajeError.value = error.response.data;
    } else {
      mensajeError.value =
        'Ocurrió un error al eliminar la empresa. Intente de nuevo.';
    }
  });
}
defineExpose({show});
</script>
