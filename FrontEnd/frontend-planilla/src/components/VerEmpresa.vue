<template>

    <div class="mb-5" style="text-align: center;" >

        <div class="page-tittle">Informacion de la empresa</div>

        <div class="info-container">
            <div class="section-tittle">Datos generales</div>

            <div class="section-subtittle">Nombre de la empresa</div>
            <div style="font-weight: bold;"> {{ Empresas[0].nombre }} </div>

            <div class="section-subtittle">Cedula juridica</div>
            <div> {{Empresas[0].cedulaJuridica}} </div>
            
            <div class="section-subtittle">Estado</div>
            <div> Activo/Inactivo </div>
            
            <div class="section-subtittle">Fecha de registro</div>
            <div> {{Empresas[0].fechaDeCreacion}} </div>

            <div class="section-subtittle">Tipo de pago</div>
            <div> {{Empresas[0].tipoDePago}} </div>
        </div>

        <div class="info-container">
            <div class="section-tittle"> Informacion detallada</div>

            <div class="subsection-tittle"> Descripcion </div>
            <div> {{Empresas[0].descripcion}}</div>
            <div class="section-subtittle"> Razon social </div>
            <div> {{Empresas[0].razonSocial}}</div>
            <div class="section-subtittle"> Empleados </div>
            <div> Cantidad total de empleados: {{cantidadEmpleados}}</div>

            <div class="subsection-tittle"> Informacion del propietario </div>
            <div class="section-subtittle">Cedula del propietario</div>
            <div> {{Empresas[0].cedulaJuridica}} </div>
            
            <div class="subsection-tittle"> Informacion de pago </div>
            <div class="section-subtittle"> Tipo de pago </div>
            <div> {{Empresas[0].tipoDePago}} </div>
            
            <div class="subsection-tittle">Informacion de contacto</div>

            <div class="section-subtittle">Correo electronico</div>
            <div> {{Correo[0].correoElectronico}} </div>
            <div class="section-subtittle">Telefono</div>
            <div> (+501) {{Telefono[0].Telefono}} </div>
            <div class="section-subtittle">Direccion</div>
            <div> {{ Direccion[0].Provincia }}, {{ Direccion[0].Canton }}, {{ Direccion[0].Distrito }}, {{ Direccion[0].OtrasSenas }} </div>
        </div>

        <div style="width: 35%; margin: 5%;">
        <button type="submit" class="btn-eliminar">Eliminar</button>
        </div>
    </div>
</template>

<script>
  import { onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import axios from 'axios'

  export default {

  name: 'InfoEmpresa',
  data() {
    return {
        Empresas:[{
        cedulaJuridica: "place holder",
        cedulaDueno: "place holder",
        cedulaAdmin: "place holder",
        tipoDePago: "place holder",
        razonSocial: "place holder",
        nombre: "place holder",
        descripcion: "place holder",
        beneficiosMaximos: 3,
        fechaDeModificacion: "place holder",
        fechaDeCreacion: "place holder",
        usuarioCreador: 1,
        ultimoEnModificar: 1,
        activo: true
      }],

      Direccion:[{
        ID: "id",
        CedulaJuridica: "place holder",
        Provincia: "provincia",
        Canton: "canton",
        Distrito: "distrito",
        OtrasSenas: "otras señas",
      }],

      Telefono:[{
        ID: "place holder",
        CedulaJuridica: "place holder",
        Telefono:"XXXX-XXXX"
      }],

      Correo:[{
        ID: "place holder",
        CedulaJuridica: "place holder",
        correoElectronico:"correo@place.holder"
      }],

      cantidadEmpleados: "place holder",
      cedulaQuerry: "1-222-333444"
    }
  },
  setup() {
    const router = useRouter()

    onMounted(() => {
      const token = localStorage.getItem('jwtToken')
      if (!token) {
        alert('Tiene que iniciar sesión primero.');
        setTimeout(() => {
          router.push('/login');
        }, 2000);
      }
    })
  },
  methods: {
    obtenerEmpresa() {
        axios.put("https://localhost:7296/api/Empresa/" + this.cedulaQuerry).then((response) =>
      { this.Empresas = response.data; });
    },
  },

  created: function () { 
    this.obtenerEmpresa();
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