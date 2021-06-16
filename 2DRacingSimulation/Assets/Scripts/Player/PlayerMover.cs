using UnityEngine;

namespace RacingSimulation.Player
{
    [System.Serializable]
    public class PlayerMover
    {
        public const float MaxStickyVelocity = 2.5f;

        public Transform Transform { get => this.transform; set => this.transform = value; }
        public Rigidbody2D Rgbd { get => this.rgb; set => this.rgb = value; }

        [SerializeField] private float speedForce = 10f;
        [SerializeField] private float torqueForce = -200f;
        [SerializeField] [Range(.5f, .9f)] private float driftFactorSticky = 0.9f;
        [SerializeField] [Range(.9f, 1f)] private float driftFactorSlipy = 1f;

        private Rigidbody2D rgb;
        private Transform transform;

        public void SetupVelocity()
        {
            this.rgb.velocity = Vector2.zero;
            this.rgb.angularVelocity = 0f;
        }

        public void Rotate(float horizontal)
        {
            float calculateTorqueForce = Mathf.Lerp(0, this.torqueForce, this.rgb.velocity.magnitude / 10);
            this.rgb.angularVelocity = horizontal * calculateTorqueForce;
        }

        public void Accelerate() => this.rgb.AddForce(this.speedForce * this.transform.up);

        public void Brake() => this.rgb.AddForce(-.5f * this.speedForce * this.transform.up);

        public void UpdateVelocity() => this.rgb.velocity = this.ForwardVelocity() + this.RightVelocity() * this.SetDriftVactor();

        private float SetDriftVactor() => this.RightVelocity().magnitude > MaxStickyVelocity ? this.driftFactorSlipy : this.driftFactorSticky;
        private Vector2 ForwardVelocity() => this.transform.up * Vector2.Dot(this.rgb.velocity, this.transform.up);
        private Vector2 RightVelocity() => this.transform.right * Vector2.Dot(this.rgb.velocity, this.transform.right);
    }
}