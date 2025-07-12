using System;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Window;

namespace StaticData
{
    [Serializable]
    public class WindowConfig
    {
        public WindowTypeId WindowTypeId;
        public AssetReferenceGameObject AssetReference;
    }
}