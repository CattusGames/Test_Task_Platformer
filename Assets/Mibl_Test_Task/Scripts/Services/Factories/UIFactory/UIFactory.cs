using System.Collections.Generic;
using System.Threading.Tasks;
using Services.AssetProvider;
using Services.StaticData;
using StaticData;
using UnityEngine;
using UnityEngine.UI;
using Window;
using Zenject;

namespace Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UIRootAddress = "UIRoot";

        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        private GameObject _uiRootPrefab;
        private Transform _uiRoot;

        public Dictionary<WindowTypeId, RectTransform> Windows { get; } = new();

        public UIFactory(IInstantiator instantiator, IStaticDataService staticDataService, IAssetProvider assetProvider) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
            _assetProvider = assetProvider;
        }

        public async Task CreateUIRoot()
        {
            _uiRootPrefab = await _assetProvider.LoadPersistence<GameObject>(UIRootAddress);
            _uiRoot = Instantiate(_uiRootPrefab).transform;

            MakeClickable();
        }

        public async Task<RectTransform> CreateWindow(WindowTypeId windowTypeId)
        {
            if (Windows.TryGetValue(windowTypeId, out var existingWindow) && existingWindow != null)
                return existingWindow;

            WindowConfig config = _staticData.ForWindow(windowTypeId);
            GameObject prefab = await _assetProvider.Load<GameObject>(config.AssetReference);
            if (prefab == null)
                Debug.LogError("Window not found");
            GameObject window = Instantiate(prefab, _uiRoot);

            var rectTransform = window.GetComponent<RectTransform>();
            Windows[windowTypeId] = rectTransform;
            return rectTransform;
        }

        public async Task<TWindow> ShowPopupWindow<TWindow>(WindowTypeId windowTypeId) where TWindow : MonoBehaviour
        {
            RectTransform windowTransform = await CreateWindow(windowTypeId);
            TWindow windowComponent = windowTransform.GetComponent<TWindow>();

            if (windowComponent != null)
                windowComponent.gameObject.SetActive(true);

            return windowComponent;
        }

        public void HideAllWindows()
        {
            foreach (var kvp in Windows)
            {
                if (kvp.Value != null)
                    kvp.Value.gameObject.SetActive(false);
            }
        }

        private void MakeClickable()
        {
            if (_uiRoot != null && !_uiRoot.GetComponent<GraphicRaycaster>())
                _uiRoot.gameObject.AddComponent<GraphicRaycaster>();
        }

        public void TrimGraphicRaycaster()
        {
            if (_uiRoot.childCount == 0 && _uiRoot.TryGetComponent(out GraphicRaycaster graphicRaycaster))
                Object.Destroy(graphicRaycaster);
        }
    }
}
