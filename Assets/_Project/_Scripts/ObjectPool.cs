using System;
using System.Collections.Generic;
using Tetris.Core;
using UnityEngine;

namespace Tetris.Pooling {
    public class ObjectPool : MonoBehaviour {
        // use a dictionary to store the pool of tetriminos, since we need to know the type of the tetrimino
        readonly Dictionary<Type, Queue<Tetrimino>> pool = new();
        [SerializeField] int size;
        public T Get<T>() where T : Tetrimino {
            // if the pool does not contain the type, return null
            if (!pool.TryGetValue(typeof(T), out var queue) || queue.Count <= 0) return null;
            var tetrimino = (T)queue.Dequeue();
            tetrimino.gameObject.SetActive(true);
            return tetrimino;
        }

        public void Return<T>(T tetrimino) where T : Tetrimino {
            if (!pool.ContainsKey(typeof(T))) pool.Add(typeof(T), new());
            tetrimino.gameObject.SetActive(false);
            pool[typeof(T)].Enqueue(tetrimino);
        }

        void Start() {
            // this will spawn size amounts of each tetrimino type
            Initialize<BlueTetrimino>();
            Initialize<RedTetrimino>();
            Initialize<GreenTetrimino>();
            Initialize<YellowTetrimino>();
            Initialize<OrangeTetrimino>();
            Initialize<PurpleTetrimino>();
            Initialize<CyanTetrimino>();
        }

        public void Initialize<T>() where T : Tetrimino {
            pool.Add(typeof(T), new(size));
            for (var i = 0; i < size; i++) {
                var tetrimino = TetriminoFactory.Create<T>();
                Return(tetrimino);
            }
        }
    }
}
