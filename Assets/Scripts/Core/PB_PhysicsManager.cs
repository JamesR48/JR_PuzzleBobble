using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_PhysicsManager : MonoBehaviour
{
    private Dictionary<int, PB_CollisionComponent> _collideables;
    private Dictionary<int, PB_MoveComponent> _movables;
    private int _movablesCount = 0;
    private int _collideablesCount = 0;

    private void Start()
    {
        PB_CollisionComponent[] colliders = FindObjectsOfType<PB_CollisionComponent>();
        _collideablesCount = colliders.Length;
        if(_collideablesCount > 0)
        {
            _collideables = new Dictionary<int, PB_CollisionComponent>();
            for (int idx = 0; idx < _collideablesCount; idx++)
            {
                _collideables.Add(idx, colliders[idx]);
            }
        }

        PB_MoveComponent[] movers = FindObjectsOfType<PB_MoveComponent>();
        _movablesCount = movers.Length;
        if (_movablesCount > 0)
        {
            _movables = new Dictionary<int, PB_MoveComponent>();
            for (int idx = 0; idx < _movablesCount; idx++)
            {
                _movables.Add(idx, movers[idx]);
            }
        }
    }

    private void FixedUpdate()
    {
        if(_movables != null)
        {
            for (int idx = 0; idx < _movablesCount; idx++)
            {
                if(_movables[idx].enabled)
                {
                    _movables[idx].UpdateMovement(Time.fixedDeltaTime);
                }
            }
        }

        if(_collideables != null)
        {
            for (int idx = 0; idx < _collideablesCount - 1; idx++)
            {
                for (int jdx = idx + 1; jdx < _collideablesCount; jdx++)
                {
                    bool bcollided = _collideables[idx].CheckCollision(_collideables[idx], _collideables[jdx]);

                    if(_collideables[idx] != null && bcollided && _collideables[idx].TryGetComponent(out PB_MoveComponent MoveComp))
                    {
                        MoveComp.OnStartMoving(Vector3.zero);
                        MoveComp.enabled = false;
                    }
                }
            }
        }
    }

    public void UpdatePhysicsObjects(GameObject spawnedGO)
    {
        if(spawnedGO != null)
        {
            if(spawnedGO.TryGetComponent(out PB_CollisionComponent collisionComp))
            {
                if(!_collideables.ContainsValue(collisionComp))
                {
                    _collideables.Add(_collideablesCount, collisionComp);
                    _collideablesCount++;
                }
            }

            if (spawnedGO.TryGetComponent(out PB_MoveComponent moveComp))
            {
                if (!_movables.ContainsValue(moveComp))
                {
                    _movables.Add(_movablesCount, moveComp);
                    _movablesCount++;
                }
            }
        }
    }
}
