# Interaction System / Sistema de Interação

### Sistema de Interação desenvolvido no projeto Solo Dev 2026, pelo membro Bernardo Miguel Florenzano do projeto Dev-U.

## Estrutura do pacote:
O Sistema de Interação se baseia em dois scripts: 
* **IInteragivel**: Definição da Interface para os objetos interagíveis;
* **PlayerInteracao**: Definição do código de interação do player, usando de um trigger collider para identificar Interfaces IInteragivel próximas.

## Como usar:
Na pasta "Exemplos" há um exemplo de objeto interagível, sendo que para criar um novo objeto e fazê-lo ser interagível pelo player deve se seguir esses passos:

1. Criar um novo script MonoBehaviour com base na interface IInteragivel:

`public class NomeDoInteragivel : MonoBehaviour, IInteragivel`

2. Implementar os membros da Interface (Funções definidas em IInteragivel)

3. Criar um GameObject com um colisor e o script de interagivel criado.

4. Para o player, adicione o script "PlayerInteracao", lembrando de se ter um trigger collider no mesmo objeto contendo o script (Use um objeto filho "ColliderInteracao", por exemplo)

5. No script de interação, foi utilizado um listener para um evento input baseado em um outro script "InputPlayerManager": `InputPlayerManager.OnInteractInput += Interage;`. Caso não se queira replicar esse script, basta remover o listener de OnEnable e OnDisable e chamar o metodo "Interage" da forma que prefirir.

6. Visual do input que aparece ao estar perto suficiente do objeto interagível pode ser customizado, bastando apenas replicar o prefab "InputSprite" com um visual e comportamentos customizados (como uma animação, por exemplo).


Obs.: Na cena CenaExemplo, foram usados scripts teste para demonstrar o funcionamento do sistema.
