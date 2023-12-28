using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    //Test
    public GameEntity playerEntity;
    public NodeEditorController nodeEditorController;
    public ProjectileSObase StartProjectileSO;
    public static ProjectileSObase StartProjectileSO_static;
    public void InitStaicData()
    {
        PortFactory.PortPrefab = Resources.Load<GameObject>("Port");
        PortFactory.DropDownPrefab = Resources.Load<GameObject>("DropDown");
        PortFactory.InputFieldPrefab = Resources.Load<GameObject>("InputField");
        PortFactory.TogglePrefab = Resources.Load<GameObject>("Toggle");
        PortFactory.StringInputFieldPrefab = Resources.Load<GameObject>("StringInputField");
        //
        StartProjectileSO_static = StartProjectileSO;
        //
        nodeEditorController.targetEntity = playerEntity;
        //Debug.Log(PortFactory.PortPrefab);
    }
    void Awake()
    {
        InitStaicData();
    }
    void Start()
    {
        if(playerEntity==null)
        return;
        if(playerEntity.gameObject.GetComponent<AbilityManager>()==null)
            playerEntity.gameObject.AddComponent<AbilityManager>();
        playerEntity.AddAbility(new MoveAbility(playerEntity));
        playerEntity.AddAbility(new ShootAbility(playerEntity));
        playerEntity.AddLogicSub(new BasePlayer(playerEntity,8f));
        //playerEntity.AddLogicSub(new BaseShoot(playerEntity, 0.5f,10f,Instantiate(StartProjectileSO) ));
        var t = playerEntity.GetComponent<SkillSystem.SkillManager>();
        // //创建技能
        // var t_debug_skill1 = new SkillSystem.Skill("debug_skill1",playerEntity,isAutoReset:false);
        // var t_shootSkillEffect1 =new SkillSystem.SkillEfect.shootSkillEffect(
        //     new Parameter<float>(0.5f),
        //     Instantiate(StartProjectileSO),
        //     Vector2.zero,
        //     10f,
        //     new Parameter<Vector2>(//这里是一个参数类,可以将"沉底"的参数传递出来,具体的方式是通过Getter类或者委托来获取参数的值
        //         Vector2.zero,
        //         new EntityVector2Getter(playerEntity, "DirectionToMouse")),
        //     1,
        //     30f,
        //     0.1f );
        // t_debug_skill1.AddEffect(t_shootSkillEffect1);

        // var t_debug_skill2 = new SkillSystem.Skill("debug_skill2",playerEntity,isAutoReset:false);
        //     var t_param_closeEntity=new RefParameter<GameEntity>(null);
        
        // t_debug_skill2.AddEffect(new SkillSystem.SkillEfect.RefParameterDefaultChangeEffect<GameEntity>(
        //     t_param_closeEntity,
        //     (sk)=>{ return sk.entity.GetClosestEntity(30f);},
        //     (_)=>{return null;}
        //     ));
        //     SkillSystem.SkillEfect.shootSkillEffect t_shootSkillEffect2;
        //     t_shootSkillEffect2=new SkillSystem.SkillEfect.shootSkillEffect(
        //     new Parameter<float>(0.5f),
        //     Instantiate(StartProjectileSO),
        //     Vector2.zero,
        //     10f,
        //     new Parameter<Vector2>(//这里是一个参数类,可以将"沉底"的参数传递出来,具体的方式是通过Getter类或者委托来获取参数的值
        //         Vector2.zero,
        //         new EntityVector2Getter(playerEntity, "DirectionToMouse")),
        //     3,
        //     30f,
        //     0.1f,
        //     new ProjectileLogicSub[]{
        //         new TrackingLogicSub(t_param_closeEntity,30)
        //         });
        // t_debug_skill2.AddEffect(t_shootSkillEffect2);
        
        // t_debug_skill2.AddEffect(new SkillSystem.SkillEfect.ProjMovControlSkillEffect(
        //     new RefParameter<Projection[]>(null,
        //         t_shootSkillEffect1.CreateProjectionsGetter()),
        //         (proj)=>{Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //                  return ((mousPos-(Vector2)proj.transform.position).normalized) *proj.DesireVelocity.magnitude;
        //                  }
                
        // ));

        // var t_debug_skill3 = new SkillSystem.Skill("debug_skill3",playerEntity,isAutoReset:false);
        // t_debug_skill3.AddEffect(new SkillSystem.SkillEfect.shootSkillEffect(
        //     new Parameter<float>(1f),
        //     Instantiate(StartProjectileSO),
        //     Vector2.zero,
        //     10f,
        //     new Parameter<Vector2>(//这里是一个参数类,可以将"沉底"的参数传递出来,具体的方式是通过Getter类或者委托来获取参数的值
        //         Vector2.zero,
        //         new EntityVector2Getter(playerEntity, "DirectionToMouse")),
        //     3,
        //     30f,
        //     0));
        // //添加技能
        // t.Add(t_debug_skill1);
        // t.Add(t_debug_skill2);
        // t.Add(t_debug_skill3);
        // //连接技能的调度
        // t.ConnectAlways(t_debug_skill1,new SkillSystem.InputTrigger("Fire1")  );
        // t_debug_skill1.ConnectTo(t_debug_skill2,new SkillSystem.InputTrigger("Fire1") &new SkillSystem.SkillProgressTrigger(t_debug_skill1,0.6f,1f),isEndFrom:true);
        // t_debug_skill2.ConnectTo(t_debug_skill3,new SkillSystem.InputTrigger("Fire1") &new SkillSystem.SkillProgressTrigger(t_debug_skill2,0.6f,1),isEndFrom:true);//注意如果是false那么上一个技能的进度就会走完,从而触发ReadyTargetWhenEnd
        // //连接技能的预备
        // t_debug_skill1.ReadyTargetWhenEnd(t_debug_skill1);//自己连自己;如果没有在最后之前跳转，那么就会自动重置,反之则不会
        // //t_debug_skill2.ReadyTargetWhenEnd(t_debug_skill2);//同上
        // //t_debug_skill3.ReadyTargetWhenEnd(t_debug_skill3);//同上
        // //之所以要用预备线,是因为如果直接用Skill的autoReset,即使在最后之前跳转,也会自动重置(具体是因为autoReset在End()函数中执行)
        // t_debug_skill2.ReadyTargetWhenEnd(t_debug_skill1);//当第二个技能结束时，第一个技能就会准备好
        // t_debug_skill3.ReadyTargetWhenEnd(t_debug_skill1);//同上
    }
}
