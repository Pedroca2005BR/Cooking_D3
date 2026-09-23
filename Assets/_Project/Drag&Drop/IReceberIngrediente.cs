using UnityEngine;

public interface IReceberIngrediente
{
    //funcao para testar que tipo de objeto o recebedor, recebe
    bool AceitaIngrediente(GameObject objeto);

    //metodo para o receber o objeto
    void ReceberIngredienteSolto(GameObject objeto);

    //metodo para remover o objeto
    void RemoverIngrediente(GameObject objeto);
}