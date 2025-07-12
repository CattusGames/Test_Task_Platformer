using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Window;

namespace Services.Factories.UIFactory
{
    public interface IUIFactory
    {
        Task CreateUIRoot();
        Task<RectTransform> CreateWindow(WindowTypeId windowTypeId);
        void TrimGraphicRaycaster();
    }
}