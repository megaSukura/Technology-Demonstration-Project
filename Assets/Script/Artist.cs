using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GameEntity))]
public class Artist : MonoBehaviour
{
    public GameEntity gameEntity;
    public GameObject MainTex;
    public SpriteRenderer MainTexRenderer;

    //Sprite flash
    public TimerExtend.ColdDown flashTime=new TimerExtend.ColdDown(0.5f);
    public void Flash(Color color,float time=0.1f)
    {    
        if(flashTime.coldDown(time))
        {  
            
            Timer.SetTimer(time).OnUpdate((_,tn)=>{SetMainTexMaterialColor("_AddColor",Color.Lerp(color,Color.black,tn));}).BindTo(this.gameObject);
            
        }
    }

    public void FlashRed(float time=0.1f)
    {
        Flash(Color.red,time);
    }

    //

    void Start()
    {
        if(!gameEntity) gameEntity = GetComponent<GameEntity>();
        if(!MainTex) ChildFinder.FindChild(gameEntity.gameObject, "MainTex", out MainTex);
        MainTexRenderer = MainTex.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(!MainTex) return;
        if(!gameEntity) gameEntity = GetComponent<GameEntity>();

        MainTex.transform.position = new Vector3(MainTex.transform.position.x, MainTex.transform.position.y, gameEntity.transform.position.y * 0.3f );
    }
        

    public void SetMainTex(Sprite sprite)
    {
        MainTexRenderer.sprite = sprite;
    }

    public void SetMainTexColor(Color color)
    {
        MainTexRenderer.color = color;
    }
    //
    public void SetMainTexMaterialColor(string name,Color color)
    {
        if(gameObject)
        MainTexRenderer.material.SetColor(name, color);
    }
}
