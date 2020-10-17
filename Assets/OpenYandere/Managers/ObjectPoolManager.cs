using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Managers
{
    internal class ObjectPoolManager : MonoBehaviour
    {
        [FormerlySerializedAs("PoolEntries")] public List<PoolEntry> poolEntries = new List<PoolEntry>();
        private                                      bool            m_isCoroutineActive;

        public GameObject this[string name]
        {
            get
            {
                PoolEntry poolEntry = poolEntries.First(pooledObject => pooledObject.objectName == name);

                if(poolEntry == null)
                {
                    return null;
                }

                foreach(GameObject pooledObject in poolEntry.pooledObjects)
                {
                    if(!pooledObject.gameObject.activeInHierarchy)
                    {
                        return pooledObject.gameObject;
                    }
                }

                return !poolEntry.automaticGrowth ? null : CreateObject(poolEntry);
            }
        }

        private void Awake()
        {
            foreach(PoolEntry poolEntry in poolEntries)
            {
                for(int i = 0; i < poolEntry.poolAmount; i++)
                {
                    CreateObject(poolEntry);
                }
            }
        }

        private GameObject CreateObject(PoolEntry poolEntry)
        {
            if(poolEntry.prefabObject == null)
            {
                Debug.LogErrorFormat("'{0}' has not been associated with a prefab.", poolEntry.objectName);

                return null;
            }

            GameObject instantinatedObject = poolEntry.parentObject != null
                ? Instantiate(poolEntry.prefabObject, poolEntry.parentObject.transform)
                : Instantiate(poolEntry.prefabObject);
            instantinatedObject.SetActive(false);

            poolEntry.pooledObjects.Add(instantinatedObject);

            if(!m_isCoroutineActive && poolEntry.pooledObjects.Count > poolEntry.poolAmount)
            {
                StartCoroutine(ClearExcess());
                m_isCoroutineActive = true;
            }

            return instantinatedObject;
        }

        private IEnumerator ClearExcess()
        {
            // Yield a few seconds before checking for the first time.
            // Otherwise it would register the first object that goes over the limit
            // then wait 60 seconds before registering any made on subsequent frames.
            yield return new WaitForSeconds(3f);

            while(true)
            {
                int excessObjects = 0;

                foreach(PoolEntry poolEntry in poolEntries)
                {
                    if(!poolEntry.automaticGrowth || poolEntry.pooledObjects.Count <= poolEntry.poolAmount)
                    {
                        continue;
                    }

                    // Get the items that are over the allowed range.
                    List<GameObject> autoPooledObjects =
                        poolEntry.pooledObjects.GetRange(poolEntry.poolAmount,
                                                         poolEntry.pooledObjects.Count - 1 -
                                                         (poolEntry.poolAmount - 1));

                    // Add the number of extra objects on to the count
                    excessObjects += autoPooledObjects.Count;

                    // Remove the objects that aren't in use (disabled)
                    foreach(GameObject pooledObject in autoPooledObjects)
                    {
                        // Skip the if object is active.
                        if(pooledObject.activeInHierarchy)
                        {
                            continue;
                        }

                        poolEntry.pooledObjects.Remove(pooledObject);
                        Destroy(pooledObject);

                        // Remove it from the excess count as it was removed.
                        excessObjects -= 1;
                    }
                }

                if(excessObjects == 0)
                {
                    m_isCoroutineActive = false;

                    break;
                }

                yield return new WaitForSeconds(60f);
            }
        }

        [Serializable]
        internal class PoolEntry
        {
            [FormerlySerializedAs("ObjectName")] [Tooltip("The name that will be used to access the pooled objects.")]
            public string objectName;

            [FormerlySerializedAs("PrefabObject")] [Tooltip("The prefab of the object to be pooled.")]
            public GameObject prefabObject;

            [FormerlySerializedAs("ParentObject")] [Tooltip("The parent that the prefab should be a child of.")]
            public GameObject parentObject;

            [FormerlySerializedAs("PoolAmount")] [Tooltip("The number of objects to be pooled.")]
            public int poolAmount;

            [FormerlySerializedAs("AutomaticGrowth")] [Tooltip("Can the pool grow if there are not enough objects?")]
            public bool automaticGrowth;

            [FormerlySerializedAs("PooledObjects")] [HideInInspector]
            public List<GameObject> pooledObjects = new List<GameObject>();
        }
    }
}