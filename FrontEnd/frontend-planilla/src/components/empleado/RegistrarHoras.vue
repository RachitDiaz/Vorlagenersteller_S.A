<template>
  <div class="registro-bg">
    <div class="form-container">
      <form @submit.prevent="registrarHoras">
        <div
          class="form-group"
          v-for="(valor, dia) in horas"
          :key="dia"
        >
          <label>{{ dia }}</label>
          <div class="hora-minutos">
            <input
              v-model.number="horas[dia].horas"
              type="number"
              min="0"
              max="24"
              placeholder="Horas"
              required
            />
            <select
              v-model="horas[dia].minutos"
              required
            >
              <option value="0">00</option>
              <option value="30">30</option>
            </select>
            <span>hrs</span>
          </div>
        </div>

        <button type="submit" class="btn-registrar">Registrar</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { backendURL } from '../../config/config.js'

export default {
  name: 'RegistroHoras',
  data() {
    return {
      horas: {
        Lunes: { horas: 0, minutos: 0 },
        Martes: { horas: 0, minutos: 0 },
        Miércoles: { horas: 0, minutos: 0 },
        Jueves: { horas: 0, minutos: 0 },
        Viernes: { horas: 0, minutos: 0 },
        Sábado: { horas: 0, minutos: 0 },
        Domingo: { horas: 0, minutos: 0 }
      }
    }
  },
  methods: {
    async registrarHoras() {

      try {
         const response = await axios.post(`${backendURL}Horario`, this.horas, {
           timeout: 2000
         });

        // Simulación de respuesta
        //const response = { data: true }

        if (response.data === true) {
          alert('Horas registradas correctamente');
          // Redirigir si es necesario
        } else {
          alert('El registro falló. Intenta nuevamente.');
        }

      } catch (error) {
        console.error('Error al registrar horas:', error);
        alert(error.response?.data || 'Error al registrar las horas');
      }
    }
  }
}
</script>

<style scoped>
.registro-bg {
  background-color: gray;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.form-container {
  width: 100%;
  max-width: 600px;
  background-color: #fff;
  border-radius: 8px;
  padding: 2rem;
  box-shadow: 0px 2px 10px rgba(0, 0, 0, 0.1);
}

.form-group {
  display: flex;
  flex-direction: column;
  margin-bottom: 1rem;
}

.hora-minutos {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

input[type="number"],
select {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 5px;
  width: 80px;
}

.btn-registrar {
  width: 100%;
  background-color: #1e1e1e;
  color: #fff;
  padding: 0.6rem;
  border: none;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
}
</style>