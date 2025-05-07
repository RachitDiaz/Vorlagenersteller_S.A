<template>

    <div class="mb-5" style="text-align: center;" >

        <div class="page-tittle">Informacion del empleado</div>

        <div class="info-container">
            <div class="section-tittle">Datos generales</div>

            <div class="section-subtittle">Nombre del empleado</div>
            <div style="font-weight: bold;"> {{ Empleados[1].nombre }} {{ Empleados[1].apellido1 }} {{ Empleados[1].apellido2 }} </div>

            <div class="section-subtittle">Cedula del empleado</div>
            <div> {{Empleados[1].cedula}} </div>
            
            <div class="section-subtittle">Posicion</div>
            <div> {{Empleados[1].posicion}} </div>
            
        </div>


        <div style="width: 35%; margin: 5%;">
        <button type="submit" class="btn-eliminar">Eliminar</button>
        </div>
    </div>
</template>

<script>

import axios from 'axios'

export default {
  
  name: 'InfoEmpleado',
  data() {
    return {
      Empleados: [{
        "id": 9,
        "nombre": "nombre",
        "apellido1": "apellido1",
        "apellido2": "apellido2",
        "cedula": "cedula",
        "posicion": "posicion"
      },

      {
        "id": 9,
        "nombre": "nombre2",
        "apellido1": "apellido1",
        "apellido2": "apellido2",
        "cedula": "cedula",
        "posicion": "posicion"
      }]
    }
  },

  methods: {
    obtenerEmpleados() {
        axios.get("https://localhost:7296/api/Empleado").then((response) =>
      { this.Empleados = response.data; });
    },
  },
  

  created: function () { 
    this.obtenerEmpleados();
    },
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
    vertical-align:top;
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