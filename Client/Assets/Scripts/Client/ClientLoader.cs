using UnityEngine;

namespace BuildingBlocks.Client
{
    public class ClientLoader : MonoBehaviour
    {
        public string IP;
        public int Port;

        public Client Client { get; private set; }

        void Start()
        {
            if (!CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
            {
                InvokeRepeating("setFocus", 10f, 10f);
            }
            UnityEngine.Input.compass.enabled = true;
            Client = new Client(new NetworkWrapper());
            Client.ConnectToServer(QRScanner.IP ?? IP, QRScanner.Port ?? Port);
        }

        void setFocus()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
        }

        void OnGUI()
        {
            Client.OnGUI();
        }

        void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
        }

		public void OnDisconnectedFromServer(NetworkDisconnection info){
			Client.OnDisconnectedFromServer (info);
		}

		public void OnFailedToConnect(NetworkConnectionError error){
			Client.OnFailedToConnect (error);
		}

		[RPC]
        void Win(int teamId)
        {
            Client.RPC_Win(teamId);
            Network.SetSendingEnabled(1, false);
            Invoke("Restart", 5);
        }
    }
}
