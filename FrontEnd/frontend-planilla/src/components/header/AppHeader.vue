<template>
  <header class="app-header">
    <div class="top-bar">
      <div class="title" @click="goToLanding">{{ currentTitle }}</div>
      <button @click="handleAuthAction" class="auth-button">
        {{ isLoggedIn ? 'Cerrar sesión' : 'Iniciar sesión' }}
      </button>
    </div>

    <nav v-if="isLoggedIn" class="nav-bar">
      <router-link
        v-for="item in menuItems"
        :key="item.path"
        :to="item.path"
        class="nav-button"
        active-class="active"
      >
        {{ item.name }}
      </router-link>
    </nav>
  </header>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'

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
  router.push('/')
}

const currentTitle = computed(() => {
  return route.meta.title || 'Default Title'
})

const menuItems = [
  { name: 'Inicio', path: '/' },
  { name: 'Lista Beneficios', path: '/ListaBeneficios' },
  { name: 'Lista Empresas', path: '/ListaEmpresas' },
  { name: 'Ver Empresa', path: '/VerEmpresa' },
  { name: 'Lista Empleados', path: '/ListaEmpleados' },
  { name: 'Ver Empleado', path: '/VerEmpleado' },
  { name: 'Beneficios Empleado', path: '/BeneficiosEmpleado' },
]
</script>

<style scoped>
.app-header {
  background-color: white;
  box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 1000;
}

.top-bar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.8rem 1rem;
}

.title {
  font-size: 1.5rem;
  font-weight: bold;
  color: #343a40;
  cursor: pointer;
}
.title:hover {
  color: #007bff;
}

.auth-button {
  background-color: #1c1c1c;
  color: white;
  padding: 0.4rem 0.8rem;
  border: none;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
}

.nav-bar {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  padding: 0.5rem 1rem;
  background-color: #f8f9fa;
  border-top: 1px solid #ddd;
}

.nav-button {
  margin: 0 0.5rem;
  padding: 0.4rem 0.8rem;
  border: none;
  background-color: transparent;
  text-decoration: none;
  color: #333;
  font-weight: 500;
  border-radius: 5px;
  transition: background-color 0.2s;
}
.nav-button:hover {
  background-color: #e2e6ea;
}
.active {
  background-color: #007bff;
  color: white;
}
</style>
