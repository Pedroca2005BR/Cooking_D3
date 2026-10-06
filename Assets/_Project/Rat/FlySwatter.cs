using DG.Tweening;
using Pedroca2005BR.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CameraCornerAnchor))]
public class FlySwatter : MonoBehaviour, IInteragivel
{
    // Interactable Settings are third-party sourced, don't change them unless you know what you're doing
    [Header("Interactable Settings")]
    int tipoObjeto = 1;
    [SerializeField] GameObject objetoInputPrefab; // Input key prefab
    [SerializeField] Vector2 inputOffset;   // Offset of the input key prefab
    bool podeInteragir = true;
    GameObject objetoInputInst = null;
    bool isEquipped = false;

    // Change the following values to fit the yellow box (Gizmos) to the fly swatter head
    [Header("FlySwatter Settings")]
    [SerializeField] Vector2 flySwatterHeadOffset;
    [SerializeField] Vector2 flySwatterHeadSize;
    CameraCornerAnchor cameraCornerAnchor;

    // Change the following values to adjust the smack animation
    [Header("Tween Settings")]
    [SerializeField] float duration = 1.0f;
    Tweener anim;
    IEnumerator routine;

    // The following values control special sprites for killing rats. Not implemented yet
    [Header("Special Sprite Settings")]
    [SerializeField] Sprite killingSprite;
    [SerializeField] SpriteRenderer mouseSpriteRenderer;

    

    

    private void Awake()
    {
        cameraCornerAnchor = GetComponent<CameraCornerAnchor>();
    }
    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.MOUSE_CLICK, OnClicked);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.MOUSE_CLICK, OnClicked);
        if (anim != null) transform.DOKill();
        cameraCornerAnchor.enabled = false;
    }

    #region IInteragivel Implementation

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

    #endregion

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
        Camera.main.GetComponent<Physics2DRaycaster>().enabled = true;
        isEquipped = false;
        cameraCornerAnchor.enabled = false;
    }

    void Equip()
    {
        Camera.main.GetComponent<Physics2DRaycaster>().enabled = false; // Disabling raycasting to prevent clicking on other objects while equipped
        isEquipped = true;
        cameraCornerAnchor.enabled = true;
    }


    // Function called when the mouse is clicked. If the fly swatter is equipped and the animation isn't active, it starts the swatting animation.
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

    // Coroutine that handles the swatting animation using DOTween and checks for collisions with rats. It disables the camera corner anchor during the animation and re-enables it afterward.
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
