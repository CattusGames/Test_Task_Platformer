using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Window;

namespace Services.Window
{
    public interface IWindowService
    {
        Task<RectTransform> OpenWindow(WindowTypeId windowTypeId);
        Task<T> ShowPopup<T>(WindowTypeId windowTypeId) where T : MonoBehaviour;
        Task CloseWindow(WindowTypeId windowTypeId);
        Dictionary<WindowTypeId, RectTransform> Windows { get; }
        void CleanUp();
    }
}