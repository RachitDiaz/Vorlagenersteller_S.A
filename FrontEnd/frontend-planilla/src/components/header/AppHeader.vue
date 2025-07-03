<template>
  <div >
    <header class="app-header">
      <div class="top-bar">
        <div class="title" @click="goToLanding">{{ currentTitle}}</div>
        <button @click="handleAuthAction" class="auth-button">
          {{ isLoggedIn ? 'Cerrar sesión' : 'Iniciar sesión' }}
        </button>
      </div>
    </header>

    <nav v-if="isLoggedIn" class="nav-bar">
      <router-link
        v-for="item in filteredMenuItems"
        :key="item.path"
        :to="item.path"
        class="nav-button"
        active-class="active"
      >
        {{ item.name }}
      </router-link>
    </nav>
  </div>
</template>


<script setup>
import {ref, computed} from 'vue';
import {useRoute, useRouter} from 'vue-router';

const route = useRoute();
const router = useRouter();

const rol = ref(localStorage.getItem('rol') || '');
const token = ref(localStorage.getItem('jwtToken'));
const isLoggedIn = computed(() => !!token.value);

const filteredMenuItems = computed(() => {
  return menuItems.filter((item) => {
    const path = item.path;

    if (path === '/') return true;
    if (
      [
        '/DashboardDueno',
        '/ListaBeneficios',
        '/ListaEmpresas',
        '/ListaEmpleados',
        '/VerPlanilla',
        '/ReporteCostos',
      ].includes(path)
    ) {
      return rol.value === 'Dueno';
    }
    if (
      [
        '/DashboardEmpleado',
        '/BeneficiosEmpleado',
        '/RegistroHoras',
        '/ReportePago',
        '/VerEmpleado',
      ].includes(path)) {
      return rol.value === 'Empleado';
    }

    return false;
  });
});

const handleAuthAction = () => {
  if (isLoggedIn.value) {
    localStorage.removeItem('jwtToken');
    token.value = null;
    router.push('/');
  } else {
    router.push('/login');
  }
};

const goToLanding = () => {
  router.push('/');
};

const currentTitle = computed(() => {
  return route.meta.title || 'Default Title';
});

const menuItems = [
  {name: 'Lista Beneficios', path: '/ListaBeneficios'},
  {name: 'Dashboard Empleado', path: '/DashboardEmpleado'},
  {name: 'Lista Empresas', path: '/ListaEmpresas'},
  {name: 'Dashboard Dueño', path: '/DashboardDueno'},
  {name: 'Lista Empleados', path: '/ListaEmpleados'},
  {name: 'Ver Empleado', path: '/VerEmpleado'},
  {name: 'Beneficios Empleado', path: '/BeneficiosEmpleado'},
  {name: 'Registro de Horas', path: '/RegistroHoras'},
  {name: 'Planilla', path: '/VerPlanilla'},
  {name: 'Reporte de Pagos', path: '/ReportePago'},
  {name: 'Reporte de pagos', path: '/ReporteCostos'},
];
</script>

<style scoped>
.app-header {
  background-color: white;
  box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  align-items: stretch;
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
  width: 100%
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
  margin-top: 3.6rem;
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
