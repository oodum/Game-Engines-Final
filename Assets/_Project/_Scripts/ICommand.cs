using Tetris.Core;
using UnityEngine;

namespace Tetris.Commands {
    public interface ICommand {
        public void Execute(Tetrimino transform);
        public void Reverse(Tetrimino transform);
    }
    public interface IRotateCommand : ICommand {
        public float RotationAngle { get; }
    }
    
    public interface IMoveCommand : ICommand {
        public Vector3 Direction { get; }
    }
    
    public class RotateLeftCommand : IRotateCommand {
        public float RotationAngle => 90;
        public void Execute(Tetrimino tetrimino) {
            tetrimino.transform.Rotate(Vector3.forward, RotationAngle);
        }

        public void Reverse(Tetrimino tetrimino) {
            tetrimino.transform.Rotate(Vector3.forward, -RotationAngle);
        }
    }
    
    public class RotateRightCommand : IRotateCommand {
        public float RotationAngle => -90;
        public void Execute(Tetrimino tetrimino) {
            tetrimino.transform.Rotate(Vector3.forward, RotationAngle);
        }

        public void Reverse(Tetrimino tetrimino) {
            tetrimino.transform.Rotate(Vector3.forward, -RotationAngle);
        }
    }
    
    public class MoveLeftCommand : IMoveCommand {
        public Vector3 Direction => Vector3.left;
        public void Execute(Tetrimino tetrimino) {
            tetrimino.transform.position += Direction * tetrimino.transform.localScale.x;
        }

        public void Reverse(Tetrimino tetrimino) {
            tetrimino.transform.position -= Direction * tetrimino.transform.localScale.x;
        }
    }
    
    public class MoveRightCommand : IMoveCommand {
        public Vector3 Direction => Vector3.right;
        public void Execute(Tetrimino tetrimino) {
            tetrimino.transform.position += Direction * tetrimino.transform.localScale.x;
        }

        public void Reverse(Tetrimino tetrimino) {
            tetrimino.transform.position -= Direction * tetrimino.transform.localScale.x;
        }
    }
    
    public class MoveDownCommand : IMoveCommand {
        public Vector3 Direction => Vector3.down;
        public void Execute(Tetrimino tetrimino) {
            tetrimino.transform.position += Direction * tetrimino.transform.localScale.y;
        }

        public void Reverse(Tetrimino tetrimino) {
            tetrimino.transform.position -= Direction * tetrimino.transform.localScale.y;
        }
    }
}