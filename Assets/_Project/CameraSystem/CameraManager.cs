using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour {

    [Header("Cameras das telas")]
    [SerializeField] private CinemachineCamera cameraBancada;
    [SerializeField] private CinemachineCamera cameraHub;
    [SerializeField] private CinemachineCamera cameraGeladeira;

    [Header("Referencia da bancada")]
    //referencia para travar o movimento quando sair da bancada
    [SerializeField] private CameraBancadaController controleBancada;

    private CinemachineCamera cameraAtual;

    private void Awake() {
        //garante que a cena comece da maneira correta(so a da bancada ligada)
        ConfigurarCameraInicial(cameraBancada);

        //evitar que tenha uma transicao de camera no inicio, mostrando o void
        if (Camera.main != null && cameraBancada != null)
            Camera.main.transform.position = cameraBancada.transform.position;
    }

    public void IrParaHub() {
        AtualizarCamera(cameraHub, false);
    }

    public void IrParaBancada() {
        AtualizarCamera(cameraBancada, true);
    }

    public void IrParaGeladeira() {
        AtualizarCamera(cameraGeladeira, false);
    }

    //funcao para atualizar a camera que o jogador esta vendo
    private void AtualizarCamera(CinemachineCamera novaCamera, bool naBancada) {
        if(novaCamera == null || novaCamera == cameraAtual) return;

        if(cameraAtual != null)
            cameraAtual.gameObject.SetActive(false);

        novaCamera.gameObject.SetActive(true);
        cameraAtual = novaCamera;

        //ativa ou desativa o movimento da bancada dependendo da tela
        if(controleBancada != null) {
            controleBancada.enabled = naBancada;
            if(!naBancada) controleBancada.PararInput();
        }
    }

    private void ConfigurarCameraInicial(CinemachineCamera inicial) {
        if(cameraBancada != null) cameraBancada.gameObject.SetActive(inicial == cameraBancada);
        if(cameraHub != null) cameraHub.gameObject.SetActive(inicial == cameraHub);
        if(cameraGeladeira != null) cameraGeladeira.gameObject.SetActive(inicial == cameraGeladeira);

        cameraAtual = inicial;

        if(controleBancada != null)
            controleBancada.enabled = (inicial == cameraBancada);
    }
}