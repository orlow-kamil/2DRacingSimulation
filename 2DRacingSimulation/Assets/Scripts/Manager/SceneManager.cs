using UnityEngine;
using TMPro;
using System;
using System.Linq;
using RacingSimulation.Road;
using System.Collections.Generic;

namespace RacingSimulation.Manager
{
    public class SceneManager : MonoBehaviour
    {
        public event EventHandler<EventArgs> OnLoopCounterUpdate;

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SceneManager>();
                    if (instance == null)
                    {
                        instance = new GameObject().AddComponent<SceneManager>();
                    }
                }
                return instance;
            }
        }

        public List<ICheckPoint> CheckPoints { get => this.checkPoints; set => this.checkPoints = value; }
        public bool EndLoop { get => this.endLoop; set => this.endLoop = value; }
        public int LoopCounter 
        { 
            get => this.loopCounter;
            set
            {
                if (!this.endLoop) return;
                this.loopCounter = value;
                this.OnLoopCounterUpdate?.Invoke(this, EventArgs.Empty);
            }
        }

        [SerializeField] private Transform startTransform;
        [SerializeField] private GameObject playerPrefab;

        [SerializeField] private TextMeshProUGUI currentTimeText;
        [SerializeField] private TextMeshProUGUI bestTimeText;
        [SerializeField] private TextMeshProUGUI loopCounterText;
        [SerializeField] private Timer timer;

        private static SceneManager instance;
        
        private List<ICheckPoint> checkPoints = new List<ICheckPoint>();
        private bool endLoop = false;
        private int loopCounter = 1;
        
        public bool IfVisitedAllCheckPoints() => !this.checkPoints.Any(x => !x.Visited);

        public void ResetCheckPoints()
        {
            foreach (var checkPoint in this.checkPoints)
            {
                checkPoint.Visited = false;
            }
        }

        public void StartTimerLoop() => this.StartCoroutine(this.timer.LoopTimer());
        public void EndTimerLoop() => this.StopCoroutine(this.timer.LoopTimer());
        public void UpdateBestTime() => this.timer.UpdateBestTime();
        public void AddPenaltyTime() => this.timer.AddPenaltyTime();

        private void Awake() => this.CheckData();

        private void CheckData()
        {
            if (this.startTransform is null) throw new ArgumentNullException($"Start transform is empty.");
            if (this.playerPrefab is null) throw new ArgumentNullException($"Player prefab is missing");
            if (this.currentTimeText is null) throw new ArgumentNullException($"Current time text is missing");
            if (this.bestTimeText is null) throw new ArgumentNullException($"Best time text is missing");
            if (this.loopCounterText is null) throw new ArgumentNullException($"Loop counter text is missing");
        }

        private void Start()
        {
            Instantiate(this.playerPrefab, this.startTransform.position, this.startTransform.rotation);
            this.SetupTimeText(this.currentTimeText, this.timer.CurrentTime);

            this.SetupEvent();
        }

        private void SetupEvent()
        {
            this.timer.OnBestTimeTextUpdate += this.UpdateBestTimeText;
            this.timer.OnCurrentTimeUpdate += this.UpdateCurrentTimeText;
            this.OnLoopCounterUpdate += this.UpdateLoopCounterText;
        }

        private void UpdateBestTimeText(object sender, EventArgs args) => this.SetupTimeText(this.bestTimeText, this.timer.BestTime);
        private void UpdateCurrentTimeText(object sender, EventArgs args) => this.SetupTimeText(this.currentTimeText, this.timer.CurrentTime);
        private void UpdateLoopCounterText(object sender, EventArgs args) => this.loopCounterText.text = $"Loop : {this.loopCounter}";

        private void SetupTimeText(TextMeshProUGUI textMesh, float seconds) => textMesh.text = TimeSpan.FromSeconds(seconds).ToString("mm\\:ss\\.fff");
    }
}