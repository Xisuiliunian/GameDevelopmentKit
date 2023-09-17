using System;
using Cysharp.Threading.Tasks;

namespace ET.Client
{
    public static partial class EnterMapHelper
    {
        public static async UniTask EnterMapAsync(Scene root)
        {
            try
            {
                G2C_EnterMap g2CEnterMap = await root.GetComponent<ClientSenderCompnent>().Call(new C2G_EnterMap()) as G2C_EnterMap;

                // 等待场景切换完成
                await root.GetComponent<ObjectWait>().Wait<Wait_SceneChangeFinish>();

                EventSystem.Instance.Publish(root, new EnterMapFinish());
            }
            catch (Exception e)
            {
                root.Fiber.Error(e);
            }
        }

        public static async UniTask Match(Fiber fiber)
        {
            try
            {
                G2C_Match g2CEnterMap = await fiber.Root.GetComponent<ClientSenderCompnent>().Call(new C2G_Match()) as G2C_Match;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}