<template>
  <div class="container">
    <div class="tab-selector">
      <button
        :class="['tab', selectedTab === 'empleado' ? 'active' : '']"
        @click="selectedTab = 'empleado'"
      >
        Empleado
      </button>
      <button
        :class="['tab', selectedTab === 'dueno' ? 'active' : '']"
        @click="selectedTab = 'dueno'"
      >
        Dueño
      </button>
    </div>

    <transition name="fade-slide" mode="out-in">
      <div :key="selectedTab" class="form-wrapper">
        <form @submit.prevent="login">
          <div class="form-group">
            <label>correo</label>
            <input
              type="email"
              v-model="correo"
              :placeholder="selectedTab === 'empleado' ? 'Correo de la empresa' : 'Correo personal'"
              required
            />
          </div>
          <div class="form-group">
            <label>contraseña</label>
            <input 
            type="password" 
            v-model="contrasena"
            required/>
          </div>
          <button class="login-button">Ingresar</button>

          <a href="#" class="link">
            {{ selectedTab === 'empleado' ? '¿Olvidó su contraseña?' : 'Crear empresa' }}
          </a>
        </form>
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const selectedTab = ref('empleado')
const correo = ref('')
const contrasena = ref('')

onMounted(async () => {
  localStorage.removeItem('jwtToken');
})

const login = async () => {
  try {
    const response = await axios.post('https://localhost:7296/api/login', {
      Correo: correo.value,
      Contrasena: contrasena.value,
      Rol: selectedTab.value
    });

    const token = response.data.token;
    localStorage.setItem('jwtToken', token);

    window.location.href = '/ListaBeneficios';
  } catch (error) {
    console.error(error.response?.data?.mensaje || 'Error en la solicitud');
  }
};
</script>

<style scoped>
.container {
  background: #d6d6d6;
  width: 400px;
  margin: 5rem auto;
  border-radius: 10px;
  padding: 1rem;
}

.tab-selector {
  display: flex;
  border-radius: 9999px;
  overflow: hidden;
  background: #e0e0e0;
  margin-bottom: 1rem;
}

.tab {
  flex: 1;
  padding: 0.6rem;
  background: transparent;
  border: none;
  font-weight: bold;
  cursor: pointer;
  transition: background 0.3s ease;
}

.tab.active {
  background: #e3d2f4;
  color: black;
}

.form-wrapper {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
}

input {
  width: 100%;
  padding: 0.5rem;
  margin-top: 0.25rem;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.login-button {
  width: 100%;
  padding: 0.6rem;
  background: #1c1c1c;
  color: white;
  border: none;
  border-radius: 5px;
  font-weight: bold;
  margin-top: 0.5rem;
}

.link {
  display: block;
  margin-top: 0.75rem;
  font-size: 0.85rem;
  color: #2a2a8e;
  text-align: left;
  text-decoration: none;
}

.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.3s ease;
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>
