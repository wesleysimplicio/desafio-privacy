const amqp = require('amqplib');
const mongoose = require('mongoose');

mongoose.connect('mongodb://localhost:27017/DeliveryDb')
  .then(() => console.log('Conectado ao MongoDB'))
  .catch(err => console.error('Erro ao conectar ao MongoDB:', err));

const orderSchema = new mongoose.Schema({
  _id: String,
  customerName: String,
  status: Boolean,
  items: [
    {
      productName: String,
      quantity: Number,
      price: Number,
    },
  ],
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
        }

        // await Orders.deleteOne({ _id: payload.Id });
        // await Orders.deleteOne({ _id: mongoose.Types.ObjectId(payload.Id) });

        // Atualiza o pedido se ele já existir, ou cria um novo
        const order = await Orders.findOneAndUpdate(
          { _id: payload.Id }, // Condição para encontrar o pedido
          orderData, // Dados a serem atualizados
          { upsert: true, new: true } // Cria se não existir e retorna o novo pedido
        );

        console.log(`[x] Pedido salvo ou atualizado no MongoDB com ID: ${order._id}`);

        channel.ack(msg);
      }
    });
  } catch (error) {
    console.error('Erro ao consumir a fila:', error);
  }
}

consumeQueue();