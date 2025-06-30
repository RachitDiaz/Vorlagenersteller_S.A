<template>
  <div class="modal fade" id="modalEditarEmpleado" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Editar datos del empleado</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>

        <div class="modal-body">
          <form @submit.prevent="submitForm">
            <div class="mb-3">
              <label class="form-label">
                Nombre
                <span v-if="errores.nombre" class="text-danger">*</span>
              </label>
              <input
                v-show="idEditable"
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.nombre }"
                v-model="infoEmpleado.empleado.nombre"
                required
              />
              
              <input
                v-show="!idEditable"
                disabled
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.nombre }"
                v-model="infoEmpleado.empleado.nombre"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Primer Apellido
                <span v-if="errores.apellido1" class="text-danger">*</span>
              </label>
              <input
                v-show="idEditable"
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.apellido1 }"
                v-model="infoEmpleado.empleado.apellido1"
                required
              />

              <input
                v-show="!idEditable"
                disabled
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.apellido1 }"
                v-model="infoEmpleado.empleado.apellido1"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Segundo Apellido
                <span v-if="errores.apellido2" class="text-danger">*</span>
              </label>
              <input
                v-show="idEditable"
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.apellido2 }"
                v-model="infoEmpleado.empleado.apellido2"
                required
              />

              <input
                v-show="!idEditable"
                disabled
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.apellido2 }"
                v-model="infoEmpleado.empleado.apellido2"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Género
                <span v-if="errores.genero" class="text-danger">*</span>
              </label>
              <select
                class="form-select"
                :class="{ 'is-invalid': errores.genero }"
                v-model="infoEmpleado.genero"
                required
              >
                <option>Masculino</option>
                <option>Femenino</option>
                <option>Otro</option>
              </select>
            </div>

            <div class="mb-3">
              <label class="form-label">
                Correo electrónico del empleado
                <span v-if="errores.correo" class="text-danger">*</span>
              </label>
              <input
                type="email"
                class="form-control"
                :class="{ 'is-invalid': errores.correo }"
                v-model="infoEmpleado.correo"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Cédula del empleado
                <span v-if="errores.cedula" class="text-danger">*</span>
              </label>
              <input
                v-show="idEditable"
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.cedula }"
                placeholder="x-xxxx-xxxx"
                v-model="infoEmpleado.empleado.cedulaEmpleado"
                required
              />

              <input
                v-show="!idEditable"
                disabled
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.cedula }"
                placeholder="x-xxxx-xxxx"
                v-model="infoEmpleado.empleado.cedulaEmpleado"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Cuenta IBAN del banco
              </label>
              <input
                type="text"
                class="form-control"
                v-model="infoEmpleado.empleado.banco"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Salario Bruto
              </label>
              <input
                type="number"
                class="form-control"
                min="0"
                step=".01"
                v-model="infoEmpleado.empleado.salarioBruto"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Tipo de contrato
                <span v-if="errores.tipoContrato" class="text-danger">*</span>
              </label>
              <select
                class="form-select"
                :class="{ 'is-invalid': errores.tipoContrato }"
                v-model="infoEmpleado.empleado.tipoContrato"
                required
              >
                <option>Tiempo completo</option>
                <option>Medio tiempo</option>
                <option>Servicios profesionales</option>
              </select>
            </div>

            <p v-if="mensajeError" class="text-danger fw-bold text-center mb-3">
              {{ mensajeError }}
            </p>

            <div class="d-flex justify-content-end gap-2">
              <button type="submit" class="btn btn-primary">Aceptar</button>
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
/* global defineEmits */

import { ref, reactive, defineExpose, onMounted } from 'vue'
import Modal from 'bootstrap/js/dist/modal'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { backendURL } from '../../config/config.js'

const emit = defineEmits(['empleado-agregado'])
const token = localStorage.getItem("jwtToken");
const router = useRouter();
const modalRef = ref(null)
let modalInstance = null
let cedulaParametro =  ""
let idEditable = false

const infoEmpleado =  reactive({
  empleado: {
  cedulaEmpleado: "",
  cedulaEmpresa: "",
  nombre: "",
  apellido1: "",
  apellido2: "",
  banco: "",
  salarioBruto: 0,
  tipoContrato: ""
  },
  genero: "",
  correo: "",
  cedulaEditable: true
})




onMounted(() => {
  modalInstance = new Modal(modalRef.value)
})

function show(cedulaBusqueda) {
  modalInstance?.show()
  
  try {
    if (!token) {
      alert('Tiene que iniciar sesión primero.');
      setTimeout(() => {
        router.push('/login');
      }, 2000);
      return;
    }

    axios.get(`${backendURL}Empleado/GetInfoEmpleadoCedula/${cedulaBusqueda}`, {
      headers: {
        Authorization: `Bearer ${token}`
      },
    }).then((response) => {
      infoEmpleado.empleado = response.data.empleado;
      infoEmpleado.genero = response.data.genero;
      infoEmpleado.correo = response.data.correo;
      infoEmpleado.cedulaEditable = response.data.cedulaEditable;
      cedulaParametro = infoEmpleado.empleado.cedulaEmpleado;
      infoEmpleado.empleado.cedulaEmpleado = cedulaParametro.substring(0, cedulaParametro.length - 1);
      idEditable = infoEmpleado.cedulaEditable;
    });
  } catch (error) {
    if (error.response && error.response.status === 401) {
      localStorage.removeItem('jwtToken');
      alert('Sesión expirada o token inválido.');
      router.push('/login');
    } else {
      console.error('Error cargando empleados:', error);
      alert('Error al cargar los empleados desde el servidor.');
    }
  }
}

defineExpose({ show })


const errores = reactive({
  correo: false,
  cedula: false,
  tipoContrato: false,
  nombre: false,
  apellido1: false,
  apellido2: false,
  genero: false
})

const mensajeError = ref('')

function validarCampos() {
  errores.cedula = !infoEmpleado.empleado.cedulaEmpleado.match(/^\d-\d{4}-\d{4}$/)
  errores.tipoContrato = !infoEmpleado.empleado.tipoContrato
  errores.nombre = !infoEmpleado.empleado.nombre
  errores.apellido1 = !infoEmpleado.empleado.apellido1
  errores.apellido2 = !infoEmpleado.empleado.apellido2
  errores.genero = !infoEmpleado.genero

  return !(errores.correo || errores.contrasena || errores.cedula || errores.tipoContrato || errores.nombre || errores.apellido1 || errores.apellido2 || errores.genero)
}

function submitForm() {
  mensajeError.value = ''

  if (!validarCampos()) {
    mensajeError.value = 'Los campos marcados con * deben de ser rellenados'
    return
  }

  axios.post(`${backendURL}Empleado/EditarInfoEmpleado`, {
    infoEmpleado: infoEmpleado,
    cedulaAEditar: cedulaParametro
  }, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  })
  .then(() => {
    alert('Empleado editado con éxito')
    modalInstance.hide()
    emit('empleado-editado')
    window.location.href = "/ListaEmpleados";
  })
  .catch(error => {
    console.error("Error al editar empleado:", error);
  })
}
</script>

<style scoped>
.is-invalid {
  border-color: red;
  background-color: #ffe5e5;
}
</style>
