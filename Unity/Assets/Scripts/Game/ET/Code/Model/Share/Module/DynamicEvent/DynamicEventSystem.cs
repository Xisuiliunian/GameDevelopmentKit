using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ET
{
    public sealed partial class DynamicEventSystem : Singleton<DynamicEventSystem>, ISingletonAwake
    {
        private readonly Dictionary<Type, HashSet<EntityRef<Entity>>> registeredEntityDict = new Dictionary<Type, HashSet<EntityRef<Entity>>>();
        private readonly HashSet<EntityRef<Entity>> needRemoveEntities = new HashSet<EntityRef<Entity>>();

        public void Awake()
        {
            
        }

        public void Update()
        {
            if (this.needRemoveEntities.Count < 1)
            {
                return;
            }
            foreach (Entity entity in this.needRemoveEntities)
            {
                if (this.registeredEntityDict.TryGetValue(entity.GetType(), out var entityRefs))
                {
                    entityRefs.Remove(entity);
                    break;
                }
            }
            this.needRemoveEntities.Clear();
        }

        public void RegisterEntity(Entity entity)
        {
            Type entityType = entity.GetType();
            if (!this.registeredEntityDict.TryGetValue(entityType, out var entityRefs))
            {
                entityRefs = new HashSet<EntityRef<Entity>>();
                this.registeredEntityDict.Add(entityType, entityRefs);
            }
            entityRefs.Add(entity);
        }

        public void UnRegisterEntity(Entity entity)
        {
            this.needRemoveEntities.Add(entity);
        }

        public void Publish<A>(Scene scene, A arg) where A : struct
        {
            SceneType domainSceneType = scene.SceneType;
            Type argType = typeof(A);
            if (DynamicEventTypeSystem.Instance.AllEventInfos.TryGetValue(argType, out List<DynamicEventInfo> dynamicEventInfos))
            {
                foreach (DynamicEventInfo dynamicEventInfo in dynamicEventInfos)
                {
                    if (!domainSceneType.HasSameFlag(dynamicEventInfo.SceneType))
                    {
                        continue;
                    }
                    IDynamicEvent<A> dynamicEvent = (IDynamicEvent<A>)dynamicEventInfo.DynamicEvent;
                    if (this.registeredEntityDict.TryGetValue(dynamicEvent.EntityType, out var entityRefs))
                    {
                        foreach (Entity entity in entityRefs)
                        {
                            if (entity is { IsDisposed: false })
                            {
                                dynamicEvent.Handle(scene, entity, arg).Forget();
                            }
                        }
                    }
                }
            }
        }

        public async UniTask PublishAsync<A>(Scene scene, A arg) where A : struct
        {
            using ListComponent<UniTask> taskList = ListComponent<UniTask>.Create();
            SceneType domainSceneType = scene.SceneType;
            Type argType = typeof(A);
            if (DynamicEventTypeSystem.Instance.AllEventInfos.TryGetValue(argType, out List<DynamicEventInfo> dynamicEventInfos))
            {
                foreach (DynamicEventInfo dynamicEventInfo in dynamicEventInfos)
                {
                    if (!domainSceneType.HasSameFlag(dynamicEventInfo.SceneType))
                    {
                        continue;
                    }
                    IDynamicEvent<A> dynamicEvent = (IDynamicEvent<A>)dynamicEventInfo.DynamicEvent;
                    if (this.registeredEntityDict.TryGetValue(dynamicEvent.EntityType, out var entityRefs))
                    {
                        foreach (Entity entity in entityRefs)
                        {
                            if (entity is { IsDisposed: false })
                            {
                                taskList.Add(dynamicEvent.Handle(scene, entity, arg));
                            }
                        }
                    }
                }
            }

            try
            {
                await UniTask.WhenAll(taskList);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}