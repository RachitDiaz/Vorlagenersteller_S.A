<template>
  <div class="modal fade" id="modalVerEmpleado" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Información del empleado</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
        </div>

        <div class="modal-body" v-if="empleado && empleado.nombre">
          <div class="section-tittle">Datos generales</div>

          <div class="section-subtittle">Nombre del empleado</div>
          <div class="valor">{{ empleado.nombre }} {{ empleado.apellido1 }} {{ empleado.apellido2 }}</div>

          <div class="section-subtittle">Cédula del empleado</div>
          <div class="valor">{{ empleado.cedulaEmpleado }}</div>

          <div class="section-subtittle">Tipo de contrato</div>
          <div class="valor">{{ empleado.tipoContrato }}</div>

          <div class="section-subtittle">Salario bruto</div>
          <div class="valor">{{ empleado.salarioBruto }}</div>

          <div class="section-subtittle">Información bancaria</div>
          <div class="valor">{{ empleado.banco }}</div>

          <div class="section-subtittle">Género</div>
          <div class="valor">{{ genero }}</div>

          <div class="section-subtittle">Correo</div>
          <div class="valor">{{ correo }}</div>

          <div class="section-subtittle">¿Cédula editable?</div>
          <div class="valor">{{ cedulaEditable ? 'Sí' : 'No' }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, defineExpose, onMounted } from 'vue'
import Modal from 'bootstrap/js/dist/modal'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { backendURL } from '../../config/config.js'

const token = localStorage.getItem("jwtToken");
const router = useRouter();
const modalRef = ref(null)
let modalInstance = null

const empleado = reactive({
  cedulaEmpleado: "",
  cedulaEmpresa: "",
  nombre: "",
  apellido1: "",
  apellido2: "",
  banco: "",
  salarioBruto: 0,
  tipoContrato: ""
})
const genero = ref("-")
const correo = ref("-")
const cedulaEditable = ref(false)

onMounted(() => {
  modalInstance = new Modal(modalRef.value)
})

function show(cedulaBusqueda) {
  modalInstance?.show()

  if (!token) {
    alert('Debe iniciar sesión primero.')
    router.push('/login')
    return
  }

  axios.get(`${backendURL}Empleado/GetInfoEmpleadoCedula/${cedulaBusqueda}`, {
    headers: { Authorization: `Bearer ${token}` }
  })
  .then((response) => {
    Object.assign(empleado, response.data.empleado)
    genero.value = response.data.genero
    correo.value = response.data.correo
    cedulaEditable.value = response.data.cedulaEditable
  })
  .catch(error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('jwtToken')
      alert('Sesión expirada.')
      router.push('/login')
    } else {
      console.error('Error al obtener datos:', error)
    }
  })
}

defineExpose({ show })
</script>

<style scoped>
.section-tittle {
  font-weight: bold;
  font-size: 1.2rem;
  margin-bottom: 1rem;
}

.section-subtittle {
  font-weight: bold;
  font-size: 1rem;
  margin-top: 0.8rem;
  color: #666;
}

.valor {
  font-weight: normal;
  font-size: 1rem;
  margin-bottom: 0.5rem;
}
</style>
