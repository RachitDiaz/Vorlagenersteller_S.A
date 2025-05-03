<template>
    <header class="app-header">
      <div class="title">{{ currentTitle }}</div>
    </header>
  </template>
  
  <script setup>
  import { useRoute } from 'vue-router'
  import { computed } from 'vue'
  
  const route = useRoute()
  
  const currentTitle = computed(() => {
    return route.meta.title ? route.meta.title : 'Default Title'
  })
  </script>
  
  <style scoped>
  .app-header {
    background-color: #f8f9fa;
    padding: 1rem;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    z-index: 1000;
  }
  .title {
    font-size: 1.5rem;
    font-weight: bold;
    color: #343a40;
  }

  .header {
    background-color: red;
    padding: 1rem;
    text-align: center;
    font-weight: bold;
    font-size: 1.25rem;
  }
  </style>