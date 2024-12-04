using System;
using System.Linq;
using Input;
using Tetris.Commands;
using Tetris.Pooling;
using UnityEngine;

namespace Tetris.Core.Player {
    public class PlayerController : MonoBehaviour {
        [SerializeField] PlayerInputProcessor input;

        Tetrimino currentTetrimino;
        void Start() {
            input.SetPlayer();
        }

        void Update() {
            currentTetrimino = GameManager.Instance.CurrentTetrimino;
        }

        void OnEnable() {
            input.OnLeftEvent += Left;
            input.OnRightEvent += Right;
            input.OnDownEvent += Down;
            input.OnRotateLeftEvent += RotateLeft;
            input.OnRotateRightEvent += RotateRight;
        }

        void OnDisable() {
            input.OnLeftEvent -= Left;
            input.OnRightEvent -= Right;
            input.OnDownEvent -= Down;
            input.OnRotateLeftEvent -= RotateLeft;
            input.OnRotateRightEvent -= RotateRight;
        }

        void Left() {
            if (currentTetrimino.TetriminoPartTransforms.Any(x => x.position.x <= GameManager.Instance.LeftBound)) return;
            currentTetrimino.ExecuteCommand(new MoveLeftCommand());
        }

        void Right() {
            if (currentTetrimino.TetriminoPartTransforms.Any(x => x.position.x >= GameManager.Instance.RightBound)) return;
            currentTetrimino.ExecuteCommand(new MoveRightCommand());
        }

        void Down() { currentTetrimino.ExecuteCommand(new MoveDownCommand()); }

        void RotateLeft() { currentTetrimino.ExecuteCommand(new RotateLeftCommand()); }

        void RotateRight() { currentTetrimino.ReverseCommand(new RotateLeftCommand()); }
    }
}
