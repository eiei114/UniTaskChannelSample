using System.Globalization;
using System.Threading;
using _UnityReact.Scripts.General;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace _UnityReact.Scripts.Manager
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private View.View _view;
        [SerializeField] private Model.Model _model;
        [SerializeField] private float _increment = 1;
        [SerializeField] private float _decrement = -1;
        [SerializeField] private float _tentimesValue = 10f;
        [SerializeField] private float _oneTenthValue = 0.1f;

        private CancellationToken _cts;

        private void Awake()
        {
            ManagerInit();
        }

        private void Start()
        {
            //Awakeが終わるのを待つ
            this.AwakeAsync();
            _model.AwakeAsync();

            _view.ButtonChannelReader
                .Where(type => type == ButtonType.Increment)
                .ForEachAsync(_ =>
                {
                    _model.SetValue(_increment);
                }, _cts);

            _view.ButtonChannelReader
                .Where(type => type == ButtonType.Decrement)
                .ForEachAsync(_ => { _model.SetValue(_decrement); }, _cts);
            
            _view.ButtonChannelReader
                .Where(type => type == ButtonType.TenTimes)
                .ForEachAsync(_ => { _model.SetTimesValue(_tentimesValue); }, _cts);
            
            _view.ButtonChannelReader
                .Where(type => type == ButtonType.OneTenth)
                .ForEachAsync(_ => { _model.SetTimesValue(_oneTenthValue); }, _cts);

            _model.Value
                .Subscribe(value =>
                {
                    _view.CounterText = value.ToString(CultureInfo.InvariantCulture);
                    _view.CounterTextAnimation();
                }, _cts);
        }

        private void ManagerInit()
        {
            _cts = this.GetCancellationTokenOnDestroy();
            _view.ViewInit(_cts).Forget();
        }
    }
}