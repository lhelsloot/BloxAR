using UnityEngine;

namespace BuildingBlocks.Input
{
    public class Accelerometer : MonoBehaviour
    {
        public bool IsUpright
        {
            get
            {
                return Mathf.Abs(1 + UnityEngine.Input.acceleration.y) <= 0.2;
            }
        }
    }
}
