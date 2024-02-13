using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_GemManager : MonoBehaviour
{
    [SerializeField]
    private PB_GemPool _gemPool = null;
    [SerializeField]
    private int _gemCount = 20;
    [SerializeField]
    private int _levelGemCount = 10;
    [SerializeField]
    private int _shootableGemCount = 2;

    private List<GameObject> _gemGOList;

    private void Awake()
    {
        if(_gemPool)
        {
            for(int idx = 0; idx < _gemCount; idx++)
            {
                _gemGOList.Add(_gemPool.GetGem());
            }
        }
    }
}
