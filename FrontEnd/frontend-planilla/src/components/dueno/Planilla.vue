<template>
  <div class="planilla-container">
    <h2>Planilla</h2>
    <button @click="agregarPlanilla">Generar Planilla</button>
    <p v-if="mensajeExito" style="color: green; margin-top: 10px;">{{ mensajeExito }}</p>
    <p v-if="mensajeError" style="color: red; margin-top: 10px;">{{ mensajeError }}</p>

    <table class="planilla-table">
      <thead>
        <tr>
          <th>Periodo</th>
          <th>Total Bruto</th>
          <th>Total Deducciones</th>
          <th>Total Neto</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in planillas" :key="index">
          <td>{{ item.periodo }}</td>
          <td>{{ formatoMoneda(item.totalBruto) }}</td>
          <td>
            {{ formatoMoneda(item.totalDeducciones) }}
            <div class="detalles-deducciones">
              Obligatorias: {{ formatoMoneda(item.obligatorias) }}<br />
              Beneficios: {{ formatoMoneda(item.beneficios) }}
            </div>
          </td>
          <td>{{ formatoMoneda(item.totalNeto) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { backendURL } from '../../config/config.js'

const token = localStorage.getItem("jwtToken")
const mensajeError = ref('')
const mensajeExito = ref('')
const planillas = ref([])
const token = localStorage.getItem("jwtToken")

function formatoMoneda(valor) {
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC',
    minimumFractionDigits: 0,
  }).format(valor)
}

const agregarPlanilla = async () => {
  mensajeError.value = ''
  mensajeExito.value = ''

  try {
    const response = await axios.post(`${backendURL}GenerarPlanilla`, {}, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })

    mensajeExito.value = '✅ Planilla generada exitosamente.'
  } catch (error) {
    if (error.response?.status === 400) {
      mensajeError.value = `⚠️ ${error.response.data.mensaje}`
    } else if (error.response?.status === 401) {
      mensajeError.value = '❌ No autorizado. Verifica tu sesión.'
    } else {
      mensajeError.value = '❌ Error al generar planilla.'
    }
    console.error(error)
  }
}
</script>

<style scoped>
.planilla-container {
  background: white;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 0 8px rgba(0, 0, 0, 0.1);
  max-width: 950px;
  margin: 0 auto;
}

.planilla-container h2 {
  font-size: 24px;
  margin-bottom: 16px;
}

.planilla-container button {
  margin-bottom: 16px;
  padding: 8px 16px;
  background-color: #357edd;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.planilla-container button:hover {
  background-color: #285bb5;
}

.planilla-table {
  width: 100%;
  border-collapse: collapse;
}

.planilla-table th,
.planilla-table td {
  border: 1px solid #ccc;
  padding: 12px;
  text-align: left;
  vertical-align: top;
}

.planilla-table thead {
  background-color: #f2f2f2;
}

.detalles-deducciones {
  font-size: 12px;
  color: #555;
  margin-top: 4px;
}
</style>

