using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBancadaController : MonoBehaviour {

    [Header("Configuracao do movimento")]
    [SerializeField] private float velocidade = 18f;

    private bool indoDireita = false;
    private bool indoEsquerda = false;

    private void Update() {
        float direcao = 0f;
        if(indoDireita) direcao += 1f;
        if(indoEsquerda) direcao -= 1f;

        //move apenas este objeto alvo; o Cinemachine segue ele com suavizacao
        if(direcao != 0f) {
            transform.position += new Vector3(direcao * velocidade * Time.deltaTime, 0f, 0f);
        }
    }

    public void OnNext(InputAction.CallbackContext context) {
        indoDireita = context.ReadValueAsButton();
    }

    public void OnPrevious(InputAction.CallbackContext context) {
        indoEsquerda = context.ReadValueAsButton();
    }

    //zera o movimento quando sair da bancada
    public void PararInput() {
        indoDireita = false;
        indoEsquerda = false;
    }
}