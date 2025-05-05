import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from "vue-router";
import AgregarBeneficios from "./components/AgregarBeneficios.vue";
import RegistrarDueno from "./components/RegistrarDueno.vue";
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
  ],
});

createApp(App).use(router).mount('#app')