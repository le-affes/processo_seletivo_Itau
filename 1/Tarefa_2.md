Abaixo estão as justificativas para os dados escolhidos:

ativo(
	id_ativo INT NOT NULL, -- Identificador único do ativo, e chave primária da tabela.
	cod VARCHAR(15) NOT NULL, -- Código do ativo conforme negociado na B3. Foi definido um tamanho de até 15 caracteres para suportar diferentes tipos de ativos, cujos códigos podem chegar a 7 caracteres. A escolha também visa otimizar o uso de armazenamento.
	nome VARCHAR(150) NOT NULL, -- Nome do ativo conforme registrado na B3. O tamanho de 150 caracteres foi definido para suportar nomes longos, especialmente de fundos imobiliários e derivativos.
)

cotacao (
	id_cotac INT NOT NULL, -- Identificador único da cotação, e chave primária da tabela.
	id_ativo INT NOT NULL, -- Chave estrangeira que referencia o ativo.
	prco_uni DECIMAL(15,2) NOT NULL, -- Valor numérico com 15 dígitos, sendo 2 após a vírgula. Escolhido para permitir o armazenamento de valores altos, suportando preços unitários até cerca de 247 vezes superiores ao ativo mais valioso da B3 atualmente.
	dt_hora DATETIME(6) NOT NULL -- Data e hora da cotação, com precisão de microssegundos. A precisão é relevante, considerando que a latência da B3 é de aproximadamente 350 microssegundos.
)


operacao (
	id_opr INT NOT NULL, -- Identificador único da operação, e chave primária da tabela.
	id_usr INT NOT NULL, -- Chave estrangeira que referencia o usuário.
	id_ativo INT NOT NULL, -- Chave estrangeira que referencia o ativo.
	qtd DECIMAL(10,2) NOT NULL, -- Quantidade de ações negociadas, com precisão de duas casas decimais para suportar o mercado fracionário (mínimo 0,01 ação).
	prco_uni DECIMAL(15,2) NOT NULL, -- Preço unitário a qual o ativo foi vendido. Segue o mesmo formato das tabelas cotacao e ativo.
	tipo_opr ENUM('C', 'V') NOT NULL, -- Enum que suporta dois valores, representando os tipos de operação permitidos: Compra (C) e Venda (V). Foi escolhido por ser um tipo de dado leve (1 byte), além de que garantir clareza.
	corrtg DECIMAL(14,4) NOT NULL, -- Valor da corretagem aplicada na operação, com até 10 dígitos antes da vírgula e 4 casas decimais para precisão em valores monetários.
	dt_hora DATETIME(6) NOT NULL -- Data e hora da cotação, com precisão de microssegundos. A precisão é relevante, considerando que a latência da B3 é de aproximadamente 350 microssegundos.
)


usuario(
  id_usr INT NOT NULL, -- Identificador único do usuário, e chave primária da tabela.
  nome VARCHAR(100) NOT NULL, -- Nome completo do usuário, com suporte a nomes compostos.
  email VARCHAR(254) NOT NULL, -- Endereço de e-mail do usuário. O tamanho segue a RFC 5321, que define 254 caracteres como o limite prático.
  pct_corrtg DECIMAL(7,6) NOT NULL, -- Percentual de corretagem normalizado entre 0 e 1, com precisão de até 6 casas decimais.
)


posicao(
	id_posic INT NOT NULL, -- Identificador único da posição, e chave primária da tabela.
	id_usr INT NOT NULL, -- Chave estrangeira que referencia o usuário.
	id_ativo INT NOT NULL, -- Chave estrangeira que referencia o ativo.
	qtd DECIMAL(10,2) NOT NULL, -- Quantidade de ações negociadas, com precisão de duas casas decimais para suportar o mercado fracionário (mínimo 0,01 ação).
	prco_medio DECIMAL(15,3) NOT NULL, -- Custo médio ponderado das aquisições do ativo na posição. O tipo de dado foi escolhido para armazenar valores monetários com alta precisão, suportando preços elevados e permitindo até 3 casas decimais.
	p_l DECIMAL(15,3) NOT NULL, -- Valor do lucro ou prejuízo do cliente, armazenado com alta precisão para suportar variações financeiras significativas, incluindo valores elevados e até 3 casas decimais.
)