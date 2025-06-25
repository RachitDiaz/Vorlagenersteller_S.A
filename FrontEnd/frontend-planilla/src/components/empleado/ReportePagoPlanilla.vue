<template>
  <div class="report-page">
    <h2 class="title" style="font-weight: bold;">Reporte de pago</h2>

    <div class="title" style=""> Seleccione una fecha de pago </div>
    <div class="title" style="padding-bottom: 2rem;">
      <select id="fechas">
        <option v-for="(info, index) in reportes" :key="index" @click="updateDisplay(index)">
          {{ info.fecha }}
        </option>
      </select>
    </div>

  <div class="reporte">
    <h3 class="title" style="font-weight: bold;"> {{ display.nombreEmpresa }}</h3>
    
    <div class="title" style="display: inline-block; width: 60%;"> {{ display.nombreEmpleado }}</div>
    <div class="title" style="display: inline-block; width: 40%;"> {{ display.tipoContrato }}</div>

    <h3 class="title"> {{ display.fecha }}</h3>

    <div class="column" style="width: 60%;">
      <div class="row" style="font-weight: bold;"> Salario Bruto </div>
      <div class="row"> SEM (Seguro Enfermedad/Maternidad) </div>
      <div class="row"> IVM (Invalidez, Vejez y Muerte) </div>
      <div class="row"> Aporte Trabajador Banco Popular </div>
      <div class="row"> Impuesto de renta </div>
      <div class="row" style="font-weight: bold;  margin-bottom: 1rem;"> Total deducciones obligatorias </div>
      <div class="row" v-show="display.beneficioCosto1"> {{ display.beneficioNombre1 }} </div>
      <div class="row" v-show="display.beneficioCosto2"> {{ display.beneficioNombre2 }} </div>
      <div class="row" v-show="display.beneficioCosto3"> {{ display.beneficioNombre3 }} </div>
      <div class="row" style="font-weight: bold;  margin-bottom: 1rem;"> Total deducciones beneficios </div>
      <div class="row" style="font-weight: bold;"> Salaio neto </div>
    </div>
    <div class="column" style="width: 40%;">
      <div class="row" style="font-weight: bold;"> {{ display.salarioBruto }}₡</div>
      <div class="row"> -{{ display.sem }}₡ </div>
      <div class="row"> -{{ display.ivm }}₡ </div>
      <div class="row"> -{{ display.bpp }}₡ </div>
      <div class="row"> -{{ display.renta }}₡ </div>
      <div class="row" style="font-weight: bold; margin-bottom: 1rem;"> -{{ display.totalDeduccionesEmpleado }}₡</div>
      <div class="row" v-show="display.beneficioCosto1"> -{{ display.beneficioCosto1 }}₡ </div>
      <div class="row" v-show="display.beneficioCosto2"> -{{ display.beneficioCosto2 }}₡ </div>
      <div class="row" v-show="display.beneficioCosto3"> -{{ display.beneficioCosto3 }}₡ </div>
      <div class="row" style="font-weight: bold; margin-bottom: 1rem;"> -{{ display.totalDeduccionesBeneficios }}₡</div>
      <div class="row" style="font-weight: bold;"> {{ display.salarioNeto }}₡</div>
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