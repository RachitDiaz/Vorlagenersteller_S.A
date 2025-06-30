<template>
    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" ref="modalRef">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modalLabel">Nueva empresa</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="submitForm">
  
              <div class="mb-3">
                <label class="form-label">Nombre <span style="color:red;">*</span></label>
                <input
                  v-model="form.nombre"
                  type="text"
                  class="form-control"
                  minlength= "6"
                  maxlength= "50"
                  placeholder= "Nombre de la empresa"
                  required
                  @invalid="mensajeNombre"
                  @input="borrarMensaje"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Cedula Juridica <span style="color:red;">*</span></label>
                <input
                  v-model="form.cedulaJuridica"
                  type= "text"
                  class= "form-control"
                  placeholder= "X-XXX-XXXXXX"
                  pattern= "\d-\d\d\d-\d\d\d\d\d\d"
                  required
                  @invalid="mensajeCedula"
                  @input="borrarMensaje"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Descripcion <span style="color:red;">*</span></label>
                <textarea
                  v-model="form.descripcion"
                  type="text"
                  class="form-control"
                  minlength= "6"
                  maxlength= "300"
                  placeholder= "Descripcion de la empresa"
                  required
                  @invalid="mensajeDescripcion"
                  @input="borrarMensaje"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Correo <span style="color:red;">*</span></label>
                <input
                  v-model="form.correo"
                  type="email"
                  class="form-control"
                  minlength= "6"
                  maxlength= "60"
                  required
                  @invalid="mensajeCorreo"
                  @input="borrarMensaje"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Cedula de propietario <span style="color:red;">*</span></label>
                <input
                  v-model="form.cedulaDueno"
                  type="text"
                  class="form-control"
                  placeholder= "X-XXXX-XXXX"
                  pattern= "\d-\d\d\d\d-\d\d\d\d"
                  required
                  @invalid="mensajeCedulaPropietario"
                  @input="borrarMensaje"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Tipo de pago <span style="color:red;">*</span></label>
                <select v-model="form.tipoDePago"
                  class="form-select"
                  required
                  placeholder= "Seleccione la frecuencia de pago"
                  @invalid="mensajePago"
                  @input="borrarMensaje"
                  >
                    <option> Mensual </option>
                    <option> Quincenal </option>
                    <option> Semanal </option>
                </select>
                
              </div>

              <div class="mb-3">
                <label class="form-label">Telefono <span style="color:red;">*</span></label>
                <input
                  v-model="form.telefono"
                  type="text"
                  class="form-control"
                  placeholder= "XXXX-XXXX"
                  pattern= "\d\d\d\d-\d\d\d\d"
                  required
                  @invalid="mensajeTelefono"
                  @input="borrarMensaje"
                />
              </div>

              <label class="form-label">Direccion <span style="color:red;">*</span></label>

              <div class="mb-3">
                <input
                  v-model="form.provincia"
                  type="text"
                  class="form-control"
                  maxlength= "20"
                  placeholder= "Provincia"
                  required
                  @invalid="mensajeDireccion"
                  @input="borrarMensaje"
                  style="float:left; width: 30%; margin-left: 2%; margin-bottom: 2%;"
                />

                <input
                  v-model="form.canton"
                  type="text"
                  class="form-control"
                  maxlength= "20"
                  placeholder= "Canton"
                  required
                  @invalid="mensajeDireccion"
                  @input="borrarMensaje"
                  style="float:left; width: 30%; margin-left: 2%; margin-bottom: 2%;"
                />

                <input
                  v-model="form.distrito"
                  type="text"
                  class="form-control"
                  maxlength= "20"
                  placeholder= "Distrito"
                  required
                  @invalid="mensajeDireccion"
                  @input="borrarMensaje"
                  style="float:left; width: 30%; margin-left: 2%; margin-bottom: 2%;"
                />

                <textarea
                  v-model="form.otrasSenas"
                  type="text"
                  class="form-control"
                  maxlength= "300"
                  placeholder= "Otras señas"
                  required
                  @invalid="mensajeOtrasSenas"
                  @input="borrarMensaje"
                />

              </div>

              <div class="mb-3">
                <label class="form-label">Razon social <span style="color:red;">*</span></label>
                <input
                  v-model="form.razonSocial"
                  type="text"
                  class="form-control"
                  maxlength= "30"
                  minlength= "6"
                  placeholder= "Razon social"
                  required
                  @invalid="mensajeRazonSocial"
                  @input="borrarMensaje"
                />
              </div>
  
              <button type="submit" class="btn btn-success">Submit</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, reactive, onMounted, defineExpose } from 'vue'
  import axios from "axios";
  import Modal from 'bootstrap/js/dist/modal'
  import { backendURL } from '../../config/config.js'
  
  const token = localStorage.getItem("jwtToken");
  const modalRef = ref(null)
  let modalInstance = null
  
  onMounted(() => {
    modalInstance = new Modal(modalRef.value)
  })
  
  function show() {
    modalInstance?.show()
  }

  const form = reactive({
    cedulaJuridica: '',
    cedulaDueno: '',
    tipoDePago: '',
    razonSocial: '',
    nombre: '',
    descripcion: '',
    beneficiosMaximos: '0',
    correo: '',
    telefono: '',
    provincia: '',
    canton: '',
    distrito: '',
    otrasSenas: '',
  })
  
  function submitForm() {
    axios.post(`${backendURL}Empresa`, {
    cedulaJuridica: form.cedulaJuridica,
    cedulaDueno: form.cedulaDueno,
    tipoDePago: form.tipoDePago,
    razonSocial: form.razonSocial,
    nombre: form.nombre,
    descripcion: form.descripcion,
    beneficiosMaximos: form.beneficiosMaximos,
    correo: form.correo,
    telefono: form.telefono,
    provincia: form.provincia,
    canton: form.canton,
    distrito: form.distrito,
    otrasSenas: form.otrasSenas
  
    }, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).then(() => {
      window.location.href = "/ListaEmpresas";
    })
    .catch(error => {
      console.error("Error al registrar empresa:", error);
      alert("Error al registrar empresa\nVerifique que la cedula sea de un dueño en el sistema y la empresa no este ya registrada");
    });
  }
  
  function mensajeNombre(event) {
    event.target.setCustomValidity("Debe ingresar el nombre de la empresa, de entre 6 y 50 caracteres")
  }

  function mensajeCedula(event) {
    event.target.setCustomValidity("Debe ingresar los 10 digitos de la cedula juridica espaciados por -, tal que X-XXX-XXXXXX")
  }

  function mensajeDescripcion(event) {
    event.target.setCustomValidity("Debe ingresar una descripcion para la empresa de entre 6 y 50 caracteres")
  }

  function mensajeCorreo(event) {
    event.target.setCustomValidity("Debe ingresar un correo valido")
  }

  function mensajeCedulaPropietario(event) {
    event.target.setCustomValidity("Debe ingresar los 9 digitos de la cedula espaciados por -, tal que X-XXXX-XXXX")
  }

  function mensajePago(event) {
    event.target.setCustomValidity("Debe elegir la frecuencia de pago")
  }

  function mensajeTelefono(event) {
    event.target.setCustomValidity("Debe ingresar los 8 digitos del telefono espaciados por -, tal que XXXX-XXXX")
  }

  function mensajeDireccion(event) {
    event.target.setCustomValidity("Debe ingresar la ubicacion, no puede pasar de 20 caracteres")
  }

  function mensajeOtrasSenas(event) {
    event.target.setCustomValidity("Debe ingresar otras señas, no puede pasar de 300 caracteres")
  }

  function mensajeRazonSocial(event) {
    event.target.setCustomValidity("Debe ingresar la razon social de la empresa, usar entre 6 y 30 caracteres")
  }

  function borrarMensaje(event) {
    event.target.setCustomValidity("")
  }
  
  defineExpose({ show })
  </script>
  