As implementações solicitadas foram feitas no próprio serviço de consumer (CotacaoConsumerService)

A observabilidade é implementada por meio de logs (ILogger) que registram cada etapa importante do processamento: consumo de mensagens, salvamento de cotações, tentativas de fallback e erros. Esses logs ajudam a entender o comportamento do sistema em produção e facilitam a detecção de falhas.

O circuit breaker (implementado com Polly) monitora as falhas nas tentativas de processar as mensagens. Quando a taxa de falhas ultrapassa um limite configurado, o circuito é “aberto”, interrompendo temporariamente novas tentativas para evitar sobrecarga e permitir recuperação. Durante esse período, novas execuções são bloqueadas até o sistema mostrar sinais de melhora.

O fallback define o que deve ser feito quando o circuito está aberto ou quando uma falha ocorre. Nesse caso, o serviço tenta buscar os dados de cotação diretamente de uma API externa como plano B, salvando-os no banco caso estejam válidos.
Nesse caso, foi usada a API para consulta de cotações: https://b3api.vercel.app/. A implementação foi feita por meio de outro serviço, o AssetApiConsumer. Ele faz uma chamada à API, mapeia os dados relevantes e salva no banco. 
