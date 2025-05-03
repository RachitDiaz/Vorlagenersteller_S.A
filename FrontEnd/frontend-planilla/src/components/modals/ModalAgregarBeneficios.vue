<template>
  <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Beneficio nuevo</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitForm">

            <div class="mb-3">
              <label class="form-label">* Nombre del beneficio</label>
              <input
                v-model="form.nombre"
                type="text"
                class="form-control"
                required
                @invalid="mensajeNombreBeneficio"
                @input="borrarMensaje"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">* Tipo de cálculo</label>
              <div class="form-check">
                <input class="form-check-input" type="radio" name="tipo_calculo" v-model="form.tipo" value="API" id="tipoApi" />
                <label class="form-check-label" for="tipoApi">Servicio externo</label>
              </div>
              <div class="form-check">
                <input class="form-check-input" type="radio" name="tipo_calculo" v-model="form.tipo" value="Fijo" id="tipoFijo" />
                <label class="form-check-label" for="tipoFijo">Monto Fijo</label>
              </div>
              <div class="form-check">
                <input class="form-check-input" type="radio" name="tipo_calculo" v-model="form.tipo" value="Porcentaje" id="tipoPorcentaje" />
                <label class="form-check-label" for="tipoPorcentaje">Porcentaje del salario</label>
              </div>
            </div>

            <div class="mb-3" v-if="form.tipo">
              <label class="form-label">{{ nombreCampo }}</label>
              <input
                v-model="form.valor"
                :type="tipoEntrada"
                class="form-control"
                :textoTemporal="textoTemporal"
                required
              />
            </div>

            <button type="submit" class="btn btn-success">Submit</button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, defineExpose, computed } from 'vue'
import Modal from 'bootstrap/js/dist/modal'

const modalRef = ref(null)
let modalInstance = null

onMounted(() => {
  modalInstance = new Modal(modalRef.value)
})

function show() {
  modalInstance?.show()
}

function submitForm() {
  if (!form.nombre.trim()) {
    alert("El nombre del beneficio es obligatorio.")
    return
  }
  console.log(form)
}

function mensajeNombreBeneficio(event) {
  event.target.setCustomValidity("Debe ingresar el nombre del beneficio")
}

function borrarMensaje(event) {
  event.target.setCustomValidity("")
}

const tipoEntrada = computed(() => {
  return form.tipo === 'API' ? 'text' : 'number'
})

const textoTemporal = computed(() => {
  if (form.tipo === 'API') return 'URL del servicio externo'
  if (form.tipo === 'Fijo') return 'Ingrese el monto fijo (ej. 5000)'
  if (form.tipo === 'Porcentaje') return 'Ingrese el porcentaje (ej. 10)'
  return ''
})

const nombreCampo = computed(() => {
  if (form.tipo === 'API') return 'URL del Servicio Externo'
  if (form.tipo === 'Fijo') return 'Monto Fijo del Beneficio'
  if (form.tipo === 'Porcentaje') return 'Porcentaje del Salario'
  return ''
})

const form = reactive({
  nombre: '',
  tipo: '',
  valor: ''
})

defineExpose({ show })
</script>
