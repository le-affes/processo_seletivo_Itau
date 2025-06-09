/***************************************************************************************************

A trigger atualiza a posição do usuário sempre que uma nova cotação é inserida, 
garantindo que o preço médio e o lucro/prejuízo estejam atualizados em tempo real. 
O preço médio é calculado apenas com operações de compra, 
pois representa o custo de aquisição das ações mantidas na posição. 
As vendas não alteram o preço médio, mas impactam a quantidade e o P&L. 
O P&L é calculado pela diferença entre o preço da última cotação e o preço médio, 
multiplicada pela quantidade da posição, refletindo o ganho ou perda atual do usuario. 

****************************************************************************************************/

CREATE TRIGGER trg_atualiza_posicao_apos_cotacao
AFTER INSERT ON cotacao
FOR EACH ROW
BEGIN
    UPDATE posicao p
    JOIN (
        SELECT 
            o.id_usr,
            o.id_ativo,
            SUM(o.qtd * o.prco_uni) / NULLIF(SUM(o.qtd), 0) AS novo_preco_medio
        FROM operacao o
        WHERE o.id_ativo = NEW.id_ativo
          AND o.tipo_opr = 'C'
        GROUP BY o.id_usr, o.id_ativo
    ) dados ON p.id_usr = dados.id_usr AND p.id_ativo = dados.id_ativo
    SET 
        p.prco_medio = dados.novo_preco_medio,
        p.p_l = (NEW.prco_uni - dados.novo_preco_medio) * p.qtd;
END