using ShadowGroveGames.SimpleWebSocketServer.Scripts.WebSocketServer;
using ShadowGroveGames.SimpleWebSocketServer.Scripts.Endpoint;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Examples
{
    public class Example2_JsonObjectEndpoint : WebSocketEndpointBehaviour
    {
        private const string CLIENT_ASSET_GUID = "56f2914af0851ad4ab741cbcd235daa4";

        [Header("UI")]
        [SerializeField]
        private Transform _innerBox;

        [SerializeField]
        private GameObject _clientButton;

        [Header("Prefab")]
        [SerializeField]
        private Example2_Counter _counter;

        private Dictionary<string, Example2_Counter> _clientStore = new Dictionary<string, Example2_Counter>();

        public override void OnServerReady()
        {
            // The "Open Client" function only works in the editor!
            // But you can copy the file Client.html to your desktop and open it there with a double click.
            if (Application.isEditor)
                _clientButton.gameObject.SetActive(true);
        }

        public override void OnServerShutdown()
        {
            if (_innerBox == null || _innerBox.transform == null)
                return;

            // Remove all counter objects on server shutdown
            foreach (Transform counter in _innerBox.transform)
            {
                if (counter?.gameObject == null)
                    continue;

                DestroyImmediate(counter.gameObject);
            }
        }

        public override void OnConnect(WebSocketEventArg argument)
        {
            _clientButton.gameObject.SetActive(false);

            // This example script can only display 7 counter
            if (_clientStore.Count >= 7)
                return;

            // Instantiate from prefab a new Counter
            var counter = Instantiate(_counter, _innerBox);
            counter.Setup(Server, argument.clientId, 0);

            // Store client to have a referance between client and Counter class
            _clientStore.Add(argument.clientId, counter);

            // Reset the Counter on client side and send client id
            Server.SendJsonMessage(
                new Example2_ServerStruct() { Id = argument.clientId.Substring(0, 7), Counter = 0 },
                argument.clientId
            );
        }

        public override void OnDisconnect(WebSocketEventArg argument)
        {
            var counterClient = GetCounterClient(argument);
            if (counterClient == null)
                return;

            // Destory counter object on disconnect and remove from store
            Destroy(counterClient.gameObject);
            _clientStore.Remove(argument.clientId);
        }

        public override void OnTextData(string text, WebSocketEventArg argument)
        {
            var counterClient = GetCounterClient(argument);
            if (counterClient == null)
                return;

            // Deserialize the data from client
            var clientData = JsonUtility.FromJson<Example2_ClientStruct>(text);

            // Call the counter class with the data
            counterClient.ChangeCouter(clientData.CountUp);
        }

        private Example2_Counter GetCounterClient(WebSocketEventArg argument)
        {
            // Check if the client id is in the client store
            if (!_clientStore.ContainsKey(argument.clientId))
                return null;

            return _clientStore[argument.clientId];
        }

        public void OpenClient()
        {
#if UNITY_EDITOR
            string relativePath = AssetDatabase.GUIDToAssetPath(CLIENT_ASSET_GUID);
            if (string.IsNullOrEmpty(relativePath))
                return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            if (!File.Exists(filePath))
                return;

            Application.OpenURL(filePath);
#else
            Debug.LogWarning("This function only works in the editor!");
#endif
        }
    }
}
