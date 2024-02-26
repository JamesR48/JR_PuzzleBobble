using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PB_EBoundType
{
    NONE,
    REPEL,
    STICK
}

public class PB_BoundComponent : MonoBehaviour
{
    [SerializeField]
    private PB_EBoundType _boundType = PB_EBoundType.NONE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PB_EBoundType GetBoundType()
    {
        return _boundType;
    }
}
