Para construir a API, foi pensado um projeto .NET com quatro camadas:
	-API/Controller: Controllers que contém os endpoints da API
	-Application: contém os services que executam as regras de negócio e se comunicam com o banco de dadaos. Também contém os DTOs para deserialização
	-Infra: contém o DbContext da aplicação, além de dados armazenados em JSON que são carregados quando a aplicação inicia
	-Domain: contém as classes que compõem o modelo da aplicação.
	
A documentação da API está no arquivo ApiCorretora.json