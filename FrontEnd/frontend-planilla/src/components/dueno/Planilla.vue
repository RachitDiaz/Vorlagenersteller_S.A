<template>
  <div class="planilla-container">
    <h2>Planilla</h2>
    <button @click="agregarPlanilla">Nueva Planilla</button>

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
            {{ formatoMoneda(item.deducciones.obligatorias + item.deducciones.beneficios) }}
            <div class="detalles-deducciones">
              Obligatorias: {{ formatoMoneda(item.deducciones.obligatorias) }}<br />
              Beneficios: {{ formatoMoneda(item.deducciones.beneficios) }}
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

function formatoMoneda(valor) {
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC',
    minimumFractionDigits: 0,
  }).format(valor)
}

const token = localStorage.getItem("jwtToken")

async function cargarPlanilla() {
  try {
    const empleadosResponse = await axios.get(`${backendURL}Empleado/GetEmpleadosEmpresa` , {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })

    const empleados = empleadosResponse.data

    const datosPlanilla = await Promise.all(
      empleados.map(async (empleado) => {
        const salarioBruto = empleado.salarioBruto

        const [beneficiosRes, obligatoriasRes] = await Promise.all([
          axios.get(`${backendURL}Empleado/GetDeducciones?cedula=${empleado.cedulaEmpleado}`, {
            headers: { Authorization: `Bearer ${token}` }
          }),
          axios.get(`${backendURL}DeduccionesPlanilla/${salarioBruto}`, {
            headers: { Authorization: `Bearer ${token}` }
          })
        ])

        const beneficios = beneficiosRes.data.total
        const obligatorias = obligatoriasRes.data.total

        const totalNeto = salarioBruto - beneficios - obligatorias

        return {
          periodo: 'Junio 2025',
          totalBruto: salarioBruto,
          deducciones: {
            obligatorias,
            beneficios
          },
          totalNeto
        }
      })
    )

    planillas.value = datosPlanilla
  } catch (error) {
    console.error("Error al cargar planilla:", error)
  }
}


onMounted(() => {
  cargarPlanilla()
})
</script>

<style scoped>
.planilla-container {
  background: white;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 0 8px rgba(0, 0, 0, 0.1);
  max-width: 900px;
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
