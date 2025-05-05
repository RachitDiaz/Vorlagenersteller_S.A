import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from "vue-router";
import RegistrarDueno from "./components/RegistrarDueno.vue";
import Login from "./components/MainLogin.vue";
import LandingPage from './components/LandingPage.vue';
import BeneficiosEmpleado from './components/empleado/BeneficiosSeleccionados.vue';
import AgregarBeneficios from './components/dueno/AgregarBeneficio.vue';
import AdministrarEmpleados from './components/dueno/AdministrarEmpleados.vue';
import '@fortawesome/fontawesome-free/css/all.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import ListaEmpresas from './components/ListaEmpresas.vue';
import VerEmpresa from './components/VerEmpresa.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/AgregarBeneficios", name: "AgregarBeneficios", component: AgregarBeneficios, meta: { title: 'Agregar Beneficios' } },
    { path: "/RegistrarDueno", name: "RegistrarDueño", component: RegistrarDueno, meta: { title: 'Registrar Dueño' } },
    { path: "/ListaEmpresas", name: "ListaEmpresa", component: ListaEmpresas, meta: { title: 'Lista de Empresas' } },
    { path: "/VerEmpresa", name: "VerEmpresa", component: VerEmpresa, meta: { title: 'Ver Empresa' } },
    { path: "/login", name: "MainLogin", component: Login, meta: { title: 'Login' } },
    { path: "/", name: "LandingPage", component: LandingPage, meta: { title: 'Landing Page' } },
    { path: "/BeneficiosEmpleado", name: "BeneficiosEmpleado", component: BeneficiosEmpleado, meta: { title: 'Beneficios' } },
    { path: "/AdministrarEmpleados", name: "AdministrarEmpleados", component: AdministrarEmpleados, meta: { title: 'Administrar Empleados' } },
  ],
});

createApp(App).use(router).mount('#app')