using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// A generic object pool that can be used for any Component type.
    /// </summary>
    /// <typeparam name="T">The type of Component to pool (e.g., Bullet, Enemy, etc.).</typeparam>
    public class ObjectPool<T> where T : Component
    {
        private T[] prefabs;
        private Queue<T> pool;
        private Transform poolContainer;

        /// <summary>
        /// Initializes the pool with a given prefab and initial size.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="initialSize">How many objects to pre-instantiate.</param>
        public ObjectPool(T[] prefabs, int initialSize, Transform poolContainer)
        {
            this.prefabs = prefabs;
            this.poolContainer = poolContainer;
            pool = new Queue<T>(initialSize);

            // Pre-instantiate objects
            for (int i = 0; i < initialSize; i++)
            {
                var randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
                var newObj = Object.Instantiate(randomPrefab, poolContainer, true);
                newObj.gameObject.SetActive(false);
                pool.Enqueue(newObj);
            }
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// If the pool is empty, a new instance is created.
        /// </summary>
        /// <returns>An activated object of type T.</returns>
        public T GetFromPool()
        {
            T obj;

            if (pool.Count == 0)
            {
                // Optionally instantiate a new object if pool is empty.
                obj = Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
            }
            else
            {
                obj = pool.Dequeue();
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Returns an object to the pool, deactivating it.
        /// </summary>
        /// <param name="obj">The object to return to the pool.</param>
        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}