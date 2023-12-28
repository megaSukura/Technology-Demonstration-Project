using UnityEngine;
namespace SkillSystem
{
    
public abstract class SkillEffectBase 
{   
    protected Skill user;
    public virtual void Apply(Skill user){
        this.user = user;
        
    }
    public abstract float Update();

    public abstract void Stop();
}





}