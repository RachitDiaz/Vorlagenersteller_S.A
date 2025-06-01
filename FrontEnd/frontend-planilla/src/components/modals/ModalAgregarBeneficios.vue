<template>
  <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Beneficio nuevo</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitForm">

            <div class="mb-3">
              <label class="form-label">* Nombre del beneficio</label>
              <input
                v-model="form.nombre"
                type="text"
                class="form-control"
                maxlength="30"
                required
                @invalid="mensajeNombreBeneficio"
                @input="borrarMensaje"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">* Descripción del beneficio</label>
              <input
                v-model="form.descripcion"
                type="text"
                class="form-control"
                maxlength="300"
                required
                @invalid="mensajeDescripcionBeneficio"
                @input="borrarMensaje"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">* Tipo de cálculo</label>
              <div class="form-check">
                <input class="form-check-input" type="radio" name="tipo_calculo" v-model="form.tipo" value="API" id="tipoApi" required/>
                <label class="form-check-label" for="tipoApi">Servicio externo</label>
              </div>
              <div class="form-check">
                <input class="form-check-input" type="radio" name="tipo_calculo" v-model="form.tipo" value="Fijo" id="tipoFijo" />
                <label class="form-check-label" for="tipoFijo">Monto Fijo</label>
              </div>
              <div class="form-check">
                <input class="form-check-input" type="radio" name="tipo_calculo" v-model="form.tipo" value="Porcentaje" id="tipoPorcentaje" />
                <label class="form-check-label" for="tipoPorcentaje">Porcentaje del salario</label>
              </div>
            </div>

            <div class="mb-3" v-if="form.tipo === 'API'">
              <label class="form-label"> URL del Servicio Externo </label>
              <input
                v-model="form.ServicioExterno"
                type="url"
                class="form-control"
                placeholder="URL del servicio externo"
                maxlength="100"
                required
                @invalid="mensajeURLInvalido"
                @input="borrarMensaje"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">* Meses mínimos para poder elegir el beneficio</label>
              <input
                v-model="form.mesesMinimos"
                type="number"
                class="form-control"
                min="0"
                required
                @invalid="mensajeMesesMinimos"
                @input="borrarMensaje"
              />
            </div>

            <div class="mb-3">
              <label class="form-label">* Cantidad de parámetros</label>
              <input
                v-model="form.cantidadParametros"
                type="number"
                class="form-control"
                min="1"
                max="5"
                required
                @invalid="mensajeCantidadParametrosInvalido"
                @input="borrarMensaje"
              />
            </div>

            <div v-for="(param, index) in form.parametros" :key="index" class="param-block">
              <h4>Parámetro {{ index + 1 }}</h4>

              <label class="form-label">* Nombre del parámetro</label>
              <input
                v-model="param.nombre"
                type="text"
                class="form-control"
                maxlength="30"
                required
                @invalid="mensajeParametro"
                @input="borrarMensaje"
              />

              <div class="mb-3"  v-if="form.tipo !== 'API'">
                <label class="form-label">* Tipo de parámetro</label>

                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_parametro' + index"
                    v-model="param.tipoParametro"
                    value="Fijo"
                    :id="'tipoFijo' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label" :for="'tipoFijo' + index">Monto Fijo</label>
                </div>

                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_parametro' + index"
                    v-model="param.tipoParametro"
                    value="Porcentaje"
                    :id="'tipoPorcentaje' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label" :for="'tipoPorcentaje' + index">Porcentaje del salario</label>
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">* Tipo de dato que debe ingresar el empleado</label>
                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_dato_parametro' + index"
                    v-model="param.datoIngreso"
                    value="Texto"
                    :id="'TipoTexto' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label" :for="'TipoTexto' + index">Parámetro de texto</label>
                </div>

                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_dato_parametro' + index"
                    v-model="param.datoIngreso"
                    value="Numero"
                    :id="'TipoNumero' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label" :for="'TipoNumero' + index">Parámetro numérico</label>
                </div>
              </div>

              <div class="mb-3" v-if="form.tipo !== 'API' && param.datoIngreso">
                <label class="form-label">* Valor del parámetro</label>
                <input
                  v-model="param.valorParametro"
                  type="number"
                  class="form-control"
                  :placeholder="textoTemporal(index)"
                  required
                  @invalid="mensajeParametro"
                  @input="borrarMensaje"
                />
              </div>

            </div>

            <button type="submit" class="btn btn-success">Agregar beneficio</button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, reactive, onMounted, defineExpose, watch} from 'vue'
  import axios from "axios";
  import Modal from 'bootstrap/js/dist/modal'
  import { backendURL } from '../../config/config.js'

  const token = localStorage.getItem("jwtToken");
  const modalRef = ref(null)
  let modalInstance = null
  
  const form = reactive({
    nombre: '',
    descripcion: '',
    tipo: '',
    ServicioExterno: '',
    mesesMinimos: '',
    cantidadParametros: '', 
    parametros: [],
  })

  onMounted(() => {
    modalInstance = new Modal(modalRef.value)
  })

  function show() {
    modalInstance?.show()
  }

  function mensajeNombreBeneficio(event) {
    const input = event.target
    if (!input.value.trim()) {
      input.setCustomValidity("Debe ingresar el nombre del beneficio.")
    }
  }

  function mensajeDescripcionBeneficio(event) {
    const input = event.target
    if (!input.value.trim()) {
      input.setCustomValidity("La descripción del beneficio es obligatoria.")
    }
  }

  function mensajeURLInvalido(event) {
    const input = event.target
    if (!input.value.trim()) {
      input.setCustomValidity("Debe ingresar la dirección URL del servicio externo.")
    } else {
      input.setCustomValidity("Debe ingresar una URL válida (ej: https://ejemplo.com).")
    }
  }

  function mensajeMesesMinimos(event) {
    const input = event.target
    const value = input.valueAsNumber

    if (!input.value.trim()) {
      input.setCustomValidity("Ingrese el mínimo de meses para poder elegir este beneficio.")
    } else if (isNaN(value) || value < 0) {
      input.setCustomValidity("Debe ingresar los meses mínimos, pueden ser 0 o más")
    }
  }

  function mensajeCantidadParametrosInvalido(event) {
    const input = event.target
    const value = input.valueAsNumber

    if (!input.value.trim()) {
      input.setCustomValidity("Ingrese la cantidad de parámetros.")
    } else if (isNaN(value) || value < 1 || value > 5) {
      input.setCustomValidity("Debe ingresar la cantidad de parámetros, mínimo 1, máximo 5")
    }
  }

  function mensajeParametro(event) {
    const input = event.target
    input.setCustomValidity("Este campo es obligatorio.")
  }

  function borrarMensaje(event) {
    event.target.setCustomValidity("")
  }

  function textoTemporal(index) {
    const tipo = form.parametros[index]?.tipoParametro
    if (tipo === 'Fijo') return 'Ingrese el monto fijo (ej. 5000)'
    if (tipo === 'Porcentaje') return 'Ingrese el porcentaje (ej. 10)'
    return ''
  }

  watch(() => form.cantidadParametros, (newVal) => {
    const cantidadParametros = form.parametros.length
    if (newVal > cantidadParametros) {
      for (let i = cantidadParametros; i < newVal; i++) {
        form.parametros.push({ nombre: '', tipoParametro: '', datoIngreso: '', valorParametro: '' })
      }
    } else {
      form.parametros.splice(newVal)
    }
  })

  function submitForm() {
    if (form.tipo === 'API') {
      form.parametros.forEach(param => {
        param.valorParametro = 0
        param.tipoParametro = 'API'
      })

    } else {
      form.ServicioExterno  = ''
    }
    console.log("Datos a guardar", form);
    axios.post(`${backendURL}Beneficios`, {
      nombre: form.nombre,
      descripcion: form.descripcion,
      tipo: form.tipo,
      ServicioExterno: form.ServicioExterno,
      mesesMinimos: form.mesesMinimos,
      cantidadParametros: form.cantidadParametros, 
      parametros: form.parametros,
    }, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).then(() => {
      window.location.href = "/ListaBeneficios";
    })
    .catch(error => {
      console.error("Error al guardar beneficio:", error);
    });

  }
  defineExpose({ show })
</script>
