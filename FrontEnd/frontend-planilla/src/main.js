import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from "vue-router";
import AgregarBeneficios from "./components/AgregarBeneficios.vue";
import RegistrarDueno from "./components/RegistrarDueno.vue";
import Login from "./components/MainLogin.vue";
import LandingPage from './components/LandingPage.vue';
import '@fortawesome/fontawesome-free/css/all.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/AgregarBeneficios", name: "AgregarBeneficios", component: AgregarBeneficios, meta: { title: 'Agregar Beneficios' } },
    { path: "/RegistrarDueno", name: "RegistrarDueño", component: RegistrarDueno, meta: { title: 'Registrar Dueño' } },
    { path: "/login", name: "MainLogin", component: Login, meta: { title: 'Login' } },
    { path: "/", name: "LandingPage", component: LandingPage, meta: { title: 'Landing Page' } },

  ],
});

createApp(App).use(router).mount('#app')