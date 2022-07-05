using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Catch.Utils
{
    /// <summary>
    /// Handles All Aspects Of GameObject Pooling
    /// </summary>
    public class UnityObjectPooler : MonoBehaviour
    {
        //Stores the prefabs given to WarmPool as keys for later use
        private Dictionary<GameObject, ObjectPool<GameObject>> _prefabPools = new();

        //Stores the Instantiated prefabs from WarmPool() as keys for later use
        private Dictionary<GameObject, ObjectPool<GameObject>> _spwndPrefabPools = new();

        public static UnityObjectPooler Instance;

        private void Awake() => Instance = this;

        #region API
        /// <summary>
        /// Creates A Pool Of Prefabs. CALL BEFORE "SPAWN/RELEASE"
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="size"></param>
        /// <param name="maxSize"></param>
        public void WarmPool(GameObject prefab, int size, int maxSize)
        {
            if (_prefabPools.ContainsKey(prefab))
            {
                print("POOL ALREADY CREATED");
                return;
            }

            var newPool = new ObjectPool<GameObject>(() => { return InstantiateObject(prefab); }, OnTakeFromPool, OnReleaseFromPool, OnDestroyFromPool, defaultCapacity: size, maxSize: maxSize);

            _prefabPools.Add(prefab, newPool);

            print("POOL CREATED");
        }

        /// <summary>
        /// Retrieves GameObject From Pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <returns></returns>
        public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            //Find A Pool Containing Prefab
            if (_prefabPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                //Get GameObject From Pool
                var spwnedObj = pool.Get();

                //If SpwndPrefabsPools Dosent Contain The Spawned Object Then Add It
                if (!_spwndPrefabPools.ContainsKey(spwnedObj))
                    _spwndPrefabPools.Add(spwnedObj, pool);

                //Set Up Spawned Object
                spwnedObj.transform.position = pos;
                spwnedObj.transform.rotation = rot;

                return spwnedObj;
            }
            else
            {
                //If Theres No Pool Then Create One With Some Default Values
                print("WARMING POOL");
                WarmPool(prefab, 100, 100);

                //Return Spawned Object From Newly Created Pool
                return _prefabPools[prefab].Get();
            }
        }

        /// <summary>
        /// Sets Supplied Object Inactive
        /// </summary>
        /// <param name="obj"></param>
        public void Release(GameObject obj)
        {
            //Find Obj In SpawnedprefabPools Then Release it
            if (_spwndPrefabPools.TryGetValue(obj, out ObjectPool<GameObject> pool))
                pool.Release(obj);
        }

        /// <summary>
        /// Resets All Pools Active
        /// </summary>
        public void ClearAllPools()
        {
            _prefabPools.Clear();
            _spwndPrefabPools.Clear();
        }
        #endregion

        #region PRIVATE
        private GameObject InstantiateObject(GameObject prefab)
        {
            var spwnedObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            return spwnedObj;
        }

        private void OnTakeFromPool(GameObject instance)
        {
            instance.SetActive(true);
        }

        private void OnReleaseFromPool(GameObject instance)
        {
            instance.SetActive(false);
        }

        private void OnDestroyFromPool(GameObject instance)
        {
            Destroy(instance.gameObject);
        }
        #endregion
    }
}
