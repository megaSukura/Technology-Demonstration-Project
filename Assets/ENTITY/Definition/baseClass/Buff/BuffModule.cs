using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffModule : BuffManager<EntityCore>, IModule
{
    public EntityCore core { get ; set; }

    public BuffModule(EntityCore core)
    {
        this.core = core;
    }
    public void AddBuff(Buff<EntityCore> buff)
    {
        base.AddBuff(buff, core);
    }
}
