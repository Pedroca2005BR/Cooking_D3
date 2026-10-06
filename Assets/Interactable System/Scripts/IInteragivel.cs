using UnityEngine;

public interface IInteragivel
{
    Transform transform { get; }    // Transform do objeto interagivel

    void Interagir();       // Função que ativa interação

    bool PodeInteragir();   // Função que retorna se objeto pode ser interagido

    void MostraInput();     // Gera objeto que mostra input

    void EscondeInput();    // Remove objeto que mostra input

    int GetTipo();          // Retorna o tipo de interação (simples, equipamento etc) Obs:. Tipo do objeto não deve ser menor que 0
}
