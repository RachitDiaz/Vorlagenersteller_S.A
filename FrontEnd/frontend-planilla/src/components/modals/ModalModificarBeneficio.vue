<template>
  <div class="modal fade" id="myModal"
    tabindex="-1" aria-labelledby="modalLabel"
    aria-hidden="true" ref="modalRef">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">Modificar beneficio</h5>
          <button type="button" class="btn-close"
            data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div v-if="mensajeError" class="alert alert-danger" role="alert">
            {{ mensajeError }}
          </div>
          <form @submit.prevent="submitForm">

            <div class="mb-3">
              <label class="form-label">
                Tipo de beneficio
              </label>
              <div class="form-check">
                <input class="form-check-input"
                  type="radio"
                  name="tipo_calculo"
                  v-model="form.tipo"
                  value="Fijo"
                  id="tipoFijo"
                  required
                  @invalid="mensajeTipoBeneficio"
                  @input="borrarMensaje"
                  />
                <label class="form-check-label"
                  for="tipoFijo">Monto Fijo</label>
              </div>
              <div class="form-check">
                <input class="form-check-input"
                  type="radio"
                  name="tipo_calculo"
                  v-model="form.tipo"
                  value="Porcentaje"
                  id="tipoPorcentaje"
                  required
                  @invalid="mensajeTipoBeneficio"
                  @input="borrarMensaje"
                  />
                <label class="form-check-label"
                  for="tipoPorcentaje">Porcentaje del salario</label>
              </div>
            </div>

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
              <label class="form-label">
                * Meses mínimos para poder elegir el beneficio
              </label>
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
            <div v-for="(param, index) in form.parametros"
              :key="index" class="param-block">
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

              <div class="mb-3">
                <label class="form-label">* Tipo de parámetro</label>

                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_parametro' + index"
                    v-model="param.tipoValorParametro"
                    value="Fijo"
                    :id="'tipoFijo' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label" :for="'tipoFijo' + index">
                    Monto Fijo
                  </label>
                </div>

                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_parametro' + index"
                    v-model="param.tipoValorParametro"
                    value="Porcentaje"
                    :id="'tipoPorcentaje' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label"
                  :for="'tipoPorcentaje' + index">
                    Porcentaje del salario
                  </label>
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">
                  * Tipo de dato que debe ingresar el empleado
                </label>
                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_dato_parametro' + index"
                    v-model="param.tipoDeDatoParametro"
                    value="Texto"
                    :id="'TipoTexto' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label"
                  :for="'TipoTexto' + index">
                    Parámetro de texto
                  </label>
                </div>

                <div class="form-check">
                  <input
                    class="form-check-input"
                    type="radio"
                    :name="'tipo_dato_parametro' + index"
                    v-model="param.tipoDeDatoParametro"
                    value="Numero"
                    :id="'TipoNumero' + index"
                    required
                    @invalid="mensajeParametro"
                    @input="borrarMensaje"
                  />
                  <label class="form-check-label"
                  :for="'TipoNumero' + index">
                    Parámetro numérico
                  </label>
                </div>
              </div>

              <div class="mb-3" v-if="form.tipo !==
              'API' && param.tipoValorParametro">
                <label class="form-label">* Valor del parámetro</label>
                <input
                  v-model="param.valorDelParametro"
                  type="number"
                  class="form-control"
                  :placeholder="textoTemporal(index)"
                  required
                  @invalid="mensajeParametro"
                  @input="borrarMensaje"
                  :min=0
                  :max="param.tipoValorParametro ===
                  'Porcentaje' ? 100 : undefined"
                />
              </div>

            </div>
            <button type="submit" class="btn btn-success">
              Modificar beneficio
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import {ref, reactive, onMounted, defineExpose, watch} from 'vue';
import axios from 'axios';
import Modal from 'bootstrap/js/dist/modal';
import {backendURL} from '../../config/config.js';

const token = localStorage.getItem('jwtToken');
const modalRef = ref(null);
let modalInstance = null;
const mensajeError = ref('');
const beneficioOriginal = ref(null);
const isInitializing = ref(false);

const form = reactive({
  nombre: '',
  descripcion: '',
  tipo: '',
  mesesMinimos: '',
  cantidadParametros: '',
  parametros: [],
});

onMounted(() => {
  modalInstance = new Modal(modalRef.value);
});

/**
 * Mostrar modal con el beneficio seleccionado
 */
function show(beneficio) {
  isInitializing.value = true;
  beneficioOriginal.value = {...beneficio};
  form.nombre = beneficio.nombre;
  form.descripcion = beneficio.descripcion;
  form.tipo = beneficio.tipo;
  form.mesesMinimos = beneficio.mesesMinimos;
  form.cantidadParametros = beneficio.cantidadParametros;
  form.parametros = beneficio.parametros.map((param) => ({
    nombre: param.nombre,
    tipoValorParametro: param.tipoValorParametro,
    tipoDeDatoParametro: param.tipoDeDatoParametro,
    valorDelParametro: param.valorDelParametro
  }));
  setTimeout(() => {
    isInitializing.value = false;
  }, 0);

  modalInstance?.show();
}

function mensajeTipoBeneficio(event) {
  input.setCustomValidity('Debe seleccionar el tipo de beneficio.');
}

function mensajeNombreBeneficio(event) {
  const input = event.target;
  if (!input.value.trim()) {
    input.setCustomValidity('Debe ingresar el nombre del beneficio.');
  }
}

function mensajeDescripcionBeneficio(event) {
  const input = event.target;
  if (!input.value.trim()) {
    input.setCustomValidity('La descripción del beneficio es obligatoria.');
  }
}

function mensajeMesesMinimos(event) {
  const input = event.target;
  const value = input.valueAsNumber;

  if (!input.value.trim()) {
    input.setCustomValidity('Ingrese el mínimo de meses para poder elegir este beneficio.');
  } else if (isNaN(value) || value < 0) {
    input.setCustomValidity('Debe ingresar los meses mínimos, pueden ser 0 o más');
  }
}

function mensajeCantidadParametrosInvalido(event) {
  const input = event.target;
  const value = input.valueAsNumber;

  if (!input.value.trim()) {
    input.setCustomValidity('Ingrese la cantidad de parámetros.');
  } else if (isNaN(value) || value < 1 || value > 5) {
    input.setCustomValidity('Debe ingresar la cantidad de parámetros, mínimo 1, máximo 5');
  }
}

function mensajeParametro(event) {
  const input = event.target;
  input.setCustomValidity('Este campo es obligatorio.');
}

function borrarMensaje(event) {
  event.target.setCustomValidity('');
}

function textoTemporal(index) {
  const tipo = form.parametros[index]?.tipoValorParametro;
  if (tipo === 'Fijo') return 'Ingrese el monto fijo (ej. 5000)';
  if (tipo === 'Porcentaje') return 'Ingrese el porcentaje (ej. 10)';
  return '';
}

watch(() => form.cantidadParametros, (newVal) => {
  if (isInitializing.value) return;
  const cantidadParametros = form.parametros.length;
  if (newVal > cantidadParametros) {
    for (let i = cantidadParametros; i < newVal; i++) {
      form.parametros.push({nombre: '', tipoDeDatoParametro: '', valorDelParametro: '', tipoValorParametro: ''});
    }
  } else {
    form.parametros.splice(newVal);
  }
});

/**
 * Function para enviar info a backend y modificar el beneficio
 */
function submitForm() {
  console.log('Datos a cambiar', form);
  const payload = {
    modificado: {
      nombre: form.nombre,
      descripcion: form.descripcion,
      tipo: form.tipo,
      mesesMinimos: form.mesesMinimos,
      cantidadParametros: form.cantidadParametros,
      parametros: form.parametros,
    },
    original: beneficioOriginal.value
  };
  axios.post(`${backendURL}Beneficios/Modificar`, payload, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  }).then(() => {
    window.location.href = '/ListaBeneficios';
  }).catch((error) => {
    console.error('Error al modificar beneficio:', error);
    if (error.response && error.response.data) {
      mensajeError.value = error.response.data;
    } else {
      mensajeError.value =
        'Ocurrió un error al guardar el beneficio. Intente de nuevo.';
    }
  });
}
defineExpose({show});
</script>
