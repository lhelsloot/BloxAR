using UnityEngine;

namespace BuildingBlocks.CubeFinger
{
    public class CubeFingerLoader : MonoBehaviour
    {
        void Awake()
        {
            gameObject.AddComponent<CubeFingerBridge>();
        }
    }
}
