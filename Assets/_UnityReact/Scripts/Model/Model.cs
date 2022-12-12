using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _UnityReact.Scripts.Model
{
    public class Model : MonoBehaviour
    {
        private readonly AsyncReactiveProperty<float> _value = new(0);

        public IReadOnlyAsyncReactiveProperty<float> Value => _value;

        public void SetValue(float value)
        {
            _value.Value += value;
        }

        public void SetTimesValue(float value)
        {
            _value.Value *= value;
        }
    }
}