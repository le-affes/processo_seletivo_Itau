Testes mutantes fazem parte de uma técnica usada para avaliar a efetividade dos testes automatizados.

Para fazer, se introduzem pequenas mudanças (mutações) no código original. Por exemplo, alterar um operador, inverter uma condição ou modificar um valor literal. Após isso, verifica-se se o teste ainda passa.
Se os testes falham após a mutação, significa que estão efetivamente cobrindo e validando o comportamento do código.
Se os testes continuam passando, isso pode indicar que o teste contém erros.



Exemplo aplicado ao método CalcularPrecoMedio

Código original (linha relevante):
	if (compra.Quantidade <= 0 || compra.PrecoUni <= 0)
		throw new ArgumentException("Quantidade e preço devem ser maiores que zero.");
	
	
Mutação aplicada: Alterar <= para <
	if (compra.Quantidade < 0 || compra.PrecoUni < 0)
		throw new ArgumentException("Quantidade e preço devem ser maiores que zero.");
		

Com essa mutação, compras com quantidade ou preço igual a zero passam sem exceção, mesmo que deveriam ser inválidas.
Um teste que valida o lançamento da exceção para Quantidade = 0 ou PrecoUni = 0 falharia, detectando a mutação.