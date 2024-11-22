using ShadowGroveGames.SimpleWebSocketServer.Scripts;
using UnityEngine;

namespace ShadowGroveGames.SimpleWebSocketServer.Examples
{
    public class Example3_SimpleServerControl : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private GameObject _startButton;

        [SerializeField]
        private GameObject _stopButton;

        [Header("Reference")]
        [SerializeField]
        private SimpleEventWebSocketServerScript _simpleEventWebSocketServerScript;

        /// <summary>
        /// Start the server and listen for incoming connections.
        /// </summary>
        public void StartServer()
        {
            _simpleEventWebSocketServerScript.StartServer();
        }

        /// <summary>
        /// Stop the server and close all the connections.
        /// </summary>
        public void StopServer()
        {
            _simpleEventWebSocketServerScript.StopServer();
        }

        public void FixedUpdate()
        {
            bool isServerRunning = _simpleEventWebSocketServerScript.GetServer().IsRunning();

            _startButton.SetActive(!isServerRunning);
            _stopButton.SetActive(isServerRunning);
        }
    }
}
