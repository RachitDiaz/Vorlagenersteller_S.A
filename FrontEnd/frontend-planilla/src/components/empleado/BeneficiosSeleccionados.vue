<template>
  <div class="benefits-container">
    <h2 class="title">Beneficios</h2>
    <p class="subtitle">Seleccionar {{ selected.length }}/{{ max }} de la lista</p>

    <div class="lists-wrapper">
      <div class="list">
        <h3>Beneficios elegidos</h3>
        <div class="benefit-list">
          <div v-for="(benefit, index) in selected" :key="benefit.id" class="benefit-item">
            <span>{{ benefit.name }}</span>
            <button class="button-gray" @click="removeBenefit(index)">Retirar</button>
          </div>
        </div>
      </div>

      <div class="list">
        <h3>Beneficios disponibles</h3>
        <div class="benefit-list">
          <div v-for="benefit in availableBenefits" :key="benefit.id" class="benefit-item">
            <span>{{ benefit.name }}</span>
            <button class="button-gray" :disabled="selected.length >= max" @click="addBenefit(benefit)">
              Agregar
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="actions">
      <button class="button-dark" @click="cancel">Cancelar</button>
      <button class="button-purple" @click="accept">Aceptar</button>
    </div>
  </div>
  <div v-if="showModal" class="custom-modal-overlay">
  <div class="custom-modal">
    <h3>Dependientes para MediSeguro</h3>
    <label for="dependientesInput">Ingrese la cantidad de dependientes</label>
    <input
      id="dependientesInput"
      v-model="dependientes"
      type="number"
      min="0"
      class="modal-input"
    />
    <div class="modal-actions">
      <button @click="cancelarModal" class="button-dark">Cancelar</button>
      <button @click="aceptarModal" class="button-purple">Aceptar</button>
    </div>
  </div>
</div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { backendURL } from '../../config/config.js'
import Modal from 'bootstrap/js/dist/modal.js'

const router = useRouter()
const max = 3

const allBenefits = ref([])
const selected = ref([])
const showModal = ref(false)
const dependientes = ref(0)
let modalInstance = null

const availableBenefits = computed(() =>
  allBenefits.value.filter(b =>
    !selected.value.find(s => s.id === b.id) &&
    b.name !== "TSE" &&
    b.name !== "Registro Nacional"
  )
)

onMounted(async () => {
  
  const token = localStorage.getItem("jwtToken")
  if (!token) {
    alert("Tiene que iniciar sesión primero.")
    setTimeout(() => router.push("/login"), 2000)
    return
  }
  const headers = { Authorization: `Bearer ${token}` }
  try {
    const { data: disponibles } = await axios.get(`${backendURL}BeneficioEmpleado/listar`, { headers })
    allBenefits.value = disponibles
      .filter(b => b.nombre !== "TSE" && b.nombre !== "Registro Nacional")
      .map(b => ({ id: b.id, name: b.nombre }))

    const { data: elegidos } = await axios.get(`${backendURL}BeneficioEmpleado/elegidos`, { headers })
    selected.value = elegidos
      .filter(b => b.nombre !== "TSE" && b.nombre !== "Registro Nacional")
      .map(b => ({ id: b.id, name: b.nombre }))
  } catch (error) {
    console.error("Error cargando beneficios:", error)
    alert("No se pudieron cargar los beneficios.")
  }
})


function addBenefit(benefit) {
  if (selected.value.length < max) {
    selected.value.push(benefit)

    if (benefit.name.trim().toLowerCase() === "mediseguro") {
      dependientes.value = 0
      showModal.value = true
    }
  }
}

async function aceptarModal() {
  const token = localStorage.getItem("jwtToken")
  if (!token) {
    alert("Debe iniciar sesión primero.")
    return
  }
  if (dependientes.value < 0) {
    alert("Debe ingresar al menos un dependiente.")
    return
  }
  const headers = {
    Authorization: `Bearer ${token}`
  }

  try {
    const { data } = await axios.post(
      `${backendURL}BeneficioEmpleado/ActualizarDependientes/${dependientes.value}`,
      {}, 
      { headers }
    )

    if (data.exito) {
      alert("Dependientes actualizados correctamente.")
      showModal.value = false
    } else {
      alert("No se pudieron actualizar los dependientes.")
    }
  } catch (error) {
    console.error("Error al actualizar dependientes:", error)
    alert("Ocurrió un error al enviar los dependientes.")
  }
}

function cancelarModal() {
  dependientes.value = 0
  showModal.value = false

  const index = selected.value.findIndex(b => b.name.trim().toLowerCase() === "mediseguro")
  if (index !== -1) {
    selected.value.splice(index, 1)
  }
}

function removeBenefit(index) {
  selected.value.splice(index, 1)
}

function cancel() {
  console.log('Cancelado')
}

async function accept() {
  const token = localStorage.getItem("jwtToken")
  const headers = {
    Authorization: `Bearer ${token}`,
    "Content-Type": "application/json"
  }

  try {
    const { data } = await axios.post(
      `${backendURL}BeneficioEmpleado/actualizar`,
      { beneficios: selected.value.map(b => b.id) },
      { headers }
    )

    if (data.exito) {
      alert("Beneficios actualizados exitosamente.")
    } else {
      alert("Hubo un error al actualizar los beneficios.")
    }
  } catch (err) {
    console.error("Error en solicitud:", err)
    alert("Error al conectar con el servidor.")
  }
}
</script>


<style scoped>
.benefits-container {
  max-width: 900px;
  margin: 0 auto;
  text-align: center;
  padding: 1rem;
}

.title {
  font-size: 1.5rem;
  font-weight: bold;
}

.subtitle {
  margin-bottom: 1.5rem;
  color: #666;
}

.lists-wrapper {
  display: flex;
  justify-content: space-between;
  gap: 2rem;
  flex-wrap: wrap;
}

.list {
  flex: 1;
  min-width: 300px;
  border: 1px solid #ddd;
  border-radius: 0.75rem;
  padding: 1rem;
}

.benefit-list {
  margin-top: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.benefit-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.5rem;
  padding: 0.5rem;
  border: 1px solid #eee;
  border-radius: 0.5rem;
}

.benefit-icon {
  width: 40px;
  height: 40px;
}

.button-gray {
  background-color: #eee;
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 0.5rem;
  cursor: pointer;
}

.button-gray:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.actions {
  margin-top: 2rem;
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.button-dark {
  background-color: #222;
  color: white;
  border: none;
  padding: 0.6rem 1.5rem;
  border-radius: 1rem;
}

.button-purple {
  background-color: #7d1fff;
  color: white;
  border: none;
  padding: 0.6rem 1.5rem;
  border-radius: 1rem;
}

.custom-modal-overlay {
  position: fixed;
  inset: 0;
  background-color: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.custom-modal {
  background: white;
  padding: 2rem;
  border-radius: 1rem;
  max-width: 400px;
  width: 90%;
  text-align: center;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
}

.modal-input {
  margin-top: 1rem;
  width: 100%;
  padding: 0.5rem;
  font-size: 1rem;
  border: 1px solid #ccc;
  border-radius: 0.5rem;
}

.modal-actions {
  display: flex;
  justify-content: space-between;
  margin-top: 1.5rem;
}
</style>