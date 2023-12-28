using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbility : AbilityBase
{
    public MoveAbility(GameEntity owner):base(owner, "MoveAbility")
    {
        
    }
    public override void StartAbility()
    {
        base.StartAbility();
        
    }
    public override void UpdateAbility()
    {
        base.UpdateAbility();
        MoveOnUpdate();
    }
    public override void EndAbility()
    {
        base.EndAbility();
        
    }

   

    //~~~~~~~moveAbility specific~~~~~~~~~~
    public Vector2 moveDirection;
    //if you want to set moveSpeed, you can set it in GameEntity or set "speed" in EntityLogicSub
    public float MaxMoveSpeed{get{return owner.speed.PanleValue;}}
    /// <summary>
    /// the time to reach disired speed
    /// </summary>
    public float reachMaxMoveSpeedTime=0.1f;
    
    bool isDashing=false;
    public void Move(Vector2 direction)
    {
        moveDirection = direction;
    }
    public void Dash(Vector2 force)
    {   
        isDashing=true;
        Timer.SetTimer(0.2f,onEnd:()=>isDashing=false);
        owner.entityRigidbody.AddForce(force, ForceMode2D.Impulse);
    }
    public void MoveOnUpdate()//TODO:使用FixedUpdate
    {   
        if(isDashing)
            return;
        const int normalAngle_from = 10;
        const int normalAngle_to = 150;
        if(moveDirection!=Vector2.zero)
        {   /*
              这样的写法是为了手感更好，当角色的速度和移动方向的夹角小于normalAngle_from时，角色会直接到达最大速度
              this code is for better feeling, when angle between velocity and moveDirection is less than normalAngle_from,
                the entity will reach maxMoveSpeed directly
            */
            
            var angle = Vector2.Angle(owner.entityRigidbody.velocity, moveDirection);
            if (angle>normalAngle_from && angle<normalAngle_to)
            {   //if angle between velocity and moveDirection is gerater than normalAngle_from and less than normalAngle_to
                
                    //if velocity in moveDirection is less than MaxMoveSpeed
                    
                    if (((owner.entityRigidbody.velocity)*moveDirection).magnitude<MaxMoveSpeed)
                    {    //add force to rigidbody to reach maxMoveSpeed in reachMaxMoveSpeedTime
                        owner.entityRigidbody.AddForce(((moveDirection.normalized* MaxMoveSpeed)-owner.entityRigidbody.velocity)
                                                         * Time.deltaTime / reachMaxMoveSpeedTime
                                                        , ForceMode2D.Impulse);
                    }else{
                        owner.entityRigidbody.velocity = moveDirection.normalized * MaxMoveSpeed;
                    }

            } 
            //if angle between velocity and moveDirection is out of normalAngle_from or normalAngle_to
            else
            {
                    //set velocity to moveDirection in reachMaxMoveSpeedTime directly
                    
                    owner.entityRigidbody.velocity = Vector2.Lerp(owner.entityRigidbody.velocity,
                                                                  moveDirection.normalized * MaxMoveSpeed,
                                                                   Time.deltaTime / reachMaxMoveSpeedTime     );
                
            }
        moveDirection=Vector2.zero;
        }
        else
        {   
            owner.entityRigidbody.velocity = Vector2.Lerp(owner.entityRigidbody.velocity,Vector2.zero,Time.deltaTime / reachMaxMoveSpeedTime);
        }
        //debug
        
            Color color;
            if(Vector2.Angle(owner.entityRigidbody.velocity, moveDirection)>normalAngle_from && Vector2.Angle(owner.entityRigidbody.velocity, moveDirection)<normalAngle_to)
                color=Color.blue;
            else
                color=Color.red;
                
            Debug.DrawRay(owner.transform.position, owner.entityRigidbody.velocity, color);
            Debug.DrawRay(owner.transform.position, moveDirection.normalized * MaxMoveSpeed, Color.green);
    }

}
