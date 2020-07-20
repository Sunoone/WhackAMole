using GarbageCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    private Action _poolCallback;
    public void SetCallback(Action poolCallback) => _poolCallback = poolCallback;

    public static Poolable Create(Poolable prefab, Pool pool)
    {
        var poolable = Instantiate(prefab, pool.transform);
        poolable.SetCallback(() => pool.PoolItem(poolable));
        return poolable;
    }

    [ContextMenu("Pool")]
    public void Pool() => _poolCallback?.Invoke();
}
