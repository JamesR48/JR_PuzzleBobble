using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PB_GemPool : MonoBehaviour
{
    [SerializeField]
    private PB_GemComponent _gemPrefab;

    [SerializeField]
    private bool _poolCollectionCheck = true;
    [SerializeField]
    private int _poolCapacity = 20;
    [SerializeField]
    private int _poolMaxSize = 40;

    public IObjectPool<PB_GemComponent> _gemPool;

    private void Awake()
    {
        _gemPool = new ObjectPool<PB_GemComponent>(CreateGem, OnGetFromPool, OnReturnedToPool, OnDestroyPoolObject, _poolCollectionCheck, _poolCapacity, _poolMaxSize);
    }

    private PB_GemComponent CreateGem()
    {
        PB_GemComponent gemInstance = Instantiate(_gemPrefab);
        return gemInstance;
    }

    // Called when an item is taken from the pool using Get
    private void OnGetFromPool(PB_GemComponent gem)
    {
        gem.gameObject.SetActive(true);
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(PB_GemComponent gem)
    {
        gem.gameObject.SetActive(false);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(PB_GemComponent gem)
    {
        Destroy(gem);
    }

    public PB_GemComponent GetGem()
    {
        Debug.Log(_gemPool);
        if(_gemPool != null)
        {
            return _gemPool.Get();
        }

        return null;
    }
}
