Para criar o **consumer**, foi necessário criar um projeto do tipo **WorkerService**.

Dentro do projeto, para garantir a segregação de responsabilidades e facilitar a manutenção, o código foi dividido em três camadas:

- **Application**:  
  Contém o consumer do Kafka.

- **Infra**:  
  Contém o `DbContext` do projeto, além do banco de dados que é carregado em memória.

- **Domain**:  
  Contém as classes que configuram o modelo da aplicação.

---

O serviço de consumer do Kafka está implementado na classe `CotacaoConsumerService` [LINK](https://github.com/le-affes/processo_seletivo_Itau/blob/main/7/WorkerService/WorkerService/Application/Consumers/CotacaoConsumerService.cs) 

Esse serviço escuta continuamente um tópico Kafka que recebe cotações de ativos. A cada nova mensagem, ele verifica se a cotação já está registrada no banco de dados, evitando duplicatas.  

- Se for uma cotação nova, ela é salva.  
- Caso contrário, a mensagem é ignorada.
