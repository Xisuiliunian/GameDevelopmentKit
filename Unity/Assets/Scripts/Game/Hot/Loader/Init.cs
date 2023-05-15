using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    [DisallowMultipleComponent]
    public class Init : MonoBehaviour
    {
        public static Init Instance
        {
            get;
            private set;
        }

        private class Runner : MonoBehaviour
        {
            private void Update()
            {
                GameHot.Update();
            }

            private void LateUpdate()
            {
                GameHot.LateUpdate();
            }

            private void OnDestroy()
            {
                GameHot.OnApplicationQuit();
            }

            private void OnApplicationPause(bool pauseStatus)
            {
                GameHot.OnApplicationPause(pauseStatus);
            }

            private void OnApplicationFocus(bool hasFocus)
            {
                GameHot.OnApplicationFocus(hasFocus);
            }
        }

        private Runner m_RunnerComponent;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartAsync().Forget();
        }

        private void OnDestroy()
        {
            if (this.m_RunnerComponent != null)
            {
                DestroyImmediate(this.m_RunnerComponent);
            }
        }

        private async UniTaskVoid StartAsync()
        {
            Assembly assembly = null;
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };
            if (Define.EnableHotfix)
            {
                byte[] assBytes = await LoadCodeBytesAsync("Game.Hot.Code.dll");
                byte[] pdbBytes = await LoadCodeBytesAsync("Game.Hot.Code.pdb");
                assembly = Assembly.Load(assBytes, pdbBytes);
            }
            else
            {
                Assembly[] assemblies = Utility.Assembly.GetAssemblies();
                foreach (Assembly ass in assemblies)
                {
                    string name = ass.GetName().Name;
                    if (name == "Game.Hot.Code")
                    {
                        assembly = ass;
                        break;
                    }
                }
            }
            
            MethodInfo methodInfo = assembly.GetType("Game.Hot.Entry").GetMethod("Start");
            methodInfo.Invoke(null, null);
            
            this.m_RunnerComponent = this.gameObject.AddComponent<Runner>();
        }
        
        private async UniTask<byte[]> LoadCodeBytesAsync(string fileName)
        {
            fileName = AssetUtility.GetGameHotAsset(Utility.Text.Format("Code/{0}.bytes", fileName));
            TextAsset textAsset = await GameEntry.Resource.LoadAssetAsync<TextAsset>(fileName);
            byte[] bytes = textAsset.bytes;
            GameEntry.Resource.UnloadAsset(textAsset);
            return bytes;
        }
    }
}