using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour {

    [Header("Configuracao das bancadas")]
    //vetor de cameras das bancadas
    public CinemachineCamera[] cameras;
    //distancia de cada uma das bancadas e posicao inicial da primeira
    public float distBancadas = 24f;
    public float posicaoInicial = 0f;
    //camera atual
    private int camAtual = 0;

    private void Awake() {
        for(int i = 0; i < cameras.Length; i++) {
            //Calcula a posicao em que cada camera deve estar no eixo X
            Vector3 posicaoCamera = cameras[i].transform.position;
            posicaoCamera.x = posicaoInicial + (i * distBancadas);
            cameras[i].transform.position = posicaoCamera;

            //garante que a cena comece da maneira correta(so a inicial ligada)
            cameras[i].gameObject.SetActive(i == camAtual);
        }

        //evitar que tenha uma trasicao de camera no inicio, mostrando o void
        if (Camera.main != null && cameras.Length > 0)
            Camera.main.transform.position = cameras[0].transform.position;
    }

    public void OnNext(InputAction.CallbackContext context) {
        if(context.performed && camAtual < cameras.Length - 1) {
            int camAnterior = camAtual;
            camAtual++;
            AtualizarCamera(camAnterior, camAtual);
        }
    }

    public void OnPrevious(InputAction.CallbackContext context) {
        if(context.performed && camAtual > 0) {
            int camAnterior = camAtual;
            camAtual--;
            AtualizarCamera(camAnterior, camAtual);
        }
    }

    //funcao para atualizar a camera que o jogador esta vendo
    private void AtualizarCamera(int antes, int atual) {
        cameras[antes].gameObject.SetActive(false);
        cameras[atual].gameObject.SetActive(true);
    }
}