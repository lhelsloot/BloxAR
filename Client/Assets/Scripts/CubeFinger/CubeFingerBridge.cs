using UnityEngine;
using BuildingBlocks.Player;
using BuildingBlocks.Team;

namespace BuildingBlocks.CubeFinger
{
    public class CubeFingerBridge : MonoBehaviour, ITrackerEventHandler
    {
        public BaseCubeFinger Finger;
        private bool showFinger;
        private string target;

        void Awake()
        {
            QCARBehaviour qcarBehaviour = FindObjectOfType<QCARBehaviour>();
            if (qcarBehaviour)
            {
                qcarBehaviour.UnregisterTrackerEventHandler(this);
                qcarBehaviour.RegisterTrackerEventHandler(this);
            }
        }

        void OnDisconnectedFromServer()
        {
            QCARBehaviour qcarBehaviour = FindObjectOfType<QCARBehaviour>();
            if (qcarBehaviour)
            {
                qcarBehaviour.UnregisterTrackerEventHandler(this);
            }
        }

        void OnNetworkInstantiate(NetworkMessageInfo info)
        {
            Finger = new CubeFinger(new GameObjectWrapper(gameObject));
        }

        void OnPlayerConnected(NetworkPlayer networkPlayer)
        {
            Finger.OnPlayerConnected(new NetworkPlayerWrapper(networkPlayer));
        }

        public void OnInitialized()
        {

        }

        public void OnTrackablesUpdated()
        {
            if (showFinger)
            {
                Finger.Update();
            }
        }

        [RPC]
        void SetPersonalFinger()
        {
            if (showFinger)
            {
                Finger.RPC_SetPersonalFinger();
            }
        }

        [RPC]
        void SetFingerParent(string parent)
        {
            this.target = parent;
            showFinger = Player.Player.LocalPlayer != null && Player.Player.LocalPlayer.Team != null && target == Player.Player.LocalPlayer.Team.Target;
            if (showFinger)
            {
                Finger.RPC_SetFingerParent(parent);
            }
        }

        [RPC]
        void SetFingerMode(int mode)
        {
            showFinger = Player.Player.LocalPlayer != null && Player.Player.LocalPlayer.Team != null && target == Player.Player.LocalPlayer.Team.Target;
            if (showFinger)
            {
                Finger.RPC_SetFingerMode(mode);
            }
        }

        [RPC]
        void ShowFinger(int show)
        {
            if (showFinger)
            {
                CubeFingerRenderer renderer = Finger.Renderer as CubeFingerRenderer;
                renderer.RPC_ShowFinger(show);
            }
        }

        [RPC]
        void ColorFinger(Vector3 color)
        {
            if (showFinger)
            {
                CubeFingerRenderer renderer = Finger.Renderer as CubeFingerRenderer;
                renderer.RPC_ColorFinger(color);
            }
        }

        [RPC]
        void MoveFinger(NetworkViewID viewId, Vector3 displacement)
        {
            if (showFinger)
            {
                CubeFingerRenderer renderer = Finger.Renderer as CubeFingerRenderer;
                renderer.RPC_MoveFinger(viewId, displacement);
            }
        }
    }
}
