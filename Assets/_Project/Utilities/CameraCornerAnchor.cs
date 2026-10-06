using UnityEngine;

[ExecuteAlways]
public class CameraCornerAnchor : MonoBehaviour
{
    public enum Corner { BottomLeft, BottomRight, TopLeft, TopRight }
    public enum OffsetMode { Viewport, WorldUnits }

    [Header("Referências")]
    [SerializeField] private Camera targetCamera;

    [Header("Ancoragem")]
    [SerializeField] private Corner corner = Corner.BottomLeft;
    [SerializeField] private OffsetMode offsetMode = OffsetMode.Viewport;

    [Tooltip("Viewport: 0 a 1 = percentual da tela (1 em X vai de um lado ao outro).\n" +
             "WorldUnits: unidades do mundo a partir do canto.")]
    [SerializeField] private Vector2 offset = Vector2.zero;

    private void Reset()
    {
        targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        if (targetCamera == null) return;

        // Posição do canto em coordenadas de viewport (0 a 1)
        Vector2 anchor = corner switch
        {
            Corner.BottomLeft => new Vector2(0f, 0f),
            Corner.BottomRight => new Vector2(1f, 0f),
            Corner.TopLeft => new Vector2(0f, 1f),
            _ => new Vector2(1f, 1f),
        };

        // Mantém a profundidade atual do objeto em relação à câmera
        float depth = transform.position.z - targetCamera.transform.position.z;
        if (!targetCamera.orthographic)
            depth = Mathf.Abs(depth);

        Vector3 worldPos;

        if (offsetMode == OffsetMode.Viewport)
        {
            Vector2 vp = anchor + offset;
            worldPos = targetCamera.ViewportToWorldPoint(new Vector3(vp.x, vp.y, depth));
        }
        else
        {
            Vector3 cornerWorld = targetCamera.ViewportToWorldPoint(new Vector3(anchor.x, anchor.y, depth));
            worldPos = cornerWorld
                     + targetCamera.transform.right * offset.x
                     + targetCamera.transform.up * offset.y;
        }

        worldPos.z = transform.position.z;
        transform.position = worldPos;
    }
}
