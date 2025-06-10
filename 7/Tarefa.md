Para criar o consumer, foi necessário criar um projeto do tipo WorkerService.
Dentro do projeto, para garantir a segregação de responsabilidades e manutenção, o código foi dividido em três camadas:
	- Application: contém o consumer do Kafka
	- Infra: contém o DbContext do projeto, além do banco de dados que é carregado em memória
	- Domain: contém as classes que configuram o modelo da aplicação

O serviço de consumer do kafka está na classe CotacaoConsumerService. 
Esse serviço escuta continuamente um tópico Kafka que recebe cotações de ativos. 
A cada nova mensagem, ele verifica se a cotação já está registrada no banco de dados, evitando duplicatas. 
Se for uma cotação nova, ela é salva; caso contrário, é ignorada. 