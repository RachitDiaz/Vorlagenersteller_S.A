<template>
    <section class="employee-table">
      <div class="header">
        <h2>Gestión de Empleados</h2>
        <button class="add-button">Añadir empleado</button>
      </div>
  
      <div class="controls">
        <input type="text" placeholder="Buscar por nombre o cédula..." v-model="search" />
        <div class="buttons">
          <button class="btn">Filtrar</button>
          <button class="btn">Exportar</button>
        </div>
      </div>
  
      <table>
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Primer Apellido</th>
            <th>Segundo Apellido</th>
            <th>Cédula</th>
            <th>Posición</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="empleado in empleadosFiltrados" :key="empleado.id">
            <td>{{ empleado.nombre }}</td>
            <td>{{ empleado.apellido1 }}</td>
            <td>{{ empleado.apellido2 }}</td>
            <td>{{ empleado.cedula }}</td>
            <td>{{ empleado.posicion }}</td>
            <td class="acciones">
              <button class="edit">Editar</button>
              <button class="delete">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </section>
  </template>
  
  <script>
  export default {
    name: "EmployeeTable",
    data() {
      return {
        search: "",
        empleados: [
          { id: 1, nombre: "Ana", apellido1: "Ramírez", apellido2: "Lopez", cedula: "1-2345-6789", posicion: "Gerente" },
          { id: 2, nombre: "Carlos", apellido1: "Mora", apellido2: "Jiménez", cedula: "2-1234-5678", posicion: "Asistente" },
          { id: 3, nombre: "Lucía", apellido1: "Gómez", apellido2: "Alvarado", cedula: "3-9876-5432", posicion: "Director" },
        ]
      };
    },
    computed: {
      empleadosFiltrados() {
        return this.empleados.filter(e =>
          e.nombre.toLowerCase().includes(this.search.toLowerCase()) ||
          e.cedula.includes(this.search)
        );
      }
    }
  };
  </script>
  
  <style scoped>
  .employee-table {
    background: #fff;
    padding: 2rem;
    border-radius: 10px;
    max-width: 1000px;
    margin: 5rem auto 2rem auto;
    box-shadow: 0 2px 8px rgba(0,0,0,0.05);
  }
  
  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
  }
  
  .header h2 {
    font-weight: bold;
    font-size: 1.5rem;
  }
  
  .add-button {
    background-color: #7c3aed;
    color: white;
    border: none;
    padding: 0.5rem 1.2rem;
    border-radius: 9999px;
    cursor: pointer;
  }
  
  .controls {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
    gap: 1rem;
  }
  
  .controls input {
    flex: 1;
    padding: 0.5rem;
    border: 1px solid #ccc;
    border-radius: 6px;
  }
  
  .buttons {
    display: flex;
    gap: 0.5rem;
  }
  
  .btn {
    background-color: #f3f4f6;
    border: 1px solid #ccc;
    padding: 0.5rem 1rem;
    border-radius: 6px;
    cursor: pointer;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
  }
  
  th {
    text-align: left;
    padding: 0.75rem;
    background-color: #f9fafb;
    border-bottom: 1px solid #e5e7eb;
  }
  
  td {
    padding: 0.75rem;
    border-bottom: 1px solid #f1f1f1;
  }
  
  .acciones {
    display: flex;
    gap: 0.5rem;
  }
  
  .edit {
    color: #3b82f6;
    background: none;
    border: none;
    cursor: pointer;
  }
  
  .delete {
    color: #ef4444;
    background: none;
    border: none;
    cursor: pointer;
  }
  </style>
  