
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;
public class Timer {
    #region timer池相关
    private static ObjectPool<Timer> Timers;
    private static List<Timer> ActiveTimers=new List<Timer>();
    public static bool isLog;
    #endregion
    #region timer对象内置变量
    private GameObject _bindObject; 
    public float Duration { get; private set; } = -1;
    public float PassTime{get;private set;}
    public int PassTimeSecond{get;private set;}
    public int LoopTimes{get;private set;}
    public bool Active{get;private set;}
    public bool is_finish{get;private set;}

    public bool is_Pause{get;private set;}
    private int loopTimes;//0或1不循环,-1无限循环,正数为循环次数
    private bool _ignoreTimescale;
    private bool _isPauseOnLoop=false;
    private TimerDriver driver;
    private float cachedTime;
    private float CurrentTime
        {
            get
            {
                return driver.GetCurrentTime(this._ignoreTimescale);
            }
        }

        
    #endregion
    #region timer事件
    public event Action OnEndEvent;
    public event Action<float,float> OnUpdateEvent;

    
        
    #endregion
    private static bool is_Init=false;
    private static void Initialize(){
        TimerDriver.Initialize();
        Timers=new ObjectPool<Timer>(Create,OnGet,OnRelease,null,true,100,5000);
        is_Init=true;
    }

    private static Timer Create(){
        var t=new Timer();
        t.driver=TimerDriver.Initialize();
        t._releaseWhenBindObjectDestroy_action=t._releaseWhenBindObjectDestroy;
        t._releaseWhenBindObjectActive_action=t._releaseWhenBindObjectActive;
        return t;
    }

    private static void OnGet(Timer timer){
        timer.Active=true;
        timer.is_finish=false;//
        timer.cachedTime=timer.CurrentTime;
        lock(ActiveTimers){
            ActiveTimers.Add(timer);
        }
        if(isLog)Debug.Log("ActiveTimers count:"+ActiveTimers.Count+"frame:"+Time.frameCount);
    }
    private static void OnRelease(Timer timer){
        timer._bindObject=null;
        timer._isBind=false;
        timer.Duration=0;
        timer.Active=false;
        timer.is_finish=true;//
        timer.loopTimes=0;
        timer._ignoreTimescale=false;
        timer.OnEndEvent=null;
        timer.OnUpdateEvent=null;
        timer.LoopTimes=0;
        
    }


    private void Update() {
        if(!Active){
            if(isLog)Debug.Log("Timer is not active in update"+"frame:"+Time.frameCount);
            return;
        }
        if(!is_finish&&!is_Pause){

            PassTime=CurrentTime-cachedTime;
            PassTimeSecond=Mathf.FloorToInt(PassTime);
            if(Duration<0||PassTime<=Duration){
                if(_isBind&&!_bindObject){Stop();return;}
                OnUpdateEvent?.Invoke(PassTime,Mathf.Clamp01(PassTime/Duration));
            }else{
                OnUpdateEvent?.Invoke(PassTime,1);
                LoopTimes+=1;
                if (loopTimes!=0&&loopTimes!=1)
                    {
                        OnEndEvent?.Invoke();//循环结束
                       if(!_isPauseOnLoop)
                            cachedTime = CurrentTime;
                        else{
                            cachedTime = CurrentTime;
                            Pause();
                        }
                        if(loopTimes>0)loopTimes--;
                    }
                    else
                    {
                        //Timers.Release(this);
                        Stop();
                    }
            }

        }
    }

    public Timer Pause(){
        if(!is_finish){
            is_Pause=true;
        }
        return this;
    }
    public Timer Resume(){
        if(is_Pause){
            is_Pause=false;
        }
        return this;
    }
    public void Stop(){
        if(isLog) Debug.Log("in Stop1");
        if(Active){
        if(isLog) Debug.Log("in Stop 2");
        OnEndEvent?.Invoke();//循环结束
        this.Active=false;
        Timers.Release(this);
        }
    }
    public static void ClearAll(){
        lock(ActiveTimers){
            Debug.Log("ClearAll");
            for(int i=0;i<ActiveTimers.Count;i++){
                ActiveTimers[i].Stop();
            }
            //Timers.Clear();
        }
    }
    public static void UpdateAll(){
        
       lock(ActiveTimers){
        ActiveTimers.RemoveAll((ti)=>{ return !ti.Active; });
        for(int i=0;i<ActiveTimers.Count;i++){
            var tcount = ActiveTimers.Count;
            ActiveTimers[i].Update();
            if(isLog)Debug.Log("tcount:"+tcount+" ActiveTimers.Count:"+ActiveTimers.Count);
            //结论:运行过程中会增加,但不会减少,所以不会出现越界
        }
        }
    }

#region 使用
    public static Timer SetTimer(float duration,int loopTimes=0,bool  is_PauseOnLoop=false,bool is_ignoreTimescale=false,Action onEnd=null,Action<float,float> onUpdate=null){
        if(!is_Init) Initialize();
        var timer = Timers.Get();
        timer.Duration=duration;
        timer.loopTimes=loopTimes;
        timer._isPauseOnLoop=is_PauseOnLoop;
        timer._ignoreTimescale=is_ignoreTimescale;
        if(onEnd!=null){
            timer.OnEndEvent +=onEnd;
        }
        if(onUpdate!=null){
            timer.OnUpdateEvent +=onUpdate;
        }
        return timer;
        
    }
    
    #region 常用函数
        public static void ChangeTo(float value,float time,Func<float> getter,Action<float> setter){
            var value_now = getter();
            SetTimer(time).OnUpdate((_,ti)=>{ setter( value_now+(value-value_now)*ti ) ;});
        }
        private bool _isBind=false;
        public void BindTo(GameObject go){
            _bindObject=go;
            _isBind=true;
            OnUpdateEvent+=_releaseWhenBindObjectDestroy_action;
        }
        public void BindToActive(GameObject go){
            _bindObject=go;
            _isBind=true;
            OnUpdateEvent+=_releaseWhenBindObjectActive_action;
        }
        //这里这么复杂是因为防止GC
        //1.不用lambda,lambda会导致GC
        //2.如果直接用方法,内部会将方法转换为delegate,这样会导致GC
        private Action<float,float> _releaseWhenBindObjectDestroy_action;
        private Action<float,float> _releaseWhenBindObjectActive_action;
        private void _releaseWhenBindObjectDestroy(float _,float __){
            if(!_bindObject){
                Stop();
            }
        }
        private void _releaseWhenBindObjectActive(float _,float __){
            if(!_bindObject||!_bindObject.activeSelf){
                Stop();
            }
        }
    #endregion
#endregion
}

public static class TimerExtend{
    public static Timer OnEnd(this Timer timer,Action Do){
        timer.OnEndEvent+=Do;
        return timer;
    }
    public static Timer OnUpdate(this Timer timer,Action<float,float> Do){
        timer.OnUpdateEvent+=Do;
        return timer;
    }
    [Obsolete("导致 3个 GC call")]
    public static void ChangeTo(this float value,float target_value,float time,Action<float> setter){
            var value_now = value;
            Timer.SetTimer(time).OnUpdate((_,ti)=>{ setter( value_now+(target_value-value_now)*ti ) ;});
            
        }
    // lambda表达式会导致GC，所以不用
    // public static void BindToThis(this Timer timer,GameObject go){
    //     timer.OnUpdateEvent+=(_,_)=>{if(go==null)timer.Stop();};
    // }
    // public static void BindToThisActive(this Timer timer,GameObject go){
    //     timer.OnUpdateEvent+=(_,_)=>{if(go==null||!go.activeSelf)timer.Stop();};
    // }
   
        
   public class ColdDown{
    public bool over=true;
    public float coldDownTime;

        public ColdDown(float coldDownTime)
        {
            this.coldDownTime = coldDownTime;
        }

        public float TimeRemain{get;private set;}
        public float Progress{get{return over?1:TimeRemain/coldDownTime;}}

    public bool coldDown(){
        if(over){
                Timer.SetTimer(coldDownTime).OnEnd(()=>{over=true;}).OnUpdate((ti,_)=>{TimeRemain=coldDownTime-ti;});
                over=false;
                return true;
            }else{
                return false;
            }
    }
    public bool coldDown(float time){
        if(over){
            coldDownTime=time;
                Timer.SetTimer(coldDownTime).OnEnd(()=>{over=true;}).OnUpdate((ti,_)=>{TimeRemain=coldDownTime-ti;});
                over=false;
                return true;
            }else{
                return false;
            }
    }

   }
    
}

public class TimerDriver:MonoBehaviour{

    #region 单例
        private static TimerDriver _instance;
        private static bool hasInit = false;
        private void Awake()
        {
            _instance = this;
            hasInit = true;
        }
        void OnApplicationQuit()
        {
            hasInit = false;
            _instance = null;
            _rerealtimeSinceStartup=default(float);
            _time=default(float);
            Timer.ClearAll();
        }
    #endregion
    private void Update() {
        _rerealtimeSinceStartup = UnityEngine.Time.realtimeSinceStartup;
        _time = UnityEngine.Time.time;

        Timer.UpdateAll();
        
    }
    public static TimerDriver Initialize()
        {   
            if (!Application.isPlaying)
                return null;
            if (!hasInit)
            {
                Debug.Log("TimerDriver Initializing");
                hasInit = true;
                _instance = new GameObject("TimerDriver").AddComponent<TimerDriver>();
                
                DontDestroyOnLoad(_instance.gameObject);
                _instance.gameObject.hideFlags = HideFlags.HideAndDontSave;//隐藏 但是不销毁
            }
            return _instance;
        }

    private float _rerealtimeSinceStartup, _time;
        public float GetCurrentTime(bool _ignoreTimescale)
        {
            return _ignoreTimescale ? _rerealtimeSinceStartup : _time;
        }
}