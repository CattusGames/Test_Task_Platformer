using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Services.Factories
{
    public abstract class Factory
    {
        protected IInstantiator _instantiator;

        protected Factory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        protected GameObject Instantiate(string uiRootPath)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate<T>(T prefab) where T: Object
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(string uiRootPath, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath, parent);
            return gameObject;
        }

        protected GameObject Instantiate<T>(T prefab, Transform parent) where T: Object => 
            _instantiator.InstantiatePrefab(prefab, parent);

        protected GameObject Instantiate(string uiRootPath, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath, position, rotation, parent);
            return parent == null ? MoveToCurrentScene(gameObject) : gameObject;
        }

        protected GameObject Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T: Object
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab, position, rotation, parent);
            return parent == null ? MoveToCurrentScene(gameObject) : gameObject;
        }
        
        private GameObject MoveToCurrentScene(GameObject gameObject)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            return gameObject;
        }
    }
}