using UnityEngine;

[RequireComponent(typeof(IngredientFuserComponent))]
public class RecipienteController : MonoBehaviour, IReceberIngrediente
{
    [Header("Configuração do Recipiente")]
    [Tooltip("BaseIngredientData do Ovo/Farinha")]
    [SerializeField] private BaseIngredientData ingredienteOculto;

    [Tooltip("O prefab do ingrediente")]
    [SerializeField] private GameObject prefabIngredienteBase; 

    private IngredientFuserComponent fuser;
    private IngredientComponent ingredienteAtual;

    private void Awake() {
        fuser = GetComponent<IngredientFuserComponent>();
    }

    public bool AceitaIngrediente(GameObject objeto) {
        IngredientComponent novoIngrediente = objeto.GetComponent<IngredientComponent>();
        
        // So aceita se for um ingrediente e se o recipiente estiver vazio
        if (novoIngrediente == null || ingredienteAtual != null) return false;

        // Cria um objeto temporario invisivel para o CanFuse
        GameObject tempObj = new GameObject("Temp_Sacrificio");
        IngredientComponent tempIng = tempObj.AddComponent<IngredientComponent>();
        tempIng.Setup(ingredienteOculto);

        // Testa se existe receita
        IngredientComponent[] paraTestar = { tempIng, novoIngrediente };
        bool aceita = ValidadorDeReceita.AceitaFusao(fuser, tempIng, novoIngrediente);        
        // Destroi o objeto de teste
        Destroy(tempObj); 
        
        return aceita;
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        IngredientComponent novoIngrediente = objeto.GetComponent<IngredientComponent>();
        if (novoIngrediente == null) return;

        // Instancia o objeto fisico real que vai ser destruido
        GameObject sacrificio = Instantiate(prefabIngredienteBase, transform.position, Quaternion.identity);
        IngredientComponent compSacrificio = sacrificio.GetComponent<IngredientComponent>();
        compSacrificio.Setup(ingredienteOculto);

        //lista de ingredientes que vao ser fundidos
        IngredientComponent[] paraFundir = { compSacrificio, novoIngrediente };

        // Executa a fusao
        if (fuser.TryFusing(paraFundir, out GameObject objetoFundido)) {
            ingredienteAtual = objetoFundido.GetComponent<IngredientComponent>();

            objetoFundido.transform.position = this.transform.position;
            IngredienteFogao ingFogao = objetoFundido.GetComponent<IngredienteFogao>();
            if (ingFogao != null) {
                ingFogao.AjustarColisor();
            }

            //atualiza o DropSystem com o novo objeto
            DropSystem dropNovo = objetoFundido.GetComponent<DropSystem>();
            if (dropNovo != null) {
                dropNovo.DefinirBancadaAtual(this, this.transform);
            }
        }else {
            Destroy(sacrificio);
            ingredienteAtual = novoIngrediente;
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        if (ingredienteAtual != null && objeto == ingredienteAtual.gameObject) {
            ingredienteAtual = null;
        }
    }
}