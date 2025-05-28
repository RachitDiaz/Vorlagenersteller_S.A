<template>
  <div class="modal fade" id="modalAgregarEmpleado" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Registrar nuevo empleado</h5>
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
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.nombre }"
                v-model="form.nombre"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Primer Apellido
                <span v-if="errores.apellido1" class="text-danger">*</span>
              </label>
              <input
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.apellido1 }"
                v-model="form.apellido1"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Segundo Apellido
                <span v-if="errores.apellido2" class="text-danger">*</span>
              </label>
              <input
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.apellido2 }"
                v-model="form.apellido2"
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
                v-model="form.genero"
                required
              >
                <option value="">Seleccione una opción</option>
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
                v-model="form.usuario.correo"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Contraseña
                <span v-if="errores.contrasena" class="text-danger">*</span>
              </label>
              <input
                type="password"
                class="form-control"
                :class="{ 'is-invalid': errores.contrasena }"
                v-model="form.usuario.contrasena"
                minlength="6"
                maxlength="30"
                required
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Cédula del empleado
                <span v-if="errores.cedula" class="text-danger">*</span>
              </label>
              <input
                type="text"
                class="form-control"
                :class="{ 'is-invalid': errores.cedula }"
                placeholder="x-xxxx-xxxx"
                v-model="form.cedula"
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
                v-model="form.banco"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">
                Salario Bruto
              </label>
              <input
                type="number"
                class="form-control"
                v-model="form.salarioBruto"
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
                v-model="form.tipoContrato"
                required
              >
                <option value="">Seleccione una opción</option>
                <option>Tiempo completo</option>
                <option>Medio tiempo</option>
                <option>Servicios profesionales</option>
              </select>
            </div>

            <p v-if="mensajeError" class="text-danger fw-bold text-center mb-3">
              {{ mensajeError }}
            </p>

            <div class="d-flex justify-content-end gap-2">
              <button type="submit" class="btn btn-primary">Registrar</button>
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
import { backendURL } from '../../config/config.js'

const emit = defineEmits(['empleado-agregado'])
const token = localStorage.getItem("jwtToken");
const modalRef = ref(null)
let modalInstance = null

onMounted(() => {
  modalInstance = new Modal(modalRef.value)
})

function show() {
  modalInstance?.show()
}

defineExpose({ show })

const form = reactive({
  usuario: {
    correo: '',
    contrasena: ''
  },
  cedula: '',
  nombre: '',
  apellido1: '',
  apellido2: '',
  genero: '',
  banco: '',
  salarioBruto: '',
  tipoContrato: ''
})

const errores = reactive({
  correo: false,
  contrasena: false,
  cedula: false,
  tipoContrato: false,
  nombre: false,
  apellido1: false,
  apellido2: false,
  genero: false
})

const mensajeError = ref('')

function validarCampos() {
  errores.correo = !form.usuario.correo.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)
  errores.contrasena = form.usuario.contrasena.length < 6 || form.usuario.contrasena.length > 30
  errores.cedula = !form.cedula.match(/^\d-\d{4}-\d{4}$/)
  errores.tipoContrato = !form.tipoContrato
  errores.nombre = !form.nombre
  errores.apellido1 = !form.apellido1
  errores.apellido2 = !form.apellido2
  errores.genero = !form.genero

  return !(errores.correo || errores.contrasena || errores.cedula || errores.tipoContrato || errores.nombre || errores.apellido1 || errores.apellido2 || errores.genero)
}

function submitForm() {
  mensajeError.value = ''

  if (!validarCampos()) {
    mensajeError.value = 'Los campos marcados con * deben de ser rellenados'
    return
  }

  const personaData = {
    cedula: form.cedula,
    nombre: form.nombre,
    apellido1: form.apellido1,
    apellido2: form.apellido2,
    genero: form.genero,
    usuario: {
      correo: form.usuario.correo,
      contrasena: form.usuario.contrasena
    }
  }

  const empleadoData = {
    cedulaEmpleado: form.cedula,
    cedulaEmpresa: '',
    nombre: form.nombre,
    apellido1: form.apellido1,
    apellido2: form.apellido2,
    banco: form.banco || null,
    salarioBruto: parseFloat(form.salarioBruto) || null,
    tipoContrato: form.tipoContrato
  }

  axios.post(`${backendURL}Empleado`, {
    persona: personaData,
    empleado: empleadoData
  }, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  })
  .then(() => {
    alert('Empleado registrado con éxito')
    modalInstance.hide()
    emit('empleado-agregado')
  })
  .catch(error => {
    console.error("Error al guardar empleado:", error);
  })
}
</script>

<style scoped>
.is-invalid {
  border-color: red;
  background-color: #ffe5e5;
}
</style>
