<template>
    <div class="container">
      <div class="card">
        <h2>Beneficios disponibles</h2>
  
        <p class="description">
          Agregar los beneficios disponibles a seleccionar por cada empleado, y la cantidad máxima de beneficios que pueden elegir.
        </p>
  
        
        <div class="benefit-list">
          <div v-for="beneficio in beneficios" :key="beneficio.id" class="benefit-item">
            <div class="avatar">
              {{ beneficio.nombre.charAt(0) }}
            </div>
            <span class="benefit-name">{{ beneficio.nombre }}</span>
          </div>
        </div>

        <button class="add-benefit " @click="showModal">Añadir beneficios</button>
        <ModalAgregarBeneficios ref="modalRef" />

        <div class="slider-section">
          <label class="slider-label">Cantidad de Beneficios por Empleado</label>
          <div class="slider-control">
            <span>0</span>
            <input type="range" min="0" :max="max" v-model="selectedAmount" class="slider" />
            <span>{{ selectedAmount }}-{{ max }}</span>
          </div>
        </div>
  
        <div class="button-group">
          <button class="cancel">Cancelar</button>
          <button class="accept" @click="submit">Aceptar</button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import axios from 'axios'
  import ModalAgregarBeneficios from '../modals/ModalAgregarBeneficios.vue'
  import { backendURL } from '../../config/config.js'

  const router = useRouter()

  const max = 4
  const selectedAmount = ref(0)
  
  const beneficios = ref([])
  const modalRef = ref(null)

  function showModal() {
    modalRef.value?.show()
  }

  function submit() {
    alert(`Has aceptado seleccionar hasta ${selectedAmount.value} beneficios por empleado.`)
  }

  onMounted(async () => {
    try {
      const token = localStorage.getItem("jwtToken");
      console.log('Token:', token);
      if (!token) {
        alert('Tiene que iniciar sesión primero.');
        setTimeout(() => {
          router.push('/login');
        }, 2000);
        return;
      }

      const response = await axios.get(`${backendURL}Beneficios`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      beneficios.value = response.data;
    } catch (error) {
      if (error.response && error.response.status === 401) {
        localStorage.removeItem('jwtToken');
        alert('Sesión expirada o token inválido.');
        router.push('/login');
      } else {
        console.error('Error cargando beneficios:', error);
        alert('Error al cargar los beneficios desde el servidor.');
      }
    }
  })
  </script>
  
  <style scoped>
  .container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 90vh;
    background-color: #fdfcfe;
  }
  
  .card {
    background-color: #e9e1f0;
    padding: 2rem;
    border-radius: 1.5rem;
    box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.1);
    max-width: 430px;
    width: 100%;
  }
  
  h2 {
    font-size: 1.2rem;
    margin: 0 0 0.5rem 0;
  }
  
  .description {
    font-size: 0.9rem;
    color: #555;
    margin-bottom: 1rem;
  }
  
  .add-benefit {
    background-color: #7e57c2;
    color: white;
    border: none;
    padding: 0.4rem 1rem;
    border-radius: 1rem;
    font-size: 0.85rem;
    cursor: pointer;
    margin-bottom: 1rem;
  }
  
  .benefit-list {
    background-color: #f8f3ff;
    border-radius: 0.5rem;
    padding: 1rem;
    margin-bottom: 1.5rem;
  }
  
  .benefit-item {
    display: flex;
    align-items: center;
    gap: 1rem;
    margin-bottom: 0.75rem;
  }
  
  .avatar {
    background-color: #e0d1fa;
    color: #6a1b9a;
    font-weight: bold;
    border-radius: 50%;
    width: 32px;
    height: 32px;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 0.9rem;
  }
  
  .slider-section {
    margin-top: 1rem;
    font-size: 0.9rem;
  }
  
  .slider-label {
    display: block;
    margin-bottom: 0.4rem;
  }
  
  .slider-control {
    display: flex;
    align-items: center;
    gap: 0.75rem;
  }
  
  .slider {
    flex: 1;
    accent-color: #6a1b9a;
  }
  
  .button-group {
    display: flex;
    justify-content: center;
    gap: 1rem;
    margin-top: 1.5rem;
  }
  
  .cancel {
    background-color: #7e57c2;
    color: white;
    border: none;
    padding: 0.6rem 1.2rem;
    border-radius: 1rem;
    cursor: pointer;
  }
  
  .accept {
    background-color: #ede7f6;
    color: #333;
    border: none;
    padding: 0.6rem 1.2rem;
    border-radius: 1rem;
    box-shadow: inset 0px 2px 4px rgba(0, 0, 0, 0.2);
    cursor: pointer;
  }
  </style>
  