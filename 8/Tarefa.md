As implementações solicitadas foram feitas no próprio serviço de consumer (`CotacaoConsumerService`).

A observabilidade é implementada por meio de logs (`ILogger`), que registram cada etapa importante do processamento:  
- Consumo de mensagens  
- Salvamento de cotações  
- Tentativas de fallback  
- Erros  

Esses logs ajudam a entender o comportamento do sistema em produção e facilitam a detecção de falhas.

O **circuit breaker** (implementado com **Polly**) monitora as falhas nas tentativas de processar as mensagens.  

- Quando a taxa de falhas ultrapassa um limite configurado, o circuito é “aberto”, interrompendo temporariamente novas tentativas para evitar sobrecarga e permitir recuperação.  
- Durante esse período, novas execuções são bloqueadas até o sistema mostrar sinais de melhora.

O **fallback** define o que deve ser feito quando o circuito está aberto ou quando ocorre uma falha.  

Nesse caso, o serviço tenta buscar os dados de cotação diretamente de uma API externa como plano B, salvando-os no banco caso estejam válidos.  

- Foi usada a API para consulta de cotações: [https://b3api.vercel.app/](https://b3api.vercel.app/).  
- A implementação foi feita por meio de outro serviço, o `AssetApiConsumer`, que faz a chamada à API, mapeia os dados relevantes e salva no banco.
