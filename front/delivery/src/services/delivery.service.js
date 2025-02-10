import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:44323/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default {
  async createOrder(order) {
    const response = await apiClient.post('/Orders', order);
    return response.data;
  },
  async getOrders() {
    const response = await apiClient.get('/Orders');
    return response.data;
  },
};