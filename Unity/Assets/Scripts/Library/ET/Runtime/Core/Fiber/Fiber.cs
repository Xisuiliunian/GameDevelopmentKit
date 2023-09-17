﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ET
{
    public static class FiberHelper
    {
        public static ActorId GetActorId(this Entity self)
        {
            Fiber root = self.Fiber();
            return new ActorId(root.Process, root.Id, self.InstanceId);
        }
    }
    
    public class Fiber: IDisposable
    {
        public bool IsDisposed;
        
        public int Id;

        public int Zone;

        public Scene Root { get; }

        public Address Address
        {
            get
            {
                return new Address(this.Process, this.Id);
            }
        }

        public int Process
        {
            get
            {
                return Options.Instance.Process;
            }
        }

        public EntitySystem EntitySystem { get; }
        public Mailboxes Mailboxes { get; private set; }
        public ThreadSynchronizationContext ThreadSynchronizationContext { get; }
        public ILog Log { get; }
        
        private EntityRef<TimerComponent> timerCompnent;
        public TimerComponent TimerComponent
        {
            get
            {
                return this.timerCompnent;
            }
            set
            {
                this.timerCompnent = value;
            }
        }
        
        private EntityRef<CoroutineLockComponent> coroutineLockComponent;
        public CoroutineLockComponent CoroutineLockComponent
        {
            get
            {
                return this.coroutineLockComponent;
            }
            set
            {
                this.coroutineLockComponent = value;
            }
        }
        
        private EntityRef<ProcessInnerSender> processInnerSender;
        public ProcessInnerSender ProcessInnerSender
        {
            get
            {
                return this.processInnerSender;
            }
            set
            {
                this.processInnerSender = value;
            }
        }

        private readonly Queue<AutoResetUniTaskCompletionSource> frameFinishTasks = new();
        
        internal Fiber(int id, int zone, SceneType sceneType, string name)
        {
            this.Id = id;
            this.Zone = zone;
            this.EntitySystem = new EntitySystem();
            this.Mailboxes = new Mailboxes();
            this.ThreadSynchronizationContext = new ThreadSynchronizationContext();
#if DOTNET
            this.Log = new NLogger(sceneType.ToString(), this.Process, this.Id, "../Config/NLog/NLog.config");
#else
            this.Log = Logger.Instance.Log;
#endif
            this.Root = new Scene(this, id, 1, sceneType, name);
        }

        internal void Update()
        {
            try
            {
                this.EntitySystem.Update();
            }
            catch (Exception e)
            {
                this.Log.Error(e);
            }
        }
        
        internal void LateUpdate()
        {
            try
            {
                this.EntitySystem.LateUpdate();
                FrameFinishUpdate();
                
                this.ThreadSynchronizationContext.Update();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public async UniTask WaitFrameFinish()
        {
            AutoResetUniTaskCompletionSource task = AutoResetUniTaskCompletionSource.Create();
            this.frameFinishTasks.Enqueue(task);
            await task.Task;
        }

        private void FrameFinishUpdate()
        {
            while (this.frameFinishTasks.Count > 0)
            {
                AutoResetUniTaskCompletionSource task = this.frameFinishTasks.Dequeue();
                task.TrySetResult();
            }
        }

        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            this.IsDisposed = true;
            
            this.Root.Dispose();
        }
    }
}