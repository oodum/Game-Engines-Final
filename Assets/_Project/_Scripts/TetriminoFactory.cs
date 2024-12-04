using System;
using UnityEngine;
using Utility;
namespace Tetris.Core {
    public class TetriminoFactory : Singleton<TetriminoFactory> {
        [SerializeField] Tetrimino blueTetrimino, redTetrimino, greenTetrimino, yellowTetrimino, orangeTetrimino, purpleTetrimino, cyanTetrimino;
        [SerializeField] Transform spawnPosition;
        [SerializeField] float scale;
        public static T Create<T>() where T : Tetrimino {
            var tetrimino = Instantiate(GetPrefab<T>(), Instance.spawnPosition.position, Quaternion.identity);
            tetrimino.transform.localScale = Vector3.one * Instance.scale;
            return tetrimino;
        }

        static T GetPrefab<T>() where T : Tetrimino {
            // unfortunately, a switch statement/expression could not be used due to the typeof check not being a constant
            if (typeof(T) == typeof(BlueTetrimino)) return Instance.blueTetrimino as T;
            if (typeof(T) == typeof(RedTetrimino)) return Instance.redTetrimino as T;
            if (typeof(T) == typeof(GreenTetrimino)) return Instance.greenTetrimino as T;
            if (typeof(T) == typeof(YellowTetrimino)) return Instance.yellowTetrimino as T;
            if (typeof(T) == typeof(OrangeTetrimino)) return Instance.orangeTetrimino as T;
            if (typeof(T) == typeof(PurpleTetrimino)) return Instance.purpleTetrimino as T;
            if (typeof(T) == typeof(CyanTetrimino)) return Instance.cyanTetrimino as T;
            throw new ArgumentException("Invalid type");
        }
    }
}
