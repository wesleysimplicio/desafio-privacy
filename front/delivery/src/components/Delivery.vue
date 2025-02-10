<template>
    <div class="container mt-5">
        <h1>Delivery</h1>

        <!-- Formulário para criar um novo pedido -->
        <div class="card mb-4 text-left">
            <div class="card-body">
                <h5 class="card-title">Novo Pedido</h5>
                <form @submit.prevent="submitOrder">
                    <div class="form-group">
                        <label for="customerName">Nome do Cliente</label>
                        <input type="text" class="form-control" id="customerName" v-model="newOrder.customerName" required>
                    </div>
                    <div class="form-group">
                        <label for="productName">Nome do Produto</label>
                        <input type="text" class="form-control" id="productName" v-model="newOrder.items[0].productName" required>
                    </div>
                    <div class="form-group">
                        <label for="quantity">Quantidade</label>
                        <input type="number" class="form-control" id="quantity" v-model="newOrder.items[0].quantity" required>
                    </div>
                    <div class="form-group">
                        <label for="price">Preço</label>
                        <input type="number" class="form-control" id="price" v-model="newOrder.items[0].price" required>
                    </div>
                    <button type="submit" class="btn btn-success">Enviar Pedido</button>
                </form>
            </div>
        </div>

        <!-- Lista de Pedidos em Andamento -->
        <div class="card mb-4">
            <div class="card-body">
                <h5 class="card-title">Pedidos em Andamento 🚚</h5>
                <ul class="list-group">
                    <li v-for="(order, index) in ordersInProgress" :key="order.id || index" class="list-group-item">
                        {{ order.customerName }} - {{ order.items[0].productName }} ({{ order.items[0].quantity }}x) -
                        R$ {{ totalOrder(order) }}
                        <button @click="completeOrder(index)" class="btn btn-sm btn-primary float-right">Finalizar</button>
                    </li>
                </ul>
            </div>
        </div>

        <!-- Histórico de Pedidos Finalizados -->
        <div class="card">
            <div class="card-body">
                <h5 class="card-title">Histórico de Pedidos ✅</h5>
                <ul class="list-group">
                    <li v-for="(order, index) in completedOrders" :key="order.id || index" class="list-group-item">
                        {{ order.customerName }} - {{ order.items[0].productName }} ({{ order.items[0].quantity }}x) -
                        R$ {{ totalOrder(order) }}
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<style>
@import '~bootstrap/dist/css/bootstrap.min.css';
</style>
<script>
import deliveryService from '../services/delivery.service.js';

export default {
    name: 'Delivery',
    data() {
        return {
            newOrder: {
                customerName: '',
                status: true,
                items: [
                    {
                        productName: '',
                        quantity: 0,
                        price: 0,
                    },
                ],
            },
            orders: [],
        };
    },
    computed: {
        ordersInProgress() {
            return this.orders.filter(order => order.status);
        },
        completedOrders() {
            return this.orders.filter(order => !order.status);
        },
    },
    methods: {
        async submitOrder() {
            try {
                const response = await deliveryService.createOrder(this.newOrder);
                this.orders.push(response);
                this.resetOrderForm();
            } catch (error) {
                console.error('Erro ao enviar pedido:', error);
            }
        },
        resetOrderForm() {
            this.newOrder = {
                customerName: '',
                status: true,
                items: [
                    {
                        productName: '',
                        quantity: 0,
                        price: 0,
                    },
                ],
            };
        },
        async fetchOrders() {
            try {
                this.orders = await deliveryService.getOrders();
            } catch (error) {
                console.error('Erro ao carregar pedidos:', error);
            }
        },
        completeOrder(index) {
            this.orders[index].status = false;
        },
        totalOrder(order) {
            return order.items[0].quantity * order.items[0].price;
        },
    },
    async created() {
        await this.fetchOrders();
    },
};
</script>