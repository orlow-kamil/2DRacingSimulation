using System.Collections;
using UnityEngine;

namespace RacingSimulation.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        public bool InvicibleMode { get => this.invicibleMode; set => this.invicibleMode = value; }
        public int CurrentCheckPointIndex { get => this.currentCheckPointIndex; set => this.currentCheckPointIndex = value; }
        public Rigidbody2D Rgbd => this.rgbd;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMover playerMover;

        [Header("InvicibleMode")]
        [SerializeField] [Range(0.5f, 5f)] private float invicibleTime = 1.5f;
        [SerializeField] private bool invicibleMode = true;


        private int currentCheckPointIndex;
        private Rigidbody2D rgbd;

        public void RestartPlayer()
        {
            this.playerMover.SetupVelocity();
            this.StartCoroutine(this.TurnInvicibleMode());
        }

        private void Awake()
        {
            this.rgbd = this.GetComponent<Rigidbody2D>();
            this.playerMover.Rgbd = this.rgbd;
            this.currentCheckPointIndex = 0;
        }

        private void Start() => this.RestartPlayer();

        private void FixedUpdate()
        {
            this.playerMover.Transform = this.transform;
            this.playerMover.UpdateVelocity();

            this.Accelarate();
            this.Brake();
            this.playerMover.Rotate(this.playerInput.GetHorizontalAxis());
        }

        private IEnumerator TurnInvicibleMode()
        {
            this.invicibleMode = true;
            yield return new WaitForSeconds(this.invicibleTime);
            this.invicibleMode = false;
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