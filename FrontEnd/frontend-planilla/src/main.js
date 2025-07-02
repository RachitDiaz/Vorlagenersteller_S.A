import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from "vue-router";
import RegistrarDueno from "./components/RegistrarDueno.vue";
import Login from "./components/MainLogin.vue";
import LandingPage from './components/LandingPage.vue';
import BeneficiosEmpleado from './components/empleado/BeneficiosSeleccionados.vue';
import ListaBeneficios from './components/dueno/ListaBeneficios.vue';
import ListaEmpleados from './components/dueno/ListaEmpleados.vue';
import Planilla from './components/dueno/Planilla.vue';
import '@fortawesome/fontawesome-free/css/all.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import ListaEmpresas from './components/dueno/ListaEmpresas.vue';
import DashboardEmpleado from './components/empleado/DashboardEmpleado.vue';
import VerEmpleado from './components/empleado/VerEmpleado.vue';
import RegistrarHoras from './components/empleado/RegistrarHoras.vue';
import ReportePagoPlanillaEmpleado from './components/empleado/ReportePagoPlanilla.vue';
import ReportePagoPlanillaEmpresa from './components/dueno/ReportePagoEmpresa.vue';
import ReportePagoHistorico from './components/dueno/ReportePagoHistorico.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/ListaBeneficios", name: "ListaBeneficios", component: ListaBeneficios, meta: { title: 'Lista de beneficios' } },
    { path: "/DashboardEmpleado", name: "DashboardEmpleado", component: DashboardEmpleado, meta: { title: 'Dashboard Empleado' } },
    { path : "/VerPlanilla", name: "VerPlanilla", component: Planilla, meta: { title: 'Planilla' } },
    { path: "/RegistrarDueno", name: "RegistrarDueño", component: RegistrarDueno, meta: { title: 'Registrar Dueño' } },
    { path: "/ListaEmpresas", name: "ListaEmpresa", component: ListaEmpresas, meta: { title: 'Lista de Empresas' } },
    { path: "/VerEmpleado", name: "VerEmpleado", component: VerEmpleado, meta: { title: 'Informacion de empleado' } },
    { path: "/login", name: "MainLogin", component: Login, meta: { title: 'Login' } },
    { path: "/", name: "LandingPage", component: LandingPage, meta: { title: 'Landing Page' } },
    { path: "/BeneficiosEmpleado", name: "BeneficiosEmpleado", component: BeneficiosEmpleado, meta: { title: 'Beneficios' } },
    { path: "/ListaEmpleados", name: "ListaEmpleados", component: ListaEmpleados, meta: { title: 'Administrar Empleados' } },
    { path: "/RegistroHoras", name: "RegistroHoras", component: RegistrarHoras, meta: { title: 'Registro de Horas' } },
    { path: "/ReportePago", name: "ReportePago", component: ReportePagoPlanillaEmpleado, meta: { title: 'Reportes de Pago' } },
    { path: "/ReporteCostos", name: "ReporteCostos", component: ReportePagoPlanillaEmpresa, meta: { title: 'Reportes de Pago' } },
    { path: "/ReporteHistorico", name: "ReporteHistorico", component: ReportePagoHistorico, meta: { title: 'Pagos historicos' } },
  ],
});

router.beforeEach((to, from, next) => {
  const publicPages = ['/', '/login', '/RegistrarDueno'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('jwtToken');

  if (authRequired && !loggedIn) {
    return next('/login');
  }

  next();
});

createApp(App).use(router).mount('#app')