using System;
using System.Linq;
using Input;
using Tetris.Commands;
using Tetris.Pooling;
using UnityEngine;

namespace Tetris.Core.Player {
    public class PlayerController : MonoBehaviour {
        [SerializeField] PlayerInputProcessor input;
        
        // store the commands here, as we do not need to create new ones every time
        ICommand LeftCommand => new MoveLeftCommand();
        ICommand RightCommand => new MoveRightCommand();
        ICommand DownCommand => new MoveDownCommand();
        ICommand RotateLeftCommand => new RotateLeftCommand();
        ICommand RotateRightCommand => new RotateRightCommand();

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
            currentTetrimino.ExecuteCommand(LeftCommand);
        }

        void Right() {
            if (currentTetrimino.TetriminoPartTransforms.Any(x => x.position.x >= GameManager.Instance.RightBound)) return;
            currentTetrimino.ExecuteCommand(RightCommand);
        }

        void Down() { currentTetrimino.ExecuteCommand(DownCommand); }

        void RotateLeft() { currentTetrimino.ExecuteCommand(RotateLeftCommand); }

        void RotateRight() { currentTetrimino.ReverseCommand(RotateRightCommand); }
    }
}
