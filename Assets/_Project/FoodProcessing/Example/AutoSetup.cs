using UnityEngine;

namespace Process.Example
{
    public class AutoSetup : MonoBehaviour
    {
        [SerializeField] BaseIngredientData _data;
        IngredientComponent _component;

        void Start ()
        {
            _component = GetComponent<IngredientComponent>();
            _component.Setup(_data);
        }
    }
}
