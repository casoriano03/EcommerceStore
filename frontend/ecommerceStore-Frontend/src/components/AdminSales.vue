<script setup>
import axios from 'axios';
import { ref, onMounted } from 'vue';

const salesRaw = ref([]);
const totalSales = ref(null);
const filteredDate = ref([]);
const filteredOrder = ref([]);
const startDate = ref(null);
const endDate = ref(null);
const chartHeader = ref(null);
const currentYear = ref(new Date().getFullYear());
const currentMonth = ref(new Date().getMonth());
const currentDate = ref(new Date().getDate());
const currentDay = ref(new Date().getDay());
const monthNames = ref([
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"]);
const dayNames = ref(["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"]);


onMounted(async()=>{
    try {
        const sales = await axios.get('https://localhost:7023/api/Order/GetAllOrders');
        salesRaw.value = sales.data;
    } catch (error) {
        console.log(error)
    }
    getThisYear();
});

const renderChart = ()=>{
    const chart = document.getElementById('myChart')
    new Chart(chart, {
        type: "line",
        data: {
            labels: filteredDate.value,
            datasets: [
                {
                    label: "Order",
                    data: filteredOrder.value,
                    backgroundColor: "rgba(54, 162, 235, 0.2)",
                    borderColor: "rgba(54, 162, 235, 1)",
                    borderWidth: 2,
                    fill: true,
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: true }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
};

const getDates = ()=>{
    filteredDate.value = [];
    filteredOrder.value = [];
    const start = new Date(startDate.value);
    const end = new Date(endDate.value);
    const sales = salesRaw.value.filter(sale =>{
        const orderDate = new Date(sale.orderDate);
        return orderDate >= start && orderDate <= end});
    
    sales.forEach(sale => {
        filteredDate.value.push(sale.orderDate.split("T")[0]);
        filteredOrder.value.push(sale.total);
    });
    renderChart();
};

const getThisYear = async()=>{
    filteredDate.value = [];
    filteredOrder.value = [];
    chartHeader.value = currentYear.value;
    var salesThisYear = 0;
    for (let month = 0; month <= monthNames.value.length; month++) {
        const startOfTheMonth = new Date(currentYear.value, month, 1).toLocaleDateString();
        const endOfTheMonth = new Date(currentYear.value, month + 1, 0).toLocaleDateString();
        filteredDate.value = monthNames.value;
        const monthlySale = await getSales(startOfTheMonth, endOfTheMonth);
        salesThisYear += monthlySale;
    }
    totalSales.value = salesThisYear.toFixed(2);
    renderChart();
};

const getSales =async(startDate, endDate)=>{
    const start = new Date(startDate);
    const end = new Date(endDate);
    var totalSale = 0;
    
    const filteredSales = salesRaw.value.filter(order => {
    const orderDate = new Date(order.orderDate);
    return orderDate >= start && orderDate <= end;
    });

    filteredSales.forEach(sale => {
        totalSale += sale.total;
    });

    filteredOrder.value = [...filteredOrder.value, Number(totalSale.toFixed(2))];
    return totalSale;
};

const getThisMonth = async()=>{
    filteredDate.value = [];
    filteredOrder.value = [];
    var totalSalesThisMonth = 0;
    const numberDaysThisMonth = new Date(currentYear.value, currentMonth.value + 1, 0).getDate();
    chartHeader.value = monthNames.value[currentMonth.value];

    for (let index = 1; index <= numberDaysThisMonth; index++) {
        const startDate = new Date(currentYear.value, currentMonth.value, index);
        const endDate = new Date(currentYear.value, currentMonth.value, index+1);
        const salePerDay = await getSales(startDate, endDate);
        totalSalesThisMonth += salePerDay; 
        filteredDate.value.push(`${index}`);
    };
    totalSales.value = totalSalesThisMonth.toFixed(2);
    renderChart();
};

const getThisWeek = async()=>{
    filteredDate.value = [];
    filteredOrder.value = [];
    var totalSalesThisWeek = 0;
    filteredDate.value = dayNames.value;
    
    for (let index = 0; index < dayNames.value.length; index++) {
        const startDate = new Date(currentYear.value, currentMonth.value, currentDate.value-currentDay.value+1+index);
        const endDate = new Date(currentYear.value, currentMonth.value, currentDate.value-currentDay.value+2+index);
        const salesPerDay = await getSales(startDate, endDate);
        totalSalesThisWeek += salesPerDay;
    };
    totalSales.value = totalSalesThisWeek.toFixed(2);
    chartHeader.value = "This Week's ";
    renderChart();
};

</script>

<template>
    <div class="container">
        <div class="row text-center">
            <div class="col border fs-2">{{ chartHeader }} Total Sales</div>
            <div class="col border fs-2">${{ totalSales }}</div>
        </div>
        <div class="row">
            <div class="col d-flex justify-content-center my-5">
                <canvas id="myChart" style="width:100%;max-width:800px"></canvas>
            </div>
        </div>
        <div class="row my-3 mx-5">
            <div class="my-3 ps-0">
                <button class="btn btn-warning text-dark me-1" @click="getThisYear">This year</button>
                <button class="btn btn-warning text-dark mx-1" @click="getThisMonth">This Month</button>
                <button class="btn btn-warning text-dark mx-1" @click="getThisWeek">This Week</button>
            </div>
            
            <label class="my-1" for="startDate">Start Date: <input v-model="startDate" type="date" name="startDate" id=""></label>
            <label class="my-1" for="endDate">End Date:  <input v-model="endDate" type="date" name="endDate" id=""></label>
            <button class="btn btn-warning text-dark my-2" style="width: 7rem;" @click="getDates">Check Sales</button>
        </div>
    </div>
</template>