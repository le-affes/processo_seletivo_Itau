Abaixo estão as justificativas para os dados escolhidos:

---

### Tabela `ativo`

- **id_ativo** (`INT NOT NULL`)
  - Identificador único do ativo, e chave primária da tabela.
- **cod** (`VARCHAR(15) NOT NULL`)
  - Código do ativo conforme negociado na B3.
  - Até 15 caracteres para suportar diferentes tipos de ativos, cujos códigos podem chegar a 7 caracteres.
  - Visa otimizar o uso de armazenamento.
- **nome** (`VARCHAR(150) NOT NULL`)
  - Nome do ativo conforme registrado na B3.
  - Até 150 carcateres para suportar nomes longos, especialmente de fundos imobiliários e derivativos.

---

### Tabela `cotacao`

- **id_cotac** (`INT NOT NULL`)
  - Identificador único da cotação, e chave primária da tabela.
- **id_ativo** (`INT NOT NULL`)
  - Chave estrangeira que referencia o ativo.
- **prco_uni** (`DECIMAL(15,2) NOT NULL`)
  - Valor numérico com 15 dígitos, sendo 2 após a vírgula.
  - Suporta preços unitários altos (até 247x o ativo mais valioso da B3).
- **dt_hora** (`DATETIME(6) NOT NULL`)
  - Data e hora da cotação com precisão de microssegundos.
  - Relevante devido à latência da B3 (~350 microssegundos).

---

### Tabela `operacao`

- **id_opr** (`INT NOT NULL`)
  - Identificador único da operação, e chave primária da tabela.
- **id_usr** (`INT NOT NULL`)
  - Chave estrangeira que referencia o usuário.
- **id_ativo** (`INT NOT NULL`)
  - Chave estrangeira que referencia o ativo.
- **qtd** (`DECIMAL(10,2) NOT NULL`)
  - Quantidade de ações negociadas.
  - Precisão de duas casas decimais para suportar mercado fracionário (mínimo 0,01).
- **prco_uni** (`DECIMAL(15,2) NOT NULL`)
  - Preço unitário a qual o ativo foi vendido.
  - Mesmo formato das tabelas `cotacao` e `ativo`.
- **tipo_opr** (`ENUM('C', 'V') NOT NULL`)
  - Tipo de operação: Compra (C) ou Venda (V).
  - Tipo de dado leve (1 byte) e garante clareza.
- **corrtg** (`DECIMAL(14,4) NOT NULL`)
  - Valor da corretagem aplicada.
  - Até 10 dígitos antes da vírgula e 4 casas decimais.
- **dt_hora** (`DATETIME(6) NOT NULL`)
  - Data e hora da operação com precisão de microssegundos.
  - Relevante devido à latência da B3 (~350 microssegundos).

---

### Tabela `usuario`

- **id_usr** (`INT NOT NULL`)
  - Identificador único do usuário, e chave primária da tabela.
- **nome** (`VARCHAR(100) NOT NULL`)
  - Nome completo do usuário, com suporte a nomes compostos.
- **email** (`VARCHAR(254) NOT NULL`)
  - Endereço de e-mail do usuário.
  - Tamanho conforme a RFC 5321 (limite prático de 254 caracteres).
- **pct_corrtg** (`DECIMAL(7,6) NOT NULL`)
  - Percentual de corretagem.
  - Normalizado entre 0 e 1, com até 6 casas decimais.

---

### Tabela `posicao`

- **id_posic** (`INT NOT NULL`)
  - Identificador único da posição, e chave primária da tabela.
- **id_usr** (`INT NOT NULL`)
  - Chave estrangeira que referencia o usuário.
- **id_ativo** (`INT NOT NULL`)
  - Chave estrangeira que referencia o ativo.
- **qtd** (`DECIMAL(10,2) NOT NULL`)
  - Quantidade de ações na posição.
  - Precisão de duas casas para ter suporte ao mercado fracionário (mínimo 0,01 ação).
- **prco_medio** (`DECIMAL(15,3) NOT NULL`)
  - Custo médio ponderado das aquisições.
  - Alta precisão para armazenar valores monetários.
- **p_l** (`DECIMAL(15,3) NOT NULL`)
  - Valor do lucro ou prejuízo do cliente.
  - Suporta variações financeiras significativas com até 3 casas decimais.
