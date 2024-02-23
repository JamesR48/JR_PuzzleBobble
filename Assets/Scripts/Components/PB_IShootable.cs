using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PB_IShootable
{
    void ShootResponse();
    PB_IShootable InstantiateShootable();
    GameObject gameObject { get; }
}
