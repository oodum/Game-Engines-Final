using System;
using Tetris.Commands;
using UnityEngine;
namespace Tetris.Core {
    [SelectionBase]
    public abstract class Tetrimino : MonoBehaviour {
        [SerializeField] Vector2[] tetriminoPartPositions;
        public Transform[] TetriminoPartTransforms;
        public abstract Color Color { get; }

        // set the color of the tetrimino in the editor validation
        void OnValidate() {
            SetColor();
        }

        // set the color of the tetrimino in the editor, and add the children of this tetrimino to the TetriminoPartTransforms array
        void Awake() {
            SetColor();
            AddChildrenToTransformsArray();
        }

        void AddChildrenToTransformsArray() {
            TetriminoPartTransforms = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++) {
                TetriminoPartTransforms[i] = transform.GetChild(i);
            }
        }

        // When a line clear happens, the tetrimino parts will be hidden and moved. This method will reset the position of the tetrimino parts.
        public void Assemble() {
            for (var i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).localPosition = tetriminoPartPositions[i];
            }
        }

        void SetColor() {
            foreach (var sr in transform.GetComponentsInChildren<SpriteRenderer>()) {
                sr.color = Color;
            }
        }
        public void ApplyGravity() => ExecuteCommand(new MoveDownCommand());
        
        // execute the command pattern
        public void ExecuteCommand(ICommand command) {
            command.Execute(this);
        }
        
        public void ReverseCommand(ICommand command) {
            command.Reverse(this);
        }
    }
}
