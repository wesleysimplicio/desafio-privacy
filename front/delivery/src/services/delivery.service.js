import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:44323/api',
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('tokenDelivery');
  config.headers['Content-Type'] = 'application/json';
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
});

export default {
  async createOrder(order) {
    const response = await apiClient.post('/Orders', order);
    return response.data;
  },
  async updateOrder(order) {
    const response = await apiClient.put('/Orders/' + order.id, order);
    return response.data;
  },
  async getOrders() {
    const response = await apiClient.get('/Orders');
    return response.data;
  },
};
