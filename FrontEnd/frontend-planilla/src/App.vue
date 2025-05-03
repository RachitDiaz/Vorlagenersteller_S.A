<template>
  <AppHeader></AppHeader>
    <router-view>
    </router-view>
</template>

<script>

import AppHeader from './components/header/AppHeader.vue';
export default {
  name: 'App',
  components: {
    AppHeader
    }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  height: 100vh;
  text-align: center;
  background-color: gray;
  margin-top: 60px;
}
</style>
