Esta aplicação foi construída com base no modelo **MVC**. Nas diferentes camadas:

- **Model**:  
  Contém as classes que compõem o modelo base da aplicação.

- **View**:  
  Contém a apresentação, ou Front-end.

- **Controller**:  
  Contém o controller que se comunica com os serviços da aplicação e orquestra o Front-end.

---

O **Front-end** e o **Back-end** foram construídos usando **.NET**.

Além disso, foram adicionadas camadas para melhor organização do código e separação de responsabilidades:

- **Camada de Infraestrutura** (`Infra`):
  - Contém o `DbContext` da aplicação.
  - Inclui uma base de dados carregada em memória ao iniciar a aplicação.

- **Camada de Aplicação** (`Application`):
  - Contém os diferentes serviços que a aplicação oferece.
  - É a única camada que se comunica diretamente com o banco de dados.

---

Os dados solicitados na tarefa estão apresentados na **Home Page**.
