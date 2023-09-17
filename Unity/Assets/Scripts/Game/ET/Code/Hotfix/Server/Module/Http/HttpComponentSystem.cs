using System;
using System.Collections.Generic;
using System.Net;
using Cysharp.Threading.Tasks;

namespace ET.Server
{
    [EntitySystemOf(typeof(HttpComponent))]
    [FriendOf(typeof(HttpComponent))]
    public static partial class HttpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this HttpComponent self, string address)
        {
            try
            {
                self.Listener = new HttpListener();

                foreach (string s in address.Split(';'))
                {
                    if (s.Trim() == "")
                    {
                        continue;
                    }
                    self.Listener.Prefixes.Add(s);
                }

                self.Listener.Start();

                self.Accept().Forget();
            }
            catch (HttpListenerException e)
            {
                throw new Exception($"请先在cmd中运行: netsh http add urlacl url=http://*:你的address中的端口/ user=Everyone, address: {address}", e);
            }
        }
        
        [EntitySystem]
        private static void Destroy(this HttpComponent self)
        {
            self.Listener.Stop();
            self.Listener.Close();
        }

        private static async UniTask Accept(this HttpComponent self)
        {
            long instanceId = self.InstanceId;
            while (self.InstanceId == instanceId)
            {
                try
                {
                    HttpListenerContext context = await self.Listener.GetContextAsync();
                    self.Handle(context).Forget();
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception e)
                {
                    self.Fiber().Error(e);
                }
            }
        }

        private static async UniTask Handle(this HttpComponent self, HttpListenerContext context)
        {
            try
            {
                IHttpHandler handler = HttpDispatcher.Instance.Get(self.IScene.SceneType, context.Request.Url.AbsolutePath);
                await handler.Handle(self.Scene(), context);
            }
            catch (Exception e)
            {
                self.Fiber().Error(e);
            }
            context.Request.InputStream.Dispose();
            context.Response.OutputStream.Dispose();
        }
    }
}