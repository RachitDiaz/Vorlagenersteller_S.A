<template>
  <div class="report-page">
    <h2 class="title" style="font-weight: bold;">Reporte de pago</h2>

    <div class="title" style=""> Seleccione una fecha de pago </div>
    <div class="title" style="padding-bottom: 2rem;">
      <select id="fechas" v-model="index" @change="updateDisplay(index)">
        <option v-for="(info, i) in reportes" :key="i" :value="i">
          {{ info.fecha }}
        </option>
      </select>
    </div>

  <div id="reporte-pdf" class="reporte" v-show="show">

  <h3 class="title" style="font-weight: bold;"> {{ display.nombreEmpresa }}</h3>
  <div class="title" style="display: inline-block; width: 50%;"> {{ display.nombreEmpleado }}</div>
  <div class="title" style="display: inline-block; width: 50%;"> {{ display.tipoContrato }}</div>

  <h3 class="title"> {{ display.fecha }}</h3>

  <div class="table-space">
    <table style="border: none; width: 100%;">
      <colgroup>
       <col span="1" style="width: 70%;">
       <col span="1" style="width: 30%;">
    </colgroup>
      <tbody style="height: 20rem;">
        <tr class="row-title">
          <td> Salario bruto </td>
          <td>{{ display.salarioBruto }}₡</td>
        </tr>
        <tr>
          <td> SEM (Seguro Enfermedad/Maternidad) </td>
          <td>-{{ display.sem }}₡</td>
        </tr>
        <tr>
          <td> IVM (Invalidez, Vejez y Muerte) </td>
          <td>-{{ display.ivm }}₡</td>
        </tr>
        <tr>
          <td> Aporte Trabajador Banco Popular </td>
          <td>-{{ display.bpp }}₡</td>
        </tr>
        <tr class="row-title">
          <td> Total deducciones de ley </td>
          <td>-{{ display.totalDeduccionesEmpleado }}₡</td>
        </tr>
        <tr v-show="display.beneficioCosto1">
          <td> {{ display.beneficioNombre1 }} </td>
          <td>-{{ display.beneficioCosto1 }}₡</td>
        </tr>
        <tr v-show="display.beneficioCosto2">
          <td> {{ display.beneficioNombre2 }} </td>
          <td>-{{ display.beneficioCosto2 }}₡</td>
        </tr>
        <tr v-show="display.beneficioCosto3">
          <td> {{ display.beneficioNombre3 }} </td>
          <td>-{{ display.beneficioCosto3 }}₡</td>
        </tr>
        <tr class="row-title">
          <td> Total deducciones de beneficios </td>
          <td>-{{ display.totalDeduccionesBeneficios }}₡</td>
        </tr>
        <tr class="row-title">
          <td> Salario Neto </td>
          <td>{{ display.salarioNeto }}₡</td>
        </tr>
      </tbody>
    </table>
    </div>
  </div>
    <button class="btn" @click="exportToPDF"> Descargar como PDF </button>
    <button class="btn" @click="reporteEmail"> Enviar por correo </button>
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
    beneficioCosto1: 1,
    beneficioCosto2: 1,
    beneficioCosto3: 0,
    beneficioNombre1: "Beneficio1",
    beneficioNombre2: "Beneficio2",
    beneficioNombre3: "Beneficio3",
    bpp: 1,
    fecha: "Formato fecha",
    ivm: 1,
    nombreEmpleado: "Empleado",
    nombreEmpresa: "Empresa",
    renta: 1,
    salarioBruto: 1,
    salarioNeto: 1,
    sem: 1,
    tipoContrato: "Contrato",
    totalDeduccionesBeneficios: 1,
    totalDeduccionesEmpleado: 1
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
    axios.get(`${backendURL}Reportes/ObtenerUltimosPagosEmpleado`, {headers})
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
  if (!reportes || !reportes[index]) {
    return;
  }
  show = true;
  const item = reportes[index];
  display.beneficioCosto1 = reportes[index].beneficioCosto1;
  display.beneficioCosto2 = reportes[index].beneficioCosto2;
  display.beneficioCosto3 = reportes[index].beneficioCosto3;
  display.beneficioNombre1 = reportes[index].beneficioNombre1;
  display.beneficioNombre2 = reportes[index].beneficioNombre2;
  display.beneficioNombre3 = reportes[index].beneficioNombre3;
  display.bpp = reportes[index].bpp;
  display.fecha = reportes[index].fecha;
  display.ivm = reportes[index].ivm;
  display.nombreEmpleado = reportes[index].nombreEmpleado;
  display.nombreEmpresa = reportes[index].nombreEmpresa;
  display.renta = reportes[index].renta;
  display.salarioBruto = reportes[index].salarioBruto;
  display.salarioNeto = reportes[index].salarioNeto;
  display.sem = reportes[index].sem;
  display.tipoContrato = reportes[index].tipoContrato;
  display.totalDeduccionesBeneficios = reportes[index].totalDeduccionesBeneficios;
  display.totalDeduccionesEmpleado = reportes[index].totalDeduccionesEmpleado;
}

function exportToPDF() {
  const nombreArchivo = 'reporte-'.concat(display.fecha,".pdf");

  html2pdf(document.getElementById("reporte-pdf"), {
    html2canvas: {
      dpi: 200,
      scale:4,
      letterRendering: true,
      useCORS: true
    },
    margin: [15, 0, 0, 0],
    filename: nombreArchivo,
  });
}

async function reporteEmail() {
  if (!token) {
    alert("Tiene que iniciar sesión primero.")
    setTimeout(() => router.push("/login"), 2000)
    return
  }

  const headers = { Authorization: `Bearer ${localStorage.getItem("jwtToken")}` }

  const nombreArchivo = 'reporte-'.concat(display.fecha,".pdf");
  const element = document.getElementById("reporte-pdf");

  html2pdf()
    .set({
      html2canvas: {
        dpi: 200,
        scale:4,
        letterRendering: true,
        useCORS: true
      },
      margin: [15, 0, 0, 0]
    })
    .from(element)
    .outputPdf('blob')
    .then(function(pdfBase64) {
      const file = new File(
        [pdfBase64],
        nombreArchivo,
        {type: 'application/pdf'}
        );

        const formData = new FormData();
        formData.append("documentoPDF", file);

        setTimeout("alert('Su solicitud esta siendo procesada');", 1);

        axios.post(`${backendURL}Reportes/enviarEmailReporte`, formData, {headers})
        .then((response) => {
          if(response.data === true){
            alert('Solicitud exitosa, en breve el informe llegara a su correo.');
          } else {
            alert('Solicitud fallida, intentelo mas tarde.')
          }
        });
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
  margin-left: 1rem;
  margin-right: 1rem;
}

</style>