using UnityEngine;

namespace RacingSimulation.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMover playerMover;

        private Rigidbody2D rgbd;

        private void Awake()
        {
            this.rgbd = this.GetComponent<Rigidbody2D>();
            this.playerMover.Rgbd = this.rgbd;
        }

        private void FixedUpdate()
        {
            this.playerMover.Transform = this.transform;
            this.playerMover.UpdateVelocity();

            this.Accelarate();
            this.Brake();
            this.playerMover.Rotate(this.playerInput.GetHorizontalAxis());
        }

        private void Accelarate()
        {
            if (this.playerInput.OnAccelerate)
            {
                this.playerMover.Accelerate();
            }
        }

        private void Brake()
        {
            if (this.playerInput.OnBrake)
            {
                this.playerMover.Brake();
            }
        }
    }
}