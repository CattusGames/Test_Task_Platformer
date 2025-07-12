using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Factories.UIFactory;
using UnityEngine;
using Window;
using Zenject;

namespace Services.Window
{
    public class WindowService : IWindowService
    {
        public Dictionary<WindowTypeId, RectTransform> Windows { get; private set; } = new();

        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async Task<RectTransform> OpenWindow(WindowTypeId windowTypeId)
        {
            RectTransform window = await GetOrCreateWindow(windowTypeId);
            window.gameObject.SetActive(true);
            return window;
        }

        public async Task<T> ShowPopup<T>(WindowTypeId windowTypeId) where T : MonoBehaviour
        {
            RectTransform window = await GetOrCreateWindow(windowTypeId);

            if (window.TryGetComponent(out T popup))
            {
                popup.gameObject.SetActive(true);
                return popup;
            }

            Debug.LogWarning($"Popup of type {typeof(T).Name} not found on window {windowTypeId}");
            return null;
        }

        public async Task CloseWindow(WindowTypeId windowTypeId)
        {
            if (Windows.TryGetValue(windowTypeId, out var window))
            {
                window.gameObject.SetActive(false);
                await Task.Yield();
            }
        }

        private async Task<RectTransform> GetOrCreateWindow(WindowTypeId windowTypeId)
        {
            if (Windows.TryGetValue(windowTypeId, out var cachedWindow))
            {
                if (cachedWindow == null || cachedWindow.gameObject == null)
                {
                    Windows.Remove(windowTypeId);
                }
                else
                {
                    return cachedWindow;
                }
            }

            RectTransform window = await _uiFactory.CreateWindow(windowTypeId);
            Windows[windowTypeId] = window;
            return window;
        }

        public void CleanUp()
        {
            foreach (var pair in Windows)
            {
                if (pair.Value != null)
                    pair.Value.gameObject.SetActive(false);
            }
        }
    }
}