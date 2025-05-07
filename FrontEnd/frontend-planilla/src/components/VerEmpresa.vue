<template>

    <div class="mb-5" >

        <div class="page-tittle">Informacion de la empresa</div>

        <div class="info-container">
            <div class="section-tittle">Datos generales</div>

            <div class="section-subtittle">Nombre de la empresa</div>
            <div style="font-weight: bold;"> {{ nombre }} </div>

            <div class="section-subtittle">Cedula juridica</div>
            <div> {{telefono}} </div>
            
            <div class="section-subtittle">Estado</div>
            <div> Activo/Inactivo </div>
            
            <div class="section-subtittle">Fecha de registro</div>
            <div> {{fechaRegistro}} </div>

            <div class="section-subtittle">Tipo de pago</div>
            <div> {{tipoPago}} </div>
        </div>

        <div class="info-container">
            <div class="section-tittle"> Informacion detallada</div>

            <div class="subsection-tittle"> Descripcion </div>
            <div> {{descripcion}}</div>
            <div class="subsection-tittle"> Razon social </div>
            <div> {{razonSocial}}</div>

            <div class="subsection-tittle"> Informacion del propietario </div>
            <div class="section-subtittle">Cedula del propietario</div>
            <div> {{cedulaJuridica}} </div>
            
            <div class="subsection-tittle"> Informacion de pago </div>
            <div class="section-subtittle"> Tipo de pago </div>
            <div> {{tipoPago}} </div>
            
            <div class="subsection-tittle">Informacion de contacto</div>

            <div class="section-subtittle">Correo electronico</div>
            <div> {{correoElectronico}} </div>
            <div class="section-subtittle">Telefono</div>
            <div> (+501) {{telefono}} </div>
            <div class="section-subtittle">Direccion</div>
            <div> {{ provincia }}, {{ canton }}, {{ distrito }}, {{ otrasSenas }} </div>
        </div>

        <div style="width: 35%;">
        <button type="submit" class="btn-eliminar">Eliminar</button>
        </div>
    </div>
</template>

<script>
  import { onMounted } from 'vue'
  import { useRouter } from 'vue-router'

  export default {

  name: 'InfoEmpresa',
  data() {
    return {
        nombre: 'Nombre empresa',
        descripcion: 'Descripcion de la empresa',
        cedulaJuridica: 'X-XXX-XXXXXX',
        cedulaDueno: 'X-XXXX-XXXX',
        correoElectronico: 'correo@ejemplo.vue',
        telefono: '(+501) XXXX-XXXX',
        razonSocial: 'Razon de la empresa',
        cantidadEmpleados: '# empleados',
        fechaRegistro:"dia/mes/año",
        tipoPago: "semanal-quincenal-mensual",
        provincia: "Provincia",
        canton: "Canton",
        distrito: "Distrito",
        otrasSenas: "Otras señas",
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
    registrarUsuario() {
      console.log('Usuario registrado:', this.form);
    }
  },

  created: function () { 
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
    border-color: grey;

    display: inline-block;
    vertical-align:top;
    margin: 0.5%;
    text-align: left;
  }

  .section-tittle {
    font-weight: bold;
    font-size: larger;
    margin-bottom: 1rem;
    margin-top: 1rem;
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