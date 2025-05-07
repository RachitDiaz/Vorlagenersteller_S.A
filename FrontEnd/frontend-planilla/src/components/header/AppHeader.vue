<template>
  <header class="app-header">
    <div class="title" @click="goToLanding"> {{ currentTitle }} </div>
    <button @click="handleAuthAction" class="auth-button">
      {{ isLoggedIn ? 'Cerrar sesión' : 'Iniciar sesión' }}
    </button>
  </header>
</template>

<script setup>
import { useRoute, useRouter } from 'vue-router'
import { computed, ref } from 'vue'

const route = useRoute()
const router = useRouter()

const token = ref(localStorage.getItem('jwtToken'))
const isLoggedIn = computed(() => !!token.value)

const handleAuthAction = () => {
  if (isLoggedIn.value) {
    localStorage.removeItem('jwtToken')
    token.value = null 
    router.push('/login')
  } else {
    router.push('/login')
  }
}

const goToLanding = () => {
  router.push('/') // Redirige al landing page
}

const currentTitle = computed(() => {
  return route.meta.title ? route.meta.title : 'Default Title'
})
</script>

<style scoped>
.app-header {
  height: 60px;
  background-color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 1000;
  padding: 0 1rem;
}

.title {
  font-size: 1.5rem;
  font-weight: bold;
  color: #343a40;
  position: relative;
  z-index: 1;
  cursor: pointer;
  transition: color 0.2s ease;
}
.title:hover {
  color: #007bff;
}

.auth-button {
  position: absolute;
  right: 1rem;
  background-color: #1c1c1c;
  color: white;
  padding: 0.4rem 0.8rem;
  border: none;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
  z-index: 2;
}
</style>
