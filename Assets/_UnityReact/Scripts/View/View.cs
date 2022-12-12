using System;
using System.Threading;
using _UnityReact.Scripts.General;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _UnityReact.Scripts.View
{
    public class View : MonoBehaviour
    {
        [SerializeField] private Button _incrementButton;
        [SerializeField] private Button _decrementButton;
        [SerializeField] private Button _tenTimesButton;
        [SerializeField] private Button _oneTenthButton;
        [SerializeField] private TextMeshProUGUI _counterText;

        private Channel<ButtonType> _buttonChannel;
        private IConnectableUniTaskAsyncEnumerable<ButtonType> _publish;
        private IDisposable _connection;
        public IUniTaskAsyncEnumerable<ButtonType> ButtonChannelReader => _publish;

        public async UniTask ViewInit(CancellationToken cts)
        {
            //単一消費者のため、Publishを使う
            _buttonChannel = Channel.CreateSingleConsumerUnbounded<ButtonType>();
            _publish = _buttonChannel.Reader.ReadAllAsync().Publish();
            _connection = _publish.Connect();

            _incrementButton.OnClickAsAsyncEnumerable()
                .Subscribe(_ =>
                {
                    _incrementButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);

                    _buttonChannel.Writer.TryWrite(ButtonType.Increment);
                })
                .AddTo(cts);
            
            _decrementButton.OnClickAsAsyncEnumerable()
                .Subscribe(_ =>
                {
                    _decrementButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
                    
                    _buttonChannel.Writer.TryWrite(ButtonType.Decrement);
                })
                .AddTo(cts);
            
            _tenTimesButton.OnClickAsAsyncEnumerable()
                .Subscribe(_ =>
                {
                    _tenTimesButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
                    
                    _buttonChannel.Writer.TryWrite(ButtonType.TenTimes);
                })
                .AddTo(cts);
            
            _oneTenthButton.OnClickAsAsyncEnumerable()
                .Subscribe(_ =>
                {
                    _oneTenthButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
                    
                    _buttonChannel.Writer.TryWrite(ButtonType.OneTenth);
                })
                .AddTo(cts);
        }
        
        public string CounterText
        {
            set => _counterText.text = value;
        }
        
        public void CounterTextAnimation()
        {
            _counterText.transform.DOPunchScale(Vector3.one * 0.5f, 0.1f);
        }
        
        private void OnDestroy()
        {
            _connection.Dispose();
            _buttonChannel.Writer.TryComplete();
        }
    }
}