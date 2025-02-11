# Sistema de Delivery Integrado

Este projeto implementa um sistema de delivery utilizando .NET, Node.js, Vue.js, MongoDB e RabbitMQ para processar, enfileirar, consumir e exibir pedidos em uma interface web. O sistema é dividido em duas etapas principais:

1. **Processamento e Enfileiramento de Pedidos**: Desenvolvido em .NET (C#), responsável por ler pedidos do MongoDB, tratá-los e enviá-los para uma fila no RabbitMQ.
2. **Consumo e Apresentação de Pedidos**: Composto por um backend em Node.js que consome mensagens do RabbitMQ, persiste os pedidos no MongoDB e um frontend em Vue.js que exibe os pedidos na interface web.

## Tecnologias Utilizadas

- **Backend**: .NET (C#) e Node.js
- **Frontend**: Vue.js
- **Banco de Dados**: MongoDB
- **Mensageria**: RabbitMQ

## Estrutura do Projeto

O projeto é composto pelos seguintes serviços:

- **deliveryapi**: Serviço em .NET que lê pedidos do MongoDB, processa e envia para o RabbitMQ.
- **mongo**: Banco de dados MongoDB para armazenamento dos pedidos.
- **rabbitmq**: Serviço de mensageria para enfileiramento e consumo de mensagens.
- **backend-node**: Serviço em Node.js que consome pedidos da fila do RabbitMQ e os persiste no MongoDB.
- **frontend-vue**: Aplicação Vue.js para exibição e gerenciamento dos pedidos.

## Configuração do Ambiente

### Passo 1: Clonar o Repositório
```bash
git clone https://github.com/wesleysimplicio/desafio-privacy.git
cd desafio-privacy
```

### Passo 2: Construir e Iniciar os Contêineres
```bash
docker-compose up --build
```

### Passo 3: Acessar os Serviços

- **API .NET**: [http://localhost:8080](http://localhost:8080)
- **MongoDB**: `mongodb://localhost:27017`
- **RabbitMQ Management UI**: [http://localhost:15672](http://localhost:15672) (usuário: `guest`, senha: `guest`)
- **Frontend Vue.js**: [http://localhost:8080](http://localhost:8080)
- **Backend Node.js**: [http://localhost:3000](http://localhost:3000)

## Variáveis de Ambiente

### Configuração do `deliveryapi`

- `MongoDbSettings__ConnectionString`: String de conexão com o MongoDB.
- `MongoDbSettings__DatabaseName`: Nome do banco de dados MongoDB.
- `JwtSettings__SecretKey`: Chave secreta para geração de tokens JWT.
- `JwtSettings__Issuer`: Emissor do token JWT.
- `JwtSettings__Audience`: Audiência do token JWT.
- `JwtSettings__ExpiryInMinutes`: Tempo de expiração do token JWT em minutos.
- `RabbitMqSettings__HostName`: Hostname do RabbitMQ.
- `RabbitMqSettings__Port`: Porta do RabbitMQ.
- `RabbitMqSettings__UserName`: Usuário do RabbitMQ.
- `RabbitMqSettings__Password`: Senha do RabbitMQ.
- `RabbitMqSettings__QueueName`: Nome da fila no RabbitMQ.

## Etapas do Sistema

### 1. Processamento e Enfileiramento de Pedidos
O serviço **deliveryapi** é responsável por:

- Ler pedidos do MongoDB.
- Processar os pedidos.
- Enviar os pedidos para a fila `orders_queue` no RabbitMQ.

### 2. Consumo e Apresentação de Pedidos

- **Backend Node.js**: Consome mensagens da fila `orders_queue` e as persiste no MongoDB.
- **Frontend Vue.js**: Exibe os pedidos em uma interface web, permitindo a visualização e gerenciamento dos pedidos.

## Fluxo do Sistema

1. O serviço **.NET (deliveryapi)** lê os pedidos do MongoDB.
2. Os pedidos são processados e enviados para a fila do RabbitMQ.
3. O **backend Node.js** consome os pedidos da fila e os salva no MongoDB.
4. O **frontend Vue.js** exibe os pedidos na interface web.

## Critérios de Avaliação

- **Conhecimento das Tecnologias**: Proficiência no uso de .NET, Node.js, Vue.js, MongoDB e RabbitMQ.
- **Qualidade do Código**: Clareza, organização, legibilidade e manutenibilidade.
- **Performance**: Desempenho e otimização do sistema.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## Contato

Para mais informações, entre em contato com **Wesley Simplicio** através do e-mail **wesleysimplicio@live.com**.
