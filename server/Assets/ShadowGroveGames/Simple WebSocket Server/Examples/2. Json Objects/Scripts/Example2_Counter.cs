using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.SimpleWebSocketServer.Examples
{
    public class Example2_Counter : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private Text _id;

        [SerializeField]
        private Text _counter;

        [SerializeField]
        private Button _buttonPlus;

        [SerializeField]
        private Button _buttonMinus;

        private BaseWebsocketServer _server;
        private string _clientId;
        private int _counterValue;

        void Awake()
        {
            _buttonPlus.onClick.AddListener(OnClickPlus);
            _buttonMinus.onClick.AddListener(OnClickMinus);
        }

        public void Setup(BaseWebsocketServer server, string clientId, int counterValue)
        {
            _server = server;
            _clientId = clientId;
            _counterValue = counterValue;
        }

        void FixedUpdate()
        {
            if (!string.IsNullOrEmpty(_clientId))
                _id.text = _clientId.Substring(0, 7);

            if (_counter)
                _counter.text = _counterValue.ToString();
        }

        public void ChangeCouter(bool countUp)
        {
            if (countUp)
            {
                OnClickPlus();
                return;
            }

            OnClickMinus();
        }

        public int GetCouter()
        {
            return _counterValue;
        }

        private void OnClickPlus()
        {
            _counterValue++;
            SendToClient();
        }

        private void OnClickMinus()
        {
            _counterValue--;
            SendToClient();
        }

        private void SendToClient()
        {
            _server.SendJsonMessage(
                new Example2_ServerStruct() { Id = _clientId.Substring(0, 7), Counter = _counterValue },
                _clientId
            );
        }
    }
}
