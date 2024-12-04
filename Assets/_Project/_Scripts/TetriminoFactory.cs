using System;
using UnityEngine;
using Utility;
namespace Tetris.Core {
    public class TetriminoFactory : Singleton<TetriminoFactory> {
        // get a reference to all the tetrimino prefabs
        [SerializeField] Tetrimino blueTetrimino, redTetrimino, greenTetrimino, yellowTetrimino, orangeTetrimino, purpleTetrimino, cyanTetrimino;
        // the spawn position of the tetrimino
        [SerializeField] Transform spawnPosition;
        // scale the tetriminos
        [SerializeField] float scale;
        
        // create a tetrimino of type T
        public static T Create<T>() where T : Tetrimino {
            // spawn the specified tetrimino at the spawn position
            var tetrimino = Instantiate(GetPrefab<T>(), Instance.spawnPosition.position, Quaternion.identity);
            // and set its scale
            tetrimino.transform.localScale = Vector3.one * Instance.scale;
            return tetrimino;
        }

        // get the prefab of type T
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
