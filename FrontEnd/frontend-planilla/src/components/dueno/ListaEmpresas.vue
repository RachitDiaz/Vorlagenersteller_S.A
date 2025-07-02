<template>
  <section class="employee-table">
    <div class="header">
      <h2>Gestión de Empresas</h2>
      <button class="add-button" @click="showModal">Agregar empresa</button>
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
          <th>Cédula Jurídica</th>
          <th>Nombre</th>
          <th>Razón Social</th>
          <th>Tipo de Pago</th>
          <th>Descripción</th>
          <th>Activo</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="empresa in empresasFiltradas" :key="empresa.cedulaJuridica">
          <td>{{ empresa.cedulaJuridica }}</td>
          <td>{{ empresa.nombre }}</td>
          <td>{{ empresa.razonSocial }}</td>
          <td>{{ empresa.tipoDePago }}</td>
          <td>{{ empresa.descripcion }}</td>
          <td>
            <span v-if="empresa.activo" class="badge bg-success">Sí</span>
            <span v-else class="badge bg-danger">No</span>
          </td>
          <td class="acciones">
            <button class="ver-btn" @click="verEmpresa(empresa.cedulaJuridica)">Ver</button>
            <button class="delete-btn" @click="eliminarEmpresa(empresa.cedulaJuridica)">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <ModalRegistrarEmpresa ref="modalRef" />
    <ModalVerEmpresa ref="modalVerEmpresa" />
  </section>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import ModalRegistrarEmpresa from '../modals/ModalRegistrarEmpresa.vue'
import ModalVerEmpresa from '../modals/ModalVerEmpresa.vue'
import { backendURL } from '../../config/config.js'

const router = useRouter()
const modalRef = ref(null)
const modalVerEmpresa = ref(null)
const empresas = ref([])
const search = ref('')

function showModal() {
  modalRef.value?.show()
}

function verEmpresa(cedula) {
  modalVerEmpresa.value?.show(cedula)
}

const empresasFiltradas = computed(() => {
  return empresas.value.filter((e) =>
    (e?.nombre?.toLowerCase() ?? '').includes(search.value.toLowerCase()) ||
    (e?.cedulaJuridica ?? '').includes(search.value)
  )
})

onMounted(async () => {
  const token = localStorage.getItem("jwtToken")
  if (!token) {
    alert('Tiene que iniciar sesión primero.')
    setTimeout(() => router.push('/login'), 2000)
    return
  }

  try {
    const response = await axios.get(`${backendURL}Empresa/ObtenerEmpresas`, {
      headers: { Authorization: `Bearer ${token}` }
    })
    empresas.value = response.data
  } catch (error) {
    console.error("Error al obtener empresas:", error)
    alert("Hubo un error al cargar las empresas.")
  }
})
</script>

<style scoped>
.employee-table {
  background: #fff;
  padding: 2rem;
  border-radius: 10px;
  max-width: 1000px;
  margin: 5rem auto 2rem auto;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
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
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  min-width: 100px;
}

.edit {
  color: #3b82f6;
  background: none;
  border: none;
  cursor: pointer;
}

.ver-btn {
  color: #6366f1;
  background: none;
  border: none;
  padding: 0.25rem 0.75rem;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.2s ease;
}

.ver-btn:hover {
  background-color: #eef2ff;
}

.delete-btn {
  color: #ef4444;
  background: none;
  border: none;
  padding: 0.25rem 0.75rem;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.2s ease;
}

.delete-btn:hover {
  background-color: #ffe4e6;
}
</style>
