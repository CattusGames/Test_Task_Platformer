using UnityEngine;
using UnityEngine.UI;

namespace Window
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _filler;
        public float Value { get; private set; }
        
        public void DrawBar(float value)
        {
            Value = Mathf.Clamp01(value);
            _filler.fillAmount = Value;
        }
    }
}