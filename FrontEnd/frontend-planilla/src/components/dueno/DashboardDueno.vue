<template>
    <div class="dashboard">
        <h2>Resumen de Empresa</h2>

        <div class="grid">
            <div class="grafico-barra">
                <Bar :data="barChartData" :options="barChartOptions" />
            </div>

            <div class="acciones">
                <h3>Próximos pagos</h3>
                <table>
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Acción</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>30/07/2025</td>
                            <td><a href="#">Pagar</a></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="pagos">
                <h3>Últimos pagos</h3>
                <div class="tabla-scroll">
                    <table>
                        <thead>
                            <tr>
                                <th>Planillas</th>
                                <th>Fecha</th>
                                <th>Costo Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="pago in pagos" :key="pago.fechaPago">
                                <td>{{ pago.nombreEmpresa }}</td>
                                <td>{{ pago.fechaPago }}</td>
                                <td>₡{{ pago.costoTotal.toLocaleString() }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="grafico-pie">
                <Pie :data="pieChartData" :options="pieChartOptions" />
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, ArcElement, CategoryScale, LinearScale } from 'chart.js'
import { Bar, Pie } from 'vue-chartjs'
import { backendURL } from '../../config/config'

ChartJS.register(Title, Tooltip, Legend, BarElement, ArcElement, CategoryScale, LinearScale)

const pagos = ref([])
const empleadosPorMes = ref([])
const graficoCostos = ref([])

const barChartData = computed(() => ({
    labels: empleadosPorMes.value.map(e => e.mes),
    datasets: [
        {
            label: 'Tiempo completo',
            data: empleadosPorMes.value.map(e => e.tiempoCompleto),
            backgroundColor: '#10b981'
        },
        {
            label: 'Medio tiempo',
            data: empleadosPorMes.value.map(e => e.horas),
            backgroundColor: '#8b5cf6'
        },
        {
            label: 'Servicios profesionales',
            data: empleadosPorMes.value.map(e => e.servicios),
            backgroundColor: '#ef4444'
        }
    ]
}))

const barChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
        legend: { position: 'top' },
        title: {
            display: true,
            text: 'Cantidad de empleados'
        }
    },
    scales: {
        y: { beginAtZero: true }
    }
}

const pieChartData = computed(() => ({
    labels: graficoCostos.value.map(e => e.name),
    datasets: [{
        data: graficoCostos.value.map(e => e.value),
        backgroundColor: ['#10b981', '#8b5cf6', '#ef4444']
    }]
}))

const pieChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
        title: {
            display: true,
            text: 'Costo planilla'
        },
        legend: {
            position: 'right'
        }
    }
}

onMounted(async () => {
    const token = localStorage.getItem("jwtToken")

    try {
        const empleadosResponse = await axios.get(`${backendURL}Empleado/GetEmpleadosEmpresa`, {
            headers: { Authorization: `Bearer ${token}` }
        })

        const empleados = empleadosResponse.data
        let tiempoCompleto = 0
        let horas = 0
        let servicios = 0

        empleados.forEach(e => {
            switch (e.tipoContrato) {
                case 'Tiempo completo': tiempoCompleto++; break
                case 'Medio tiempo': horas++; break
                case 'Servicios profesionales': servicios++; break
            }
        })

        empleadosPorMes.value = [
            { mes: 'Actual', tiempoCompleto, horas, servicios }
        ]

        const pagosResponse = await axios.get(`${backendURL}Reportes/ObtenerUltimosPagosEmpresa`, {
            headers: { Authorization: `Bearer ${token}` }
        })
        pagos.value = pagosResponse.data.map(p => ({
            nombreEmpresa: p.nombreEmpresa,
            fechaPago: p.fechaPago,
            costoTotal: p.salariosTiempoCompleto + p.totalPagosLey + p.beneficiosTotales
        }))

        if (pagosResponse.data.length > 0) {
            const ultimo = pagosResponse.data[0]
            const totalPagosLey =
                ultimo.totalSEM + ultimo.totalIVM + ultimo.totalBP +
                ultimo.totalAF + ultimo.totalIMAS + ultimo.totalINA +
                ultimo.totalFCL + ultimo.totalPC + ultimo.totalINS

            graficoCostos.value = [
                { name: 'Tiempo Completo', value: ultimo.salariosTiempoCompleto },
                { name: 'Cargas Sociales', value: totalPagosLey },
                { name: 'Beneficios', value: ultimo.beneficiosTotales }
            ]
        }

    } catch (error) {
        console.error("Error cargando datos del dashboard:", error)
    }
})
</script>

<style scoped>
.dashboard {
    padding: 2rem;
}

.grid {
    display: grid;
    grid-template-columns: 2fr 1fr;
    grid-template-rows: auto auto;
    gap: 2rem;
}

.grafico-barra {
    grid-column: 1 / 2;
    height: 300px;
}

.grafico-pie {
    grid-column: 2 / 3;
    height: 300px;
}

.acciones,
.pagos {
    background: #f2f5fc;
    padding: 1rem;
    border-radius: 10px;
}

.acciones table,
.pagos table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.95rem;
}

th,
td {
    padding: 0.4rem;
    border: 1px solid #ccc;
}

a {
    text-decoration: underline;
    color: #1e40af;
}
.tabla-scroll {
  max-height: 200px; /* Ajusta según el tamaño de tu fila */
  overflow-y: auto;
}
</style>
