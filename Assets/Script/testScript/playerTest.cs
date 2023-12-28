using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GameEntity))]
public class playerTest : MonoBehaviour
{
    private GameEntity entity;
    private string currentButton;
    private void Start() {
        entity=GetComponent<GameEntity>();
        moveAbility = entity.abilityManager.AddAbility(new MoveAbility(entity)) as MoveAbility;
        shootAbility = entity.abilityManager.AddAbility(new ShootAbility(entity)) as ShootAbility;
        entity.health.Value = 2000f;
        entity.speed.Value = 10f;
        // entity.health.OnValueChange+=(oldValue,newValue)=>{
        //     Debug.Log("health change from "+oldValue+" to "+newValue);
        // };
    }
    //test move
    private MoveAbility moveAbility;
    private TimerExtend.ColdDown moveCD=new TimerExtend.ColdDown(0.2f);
    public bool isMove=true;
    public bool isMoveRandomly=false;
    public float moveSpeed=10f;
    public float moveRandomlyCD=0.2f;
    private Vector2 movedir;
    //test projectile
    public ShootAbility shootAbility;
    private void Update() {
        
        entity.speed.Value = moveSpeed;
        moveAbility.Move(movedir);
        //test move
        if(isMove){if(isMoveRandomly){
            if(moveCD.coldDown(moveRandomlyCD))
                movedir=new Vector2(UnityEngine.Random.Range(-1f,1f),UnityEngine.Random.Range(-1f,1f));
                }
            }
            
        //test projectile
        // if(Input.GetButtonDown("Fire1")||Input.GetMouseButtonDown(0))
        // {
            
        //         if(projectionSO!=null)
        //         {   
        //             var t= shootAbility.Shoot(projectionSO);
        //             if (target!=null)
        //             t.UpdateLogicSubHandler.FindLogicSub<simpleLogicSub>().target=target;
                    

        //         }
        //     Debug.DrawLine(entity.transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition),Color.green);
        // }
    }
}
