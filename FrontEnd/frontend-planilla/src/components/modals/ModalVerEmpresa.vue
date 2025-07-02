<template>
  <div class="modal fade" id="modalVerEmpresa" tabindex="-1" ref="modalRef">
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Información de la empresa</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
        </div>
        <div class="modal-body" v-if="empresa && empresa.nombre">
          <div class="row">
            <div class="col-md-6">
              <h6><strong>Datos generales</strong></h6>
              <p><strong>Nombre:</strong> {{ empresa.nombre }}</p>
              <p><strong>Cédula jurídica:</strong> {{ empresa.cedulaJuridica }}</p>
              <p><strong>Estado:</strong> {{ empresa.activo ? 'Activo' : 'Inactivo' }}</p>
              <p><strong>Tipo de pago:</strong> {{ empresa.tipoDePago }}</p>
              <p><strong>Fecha creación:</strong> {{ empresa.fechaDeCreacion?.split('T')[0] }}</p>
            </div>
            <div class="col-md-6">
              <h6><strong>Información detallada</strong></h6>
              <p><strong>Descripción:</strong> {{ empresa.descripcion }}</p>
              <p><strong>Razón social:</strong> {{ empresa.razonSocial }}</p>
              <p><strong>Cédula del dueño:</strong> {{ empresa.cedulaDueno }}</p>
              <p><strong>Máximo de beneficios:</strong> {{ empresa.beneficiosMaximos }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, defineExpose, onMounted } from 'vue'
import Modal from 'bootstrap/js/dist/modal'
import axios from 'axios'
import { backendURL } from '../../config/config'
import { useRouter } from 'vue-router'

const router = useRouter()
const token = localStorage.getItem("jwtToken")
const modalRef = ref(null)
let modalInstance = null

const empresa = reactive({
  cedulaJuridica: '',
  nombre: '',
  razonSocial: '',
  descripcion: '',
  tipoDePago: '',
  cedulaDueno: '',
  beneficiosMaximos: '',
  fechaDeCreacion: '',
  activo: true,
})

onMounted(() => {
  modalInstance = new Modal(modalRef.value)
})

function show(cedulaJuridica) {
  if (!token) {
    alert('Debe iniciar sesión primero.')
    router.push('/login')
    return
  }

  axios.get(`${backendURL}Empresa/ObtenerEmpresa/${cedulaJuridica}`, {
    headers: { Authorization: `Bearer ${token}` }
  })
  .then((res) => {
    console.log("Datos de empresa recibidos:", res.data)
    Object.assign(empresa, res.data[0])
    modalInstance?.show()
  })
  .catch((err) => {
    console.error('Error al obtener la empresa:', err)
    alert('Error al cargar la información de la empresa.')
  })
}

defineExpose({ show })
</script>
