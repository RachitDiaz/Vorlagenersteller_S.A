<!-- EmployeeTable.vue -->
<template>
  <section class="employee-table">
    <div class="header">
      <h2>Gestión de Empleados</h2>
      <button class="add-button " @click="abrirModal">Añadir empleado</button>
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
    <ModalAgregarEmpleado ref="modalAgregarEmpleado" />
  </section>
</template>

<script>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import ModalAgregarEmpleado from '../modals/ModalAgregarEmpleado.vue'

export default {
  name: "AdministrarEmpleados",
  components: {
    ModalAgregarEmpleado
  },
  data() {
    return {
      search: "",
      empleados: []
    };
  },
  computed: {
    empleadosFiltrados() {
      return this.empleados.filter(e =>
        e.nombre.toLowerCase().includes(this.search.toLowerCase()) ||
        e.cedula.includes(this.search)
      );
    }
  },
  setup() {
    const router = useRouter()

    onMounted(() => {
      const token = localStorage.getItem('jwtToken')
      if (!token) {
        alert('Tiene que iniciar sesión primero.');
        setTimeout(() => {
          router.push('/login');
        }, 2000);
      }
    })
  },
  methods: {
    abrirModal() {
      this.$refs.modalAgregarEmpleado.show()
    },
    async obtenerEmpleados() {
      try {
        const response = await axios.get('https://localhost:7296/api/Empleado')
        this.empleados = response.data
      } catch (error) {
        console.error('Error al obtener empleados:', error)
      }
    }
  },
  mounted() {
    this.obtenerEmpleados()
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
  