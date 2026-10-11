using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class CutsceneScript : MonoBehaviour
{
    [Header("Componentes")]
    public Image telaDeFundo;
    public GameObject botaoInvisivel;
    [Header("Artes da História")]
    public Sprite[] paineisHistoria;
    private int indiceAtual = 0;

    void Start()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botaoInvisivel);
        }

        StartCoroutine(FocarBotaoAtrasado());
    }

    public void ProximoPainel()
    {
        indiceAtual++;

        if (indiceAtual < paineisHistoria.Length)
        {
            telaDeFundo.sprite = paineisHistoria[indiceAtual];
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private IEnumerator FocarBotaoAtrasado()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(botaoInvisivel);
    }
}