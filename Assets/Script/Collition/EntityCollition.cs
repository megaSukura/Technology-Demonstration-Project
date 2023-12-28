using System;
using System.Collections.Generic;
using UnityEngine;

public static class EntityCollition 
{   
    public static LayerMask DefaultLayerMask=LayerMask.GetMask("Entity");
    public static Color DefaultDebugColor = Color.red;
    public static int FanshapeCollition(this GameEntity entity,out List<GameEntity> result ,Vector2 center,float radius,float angle,Vector2 direction,LayerMask layerMask){
        
        var collitionList=Physics2D.OverlapCircleAll(center,radius,layerMask);//GC
        if(DefaultDebugColor==default){Debug.DrawLine(center,center+direction*radius,DefaultDebugColor);
        Vector2 d_dir1=Quaternion.Euler(0,0,angle/2)*direction;
        Vector2 d_dir2=Quaternion.Euler(0,0,-angle/2)*direction;
        Debug.DrawLine(center,center+d_dir1*radius,DefaultDebugColor);
        Debug.DrawLine(center,center+d_dir2*radius,DefaultDebugColor);
        }

        if(collitionList.Length==0)
        {   result=null;
            return 0;
            }
        
        var angleStart=-angle/2;
        var angleEnd=angle/2;
        
         result=new List<GameEntity>();
        foreach (var collition in collitionList)
        {
            var entityCollition = collition.GetComponent<GameEntity>();
            if (entityCollition != null&&entityCollition!=entity)
            {
                
                    
                    var entityPos = entityCollition.transform.position;
                    var entityDir = (Vector2)entityPos - center;
                    var angleEntity = Vector2.SignedAngle(direction, entityDir);
                    if (angleEntity >= angleStart && angleEntity <= angleEnd)
                    {
                        result.Add(entityCollition);
                    }
                
            }
        }
        return result.Count;
    }
    
    public static void FanshapeCollition(this GameEntity entity,Vector2 center,float radius,float angle,Vector2 direction,LayerMask layerMask,EntityCollitionCallback callback){
        List<GameEntity> result;
        var count=entity.FanshapeCollition(out result,center,radius,angle,direction,layerMask);
        if(count>0)
        {
            callback(result);
        }
    }

    public static void FanshapeDamage(this GameEntity entity,Vector2 center,float radius,float angle,Vector2 direction,LayerMask layerMask,float damage){
        entity.FanshapeCollition(center,radius,angle,direction,layerMask,(result)=>{
            foreach (var item in result)
            {
                item.Damage(damage,entity);
            }
        });
    }
        public static void FanshapeDamage(this GameEntity entity,Vector2 center,float radius,float angle,Vector2 direction,LayerMask layerMask,float damage,float knockbackForce){
        entity.FanshapeCollition(center,radius,angle,direction,layerMask,(result)=>{
            foreach (var item in result)
            {   
                item.entityRigidbody.AddForce(((Vector2)item.transform.position-center).normalized*knockbackForce);
                item.Damage(damage,entity);
            }
        });
    }
    
    public static void FanshapeDamage(this GameEntity entity ,float radius,float angle,Vector2 direction,LayerMask layerMask,float damage){
        entity.FanshapeDamage(entity.transform.position,radius,angle,direction,layerMask,damage);
    }
    public delegate void EntityCollitionCallback(List<GameEntity> result);
}
