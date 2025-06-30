<template>
  <div class="modal fade" id="myModal"
    tabindex="-1" aria-labelledby="modalLabel"
    aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Eliminar empleado</h5>
          <button type="button" class="btn-close"
            data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div v-if="mensajeError" class="alert alert-danger" role="alert">
          {{ mensajeError }}
        </div>
        <div class="modal-body">
          ¿Estás seguro que deseas eliminar el empleado {{cedulaEmpleado}}?
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
const isInitializing = ref(false);

const cedulaEmpleado = ref('');

onMounted(() => {
  modalInstance = new Modal(modalRef.value);
});
/**
 *  Mostrar confirmación para eliminar el empleado
 * @param {String} cedula del empleado a eliminar
 */
function show(cedula) {
  isInitializing.value = true;
  mensajeError.value = '';
  cedulaEmpleado.value = cedula;
  setTimeout(() => {
    isInitializing.value = false;
  }, 0);
  modalInstance?.show();
}

/**
 * Function para enviar info a backend y modificar el beneficio
 */
function eliminarEmpleado() {
  axios.delete(`${backendURL}Empleado/EliminarEmpleado?cedulaEmpleado=${cedulaEmpleado.value}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  }).then(() => {
    window.location.href = '/ListaEmpleados';
  }).catch((error) => {
    console.error('Error al eliminar empleado:', error);
    if (error.response && error.response.data) {
      mensajeError.value = error.response.data;
    } else {
      mensajeError.value =
        'Ocurrió un error al eliminar el empleado. Intente de nuevo.';
    }
  });
}
defineExpose({show});
</script>
