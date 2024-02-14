using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_GemManager : MonoBehaviour
{
    [SerializeField]
    private PB_GemPool _gemPool = null;
    [SerializeField]
    private int _gemCount = 10;
    [SerializeField]
    private PB_CharacterComponent _characterComponent = null;

    private List<PB_GemComponent> _gemList;

    private void Start()
    {
        if(_gemPool != null)
        {
            //_gemList = new List<PB_GemComponent>();
            //for(int idx = 0; idx < _gemCount; idx++)
            //{
            //    PB_GemComponent newGem = _gemPool.GetGem();
            //    if(newGem != null)
            //    {
            //        _gemList.Add(newGem);
            //    }
            //}

            UpdateShootableGems();
        }
    }

    public void UpdateShootableGems()
    {
        if (_gemPool != null && _characterComponent != null)
        {
            PB_GemComponent[] gems = new PB_GemComponent[2];
            gems[0] = _gemPool.GetGem(); gems[1] = _gemPool.GetGem();
            _characterComponent.UpdateShootableGems(gems);
        }
    }
}
