using System;
using System.Collections;
using UnityEngine;

namespace RacingSimulation.Manager
{
    [Serializable]
    public class Timer
    {
        public const float PenaltyTime = 1f;

        public event EventHandler<EventArgs> OnCurrentTimeUpdate;
        public event EventHandler<EventArgs> OnBestTimeTextUpdate;

        public float CurrentTime { get => this.currentTime; set => this.currentTime = value; }
        public float BestTime { get => this.bestTime; set => this.bestTime = value; }

        private float currentTime;
        private float bestTime = Mathf.Infinity;

        public void UpdateBestTime()
        {
            if (this.currentTime < this.bestTime)
            {
                this.bestTime = this.currentTime;
                this.OnBestTimeTextUpdate?.Invoke(this, EventArgs.Empty);
            }
        }
        public void AddPenaltyTime() => this.currentTime += PenaltyTime;

        public IEnumerator LoopTimer()
        {
            SceneManager.Instance.EndLoop = false;
            this.currentTime = 0f;
            while (!SceneManager.Instance.EndLoop)
            {
                yield return this.OneTik();
                this.OnCurrentTimeUpdate?.Invoke(this, EventArgs.Empty);
            }
            ++SceneManager.Instance.LoopCounter;
        }

        private IEnumerator OneTik()
        {
            yield return new WaitForEndOfFrame();
            this.currentTime += Time.deltaTime;
        }
    }
}