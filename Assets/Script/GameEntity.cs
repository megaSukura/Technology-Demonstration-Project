using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    #region reference
    [HideInInspector]
    public Rigidbody2D entityRigidbody;
    [HideInInspector]
    public Collider2D entityCollider;
    [HideInInspector]
    private GameSystem gameSystem;
    [HideInInspector]
    public Artist artist;
    #endregion

    #region GameAttribute
        public AttributeModule attributeModule=new AttributeModule( null );
        //health
        public AttributeFloat health{
            get{
                return attributeModule.attributeFloat("health",100f);
            }
        }
        //speed
        public AttributeFloat speed{
            get{
                return attributeModule.attributeFloat("speed",1f);
            }
            
        }

    #endregion

    #region Buff
        private BuffManager<GameEntity> buffModule=new BuffManager<GameEntity>();
        public void AddBuff(Buff<GameEntity> buff){
            buffModule.AddBuff(buff,this);
        }
    #endregion

    #region Ability
        public AbilityManager abilityManager;
        public AbilityBase AddAbility(AbilityBase ability){
            if(abilityManager==null)
                abilityManager=gameObject.AddComponent<AbilityManager>();
            abilityManager.AddAbility(ability);
            return ability;
        }
        public T FindAbility<T>(string abilityName) where T:AbilityBase{
            if(abilityManager==null)
                return null;
            return abilityManager.abilities.Find((ability) => ability.AbilityName == abilityName) as T;
        }
        public T FindAbility<T>() where T:AbilityBase{
            if(abilityManager==null)
                return null;
            return abilityManager.abilities.Find((ability) => ability is T) as T;
        }
        public bool HasAbility(Type type){
            if(abilityManager==null)
                return false;
            return abilityManager.abilities.Find((ability) => ability.GetType() == type) != null;
        }
    #endregion

    #region Command
        private EntityCommandHandler commandHandler=new EntityCommandHandler();
        public void SendCommand(EntityCommand command){
            commandHandler.Send(command);
        }
    #endregion
    #region LogicSub
        protected LogicSubhandler entityLogicSubhandler=new LogicSubhandler();
        
        public LogicSubhandler EntityLogicSubhandler{
            get{
                return entityLogicSubhandler;
            }
        }
        
        public void AddLogicSub(EntityLogicSub logicSub){
            
            logicSub.entity=this;
            logicSub.Init();
            foreach(var type in logicSub.requiredAbilityTypes){
                if(HasAbility(type)==false){
                    AddAbility((AbilityBase)System.Activator.CreateInstance(type));
                }
            }
            entityLogicSubhandler.AddLogicSub(logicSub);
        }
    #endregion
    
    #region unnecessary
        public Vector3 VisualPosition{
            get{
                return transform.position+VisualOffset;
            }
        }
        public Vector3 VisualOffset;
        public void SetSpeed(float speed){
            this.speed.Value=speed;
        }
        public void SetHealth(float health){
            this.health.Value=health;
        }

        public void Damage(float damage,GameEntity sender){
            SendCommand(DamageCommand.Get(damage,0,sender));
        }
    #endregion

    #region Parameter get
    /// <summary>
    /// 通过参数名获取参数,注意损耗性能(装箱拆箱),推荐GetFloatParameter和GetVector2Parameter
    /// </summary>
    public T GetParameter<T>(string parameterName){
        switch(parameterName){
            case "health":
                return (T)(object)health.Value;
            case "speed":
                return (T)(object)speed.Value;
            case "RigPosition":
                return (T)(object)entityRigidbody.position;
            case "VisualPosition":
                return (T)(object)VisualPosition;
            case "Velocity":
                return (T)(object)entityRigidbody.velocity;
            case "DirectionToMouse":
                return (T)(object)this.GetDirectionToMouse();
            case "Artist":
                return (T)(object)artist;
            case "AbilityManager":
                return (T)(object)abilityManager;
            case "AttributeModule":
                return (T)(object)attributeModule;
            case "Rigidbody":
                return (T)(object)entityRigidbody;
            case "Collider":
                return (T)(object)entityCollider;
            default:
                return default(T);
        }
    }
    
    public float GetFloatParameter(string parameterName){
        switch(parameterName){
            case "health":
                return health.Value;
            case "speed":
                return speed.Value;
            default:
                return 0;
        }
    }
    public Vector2 GetVector2Parameter(string parameterName){
        switch(parameterName){
            case "RigPosition":
                return entityRigidbody.position;
            case "VisualPosition":
                return VisualPosition;
            case "Velocity":
                return entityRigidbody.velocity;
            case "DirectionToMouse":
                return this.GetDirectionToMouse();
            default:
                return Vector2.zero;
        }
    }
    
    #endregion
    
    #region Skill
    public SkillSystem.SkillManager skillManager;
    #endregion

    private void Awake() {
        getReference();
        gameObject.tag="Entity";
        var t_h_layer=new PanleLayer("Base",0,0,true,false,null);//应该在工厂里面设置
        health.SetPanleComponent(new List<PanleLayer>{t_h_layer},new List<PanleLayer>{t_h_layer});
        var t_s_layer=new PanleLayer("Base",0,0,true,false,null);
        speed.SetPanleComponent(new List<PanleLayer>{t_s_layer},new List<PanleLayer>{t_s_layer});
        
    }
    
    private void Start() {
        health.OnValueChange+=(o,n)=>{if((n-o)<0) artist.Flash(Color.red*10,0.1f); };
        health.OnValueChange+=(_,_)=>{ if(health.Value<=0) Destroy(gameObject); };
    }
    private void Update() {
        if(entityLogicSubhandler!=null){
            entityLogicSubhandler.UpdateLogicSubs();
        }

        if(attributeModule!=null){
            attributeModule.processCommand();
        }

        if(commandHandler!=null){
            commandHandler.ExecuteAll();
        }
    }

    private void LateUpdate()
    {
        
    }
    //get reference
    private void getReference()
    {
        entityRigidbody = GetComponentInChildren<Rigidbody2D>();
        entityCollider = GetComponentInChildren<Collider2D>();
        gameSystem = GameObject.Find("GameSystem")?.GetComponent<GameSystem>();
        abilityManager = GetComponent<AbilityManager>()??gameObject.AddComponent<AbilityManager>();
        skillManager = GetComponent<SkillSystem.SkillManager>()??gameObject.AddComponent<SkillSystem.SkillManager>();
        artist = GetComponent<Artist>()??gameObject.AddComponent<Artist>();
    }

}


public static class EntityTools{
    public static Vector2 GetVectorToMouse(this GameEntity entity){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)entity.transform.position;
        return direction;
    }
    public static Vector2 GetDirectionToMouse(this GameEntity entity){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)entity.transform.position;
        return direction.normalized;
    }
    public static GameEntity GetClosestEntity(this GameEntity entity,float range)
    {
        //FanshapeCollition(this GameEntity entity,out List<GameEntity> result ,Vector2 center,float radius,float angle,Vector2 direction,LayerMask layerMask)
        
        entity.FanshapeCollition(out List<GameEntity> result,entity.transform.position,range,360,Vector2.zero,LayerMask.GetMask("Entity"));
        if(null!=result)
        {
            var minDistance=range;
            GameEntity minEntity=null;
            foreach(var e in result)
            {
                var distance=Vector2.Distance(entity.transform.position,e.transform.position);
                if(distance<minDistance)
                {
                    minDistance=distance;
                    minEntity=e;
                }
            }
            return minEntity;
        }
        return null;
    } 
}