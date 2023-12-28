using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 单个buff的类
/// </summary>
public class Buff<T>
{
    public string Name { get; set; }
    public float Duration { get; set; }
    public BuffEffect<T> Effect { get; set; }
}
/// <summary>
/// buff的效果类,需要继承这个类,并实现里面的方法
/// </summary>
public abstract class BuffEffect<T>
{
    public string Name { get; protected set; }
    public abstract void Apply(Buff<T> buff, T target);
    public abstract void Remove(Buff<T> buff, T target);
    public abstract void Update(Buff<T> buff, T target, float timePassed, float percentage);
    public abstract void OnStack(Buff<T> buff, T target, Buff<T> newBuff,List<Buff<T>> buffs,BuffManager<T> buffManager=null);
}
/// <summary>
/// buff管理类
/// </summary>
public class BuffManager<T>
{
    private List<Buff<T>> _buffs = new List<Buff<T>>();

    public void AddBuff(Buff<T> buff, T target)
    {
        Buff<T> existingBuff = _buffs.Find(b => b.Name == buff.Name);
        if (existingBuff != null)
        {
            existingBuff.Effect.OnStack(existingBuff, target, buff, _buffs, this);
        }
        else
        {
            _buffs.Add(buff);
            buff.Effect.Apply(buff, target);

            Timer timer = Timer.SetTimer(buff.Duration, 1);
            timer.OnEnd(() =>
            {
                buff.Effect.Remove(buff, target);
                _buffs.Remove(buff);
            });
            timer.OnUpdate((timePassed, percentage) =>
            {
                buff.Effect.Update(buff, target, timePassed, percentage);
            });
        }
    }

    public void RemoveBuff(Buff<T> buff, T target)
    {
        buff.Effect.Remove(buff, target);
        _buffs.Remove(buff);
    }
}
//*********************************************************************************************************************
/// <summary>
/// 带层数机制的buff的效果类
/// </summary>
public abstract class LayerBuffEffect<T> : BuffEffect<T>
{
    private int _layer;
    private List<LayerBuffEffect<T>> _layerBuffEffects = new List<LayerBuffEffect<T>>();
    private Action<int> _secondaryEffect;

    public LayerBuffEffect(int layer, Action<int> secondaryEffect)
    {
        _layer = layer;
        _secondaryEffect = secondaryEffect;
    }

    
        // 要求主效果的实现


    
        // 要求移除主效果的实现
    

    public override void Update(Buff<T> buff, T target, float timePassed, float percentage)
    {
        // 更新主效果的实现
        foreach (var layerBuffEffect in _layerBuffEffects)
        {
            _secondaryEffect?.Invoke(layerBuffEffect._layer);
        }
    }

    public override void OnStack(Buff<T> buff, T target, Buff<T> newbuff,List<Buff<T>> buffs,BuffManager<T> buffManager=null)
    {
        // 堆叠策略的实现：将层数相加
        if (newbuff.Effect is LayerBuffEffect<T> layerBuffEffect)
        {
            _layer += layerBuffEffect._layer;
            _layerBuffEffects.Add(layerBuffEffect);
            //刷新buff!!!(错误的做法)
            buff.Effect.Remove(buff, target);
            buff.Effect.Apply(buff, target);
        }
    }
}