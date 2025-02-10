# Sistema de Delivery Integrado

Este projeto é um sistema de delivery que utiliza .NET, Node.js, Vue.js, MongoDB e RabbitMQ para processar pedidos, enfileirá-los, consumi-los e apresentá-los em uma interface web. O sistema é dividido em duas etapas principais:

1. **Processamento e Enfileiramento de Pedidos**: Desenvolvido em .NET (C#), responsável por ler pedidos do MongoDB, tratá-los e enviá-los para uma fila no RabbitMQ.
2. **Consumo e Apresentação de Pedidos**: Composto por um backend em Node.js que consome mensagens do RabbitMQ e persiste os pedidos no MongoDB, e um frontend em Vue.js que exibe os pedidos em uma interface web.

## Tecnologias Utilizadas

- **Backend**: .NET (C#) e Node.js
- **Frontend**: Vue.js (Web Components)
- **Banco de Dados**: MongoDB
- **Mensageria**: RabbitMQ

## Estrutura do Projeto

O projeto é composto pelos seguintes serviços:

- **deliveryapi**: Serviço em .NET que lê pedidos do MongoDB, realiza o tratamento e envia para o RabbitMQ.
- **mongo**: Banco de dados MongoDB para armazenamento dos pedidos.
- **rabbitmq**: Serviço de mensageria RabbitMQ para enfileiramento e consumo de mensagens.
- **backend-node**: Serviço em Node.js que consome pedidos da fila do RabbitMQ e os persiste no MongoDB.
- **frontend-vue**: Aplicação Vue.js que exibe os pedidos em uma interface web.

## Configuração do Ambiente

Para rodar o projeto, siga os passos abaixo:

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git
   cd seu-repositorio
Construa e inicie os contêineres:

bash
Copy
docker-compose up --build
Acesse os serviços:

API .NET: http://localhost:5000

MongoDB: mongodb://localhost:27017

RabbitMQ Management UI: http://localhost:15672 (usuário: guest, senha: guest)

Frontend Vue.js: http://localhost:8080 (ou a porta configurada no Vue.js)

Backend Node.js: http://localhost:3000 (ou a porta configurada no Node.js)

Variáveis de Ambiente
O serviço deliveryapi utiliza as seguintes variáveis de ambiente:

MongoDbSettings__ConnectionString: String de conexão com o MongoDB.

MongoDbSettings__DatabaseName: Nome do banco de dados MongoDB.

JwtSettings__SecretKey: Chave secreta para geração de tokens JWT.

JwtSettings__Issuer: Emissor do token JWT.

JwtSettings__Audience: Audiência do token JWT.

JwtSettings__ExpiryInMinutes: Tempo de expiração do token JWT em minutos.

RabbitMqSettings__HostName: Hostname do RabbitMQ.

RabbitMqSettings__Port: Porta do RabbitMQ.

RabbitMqSettings__UserName: Usuário do RabbitMQ.

RabbitMqSettings__Password: Senha do RabbitMQ.

RabbitMqSettings__QueueName: Nome da fila no RabbitMQ.

Etapas do Sistema
Etapa 1: Processamento e Enfileiramento de Pedidos
O serviço deliveryapi é responsável por:

Ler pedidos do MongoDB.

Realizar o tratamento dos pedidos.

Enviar os pedidos tratados para a fila orders_queue no RabbitMQ.

Etapa 2: Consumo e Apresentação de Pedidos
Backend Node.js: Consome as mensagens da fila orders_queue e persiste os pedidos no MongoDB.

Frontend Vue.js: Exibe os pedidos em uma interface web, permitindo que os usuários visualizem e gerenciem os pedidos.

Como Funciona o Fluxo
O serviço .NET (deliveryapi) lê os pedidos do MongoDB.

Os pedidos são tratados e enviados para a fila do RabbitMQ.

O backend Node.js consome os pedidos da fila e os persiste no MongoDB.

O frontend Vue.js consulta o MongoDB e exibe os pedidos em uma interface web.

Critérios de Avaliação
Conhecimento das Tecnologias: Proficiência no uso de .NET, Node.js, Vue.js, MongoDB e RabbitMQ.

Qualidade do Código: Clareza, organização, legibilidade e manutenibilidade do código.

Performance: Desempenho e otimização do sistema.

Licença
Este projeto está licenciado sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

Contato
Para mais informações, entre em contato com [seu nome] em [seu email].

Copy

### Melhorias e Destaques:
1. **Foco no Delivery**: O título e a descrição agora destacam que o sistema é voltado para delivery.
2. **Frontend Vue.js**: Adicionei detalhes sobre o frontend Vue.js, incluindo como acessá-lo.
3. **Backend Node.js**: Incluí informações sobre o backend Node.js que consome a fila do RabbitMQ.
4. **Fluxo do Sistema**: Adicionei uma seção explicando o fluxo completo do sistema, desde o processamento até a exibição dos pedidos.
5. **Portas dos Serviços**: Especifiquei as portas para acessar o frontend Vue.js e o backend Node.js.
