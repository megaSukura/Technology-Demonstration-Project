using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementData
{
    public Vector3 position; 
    public Vector3 speed;
    public Vector3 acceleration;

    public MovementData(Vector3 pos,Vector3 spd,Vector3 acc){
        position = pos;
        speed = spd;
        acceleration = acc;
        
    }

    public override string ToString()
    {
        return $"[position{position} , speed:{speed} , acceleration:{acceleration}]";
    }

    static bool isPosEqual( MovementData a,MovementData b ){
        return Mathf.Abs(Vector3.Distance((a.position - b.position) , Vector3.zero))<0.01f;
    }
    static bool isSpdEqual( MovementData a,MovementData b ){
        return Mathf.Abs(Vector3.Distance((a.speed - b.position) , Vector3.zero))<0.01f;
    }
    static bool isAccEqual( MovementData a,MovementData b ){
        return Mathf.Abs(Vector3.Distance((a.acceleration - b.acceleration) , Vector3.zero))<0.01f;
    }

}
