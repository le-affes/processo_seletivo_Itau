Testes mutantes fazem parte de uma técnica usada para avaliar a efetividade dos testes automatizados.

Para isso, pequenas mudanças (mutações) são introduzidas no código original. Por exemplo:  
- Alterar um operador  
- Inverter uma condição  
- Modificar um valor literal  

Após a mutação, verifica-se se os testes ainda passam.  

- Se os testes falham, significa que estão efetivamente cobrindo e validando o comportamento do código.  
- Se os testes continuam passando, isso pode indicar que o teste contém falhas ou não é suficientemente rigoroso.

---

### Exemplo aplicado ao método `CalcularPrecoMedio`

**Código original (linha relevante):**

```csharp
if (compra.Quantidade <= 0 || compra.PrecoUni <= 0)
    throw new ArgumentException("Quantidade e preço devem ser maiores que zero.");
```
**Mutação aplicada: alterar <= para <**

```csharp
if (compra.Quantidade < 0 || compra.PrecoUni < 0)
    throw new ArgumentException("Quantidade e preço devem ser maiores que zero.");
```
Com essa mutação, compras com quantidade ou preço igual a zero passam sem exceção, mesmo que deveriam ser inválidas.

Um teste que valida o lançamento da exceção para Quantidade = 0 ou PrecoUni = 0 falharia, detectando a mutação.
