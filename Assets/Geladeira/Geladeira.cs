using UnityEngine;

public class Geladeira : MonoBehaviour
{
    [SerializeField] private GameObject[] compartimentosObrigatorios;
    [SerializeField] private GameObject[] compartimentosPossiveis;
    [SerializeField] private Transform[] prateleiras;

    void Start()
    {
        EncherGeladeira();
    }

    private void EncherGeladeira()
    {
        foreach (Transform prateleira in prateleiras)
        {
            GameObject compartimento;

            if (compartimentosObrigatorios.Length > 0)
            {
                compartimento = compartimentosObrigatorios[
                    Random.Range(0, compartimentosObrigatorios.Length)
                ];
            }
            else
            {
                compartimento = compartimentosPossiveis[
                    Random.Range(0, compartimentosPossiveis.Length)
                ];
            }

            Instantiate(
                compartimento,
                prateleira.position,
                Quaternion.identity,
                prateleira
            );
        }
    }
}