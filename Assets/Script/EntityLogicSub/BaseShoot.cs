using UnityEditor;
using UnityEngine;

public class BaseShoot : EntityLogicSub
{
    private const float MOVECAUSE = 0.2f;
    ShootAbility shootAbility;
    ProjectileSObase projectileType;
    //test:
    
    public float ProjectionSpeed=10f;

    public int shootNum=1;
    public float sootAngle=30f;

    AttributeValue<float> shootSpeed{
        get{
            if(entity==null)
            return null;
            return entity.attributeModule.attributeValue<float>("shootSpeed",1f);
        }
    }
    public void SetShootSpeed(float speed){
        if(speed>0)
        shootSpeed.Value=speed;
       
    }
    TimerExtend.ColdDown shootCD=new TimerExtend.ColdDown(1f);

    public BaseShoot(GameEntity entity) : base(entity)
    {
    }
    public BaseShoot(GameEntity entity,float shootSpeed,float projectionSpeed,ProjectileSObase projectileType=null) : base(entity)
    {
        this.shootSpeed.Value=shootSpeed;
        this.ProjectionSpeed=projectionSpeed;
        if(projectileType!=null)
            this.projectileType=projectileType;
    }
    
    bool isInit=false;
    public override bool Execute()
    {   
        
        if(!isInit)
        {
             shootAbility = entity.FindAbility<ShootAbility>("ShootAbility");
                isInit=true;
        }

        if(Input.GetButton("Fire1"))
        {   
            Shoot( entity.GetDirectionToMouse() );
            
        }
        Vector2 shootDir=new Vector2(Input.GetAxisRaw("FireJoystickHorizontal"),Input.GetAxisRaw("FireJoystickVertical"));
        if( shootDir.magnitude>0.5f ){
            
            
            Shoot(shootDir.normalized);
        }
        
        return true;
    }
    private Projection[] Shoot(Vector2 shootDir){
        if(projectileType==null || shootDir==Vector2.zero)
            return null;
        
        if(!shootCD.coldDown(shootSpeed.Value))
                return null;
            //shoot to 
             shootDir=shootDir+(entity.entityRigidbody.velocity).normalized* MOVECAUSE;
            
             return  shootAbility.ShootProjectile(projectileType,entity.entityCollider.bounds.center,shootDir.normalized*ProjectionSpeed,shootNum    ,sootAngle);
    }
    #if UNITY_EDITOR
    public override void OndrawInspector()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.ObjectField("Projectile type", projectileType, typeof(ProjectileSObase), false);
        //shoot speed
        shootSpeed.Value = EditorGUILayout.FloatField("Shoot speed", shootSpeed.Value);
        //projection speed
        ProjectionSpeed = EditorGUILayout.FloatField("Projection speed", ProjectionSpeed);
        //
        shootNum = EditorGUILayout.IntField("Shoot num", shootNum);
        //
        sootAngle = EditorGUILayout.FloatField("Shoot angle", sootAngle);
        

        EditorGUILayout.EndVertical();
    }
    #endif
}
