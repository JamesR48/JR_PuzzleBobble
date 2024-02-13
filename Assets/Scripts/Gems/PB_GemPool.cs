using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PB_GemPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _gemPrefab;

    [SerializeField]
    private bool _poolCollectionCheck = true;
    [SerializeField]
    private int _poolCapacity = 20;
    [SerializeField]
    private int _poolMaxSize = 40;

    private IObjectPool<GameObject> _gemPool;

    private void Awake()
    {
        _gemPool = new ObjectPool<GameObject>(CreateGem, OnGetFromPool, OnReturnedToPool, OnDestroyPoolObject, _poolCollectionCheck, _poolCapacity, _poolMaxSize);
    }

    private GameObject CreateGem()
    {
        GameObject gemInstance = Instantiate(_gemPrefab);
        return gemInstance;
    }

    // Called when an item is taken from the pool using Get
    private void OnGetFromPool(GameObject gem)
    {
        gem.SetActive(true);
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(GameObject gem)
    {
        gem.SetActive(false);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(GameObject gem)
    {
        Destroy(gem);
    }

    public GameObject GetGem()
    {
        return _gemPool.Get();
    }
}
