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
    private BoxCollider2D _boxCollision = null;
    [SerializeField]
    private PB_EBoundType _boundType = PB_EBoundType.NONE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public PB_EBoundType GetBoundType()
    {
        return _boundType;
    }

    public void SetBoundsSize(float InSizeX = 1, float InSizeY = 1)
    {
        if(_boxCollision != null)
        {
            _boxCollision.size = new Vector2(InSizeX, InSizeY);
        }
    }
}
