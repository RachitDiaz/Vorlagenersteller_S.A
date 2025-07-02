<template>
  <div class="planilla-container">
    <h2>Planilla</h2>
    <button @click="generarNuevaPlanilla">Nueva Planilla</button>

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

const planillas = ref([])
const token = localStorage.getItem("jwtToken")

function formatoMoneda(valor) {
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC',
    minimumFractionDigits: 0,
  }).format(valor)
}

async function cargarPlanillas() {
  try {
    const response = await axios.get(`${backendURL}Reportes/ObtenerUltimosPagosEmpresa`, {
      headers: { Authorization: `Bearer ${token}` }
    })

    const datos = response.data

    planillas.value = datos.map((item) => {
      const obligatorias = item.totalSEM + item.totalIVM + item.totalBP + item.totalAF + item.totalIMAS + item.totalINA + item.totalFCL + item.totalPC + item.totalINS
      const beneficios = item.beneficiosTotales
      const totalBruto = item.salariosTiempoCompleto
      const totalDeducciones = obligatorias + beneficios
      const totalNeto = totalBruto - totalDeducciones

      return {
        periodo: item.peridoPago,
        totalBruto,
        obligatorias,
        beneficios,
        totalDeducciones,
        totalNeto
      }
    })
  } catch (error) {
    console.error("Error al cargar datos de planilla:", error)
  }
}

function generarNuevaPlanilla() {
}

onMounted(() => {
  cargarPlanillas()
})
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
