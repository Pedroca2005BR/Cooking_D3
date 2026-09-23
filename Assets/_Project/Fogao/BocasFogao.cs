using UnityEngine;

public class BocasFogao : MonoBehaviour
{
    [SerializeField] private float tamanhoColisor = 0.8f; //tamanho do colisor(trigger)
    
    private void Awake() {
        GerarBocas();
    }

    private void GerarBocas() {
        //le o tamanho do sprite
        SpriteRenderer spriteFogao = GetComponent<SpriteRenderer>();
        float largura = spriteFogao.sprite.bounds.size.x;
        float altura = spriteFogao.sprite.bounds.size.y;

        //calcula onde é o canto superior esquerdo e o define como a posicao para a primeira boca do fogão
        float posInicialX = -largura / 4f;
        float posInicialY = altura / 4f;

        //define a distancia de cada boca
        float distX = largura / 2f;
        float distY = altura / 2f;

        //calcula o tamanho do collider
        Vector2 tamanhoCollider = new Vector2(distX, distY) * tamanhoColisor;

        for(int i = 0; i < 4; i++) {
            GameObject novaBoca = new GameObject($"bocaFogao{i}"); //instancia uma nova boca

            novaBoca.transform.SetParent(this.transform); //torna ela filha do fogao

            int linha = i/2; //conta para saber em que boca do fogao esta. Troca de linha a cada 2
            int coluna = i%2; //repete a coluna a cada 2

            //encontra a posicao no fogao. Usa a posicao relativa ao proprio fogao e n do mundo
            float posX = posInicialX + (coluna * distX);
            float posY = posInicialY - (linha * distY);
            novaBoca.transform.localPosition = new Vector3(posX, posY, 0);

            //adiciona o collider e configura como trigger 
            BoxCollider2D collider = novaBoca.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = tamanhoCollider;

            novaBoca.AddComponent<BocaReceber>();
        }
    }
}
