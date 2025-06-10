Para aplicar auto-scaling horizontal no serviço CotacaoConsumerService são necessários os passos:

	Criar um Dockerfile para empacotar o serviço como container, permitindo que ele seja facilmente replicado em múltiplas instâncias.

	Escolher uma plataforma de orquestração, como Kubernetes ou AWS, para configurar o escalonamento automático com base em métricas.
	No caso do AWS Auto Scaling, o serviço ajusta automaticamente a capacidade dos aplicativos para manter desempenho e reduzir custos. 
	Além disso, permite escalar recursos como instâncias EC2, tarefas ECS, tabelas DynamoDB e réplicas Aurora. 

	Todas as instâncias do serviço devem utilizar o mesmo GroupId Kafka, garantindo que o Kafka distribua automaticamente as partições do tópico entre os consumidores ativos.

	É importante garantir que o tópico Kafka esteja configurado com múltiplas partições. O paralelismo do consumo depende da quantidade de partições disponíveis.



Para a comparação, foram escolhidos dois métodos: Round-robin vs Método de conexão mínima

Método Round-robin (estático):
O balanceamento é feito pelo servidor de nomes autoritativo, que retorna os endereços IP dos servidores de forma sequencial, um a um, distribuindo as requisições de forma uniforme. 
Não considera a carga atual dos servidores, sendo simples e fácil de implementar. É adequado para ambientes homogêneos onde os servidores possuem capacidade semelhante.

Método de conexão mínima (dinâmico):
O balanceador de carga monitora o número de conexões ativas em cada servidor e direciona as novas requisições para aquele com menor quantidade de conexões. 
Assume que todas as conexões consomem recursos equivalentes. Assim, permite um balanceamento mais eficiente em ambientes com cargas variáveis entre servidores.

O método round-robin não responde a variações na carga dos servidores, podendo sobrecarregar alguns enquanto outros ficam ociosos. 
Já o método de conexão mínima ajusta o tráfego conforme a carga, melhorando o uso dos recursos.

A escolha entre eles depende da necessidade de controle dinâmico da carga e da complexidade que se deseja para o balanceamento. 
O Round-robin é mais simples, enquanto o Método de conexão mínima é mais adaptável a variações no ambiente.