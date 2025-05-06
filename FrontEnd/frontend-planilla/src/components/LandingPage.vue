<template>
    <section class="landing">
      <div class="content">
        <div class="text">
          <h1>
            Simplifique la<br />
            administración de<br />
            planillas de empleados
          </h1>
          <p>
            Vorlagenersteller le ofrece una solución completa para gestionar
            las planillas de sus empleados de manera eficiente y sin complicaciones.
          </p>
          <div class="buttons">
            <router-link to="/login" class="primary">Comenzar ahora</router-link>
          </div>
        </div>
  
      </div>
    </section>
  </template>
  
  <script setup>
  </script>
  
  <style scoped>
  .landing {
    padding: 4rem 2rem;
    background-color: #FAF7FF;
  }
  
  .content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 3rem;
    max-width: 1200px;
    margin: 0 auto;
    flex-wrap: wrap;
  }
  
  .text {
    flex: 1 1 500px;
  }
  
  .text h1 {
    font-size: 2.8rem;
    font-weight: 800;
    line-height: 1.2;
    margin-bottom: 1rem;
  }
  
  .text p {
    font-size: 1rem;
    color: #6b7280; /* gris suave */
    margin-bottom: 2rem;
  }
  
  .buttons {
    display: flex;
    gap: 1rem;
  }
  
  .primary {
    background-color: #7c3aed;
    color: white;
    border: none;
    padding: 0.75rem 1.5rem;
    border-radius: 6px;
    font-weight: bold;
    cursor: pointer;
  }
  
  .secondary {
    background-color: transparent;
    border: 1px solid #d1d5db;
    color: #111827;
    padding: 0.75rem 1.5rem;
    border-radius: 6px;
    font-weight: bold;
    cursor: pointer;
  }
  
  .image-placeholder {
    flex: 1 1 400px;
    background-color: #e5e5e5;
    height: 300px;
    border-radius: 8px;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 2rem;
    color: #9ca3af;
  }
  </style>
  