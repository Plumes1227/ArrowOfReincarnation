using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerArrowPools;
    [SerializeField] Pool[] playerAfterimage;
    [SerializeField] Pool[] Enemy_SlimePools;
    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(enemyPools);
        Initialize(playerArrowPools);
        Initialize(playerAfterimage);
        Initialize(Enemy_SlimePools);
    }

    #if UNITY_EDITOR
    void OnDestroy()
    {
        CheackPoolSize(enemyPools);
        CheackPoolSize(playerArrowPools);
        CheackPoolSize(playerAfterimage);
        CheackPoolSize(Enemy_SlimePools);
    }
    #endif
    
    void CheackPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}!",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }
    void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
        #if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("再多個對象池中發現相同預制體:" + pool.Prefab.name);

                continue;
            }
        #endif
            dictionary.Add(pool.Prefab, pool);
            
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("池管理器中找不到prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("池管理器中找不到prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("池管理器中找不到prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("池管理器中找不到prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
