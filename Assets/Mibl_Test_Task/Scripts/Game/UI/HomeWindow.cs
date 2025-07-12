using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HomeWindow: MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Text _text;

        public void Initialize(int level)
        {
            _text.text = "Level " + (level+1);
        }

        public async Task WaitForStartButtonClickAsync()
        {
            if (_startButton == null)
            {
                return;
            }

            
            var taskCompletionSource = new TaskCompletionSource<bool>();
            void OnClick()
            {
                _startButton.onClick.RemoveListener(OnClick);
                taskCompletionSource.SetResult(true);
            }

            _startButton.onClick.AddListener(OnClick);
            await taskCompletionSource.Task;
        }
    }
}