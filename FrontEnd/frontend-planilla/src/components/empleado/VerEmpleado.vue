<template>
  <div class="mb-5" style="text-align: center;">
    <div class="page-tittle">Informacion del empleado</div>

    <div v-if="empleado && empleado.nombre" class="info-container">
      <div class="section-tittle">Datos generales</div>

      <div class="section-subtittle">Nombre del empleado</div>
      <div style="font-weight: bold;">
        {{ empleado.nombre }} {{ empleado.apellido1 }} {{ empleado.apellido2 }}
      </div>

      <div class="section-subtittle">Cédula del empleado</div>
      <div>{{ empleado.cedulaEmpleado }}</div>

      <div class="section-subtittle">Tipo de contrato</div>
      <div>{{ empleado.tipoContrato }}</div>

      <div class="section-subtittle">Salario bruto</div>
      <div>{{ empleado.salarioBruto }}</div>

      <div class="section-subtittle">Información bancaria</div>
      <div>{{ empleado.banco }}</div>

      <div class="section-subtittle">Género</div>
      <div>{{ genero }}</div>

      <div class="section-subtittle">Correo</div>
      <div>{{ correo }}</div>

      <div class="section-subtittle">¿Cédula editable?</div>
      <div>{{ cedulaEditable ? "Sí" : "No" }}</div>
    </div>

    <div style="width: 35%; margin: 5%;">
      <button type="submit" class="btn-eliminar">Eliminar</button>
    </div>
  </div>
</template>


<script>
import axios from 'axios'
import { backendURL } from '../../config/config.js'

const token = localStorage.getItem("jwtToken");

export default {
  name: 'InfoEmpleado',
  data() {
    return {
      empleado: {
        cedulaEmpleado: "-",
        cedulaEmpresa: "-",
        nombre: "-",
        apellido1: "-",
        apellido2: "-",
        banco: "-",
        salarioBruto: 0,
        tipoContrato: "-",
      },
      genero: "-",
      correo: "-",
      cedulaEditable: false
    };
  },

  methods: {
    obtenerEmpleado() {
      axios.get(`${backendURL}Empleado/GetInfoEmpleado/`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((response) => {
          this.empleado = response.data.empleado; 
          this.genero = response.data.genero;
          this.correo = response.data.correo;
          this.cedulaEditable = response.data.cedulaEditable;
        })
        .catch((error) => {
          console.error("Error al obtener el empleado:", error);
        });
    }
  },
  created() {
    this.obtenerEmpleado();
  }
}
</script>


<style scoped>
.page-tittle {
  font-size: xx-large;
  font-weight: bold;
  text-align: left;
  padding-left: 15%;
  padding-right: 15%;
  padding-top: 1rem;
  padding-bottom: 1rem;
  color: black;
}

.info-container {
  width: 35%;
  margin: 0 auto;

  margin-top: 10%;
  padding: 2rem;
  background-color: #fff;
  border-radius: 8px;
  border-color: rgb(172, 172, 172);
  border-style: solid;
  border-width: 2px;

  display: inline-block;
  vertical-align: top;
  margin: 0.5%;
  text-align: left;
}

.section-tittle {
  font-weight: bold;
  font-size: larger;
  margin-bottom: 1rem;
}

.section-subtittle {
  font-weight: normal;
  font-size: medium;
  color: grey;
  margin-top: 0.5rem;
}

.subsection-tittle {
  font-weight: bold;
  font-size: normal;
  margin-bottom: 0.5rem;
  margin-top: 0.5rem;
}

.btn-eliminar {
  width: 20%;
  background-color: #1e1e1e;
  color: #fff;
  padding: 0.6rem;
  border: none;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
  text-align: left;
}

.btn-eliminar:hover {
  background-color: #ff3232;
}
</style>