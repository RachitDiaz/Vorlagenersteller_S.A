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
                  Correo electrónico del empleado
                  <span v-if="errores.correo" class="text-danger">*</span>
                </label>
                <input
                  type="email"
                  class="form-control"
                  :class="{ 'is-invalid': errores.correo }"
                  v-model="form.correo"
                  required
                />
              </div>
  
              <div class="mb-3">
                <label class="form-label">
                  Contraseña
                  <span v-if="errores.contraseña" class="text-danger">*</span>
                </label>
                <input
                  type="password"
                  class="form-control"
                  :class="{ 'is-invalid': errores.contraseña }"
                  v-model="form.contraseña"
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
  import { ref, reactive, defineExpose, onMounted } from 'vue'
  import Modal from 'bootstrap/js/dist/modal'
  
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
    correo: '',
    contraseña: '',
    cedula: '',
    tipoContrato: ''
  })
  
  const errores = reactive({
    correo: false,
    contraseña: false,
    cedula: false,
    tipoContrato: false
  })
  
  const mensajeError = ref('')
  
  function validarCampos() {
    errores.correo = !form.correo.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)
    errores.contraseña = form.contraseña.length < 6 || form.contraseña.length > 30
    errores.cedula = !form.cedula.match(/^\d-\d{4}-\d{4}$/)
    errores.tipoContrato = !form.tipoContrato
  
    return !(errores.correo || errores.contraseña || errores.cedula || errores.tipoContrato)
  }
  
  function submitForm() {
    mensajeError.value = ''
  
    if (!validarCampos()) {
      mensajeError.value = 'Los campos marcados con * deben de ser rellenados'
      return
    }
  
    const empleadosExistentes = [
      { correo: 'ejemplo@empresa.com', cedula: '1-2345-6789' }
    ]
  
    const duplicado = empleadosExistentes.some(emp =>
      emp.correo === form.correo || emp.cedula === form.cedula
    )
  
    if (duplicado) {
      errores.correo = true
      errores.cedula = true
      mensajeError.value = 'El correo electrónico o la cédula ya está registrado en el sistema'
      return
    }
  
    setTimeout(() => {
      alert('Empleado registrado con éxito')
      modalInstance.hide()
    }, 1000)
  }
  </script>
  
  <style scoped>
  .is-invalid {
    border-color: red;
    background-color: #ffe5e5;
  }
  </style>
  