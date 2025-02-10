const amqp = require('amqplib');
const mongoose = require('mongoose');

mongoose.connect('mongodb://localhost:27017/DeliveryDb')
  .then(() => console.log('Conectado ao MongoDB'))
  .catch(err => console.error('Erro ao conectar ao MongoDB:', err));

const orderSchema = new mongoose.Schema({
  customerName: String,
  status: Boolean,
  items: [
    {
      productName: String,
      quantity: Number,
      price: Number,
    },
  ],
  createdAt: { type: Date, default: Date.now },
});

const Orders = mongoose.model('orders', orderSchema);

async function consumeQueue() {
  try {
    const connection = await amqp.connect('amqp://localhost');
    const channel = await connection.createChannel();

    const queue = 'orders_queue';
    await channel.assertQueue(queue, { durable: true });

    console.log(`[*] Aguardando mensagens em ${queue}. Para sair, pressione CTRL+C`);

    channel.consume(queue, async (msg) => {
      if (msg !== null) {
        const payload = JSON.parse(msg.content.toString());
        console.log(`[x] Recebido: ${JSON.stringify(payload)}`);

        if (!payload.CustomerName || !Array.isArray(payload.Items)) {
          console.error('Payload inválido:', payload);
          return;
        }

        const orderData = {
          customerName: payload.CustomerName,
          status: payload.Status,
          items: payload.Items.map(item => ({
            productName: item.ProductName,
            quantity: Number(item.Quantity),
            price: Number(item.Price),
          })),
          createdAt: new Date(),
        }

        const order = new Orders(orderData);
        await order.save(); 

        console.log(`[x] Pedido salvo no MongoDB com ID: ${order._id}`);

        channel.ack(msg);
      }
    });
  } catch (error) {
    console.error('Erro ao consumir a fila:', error);
  }
}

consumeQueue();
