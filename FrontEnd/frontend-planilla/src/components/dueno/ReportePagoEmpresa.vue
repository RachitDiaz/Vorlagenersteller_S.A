<template>
  <div class="report-page">
    <h2 class="title" style="font-weight: bold;">Reporte de pago</h2>

    <div class="title" style=""> Seleccione una fecha de pago </div>
    <div class="title" style="padding-bottom: 2rem;">
      <select id="fechas">
        <option v-for="(info, index) in reportes" :key="index" @click="updateDisplay(index)">
          {{ info.fechaPago }}
        </option>
      </select>
    </div>

  <div class="reporte" v-show="show">
    <h3 class="title" style="font-weight: bold; padding-bottom: 0.1rem;"> {{ display.nombreEmpresa }}</h3>
    
    <div class="title" style="padding-bottom: 0.2rem;"> {{ display.nombreEmpleador }}</div>

    <h3 class="title"> {{ display.fechaPago }} / {{ display.peridoPago }}</h3>

    <div class="column" style="width: 60%;">
      <div class="row"> Salarios tiempo completo </div>
      <div class="row" style="font-weight: bold;  margin-bottom: 1rem;"> Total Salarios </div>
      <div class="row"> SEM (Seguro Enfermedad/Maternidad) </div>
      <div class="row"> IVM (Invalidez, Vejez y Muerte) </div>
      <div class="row"> Cuotas Banco Popular (0.25% + 0.25%) </div>
      <div class="row"> Asignaciones Familiares (5.00%) </div>
      <div class="row"> IMAS (0.50%) </div>
      <div class="row"> INA (1.50%) </div>
      <div class="row"> FCL - Fondo de Capitalización Laboral (3.00%) </div>
      <div class="row"> Fondo de Pensiones Complementarias (0.50%) </div>
      <div class="row"> INS (1.00%) </div>
      <div class="row" style="font-weight: bold; margin-bottom: 1rem;"> Total pagos de ley </div>
      <div class="row" style="font-weight: bold; margin-bottom: 1rem;"> Beneficios totales </div>
      <div class="row" style="font-weight: bold; font-size: 1.4rem; margin-bottom: 1rem;;"> Costo total empleador </div>
    </div>
    <div class="column" style="width: 40%;">
      <div class="row"> -{{ display.salariosTiempoCompleto }}₡ </div>
      <div class="row" style="font-weight: bold;  margin-bottom: 1rem;"> -{{ display.salariosTiempoCompleto }}₡ </div>
      <div class="row"> -{{ display.totalSEM }}₡ </div>
      <div class="row"> -{{ display.totalIVM }}₡ </div>
      <div class="row"> -{{ display.totalBP }}₡ </div>
      <div class="row"> -{{ display.totalAF }}₡ </div>
      <div class="row"> -{{ display.totalIMAS }}₡ </div>
      <div class="row"> -{{ display.totalINA }}₡ </div>
      <div class="row"> -{{ display.totalFCL }}₡ </div>
      <div class="row"> -{{ display.totalPC }}₡ </div>
      <div class="row"> -{{ display.totalINS }}₡ </div>
      <div class="row" style="font-weight: bold; margin-bottom: 1rem;"> -{{ display.totalPagosLey }}₡ </div>
      <div class="row" style="font-weight: bold; margin-bottom: 1rem;"> -{{ display.beneficiosTotales }}₡ </div>
      <div class="row" style="font-weight: bold; font-size: 1.4rem; margin-bottom: 1rem;"> -{{ display.costoTotal }}₡ </div>
    </div>
  </div>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { backendURL } from '../../config/config.js'

const token = localStorage.getItem("jwtToken")
const router = useRouter()
var display = reactive(
  {
    beneficiosTotales: 0,
    costoTotal: 0,
    fechaPago: "26/6/2025 22:49:48",
    nombreEmpleador: "Daniel Shih Tang",
    nombreEmpresa: "PlasTico",
    peridoPago: "2025-6 2025-7  ",
    salariosTiempoCompleto: 0,
    totalAF: 0,
    totalBP: 0,
    totalFCL: 0,
    totalIMAS: 0,
    totalINA: 0,
    totalINS: 0,
    totalIVM: 0,
    totalPC: 0,
    totalPagosLey: 0,
    totalSEM: 0,
  }
)
var reportes = reactive([])
var show = false;

function obtenerReportes() {

  if (!token) {
    alert("Tiene que iniciar sesión primero.")
    setTimeout(() => router.push("/login"), 2000)
    return
  }

  const headers = { Authorization: `Bearer ${localStorage.getItem("jwtToken")}` }

  try {
    axios.get(`${backendURL}Reportes/ObtenerUltimosPagosEmpresa`, {headers})
    .then((response) => {
      reportes = response.data;
      updateDisplay(0);
    });
  } catch (error) {
    console.error("Error cargando planilla:", error)
    alert("No se pudo cargar la informacion.")
  }
}

onMounted(() => {
  if (!token) {
    alert("Tiene que iniciar sesión primero.")
    setTimeout(() => router.push("/login"), 2000)
    return
  }
  obtenerReportes()
})

function updateDisplay(index) {
  show = true;
  display.beneficiosTotales = reportes[index].beneficiosTotales;
  display.costoTotal = reportes[index].costoTotal;
  display.fechaPago = reportes[index].fechaPago;
  display.nombreEmpleador = reportes[index].nombreEmpleador;
  display.nombreEmpresa = reportes[index].nombreEmpresa;
  display.peridoPago = reportes[index].peridoPago;
  display.salariosTiempoCompleto = reportes[index].salariosTiempoCompleto;
  display.totalAF = reportes[index].totalAF;
  display.totalBP = reportes[index].totalBP;
  display.totalFCL = reportes[index].totalFCL;
  display.totalIMAS = reportes[index].totalIMAS;
  display.totalINA = reportes[index].totalINA;
  display.totalINS = reportes[index].totalINS;
  display.totalIVM = reportes[index].totalIVM;
  display.totalPC = reportes[index].totalPC;
  display.totalPagosLey = reportes[index].totalPagosLey;
  display.totalSEM = reportes[index].totalSEM;
}

</script>


<style scoped>
.report-page {
  max-width: 80%;
  margin: 0 auto;
  text-align: center;
  padding: 1rem;
}

.title {
  font-size: 1.5rem;
  font-weight: normal;
  font-size: 1.5re;
  text-align: left;
  padding-left: 10%;
  padding-right: 10%;
  padding-top: 0.4rem;
  padding-bottom: 0.4rem;
  color: black;
}

.column {
  width: 50%;
  margin: 0 auto;
  font-size: 1.2rem;
  font-weight: normal;
    
  padding-left: 12%;
  padding-right: 10%;
  padding-top: 0.4rem;
  padding-bottom: 0.4rem;

  display: inline-block;
  vertical-align:top;
  text-align: left;

  color: black;
}

.row {
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
}

</style>