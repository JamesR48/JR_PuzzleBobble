using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum PB_EColliderType
{
    NONE,
    CIRCLE,
    LINE_VERTICAL,
    LINE_HORIZONTAL
}

public class PB_CollisionComponent : MonoBehaviour
{
    [SerializeField]
    private PB_EColliderType _colliderType = PB_EColliderType.NONE;
    [SerializeField]
    private bool bDrawDebugCollider = false;
    [SerializeField]
    private bool bOverlapOnly = false;
    [SerializeField]
    private bool bHasCustomStart = false;
    [SerializeField, ConditionalField("bHasCustomStart", true), ConditionalField("_colliderType", PB_EColliderType.NONE, ConditionalFieldAttribute.ComparingType.NotEqual)]
    private Vector2 _customLineCenter = Vector2.zero;
    [SerializeField, ConditionalField("_colliderType", PB_EColliderType.CIRCLE)]
    private float _circleRadius = 1.0f;
    [SerializeField, ConditionalField("_colliderType", PB_EColliderType.CIRCLE, ConditionalFieldAttribute.ComparingType.NotEqual), ConditionalField("_colliderType", PB_EColliderType.NONE, ConditionalFieldAttribute.ComparingType.NotEqual)]
    private float _lineExtent = 1.0f;

    private PB_CollisionComponent[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        colliders = FindObjectsOfType<PB_CollisionComponent>();
    }

    void FixedUpdate()
    {

    }

    public bool CheckCollision(PB_CollisionComponent A, PB_CollisionComponent B)
    {
        if (A == null || B == null || A._colliderType == PB_EColliderType.NONE || B._colliderType == PB_EColliderType.NONE)
        {
            return false;
        }

        bool bAIsCircle = A._colliderType == PB_EColliderType.CIRCLE;
        bool bBIsCircle = B._colliderType == PB_EColliderType.CIRCLE;
        if (bAIsCircle && bBIsCircle)
        {
            return CheckCircleCircle(A, B);
        }
        else
        {
            return false; // Line - Circle
        }
    }

    public bool CheckCircleCircle(PB_CollisionComponent A, PB_CollisionComponent B)
    {
        if(A == null || B == null)
        {
            return false;
        }

        float distSqr = Vector3.SqrMagnitude(B.transform.position - A.transform.position);
        float radiusSum = A.GetCircleRadius()* A.GetCircleRadius() + B.GetCircleRadius()* B.GetCircleRadius();
        if (distSqr > radiusSum)
        {
            return false;
        }

        Debug.Log("COLLIDE " + distSqr + " " + radiusSum);
        return true;
    }

    public PB_EColliderType GetColliderType() { return _colliderType; }

    public float GetCircleRadius() { return _circleRadius; }

    public Vector3 GetRelativeCustomCenter() 
    {
        return -transform.InverseTransformPoint(_customLineCenter); 
    }

    public Vector3 GetLineStart()
    {
        Vector3 extentAxis = _colliderType == PB_EColliderType.LINE_VERTICAL ? Vector3.up * _lineExtent : Vector3.right * _lineExtent;
        Vector3 start = GetRelativeCustomCenter() - extentAxis;
        return start; 
    }

    public Vector3 GetLineEnd()
    {
        Vector3 extentAxis = _colliderType == PB_EColliderType.LINE_VERTICAL ? Vector3.up * _lineExtent : Vector3.right * _lineExtent;
        Vector3 end = GetRelativeCustomCenter() + extentAxis;
        return end;
    }

    public bool GetDrawDebugCollider() { return bDrawDebugCollider; }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 1, 0, 1.0f);
        //Gizmos.DrawWireSphere(transform.position, GetCircleRadius());
    }
}

public class MyScriptGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoForMyScript(PB_CollisionComponent scr, GizmoType gizmoType)
    {
       if(scr != null && scr.GetDrawDebugCollider())
       {
            if(scr.GetColliderType() == PB_EColliderType.CIRCLE)
            {
                Gizmos.DrawWireSphere(scr.GetRelativeCustomCenter(), scr.GetCircleRadius());
            }
            else if(scr.GetColliderType() == PB_EColliderType.LINE_VERTICAL 
                || scr.GetColliderType() == PB_EColliderType.LINE_HORIZONTAL) 
            {
                Gizmos.DrawLine(scr.GetLineStart(), scr.GetLineEnd());
            }
       }
    }
}
