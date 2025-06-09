Essa aplicação foi construída com base no modelo MVC. Nas diferentes camadas:
	-Model: contém as classes que compõem o modelo base da aplicação
	-View: contém a apresentação, ou Front-end
	-Controller: contém o controller que se comunica com os serviços da aplicação e orquestra o Front-end

O Front-end, assim como o Back-end foram construídos usando .NET. 
Além disso, adicionei camadas para organizar melhor o código e separar responsabilidades. Foram adicionadas;
	-Camada de Infraestrutura (diretório Infra): contém o DbContext da aplicação, e uma base de dados que é carregada em memória quando a aplicação se inicia.
	-Camada de Aplicação (diretório Application): contém os diferentes serviços que a aplicação oferece, além de ser a única camada que se comunica com o banco de dados.
	
Os dados pedidos na tarefa estão apresentados na Home Page.