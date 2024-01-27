using System.Collections.Generic;
using System.Linq;
using Game;

namespace ET.Client
{
    /// <summary>
    /// 管理Scene上的Entity
    /// </summary>
    [ComponentOf]
    [EnableMethod]
    public class UGFEntityComponent : Entity, IAwake, IDestroy
    {
        //所有显示的Entity实体
        public readonly HashSet<EntityRef<UGFEntity>> AllShowEntities = new HashSet<EntityRef<UGFEntity>>();

        public override void Dispose()
        {
            foreach (UGFEntity entity in AllShowEntities.ToArray())
            {
                if(entity == null)
                    continue;
                GameEntry.Entity.HideEntity(entity.Entity);
            }
            AllShowEntities.Clear();
            base.Dispose();
        }
    }
}