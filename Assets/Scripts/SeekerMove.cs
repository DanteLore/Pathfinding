using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMove : UnitMove
{
    public Transform target;
    
    void Update()
    {
        if(navTargetPosition != target.position )
        {
            NavigateTo(target.position);
        }
    }
}
