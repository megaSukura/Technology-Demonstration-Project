using System.Linq;
public abstract class ProjectileLogicSub : LogicSub ,Isnapshot<ProjectileLogicSub>,Iclone<ProjectileLogicSub>
{
    public Projection proj;
    public abstract ProjectileLogicSub Clone();
    public abstract ProjectileLogicSub GetSnapshot();
}

public static class ProjectileLogicSubExtension{
        public static ProjectileLogicSub[] CloneAll(this ProjectileLogicSub[] all){
            return all.Select(x=>x.Clone()).ToArray();
        }
        public static ProjectileLogicSub[] GetSnapshotAll(this ProjectileLogicSub[] all){
            return all.Select(x=>x.GetSnapshot()).ToArray();
        }
    }