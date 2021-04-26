using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMove : UnitMove
{
    public Transform target;
    public float navigationUpdateThreshold = 50.0f;

    void Update()
    {
        if((navTargetPosition - target.position).sqrMagnitude > navigationUpdateThreshold * navigationUpdateThreshold)
        {
            NavigateTo(target.position);
        }
    }
}
