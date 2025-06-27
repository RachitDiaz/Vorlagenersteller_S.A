<template>
  <div class="registro-bg">
    <div class="form-container">
      <form @submit.prevent="registrarDueno">
        <div class="form-group">
          <label>Nombre</label>
          <input v-model="form.nombre" type="text" required />
        </div>

        <div class="form-group">
          <label>Apellido 1</label>
          <input v-model="form.apellido1" type="text" required />
        </div>

        <div class="form-group">
          <label>Apellido 2</label>
          <input v-model="form.apellido2" type="text" required />
        </div>

        <div class="form-group">
          <label>Género</label>
          <select v-model="form.genero" required>
            <option value="Masculino">Masculino</option>
            <option value="Femenino">Femenino</option>
            <option value="Otro">Otro</option>
          </select>
        </div>

        <div class="form-group">
          <label>Cédula</label>
          <input v-model="form.cedula" type="text" placeholder="x-xxxx-xxxx" required />
        </div>

        <div class="form-group">
          <label>Correo Electrónico</label>
          <input v-model="form.correo" type="email" placeholder="ejemplo@correo.com" required />
        </div>

        <div class="form-group">
          <label>Contraseña*</label>
          <input v-model="form.contrasena" type="password" required />
        </div>

        <div class="form-group">
          <label>Confirmar Contraseña*</label>
          <input v-model="form.confirmarContrasena" type="password" required />
        </div>

        <div class="form-group">
          <label>Teléfono</label>
          <input v-model="form.telefono" type="tel" placeholder="(+506) xxxx-xxxx" />
        </div>

        <div class="form-group">
          <label>Dirección</label>
          <input v-model="form.direccion" type="text" />
        </div>

        <button type="submit" class="btn-registrar">Registrar</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { backendURL } from '../config/config.js'

export default {
  name: 'RegistroDueno',
  data() {
    return {
      form: {
        nombre: '',
        apellido1: '',
        apellido2: '',
        cedula: '',
        correo: '',
        contrasena: '',
        confirmarContrasena: '',
        telefono: '',
        direccion: '',
        genero: 'Masculino'
      }
    }
  },
  methods: {
    async registrarDueno() {
      if (this.form.contrasena !== this.form.confirmarContrasena) {
        alert('Las contraseñas no coinciden');
        return;
      }

      try {
        const persona = {
          cedula: this.form.cedula,
          nombre: this.form.nombre,
          apellido1: this.form.apellido1,
          apellido2: this.form.apellido2,
          genero: this.form.genero,
          usuario: {
            correo: this.form.correo,
            contrasena: this.form.contrasena
          }
        };

        const payload = {
          persona,
          telefono: this.form.telefono,
          direccion: this.form.direccion
        };

        const response = await axios.post(`${backendURL}Dueno`, payload, {
          timeout: 2000
        });

        if (response.data === true) {
          alert('Dueño registrado correctamente');
          this.$router.push('/login');
        } else {
          alert('El registro falló. Intenta nuevamente.');
        }

      } catch (error) {
        console.error('Error al registrar el dueño:', error);
        alert(error.response?.data || 'Error al registrar el dueño');
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

input {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 5px;
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
