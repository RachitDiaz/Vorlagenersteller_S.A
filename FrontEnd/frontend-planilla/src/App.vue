<template>
  <div id="layout">
    <AppHeader />
    <main>
      <router-view />
    </main>
    <AppFooter />
  </div>
</template>

<script>
import AppHeader from './components/header/AppHeader.vue';
import AppFooter from './components/footer/AppFooter.vue';
export default {
  components: { AppHeader, AppFooter }
}
</script>

<style>
#layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

main {
  flex: 1;
  padding-top: 60px; /* espacio para el header fijo */
  background-color: #FAF7FF; /* color de fondo del main */
}
</style>
