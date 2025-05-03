import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from "vue-router";
import AgregarBeneficios from "./components/AgregarBeneficios.vue";
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {path: "/AgregarBeneficios", name: "AgregarBeneficios", component: AgregarBeneficios},
  ],
});

createApp(App).use(router).mount('#app')