<template>
    <div class="container mt-5">
      <h1 class="mb-4 text-center">Delivery</h1>
      <div class="row">
        <!-- Formulário para Novo Pedido -->
        <div class="col-lg-6 mb-4">
          <div class="card shadow">
            <div class="card-header bg-primary text-white">
              Novo Pedido
            </div>
            <div class="card-body">
              <form @submit.prevent="submitOrder">
                <div class="mb-3">
                  <label for="customerName" class="form-label">Nome do Cliente</label>
                  <input type="text"
                         class="form-control"
                         id="customerName"
                         v-model="newOrder.customerName"
                         placeholder="Digite o nome do cliente"
                         required>
                </div>
                <div class="mb-3">
                  <label for="productName" class="form-label">Nome do Produto</label>
                  <input type="text"
                         class="form-control"
                         id="productName"
                         v-model="newOrder.items[0].productName"
                         placeholder="Digite o nome do produto"
                         required>
                </div>
                <div class="row mb-3">
                  <div class="col">
                    <label for="quantity" class="form-label">Quantidade</label>
                    <input type="number"
                           class="form-control"
                           id="quantity"
                           v-model="newOrder.items[0].quantity"
                           placeholder="Quantidade"
                           required>
                  </div>
                  <div class="col">
                    <label for="price" class="form-label">Preço</label>
                    <input type="number"
                           class="form-control"
                           id="price"
                           v-model="newOrder.items[0].price"
                           placeholder="Preço"
                           required>
                  </div>
                </div>
                <button type="submit" class="btn btn-success w-100">Enviar Pedido</button>
              </form>
            </div>
          </div>
        </div>
  
        <!-- Pedidos em Andamento e Histórico -->
        <div class="col-lg-6">
          <!-- Pedidos em Andamento -->
          <div class="card shadow mb-4">
            <div class="card-header bg-warning text-dark">
              Pedidos em Andamento 🚚
            </div>
            <div class="card-body">
              <ul class="list-group">
                <li v-for="(order, index) in ordersInProgress"
                    :key="order.id || index"
                    class="list-group-item d-flex justify-content-between align-items-center">
                  <div v-if="order.items && order.items.length">
                    <strong>{{ order.customerName }}</strong><br>
                    {{ order.items[0].productName }} ({{ order.items[0].quantity }}x)
                    - R$ {{ totalOrder(order) }}
                  </div>
                  <button @click="completeOrder(index)"
                          class="btn btn-sm btn-primary">
                    Finalizar
                  </button>
                </li>
              </ul>
            </div>
          </div>
          <!-- Histórico de Pedidos -->
          <div class="card shadow">
            <div class="card-header bg-success text-white">
              Histórico de Pedidos ✅
            </div>
            <div class="card-body">
              <ul class="list-group">
                <li v-for="(order, index) in completedOrders"
                    :key="order.id || index"
                    class="list-group-item">
                  <div v-if="order.items && order.items.length">
                    <strong>{{ order.customerName }}</strong><br>
                    {{ order.items[0].productName }} ({{ order.items[0].quantity }}x)
                    - R$ {{ totalOrder(order) }}
                  </div>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <style>
  @import '~bootstrap/dist/css/bootstrap.min.css';
  
  /* Você pode adicionar estilos customizados aqui, se desejar */
  </style>
  
  <script>
  import deliveryService from '../services/delivery.service.js';
  
  export default {
    name: 'DeliveryComponent',
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
          this.orders = (await deliveryService.getOrders()).map(order => ({
            ...order,
            items: order.items || [],
          }));
        } catch (error) {
          console.error('Erro ao carregar pedidos:', error);
        }
      },
      async completeOrder(index) {
        if (!this.orders[index]) return;
        this.orders[index].status = false;
        try {
          await deliveryService.updateOrder(this.orders[index]);
        } catch (error) {
          console.error('Erro ao atualizar pedido:', error);
        }
      },
      totalOrder(order) {
        if (!order.items || order.items.length === 0) return 0;
        return order.items[0].quantity * order.items[0].price;
      },
    },
    async created() {
      await this.fetchOrders();
    },
  };
  </script>
  