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
import { ref } from 'vue'

import { useRouter } from 'vue-router'  

const planillas = ref([
  {
    periodo: 'Enero 2025',
    totalBruto: 10000000,
    deducciones: {
      obligatorias: 700000,
      beneficios: 300000,
    },
    totalNeto: 9000000,
  },
  {
    periodo: 'Febrero 2025',
    totalBruto: 0,
    deducciones: {
      obligatorias: 0,
      beneficios: 0,
    },
    totalNeto: 0,
  },
])

function agregarPlanilla() {
  planillas.value.push({
    periodo: 'Nuevo Periodo',
    totalBruto: 0,
    deducciones: {
      obligatorias: 0,
      beneficios: 0,
    },
    totalNeto: 0,
  })
}

function formatoMoneda(valor) {
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC',
    minimumFractionDigits: 0,
  }).format(valor)
}
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
