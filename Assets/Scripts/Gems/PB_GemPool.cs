using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PB_GemPool : MonoBehaviour
{
    [SerializeField]
    private PB_GemComponent _gemPrefab;

    [SerializeField]
    private int _poolSize = 33;
    [SerializeField]
    private Transform _poolParent; //for making a parent hierarchy on the inspector when spawning multiple objects (just for visual organization)

    public Queue<PB_GemComponent> _gemPool;

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        //if the queue hasn't been created yet or was created but is empty, then create a new one
        if (_gemPool == null || _gemPool.Count == 0)
        {
            _gemPool = new Queue<PB_GemComponent>();
        }

        //if we already have as many of this objects spawned, don't spawn anymore
        if (_gemPool.Count >= _poolSize)
        {
            return;
        }

        //if there's no parent, create a new one with the name of this object
        if (!_poolParent)
        {
            _poolParent = new GameObject(name).transform;
        }

        while (_gemPool.Count < _poolSize)
        {
            PB_GemComponent newGem = Instantiate(_gemPrefab, _poolParent);
            newGem.gameObject.SetActive(false);
            _gemPool.Enqueue(newGem);
        }
    }

    public PB_GemComponent GetGem()
    {
        if (_gemPool == null || _gemPool.Count == 0)
        {
            InitPool();
            Debug.LogWarning($"{name} spawned mid game. Consider spawning it at the start!");
        }

        //get a reference to the first object (at the beginning of the queue) by removing it from the queue
        PB_GemComponent gem = _gemPool.Dequeue();

        if(gem == null)
        {
            return null;
        }
        //so we take the object from the front, and then stick it to the back
        _gemPool.Enqueue(gem);

        gem.gameObject.SetActive(true);

        return gem;
    }
}
