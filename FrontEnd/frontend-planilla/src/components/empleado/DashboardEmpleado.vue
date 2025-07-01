<template>
  <div class="dashboard-container">
    <h2 class="dashboard-title">Resumen de pagos</h2>

    <div class="stats-cards">
      <div class="card">
        <p class="label">Salario bruto promedio</p>
        <p class="value">{{ formatCurrency(salarioBrutoPromedio) }}</p>
      </div>
      <div class="card">
        <p class="label">Salario neto promedio</p>
        <p class="value">{{ formatCurrency(salarioNetoPromedio) }}</p>
      </div>
      <div class="card">
        <p class="label">Deducciones totales</p>
        <p class="value">{{ formatCurrency(totalDeducciones) }}</p>
      </div>
      <div class="card">
        <p class="label">Número de pagos</p>
        <p class="value">{{ pagos.length }}</p>
      </div>
    </div>

    <h3 class="chart-title">Historial de pagos</h3>
    <canvas id="pagoChart"></canvas>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import Chart from 'chart.js/auto'

import { backendURL } from '../../config/config.js'

const pagos = ref([])

const salarioBrutoPromedio = ref(0)
const salarioNetoPromedio = ref(0)
const totalDeducciones = ref(0)

function formatCurrency(value) {
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC'
  }).format(value)
}

async function cargarPagos() {
  const token = localStorage.getItem("jwtToken")
  if (!token) {
    alert("Tiene que iniciar sesión primero.")
    setTimeout(() => router.push("/login"), 2000)
    return
  }
  const headers = { Authorization: `Bearer ${localStorage.getItem("jwtToken")}` }
  try {
    const response = await fetch(`${backendURL}Reportes/ObtenerUltimosPagosEmpleado`, {headers})

    if (!response.ok) throw new Error('Error al obtener los pagos')

    const data = await response.json()
    pagos.value = data

    const count = pagos.value.length
    if (count > 0) {
      salarioBrutoPromedio.value = pagos.value.reduce((acc, p) => acc + p.salarioBruto, 0) / count
      salarioNetoPromedio.value = pagos.value.reduce((acc, p) => acc + p.salarioNeto, 0) / count
      totalDeducciones.value = pagos.value.reduce((acc, p) => acc + p.totalDeduccionesEmpleado + p.totalDeduccionesBeneficios, 0)
    }

    graficar()
  } catch (err) {
    console.error('Error al cargar los pagos:', err)
  }
}

function graficar() {
  const ctx = document.getElementById('pagoChart')
  new Chart(ctx, {
    type: 'line',
    data: {
      labels: pagos.value.map(p => p.fecha.split(' ')[0]),
      datasets: [
        {
          label: 'Salario bruto',
          data: pagos.value.map(p => p.salarioBruto),
          borderWidth: 2,
          fill: false
        },
        {
          label: 'Salario neto',
          data: pagos.value.map(p => p.salarioNeto),
          borderWidth: 2,
          fill: false
        }
      ]
    },
    options: {
      responsive: true,
      plugins: {
        legend: { position: 'top' },
        title: { display: true, text: 'Tendencia de salarios' }
      }
    }
  })
}

onMounted(cargarPagos)
</script>
<style scoped>
.dashboard-container {
  max-width: 1000px;
  margin: 0 auto;
  padding: 2rem;
}

.dashboard-title {
  font-size: 2rem;
  margin-bottom: 2rem;
  text-align: center;
}

.stats-cards {
  display: flex;
  gap: 1rem;
  justify-content: space-between;
  flex-wrap: wrap;
  margin-bottom: 2rem;
}

.card {
  background-color: #f0f4f8;
  border-radius: 8px;
  padding: 1rem;
  width: 220px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.label {
  font-size: 1rem;
  color: #555;
}

.value {
  font-size: 1.4rem;
  font-weight: bold;
  color: #222;
}

.chart-title {
  font-size: 1.2rem;
  margin-bottom: 1rem;
}
</style>
