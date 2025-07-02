<template>
  <div class="report-page">
    <h2 class="title" style="font-weight: bold;">Reporte de pago</h2>

    <div class="title" style=""> Seleccione una fecha de pago </div>
    <div class="title" style="padding-bottom: 2rem;">
      <select id="fechas" v-model="index" @change="updateDisplay(index)">
        <option v-for="(info, i) in reportes" :key="i" :value="i">
          {{ info.fechaPago }}
        </option>
      </select>
    </div>

  <div id="reporte-pdf" class="reporte" v-show="show">
    <h3 class="title" style="font-weight: bold; padding-bottom: 0.1rem;"> {{ display.nombreEmpresa }}</h3>
    
    <div class="title" style="padding-bottom: 0.2rem;"> {{ display.nombreEmpleador }}</div>

    <h3 class="title"> {{ display.fechaPago }} / {{ display.peridoPago }}</h3>
    
  <div class="table-space">
    <table style="border: none; width: 100%;">
      <colgroup>
       <col span="1" style="width: 70%;">
       <col span="1" style="width: 30%;">
    </colgroup>
      <tbody style="height: 30rem;">
        <tr>
          <td> Salarios tiempo completo </td>
          <td>-{{ display.salariosTiempoCompleto }}₡</td>
        </tr>
        <tr class="row-title">
          <td> Total Salarios </td>
          <td>-{{ display.salariosTiempoCompleto }}₡</td>
        </tr>
        <tr>
          <td> SEM (Seguro Enfermedad/Maternidad) </td>
          <td>-{{ display.totalSEM }}₡</td>
        </tr>
        <tr>
          <td> IVM (Invalidez, Vejez y Muerte) </td>
          <td>-{{ display.totalIVM }}₡</td>
        </tr>
        <tr>
          <td> Cuotas Banco Popular (0.25% + 0.25%) </td>
          <td>-{{ display.totalBP }}₡</td>
        </tr>
        <tr>
          <td> Asignaciones Familiares (5.00%) </td>
          <td>-{{ display.totalAF }}₡</td>
        </tr>
        <tr>
          <td> IMAS (0.50%) </td>
          <td>-{{ display.totalIMAS }}₡</td>
        </tr>
        <tr>
          <td> INA (1.50%) </td>
          <td>-{{ display.totalINA }}₡</td>
        </tr>
        <tr>
          <td> FCL - Fondo de Capitalización Laboral (3.00%) </td>
          <td>-{{ display.totalFCL }}₡</td>
        </tr>
        <tr>
          <td> Fondo de Pensiones Complementarias (0.50%) </td>
          <td>-{{ display.totalPC }}₡</td>
        </tr>
        <tr>
          <td> INS (1.00%) </td>
          <td>-{{ display.totalINS }}₡</td>
        </tr>
        <tr class="row-title">
          <td> Total pagos de ley </td>
          <td>-{{ display.totalPagosLey }}₡</td>
        </tr>
        <tr>
          <td> Beneficios totales </td>
          <td>-{{ display.beneficiosTotales }}₡</td>
        </tr>
        <tr class="row-title">
          <td> Costo total empleador </td>
          <td>-{{ display.costoTotal }}₡</td>
        </tr>
      </tbody>
    </table>
    </div>
  </div>
  <button class="btn" @click="exportToPDF"> Descargar como PDF </button>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { backendURL } from '../../config/config.js'
import html2pdf from "html2pdf.js";

const token = localStorage.getItem("jwtToken")
const router = useRouter()
const index = ref(0)
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
      if (response.data.length != 0) {
        reportes = response.data;
        updateDisplay(0);
      }
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

function exportToPDF() {
  html2pdf(document.getElementById("reporte-pdf"), {
    margin: 1,
    filename: "Reporte costos.pdf",
  });
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

.table-space {
  width:100%;
  margin: 0 auto;
  font-size: 1.2rem;
  font-weight: normal;
    
  padding-left: 10%;
  padding-right: 10%;
  padding-top: 0.4rem;
  padding-bottom: 0.4rem;

  vertical-align:top;
  text-align: left;

  color: black;
}

.row-title {
  margin-top: 0.6rem;
  margin-bottom: 1rem;
  font-weight: bold;
}

.btn {
  background-color: #f3f4f6;
  border: 1px solid #ccc;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
}

</style>