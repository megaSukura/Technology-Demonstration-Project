
using UnityEngine;
using UnityEditor;
public class BasePlayer : EntityLogicSub
{
    MoveAbility moveAbility;
    
    Vector2 moveDir;
    float dushForceMultiplier=2;

    public BasePlayer(GameEntity entity) : base(entity)
    {
    }
    public BasePlayer( GameEntity entity,float StartMoveSpeed):base(entity)
    {
        this.StartSpeed=StartMoveSpeed;
    }
    private float StartSpeed=1f;
    public override void Init()
    {
        Require<MoveAbility>();
    }
    bool isInit=false;
    public override bool Execute()
    {
        #region 空引用检测
            if(!isInit)
            {
                entity.speed.Value=StartSpeed;
                moveAbility=entity.FindAbility<MoveAbility>();
                isInit=true;
            }
        #endregion

        #region 普通移动
            
            
            moveDir=new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            moveAbility.Move(moveDir);
            
        #endregion
        #region dash
            if(Input.GetButtonDown("Jump"))
            {   
                moveAbility.Dash(moveDir.normalized*entity.speed.Value*dushForceMultiplier);
            }
        #endregion
        if(Input.GetKey(KeyCode.LeftShift)){
            //Debug.Log(entity.speed.Value+" "+entity.speed.PanleValue);
            //entity.AddBuff();
            var t =BuffCommand.Get(new EntityBuff(5,0.2f,new EntityBuffEffect_Slow()),sender:entity).SetTarget(entity);
            entity.SendCommand(t);
        }

        return true;
    }

    #if UNITY_EDITOR
    public override void OndrawInspector()
    {
        //绘制一个框
        EditorGUILayout.BeginVertical("box");
        //绘制Entity的速度speed
        entity.speed.Value = EditorGUILayout.FloatField("Entity speed", entity.speed.Value);
        //绘制dash的力的倍数
        dushForceMultiplier = EditorGUILayout.FloatField("Dush force multiplier", dushForceMultiplier);
        EditorGUILayout.EndVertical();
    }
    #endif
}

