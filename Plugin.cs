using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using UnityEngine.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System;
using static AbilityApi.Api;
using UnityEngine.SceneManagement;
using AbilityApi;

namespace MyFirstCustomAbility
{
    [BepInPlugin("com.David_Loves_JellyCar_Worlds.MyFirstCustomAbility", "MyFirstCustomAbility", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Plugin MyFirstCustomAbility is loaded!");

            Harmony harmony = new Harmony("com.David_Loves_JellyCar_Worlds.MyFirstCustomAbility");

            Logger.LogInfo("harmany created");
            harmony.PatchAll();
            Logger.LogInfo("MyFirstCustomAbility Patch Compleate!");
            var directoryToModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //this name will automaticly be renamed to whatever the ability name is. 
            //currently only instant abilitys are supported but normal abilitys will be supported too.
            var testAbilityPrefab = Api.ConstructInstantAbility<FirstInstantAbility>("My Custom Ability");
            var testAbilityTex = Api.LoadImage(Path.Combine(directoryToModFolder, "BlinkTest.png"));
            var testSprite = Sprite.Create(testAbilityTex, new Rect(0f, 0f, testAbilityTex.width, testAbilityTex.height), new Vector2(0.5f, 0.5f));
            //dont use the same name multiple times or it will break stuff
            NamedSprite test = new NamedSprite("My Custom Ability", testSprite, testAbilityPrefab.gameObject, true);
            Api.RegisterNamedSprites(test, true);
        }
    }
    public class FirstInstantAbility : MonoUpdatable
    {
        public void Awake()
        {
            Updater.RegisterUpdatable(this);
            //when calling Api.ConstructInstantAbility<FirstInstantAbility> it created a gameobject and added the InstantAbility MonoBehaviour and the 
            //FirstInstantAbility MonoBehaviour.
            InstantAbility ab = GetComponent<InstantAbility>();
            if (ab.funcOnEnter == null)
            {
                ab.funcOnEnter = new();
            }
            var func = ab.funcOnEnter;

            func.AddListener(CastAbility);
        }

        public void CastAbility()
        {
            AudioManager.Get().Play("startGame");
        }

        public override void Init()
        {

        }

        public override void UpdateSim(Fix SimDeltaTime)
        {

        }
    }
}