<template>

    <div class="mb-5" style="text-align: center;" >

        <div class="page-tittle">Informacion del empleado</div>

        <div class="info-container">
            <div class="section-tittle">Datos generales</div>

            <div class="section-subtittle">Nombre del empleado</div>
            <div style="font-weight: bold;"> {{ nombreEmpleado }} {{ apellidoI }} {{ apellidoII }} </div>

            <div class="section-subtittle">Cedula del empleado</div>
            <div> {{cedulaEmpleado}} </div>
            
            <div class="section-subtittle">Empleador</div>
            <div> {{nombreEmpresa}} </div>
            
            <div class="section-subtittle">Fecha de ingreso</div>
            <div> {{fechaRegistro}} </div>

            <div class="section-subtittle">Tipo de contrato</div>
            <div> {{contrato}} </div>
        </div>

        <div class="info-container">
            <div class="section-tittle"> Informacion detallada</div>

            <div class="subsection-tittle"> Informacion de empleador </div>
            <div class="section-subtittle">Cedula juridica</div>
            <div> {{cedulaJuridica}} </div>
            
            <div class="subsection-tittle"> Informacion de pago </div>
            <div class="section-subtittle"> Informacion bancaria </div>
            <div> {{banco}} </div>
            <div class="section-subtittle"> Salario bruto </div>
            <div> {{salario}} </div>
            
            <div class="subsection-tittle">Informacion de contacto</div>

            <div class="section-subtittle">Correo electronico</div>
            <div> {{correoElectronico}} </div>
            <div class="section-subtittle">Telefono</div>
            <div> (+501) {{telefono}} </div>
            <div class="section-subtittle">Direccion</div>
            <div> {{ provincia }}, {{ canton }}, {{ distrito }}, {{ otrasSenas }} </div>
        </div>

        <div style="width: 35%; margin: 5%;">
        <button type="submit" class="btn-eliminar">Eliminar</button>
        </div>
    </div>
</template>

<script>


export default {
  name: 'InfoEmpleado',
  data() {
    return {
        nombreEmpleado: 'Nombre',
        apellidoI: 'Apellido1',
        apellidoII: 'Apellido2',
        salario: 'Monto con 2 decimales',
        contrato: "Tipo de contratacion",
        cedulaJuridica: 'X-XXX-XXXXXX',
        cedulaEmpleado: 'X-XXXX-XXXX',
        correoElectronico: 'correo@ejemplo.vue',
        telefono: '(+501) XXXX-XXXX',
        banco: 'Cuenta de deposito',
        fechaRegistro:"dia/mes/año",
        tipoPago: "Semanal-1uincenal-Mensual",
        provincia: "Provincia",
        canton: "Canton",
        distrito: "Distrito",
        otrasSenas: "Otras señas",
        nombreEmpresa: "Nombre empresa",
    }
  },
  methods: {
    registrarUsuario() {
      console.log('Usuario registrado:', this.form);
      // Aquí puedes hacer un POST a tu backend
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