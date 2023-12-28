using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEditor;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projection : MonoBehaviour
{
    #region Pool
    private static ObjectPool<GameObject> projectionPool;
    private static ObjectPool<GameObject> ProjectionPool{
        get{
            if(projectionPool==null){
                SetPool();Debug.Log("==null");}
            return projectionPool;
        }
        
    }
    public static int PoolCount{
        get{
            return ProjectionPool.CountAll;
        }
    }
    public static int PoolActiveCount{
        get{
            return ProjectionPool.CountActive;
        }
    }

    private static void SetPool(){
        Application.quitting+=()=>{projectionPool?.Dispose();};
        projectionPool=new ObjectPool<GameObject>(CreateProjection,
    actionOnGet: (go)=>{
        //go.SetActive(true);//move to GetProjection
        go.GetComponent<Projection>().isActivated=true;
    },
    actionOnRelease: gameObject =>{
        var t=gameObject.GetComponent<Projection>();
        t.EndLogics?.Invoke(t);
        gameObject.transform.localScale=Vector3.one*0.2f;
        gameObject.transform.position=Vector3.zero;
        foreach(var i in t.effectCommands)
            EntityCommand.pool.Release(i);
        t.effectCommands.Clear();
        t.UpdateLogicSubHandler.RemoveAllLogicSub();
        t.EndLogics=null;
        t.StartLogics=null;
        t.CollisionFilter=null;
        t.isActivated=false;
        t.IsPierced=false;
        t.CollidersInPierced.Clear();
        t.LifeTime=0;
        t.DesireVelocity=Vector2.zero;
        t.speed=0;
        t.AngularVelocity=0;
        t.Radius=0.5f;
        t.sender=null;
        gameObject.SetActive(false);
    }
    ,defaultCapacity: 250);
    
    }
    #region default projection
    public static GameObject DefaultProjection { get {
        if(defaultProjection==null)
            defaultProjection=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Script/Functional Module/Projectile/Projection.prefab");
        return defaultProjection;
    } set => defaultProjection = value; }
    private static GameObject defaultProjection;//TODO:unity editor
                                                //set default projection in editor
#if UNITY_EDITOR
    [MenuItem("Tools/Projection/SetDefaultProjection")]
     public static void SetDefaultProjection(){
        string path=EditorUtility.OpenFilePanel("Select Projection Prefab","Assets/Script/Functional Module/Projectile","prefab");
        path=path.Substring(path.IndexOf("Assets"));
        Debug.Log(path);
         DefaultProjection=AssetDatabase.LoadAssetAtPath<GameObject>(path);
     }
   #endif
   [ContextMenu("LogDefaultProjection")]
    public void LogdefaultProjection(){
        Debug.Log(DefaultProjection);
    }
    #endregion
    private static GameObject CreateProjection(){
        var t=GameObject.Instantiate(DefaultProjection);
        t.hideFlags = HideFlags.HideInHierarchy;
        t.SetActive(false);
        return t;
    }
    #endregion
    #region Get
        public static GameObject GetProjection(){
            return ProjectionPool.Get();
        }
        public static GameObject GetProjection(ProjectionConfig config){
            
            var t=ProjectionPool.Get();
            //Debug.Log(t);
            t.GetComponent<Projection>().SetProjectionConfig(config);
            t.SetActive(true);
            return t;
        }
        // public static GameObject GetProjection(ProjectileSObase projectileScriptableObject){
        //     var t=ProjectionPool.Get();
        //     var e=t.GetComponent<Projection>();
        //     projectileScriptableObject.SetBaseInfo(e);
        //     projectileScriptableObject.SetHitInfo(e);
        //     projectileScriptableObject.SetFilterInfo(e);
        //     projectileScriptableObject.SetLogicInfo(e);
        //     t.SetActive(true);
        //     return t;
        // }
        public static Projection GetProjection(GameEntity sender,ProjectileSObase projectileScriptableObject){
            var t=ProjectionPool.Get();
            var e=t.GetComponent<Projection>();
            e.SignSender(sender);
            projectileScriptableObject.SetBaseInfo(e);
            projectileScriptableObject.SetHitInfo(e);
            projectileScriptableObject.SetFilterInfo(e);
            projectileScriptableObject.SetLogicInfo(e);
            t.SetActive(true);
            return e;
        }
        public static Projection GetProjection(GameEntity sender,ProjectileSObase projectileScriptableObject,Vector2 position,Vector2 velocity){
            var t=ProjectionPool.Get();
            var e=t.GetComponent<Projection>();
            e.SignSender(sender);
            projectileScriptableObject.SetBaseInfo(e);
            projectileScriptableObject.SetHitInfo(e);
            projectileScriptableObject.SetFilterInfo(e);
            projectileScriptableObject.SetLogicInfo(e);
            t.transform.position=position;
            e.DesireVelocity=velocity;
            t.SetActive(true);
            return e;
        }
    
    #endregion
    
    #region reference
    [HideInInspector]
    public CircleCollider2D ProjectionCollider{get;private set;}
    [HideInInspector]
    public Rigidbody2D ProjectionRigidbody{get;private set;}
    #endregion
    #region Effect(command)
    private List<EntityCommand> effectCommands=new List<EntityCommand>();
    public void AddEffectCommand(EntityCommand command){
        effectCommands.Add(command);
    }
    public void AddEffectCommands(EntityCommand[] commands){
        effectCommands.AddRange(commands);
    }
    private void ExecuteEffectCommands(GameEntity target){
        foreach(EntityCommand command in effectCommands){
            command.Target=target;
            target.SendCommand(command);
        }
        // effectCommands.Clear();
    }
    #endregion
    #region Collision entity Filter
    private Func<Collider2D,bool> CollisionFilter;

    public GameEntity sender{get;private set;}
    public void SignSender(GameEntity sender){
        this.sender=sender;
    }
    public void SetCollisionFilter(Func<Collider2D,bool> filter){
        CollisionFilter=filter;
    }
    #endregion
    #region ProjectionCofig

    public bool IsPierced=false;
    private List<Collider2D> CollidersInPierced=new List<Collider2D>();
    public float LifeTime=0f;//0 means infinite
    public Vector2 DesireVelocity;
    public float speed;
    public float SpeedMultiplier=1;
    public Projection SetVelocity(Vector2 velocity){
        ProjectionRigidbody.velocity=velocity*SpeedMultiplier;
        return this;
    }
    public Projection SetPosition(Vector2 position){
        transform.position=position;
        return this;
    }
    public float AngularVelocity;
    public float Radius{
        get{
            return ProjectionCollider.radius;
        }
        set{
            if (value<0)
                value=0;
            ProjectionCollider.radius=value;
        }
    }


    public void SetProjectionConfig(ProjectionConfig config){
        transform.position=config.position;
        transform.rotation=Quaternion.Euler(0,0,config.rotation);
        if(config.scale.x!=0&&config.scale.y!=0)
            transform.localScale=new Vector3(config.scale.x,config.scale.y,1);
        IsPierced=config.isPierced;
        LifeTime=config.lifeTime;
        if(config.radius!=0)
            Radius=config.radius;
        DesireVelocity=config.velocity;
        speed=DesireVelocity.magnitude;
        AngularVelocity=config.angularVelocity;
        if(sender==null&&config.sender!=null)
            sender=config.sender;
        if(config.effectCommands!=null&&config.effectCommands.Length>0)
             AddEffectCommands(config.effectCommands);
        if(config.CollisionFilter!=null&&config.CollisionFilter!=default)
            SetCollisionFilter(config.CollisionFilter);
    }
    
    #endregion
    #region Logic
    /// <summary>
    /// call when every time enabled
    /// </summary>
    public event Action<Projection> StartLogics;
    /// <summary>
    /// call every frame
    /// </summary>
    public LogicSubhandler UpdateLogicSubHandler{get;private set;}
    public void AddUpdateLogicSub(ProjectileLogicSub sub){
        sub.proj=this;
        UpdateLogicSubHandler.AddLogicSub(sub);
    }
    /// <summary>
    /// 克隆数组中的子逻辑，然后添加到逻辑列表中
    /// </summary>
    public void AddUpdateLogicSub(ProjectileLogicSub[] subs){
        foreach(ProjectileLogicSub sub in subs){
            var clone_sub=sub.Clone();
            clone_sub.proj=this;
            UpdateLogicSubHandler.AddLogicSub(clone_sub);
        }
    }
    /// <summary>
    /// call when releasing to pool
    /// </summary>
    public event Action<Projection> EndLogics;
    #endregion
    public bool isActivated{get;private set;}
    private static bool isInited=false;
    private void Awake() {
        if(!isInited){
            SetPool();
            isInited=true;
        }
        //getReference
        ProjectionCollider=GetComponent<CircleCollider2D>();
        ProjectionRigidbody=GetComponent<Rigidbody2D>();
        //set default value
        UpdateLogicSubHandler = new LogicSubhandler();
        _timer_OnEnd_action=_timer_OnEnd;
    }
    private void _timer_OnEnd()
    {   
        if(gameObject.activeSelf)
            ProjectionPool.Release(gameObject);
    }
    private Action _timer_OnEnd_action;
    void OnEnable()
    {
        if(LifeTime>0f){
            //Debug.Log("projec timer set");
            Timer.SetTimer(LifeTime).OnEnd(_timer_OnEnd_action).BindToActive(gameObject);
        }
        if(DesireVelocity!=Vector2.zero){
            ProjectionRigidbody.velocity=DesireVelocity;
        }
        if(AngularVelocity!=0f){
            ProjectionRigidbody.angularVelocity=AngularVelocity;
        }
        StartLogics?.Invoke(this);
    }
    void Update()
    {
        if (!isActivated)
            return;
        UpdateLogicSubHandler.UpdateLogicSubs();
    }
    void OnTriggerEnter2D(Collider2D other)
    {   
        if (!isActivated)
            return;
            
        if(other.gameObject.CompareTag("Entity") ){
            
            if( CollisionFilter==null || CollisionFilter(other) ){
                GameEntity entity=other.gameObject.GetComponent<GameEntity>();
                if(sender!=null&&entity!=null&&entity==sender)
                    return;
                if(entity!=null){
                    
                    if (!IsPierced)
                    {   
                        ExecuteEffectCommands(entity);
                        isActivated=false;
                        
                    }
                    else
                    {
                        if(CollidersInPierced.Exists(c=>c==other))
                            return;
                        ExecuteEffectCommands(entity);
                        CollidersInPierced.Add(other);
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isActivated)
            return;
        if(other.gameObject.CompareTag("Entity") ){
            if(IsPierced){
                if(CollidersInPierced.Exists(c=>c==other))
                    CollidersInPierced.Remove(other);
            }
        }
    }
    void LateUpdate()
    {
        if(!isActivated){
            ProjectionPool.Release(gameObject);
        }
    }
    [Serializable]
    public struct ProjectionConfig
    {
        public Vector2 position;
        public Vector2 velocity;
        public float rotation;
        public float angularVelocity;
        public float lifeTime;
        public float radius;
        public Vector2 scale;
        public bool isPierced;
        public GameEntity sender;
        public EntityCommand[] effectCommands;
        public Func<Collider2D,bool> CollisionFilter;

        public ProjectionConfig(Vector2 position=default, Vector2 velocity=default,Vector2 scale=default,float lifeTime=0.1f,float radius=0.2f,GameEntity sender=null,bool _isPierced=false,float rotation=0f, float angularVelocity=0f,  EntityCommand[] effectCommands=null, Func<Collider2D, bool> collisionFilter=null)
        {
            this.position = position;
            this.velocity = velocity;
            this.rotation = rotation;
            this.lifeTime = lifeTime;
            this.sender = sender;
            this.angularVelocity = angularVelocity;
            this.scale = scale!=Vector2.zero?scale:0.5f*Vector2.one;
            this.isPierced = _isPierced;
            this.effectCommands = effectCommands;
            this.radius = radius;
            CollisionFilter = collisionFilter;
        }
    }

}
