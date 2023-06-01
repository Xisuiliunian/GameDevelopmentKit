using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ET.Server
{
    public static class RobotCaseSystem
    {
        // 创建机器人，生命周期是RobotCase
        public static async UniTask NewRobot(this RobotCase self, int count, List<Scene> scenes)
        {
            UniTask[] tasks = new UniTask[count];
            for (int i = 0; i < count; ++i)
            {
                tasks[i] = self.NewRobot(scenes);
            }

            await ETTaskHelper.WaitAll(tasks);
        }

        private static async UniTask NewRobot(this RobotCase self, List<Scene> scenes)
        {
            try
            {
                scenes.Add(await self.NewRobot());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        // 创建机器人，生命周期是RobotCase
        public static async UniTask NewZoneRobot(this RobotCase self, int zone, int count, List<Scene> scenes)
        {
            UniTask[] tasks = new UniTask[count];
            for (int i = 0; i < count; ++i)
            {
                tasks[i] = self.NewZoneRobot(zone + i, scenes);
            }

            await UniTask.WhenAll(tasks);
        }

        // 这个方法创建的是进程所属的机器人，建议使用RobotCase.NewRobot来创建
        private static async UniTaskTask NewZoneRobot(this RobotCase self, int zone, List<Scene> scenes)
        {
            try
            {
                scenes.Add(await self.NewRobot(zone));
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async UniTask<Scene> NewRobot(this RobotCase self, int zone)
        {
            return await self.NewRobot(zone, $"Robot_{zone}");
        }

        public static async UniTask<Scene> NewRobot(this RobotCase self, int zone, string name)
        {
            Scene clientScene = null;
            try
            {
                clientScene = await Client.SceneFactory.CreateClientScene(zone, name);
                await Client.LoginHelper.Login(clientScene, zone.ToString(), zone.ToString());
                await Client.EnterMapHelper.EnterMapAsync(clientScene);
                Log.Debug($"create robot ok: {zone}");
                return clientScene;
            }
            catch (Exception e)
            {
                clientScene?.Dispose();
                throw new Exception($"RobotCase create robot fail, zone: {zone}", e);
            }
        }

        private static async UniTask<Scene> NewRobot(this RobotCase self)
        {
            int zone = self.GetParent<RobotCaseComponent>().GetN();
            Scene clientScene = null;

            try
            {
                clientScene = await Client.SceneFactory.CreateClientScene(zone, $"Robot_{zone}");
                await Client.LoginHelper.Login(clientScene, zone.ToString(), zone.ToString());
                await Client.EnterMapHelper.EnterMapAsync(clientScene);
                Log.Debug($"create robot ok: {zone}");
                return clientScene;
            }
            catch (Exception e)
            {
                clientScene?.Dispose();
                throw new Exception($"RobotCase create robot fail, zone: {zone}", e);
            }
        }
    }
}