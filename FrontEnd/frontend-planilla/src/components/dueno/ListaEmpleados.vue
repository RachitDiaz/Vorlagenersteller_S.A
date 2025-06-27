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
        <tr v-for="empleado in empleadosFiltrados" :key="empleado.cedulaEmpleado">
          <td>{{ empleado.nombre }}</td>
          <td>{{ empleado.apellido1 }}</td>
          <td>{{ empleado.apellido2 }}</td>
          <td>{{ empleado.cedulaEmpleado }}</td>
          <td>Empleado</td>
          <td class="acciones">
            <button class="edit" @click="abrirEdicion(empleado.cedulaEmpleado)">Editar</button>
            <button class="delete">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>
    <ModalAgregarEmpleado ref="modalAgregarEmpleado" />
    <ModalEditarEmpleado ref="modalEditarEmpleado" />
  </section>
</template>

<script>
import axios from 'axios'
import ModalAgregarEmpleado from '../modals/ModalAgregarEmpleado.vue'
import ModalEditarEmpleado from '../modals/ModalEditarEmpleado.vue'
import { useRouter } from 'vue-router'
import { backendURL } from '../../config/config.js'

const router = useRouter();
const token = localStorage.getItem("jwtToken");
export default {
  name: "ListaEmpleados",
  components: {
    ModalAgregarEmpleado,
    ModalEditarEmpleado
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
        (e?.Nombre?.toLowerCase() ?? "").includes(this.search.toLowerCase()) ||
        (e?.CedulaEmpleado ?? "").includes(this.search)
      );
    }
  },
  methods: {
    abrirModal() {
      this.$refs.modalAgregarEmpleado.show()
    },
    abrirEdicion(cedulaEmpleado)  {
      this.$refs.modalEditarEmpleado.show(cedulaEmpleado)
    },
    async obtenerEmpleados() {

      try {

        if (!token) {
          alert('Tiene que iniciar sesión primero.');
          setTimeout(() => {
            router.push('/login');
          }, 2000);
          return;
        }
        const response = await axios.get(`${backendURL}Empleado/GetEmpleadosEmpresa`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });

        this.empleados = response.data;
      } catch (error) {
        if (error.response && error.response.status === 401) {
          localStorage.removeItem('jwtToken');
          alert('Sesión expirada o token inválido.');
          router.push('/login');
        } else {
          console.error('Error cargando empleados:', error);
          alert('Error al cargar los empleados desde el servidor.');
        }
      }
    }
  },
  mounted() {
    this.obtenerEmpleados()
  }
}
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
  