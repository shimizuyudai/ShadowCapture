using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class StepSequencer : MonoBehaviour
    {
        [SerializeField]
        int column, row;
        public int Column
        {
            get;
            private set;
        }

        public int Row
        {
            get;
            private set;
        }

        public float duration;
        float speed;
        public int Index
        {
            get;
            private set;
        }//現在のカラム
        float t;//現在のノーマライズされた秒数
        public bool[,] Grid { get; private set; }
        public bool isPlaying { get; set; }
        [SerializeField]
        bool playOnAwake;
        public delegate void OnAttack(int column, int[] activeElementIds);
        public delegate void OnInitialized();
        public delegate void OnChangeElement(int column, int row, bool isActive);
        public event OnAttack AttackEvent;
        public event OnInitialized InitializedEvent;
        public event OnChangeElement ChangeElementEvent;


        bool isRewind;

        private void Awake()
        {

        }
        // Use this for initialization
        void Start()
        {
            Init();
            if (playOnAwake) Play();
        }

        public void Init()
        {
            Init(this.column, this.row, this.duration);
        }

        public void Init(int column, int row, float duration)
        {
            Grid = new bool[column, row];
            this.Column = column;
            this.Row = row;
            this.duration = duration;
            this.Index = -1;
            if (InitializedEvent != null) InitializedEvent();
        }

        [ContextMenu("Reload")]
        public void Reload()
        {
            Init();
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Stop()
        {
            isPlaying = false;
            t = 0f;
            this.Index = -1;
            if (AttackEvent != null) AttackEvent(0, new int[] { });
        }

        public void Rewind()
        {
            isRewind = true;
            isPlaying = true;
        }

        public void Play()
        {
            isRewind = false;
            isPlaying = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlaying) return;
            var speed = 1f / duration * (isRewind ? -1f : 1f);
            t += speed * Time.deltaTime;
            if (!isRewind)
            {
                if (t >= 1f) { t = 0f; }
            }
            else
            {
                if (t <= 0) { t = 1f; }
            }

            var n = Mathf.FloorToInt(t * (float)(this.Column));
            if (this.Index != n)
            {
                attack(this.Index);
                this.Index = n;
            }
        }

        void attack(int column)
        {
            if (column < 0 || column >= this.Column) return;
            var activeElementList = new List<int>();
            for (var i = 0; i < this.Row; i++)
            {
                if (Grid[column, i])
                {
                    activeElementList.Add(i);
                }
            }
            if (AttackEvent != null) AttackEvent(column, activeElementList.ToArray());
        }

        public void SetActiveElemnt(int column, int row, bool isActive)
        {
            if (column < 0 || column >= this.Column) return;
            if (row < 0 || row >= this.Row) return;
            var previous = Grid[column, row];
            Grid[column, row] = isActive;
            if (previous != isActive)
            {
                ChangeElementEvent?.Invoke(column, row, isActive);
            }
        }

        public bool IsActiveElement(int column, int row)
        {
            bool result = false;
            if (column < 0 || column >= this.Column) return result;
            if (row < 0 || row >= this.Row) return result;
            return result;
        }

        public bool Toggle(int column, int row)
        {
            var result = false;
            if (column < 0 || column >= this.Column) return result;
            if (row < 0 || row >= this.Row) return result;
            result = !Grid[column, row];
            SetActiveElemnt(column, row, result);
            return result;
        }

        [ContextMenu("EditorControlInit")]
        void EditorControlInit()
        {
            Stop();
            Init(this.column, this.row, this.duration);
            if (playOnAwake) Play();
        }
    }
}