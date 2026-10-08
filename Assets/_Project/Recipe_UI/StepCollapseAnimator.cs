using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StepCollapseAnimator : MonoBehaviour
{
    [SerializeField] float duration = 0.25f;

    RectTransform rect;
    LayoutElement layoutElement;
    CanvasGroup group;
    Coroutine routine;

    bool? visible;          // null = ainda não inicializado
    float progress = 1f;    // 1 = totalmente visível, 0 = recolhido
    float fullHeight;

    void Awake()
    {
        rect = (RectTransform)transform;
        if (!TryGetComponent(out layoutElement)) layoutElement = gameObject.AddComponent<LayoutElement>();
        if (!TryGetComponent(out group)) group = gameObject.AddComponent<CanvasGroup>();
        if (!TryGetComponent<RectMask2D>(out _)) gameObject.AddComponent<RectMask2D>(); // corta o conteúdo enquanto a altura encolhe
    }

    void OnEnable()
    {
        // Se o objeto foi desativado no meio da animação, reaplica o estado final
        if (visible.HasValue) Apply(visible.Value ? 1f : 0f);
    }

    public void SetVisible(bool show, bool instant = false)
    {
        if (visible == show) return;
        visible = show;

        if (routine != null) StopCoroutine(routine);

        if (instant || !isActiveAndEnabled)
        {
            Apply(show ? 1f : 0f);
            return;
        }

        routine = StartCoroutine(Animate(show ? 1f : 0f));
    }

    IEnumerator Animate(float target)
    {
        // Mede a altura natural só quando parte de um extremo (se interrompido no meio, reaproveita a medida)
        if (progress >= 1f || progress <= 0f) MeasureFullHeight();

        float speed = 1f / Mathf.Max(duration, 0.01f);
        while (!Mathf.Approximately(progress, target))
        {
            Apply(Mathf.MoveTowards(progress, target, Time.unscaledDeltaTime * speed));
            yield return null;
        }

        Apply(target);
        routine = null;
    }

    void MeasureFullHeight()
    {
        // Limpa as sobrescritas para medir a altura natural do conteúdo
        layoutElement.minHeight = -1f;
        layoutElement.preferredHeight = -1f;
        layoutElement.ignoreLayout = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        fullHeight = LayoutUtility.GetPreferredHeight(rect);
    }

    void Apply(float p)
    {
        progress = p;
        float eased = Mathf.SmoothStep(0f, 1f, p);

        group.alpha = eased;
        group.blocksRaycasts = p > 0.99f;
        group.interactable = p > 0.99f;

        if (p >= 1f)
        {
            // Totalmente visível: devolve o controle ao layout (acompanha mudanças de texto)
            layoutElement.ignoreLayout = false;
            layoutElement.minHeight = -1f;
            layoutElement.preferredHeight = -1f;
        }
        else if (p <= 0f)
        {
            // Totalmente recolhido: sai do layout (sem sobrar spacing)
            layoutElement.ignoreLayout = true;
        }
        else
        {
            float h = fullHeight * eased;
            layoutElement.ignoreLayout = false;
            layoutElement.minHeight = h;
            layoutElement.preferredHeight = h;
        }
    }
}