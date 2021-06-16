using UnityEngine;

namespace RacingSimulation.Player
{
    [System.Serializable]
    public class PlayerInput
    {
        public bool OnAccelerate => Input.GetKey(this.accelerateKeyCode);
        public bool OnBrake => Input.GetKey(this.brakeKeyCode);
        
        [SerializeField] private KeyCode accelerateKeyCode = KeyCode.LeftShift;
        [SerializeField] private KeyCode brakeKeyCode = KeyCode.LeftControl;

        public float GetHorizontalAxis() => Input.GetAxis("Horizontal");
    }
}