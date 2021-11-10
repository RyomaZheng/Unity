using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        // ��ȡ����
        var vertorToTarget = target.position - transform.position;
        vertorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vertorToTarget);
        Debug.Log("dot:" + dot);

        return dot >= dotThreshold;
    }
}
