CREATE INDEX idx_operacao_usr_ativo 
ON operacao (id_usr, id_ativo);

/*****************************************************************************************************************************************************************************

O índice composto criado na tabela operacao com as colunas (id_usr, id_ativo, dt_hora) visa otimizar consultas frequentes que filtram operações realizadas por um determinado usuário em um ativo específico. 
A escolha da ordem das colunas no índice segue a seletividade natural dos dados: id_usr e id_ativo combinados são altamente seletivos, permitindo uma redução significativa no volume de dados escaneados.

*****************************************************************************************************************************************************************************/

SET @USUARIO = 1; -- valor de exemplo
SET @ATIVO = 10; -- valor de exemplo

SELECT 
    o.*,
    a.cod AS codigo_ativo
FROM 
    (
        SELECT * 
			FROM operacao
		WHERE id_ativo = @ATIVO
			AND dt_hora BETWEEN (NOW() - INTERVAL 30 DAY) AND NOW()
			AND id_usr = @USUARIO
    ) o
INNER JOIN ativo a ON o.id_ativo = a.id_ativo
ORDER BY o.dt_hora DESC;

/*****************************************************************************************************************************************************************************

A consulta foi estruturada para aplicar os filtros diretamente dentro da subquery que acessa a tabela operacao.
Assim, restringindo os registros por: id_ativo, id_usr e intervalo de datas (dt_hora) antes de realizar o join com a tabela ativo. 

Essa abordagem reduz o volume de dados envolvidos na junção, para que o banco de dados trabalhe com um conjunto menor de registros.
Ao filtrar antecipadamente, evita-se a manipulação desnecessária de dados que não atendem aos critérios definidos, o que contribui para a melhoria do desempenho da consulta.

*****************************************************************************************************************************************************************************/