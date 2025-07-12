using Services.Factories.GameFactory;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(0f, 1f, -10f);
        [SerializeField] private float _smoothSpeed = 5f;

        private IGameFactory _gameFactory;
        private Transform _playerTransform;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
            if (_gameFactory.Player != null)
            {
                _playerTransform = _gameFactory.Player.transform;
            }
        }

        private void LateUpdate()
        {
            if (_playerTransform == null) return;

            Vector3 desiredPosition = _playerTransform.position + _offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}