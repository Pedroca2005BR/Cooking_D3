using DG.Tweening;
using Pedroca2005BR.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CameraCornerAnchor))]
public class FlySwatter : MonoBehaviour, IInteragivel
{
    [Header("Interactable Settings")]
    [SerializeField] int tipoObjeto = 1;
    [SerializeField] GameObject objetoInputPrefab;
    [SerializeField] Vector2 inputOffset;
    bool podeInteragir = true;
    GameObject objetoInputInst = null;
    bool isEquipped = false;

    [Header("FlySwatter Settings")]
    [SerializeField] Vector2 flySwatterHeadOffset;
    [SerializeField] Vector2 flySwatterHeadSize;
    [SerializeField] Sprite killingSprite;
    CameraCornerAnchor cameraCornerAnchor;
    [SerializeField] SpriteRenderer mouseSpriteRenderer;

    [Header("Tween Settings")]
    [SerializeField] float duration = 1.0f;
    Tweener anim;
    IEnumerator routine;

    

    private void Awake()
    {
        cameraCornerAnchor = GetComponent<CameraCornerAnchor>();
    }

    public bool PodeInteragir()
    {
        return podeInteragir;
    }

    public void Interagir()
    {
        if (!podeInteragir)
            return;

        TryEquip();
    }

    private void TryEquip()
    {
        if (isEquipped)
        {
            Unequip();
        }
        else
        {
            Equip();
        }
    }

    void Unequip()
    {
        //podeInteragir = true;
        Camera.main.GetComponent<Physics2DRaycaster>().enabled = true;
        isEquipped = false;
        cameraCornerAnchor.enabled = false;
    }

    void Equip()
    {
        //podeInteragir = false;
        Camera.main.GetComponent<Physics2DRaycaster>().enabled = false;
        isEquipped = true;
        cameraCornerAnchor.enabled = true;
    }

    public void MostraInput()
    {
        if (podeInteragir && objetoInputInst == null)
        {
            objetoInputInst = Instantiate(objetoInputPrefab, transform.position + (Vector3)inputOffset, Quaternion.identity);
        }
    }

    public void EscondeInput()
    {
        if (objetoInputInst != null)
        {
            Destroy(objetoInputInst);
            objetoInputInst = null;
        }
    }

    public int GetTipo()
    {
        return tipoObjeto;
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.MOUSE_CLICK, OnClicked);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.MOUSE_CLICK, OnClicked);
        if (anim != null)   transform.DOKill();
    }

    private void OnClicked(object obj)
    {
        // If theres a click action and the tween isnt active, activate it
        if (isEquipped && (anim == null || !anim.active))
        {
            Transform ts = obj as Transform;

            if (ts == null)
            {
                Debug.LogError($"Wrong value passed in event {EventConstantNames.MOUSE_CLICK}");
                return;
            }

            routine = Swatting(ts);
            StartCoroutine(routine);
        }
    }

    IEnumerator Swatting(Transform ts)
    {
        cameraCornerAnchor.enabled = false;
        Vector3 original = transform.position;
        Vector3 endvalue = new Vector3(ts.position.x - flySwatterHeadOffset.x, ts.position.y - flySwatterHeadOffset.y, 0);
        anim = transform.DOMove(endvalue, duration).SetEase(Ease.InExpo).SetLoops(2, LoopType.Yoyo).OnStepComplete( () =>
        {
            var colls = Physics2D.OverlapBoxAll(endvalue + (Vector3)flySwatterHeadOffset, flySwatterHeadSize, 0);
            foreach(var coll in colls)
            {
                if (coll.TryGetComponent<RatBehaviour>(out var rat))
                {
                    rat.Die();
                }
            }
        } );

        yield return anim.WaitForCompletion();
        cameraCornerAnchor.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)flySwatterHeadOffset, flySwatterHeadSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)inputOffset, Vector3.one);
    }
}
