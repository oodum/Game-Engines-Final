using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Pooling;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using Random = UnityEngine.Random;
namespace Tetris.Core {
    public class GameManager : Singleton<GameManager> {
        public readonly List<Tetrimino> ActiveTetriminos = new();
        [SerializeField] ObjectPool pool;
        [SerializeField] LayerMask tetriminoLayer;
        public float LowerBound, LeftBound, RightBound;
        public Tetrimino CurrentTetrimino;

        [SerializeField] float gravityTickRateSeconds;
        float timer;

        public void AddTetrimino(Tetrimino tetrimino) {
            ActiveTetriminos.Add(tetrimino);
        }
        public void ApplyGravity() {
            foreach (var tetrimino in ActiveTetriminos) {
                tetrimino.ApplyGravity();
            }
        }

        void Start() {
            CurrentTetrimino = Get();
        }

        Tetrimino Get() {
            var random = Random.Range(0, 7);
            return random switch {
                0 => pool.Get<CyanTetrimino>(),
                1 => pool.Get<BlueTetrimino>(),
                2 => pool.Get<RedTetrimino>(),
                3 => pool.Get<GreenTetrimino>(),
                4 => pool.Get<YellowTetrimino>(),
                5 => pool.Get<OrangeTetrimino>(),
                6 => pool.Get<PurpleTetrimino>(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        void ApplyGravityToCurrent() => CurrentTetrimino.ApplyGravity();

        void Update() {
            HandleGravity();
            HandleBounds();
        }
        void HandleBounds() {
            if (CurrentTetrimino == null) return;
            if (CurrentTetrimino.TetriminoPartTransforms.Any(x => x.position.y <= LowerBound)) {
                Debug.Log("Hit the bottom");
                CurrentTetrimino = Get();
            }
        }

        void HandleGravity() {
            timer += Time.deltaTime;
            if (!(timer >= gravityTickRateSeconds)) return;
            ApplyGravityToCurrent();
            timer = 0;
        }

        void HandleTouching() {
            /*
            foreach (var VARIABLE in COLLECTION) {
                
            }
            var hit = Physics.Raycast()
        */
        }
    }
}
