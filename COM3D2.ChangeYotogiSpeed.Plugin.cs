using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using BepInEx;

using UnityEngine;
using System.Collections;

using Yotogis;

namespace COM3D2.ChangeYotogiSpeed.Plugin
{
    [BepInPlugin("COM3D2.ChangeYotogiSpeed.Plugin", "Change Yotogi Speed", "0.0.1.0")]
    public class ChangeYotogiSpeed : BaseUnityPlugin
    {
        public Maid maid;
        public Animation anm_BO_body001;
        public Animation[] anm_BO_mbody;

        public bool isChange = false;
        public float speed = 1.5f;
        public int manCount = 1;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            //UnityEngine.Debug.Log("Hello, world!");
            Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin loaded, turning on by keyboard ' space ' (only works in scene with maid within)");
            Update();
        }

        public void Start()
        {
            ///get maid0 animation
            this.maid = GameMain.Instance.CharacterMgr.GetMaid(0);
            if (this.maid == null) { Console.WriteLine("this.maid = null"); }

            this.anm_BO_body001 = this.maid.body0.GetAnimation();
            if (this.anm_BO_body001 == null) { Console.WriteLine("this.anm_BO_body001 = null"); }
            ///


            /*
            try
            {
                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (object go in allObjects)
                {
                    Console.WriteLine(go);
                }
                Thread.Sleep(5000);  
            }
            catch { Console.WriteLine("get go.name went wrong"); }
            */


            ///change animation speed
            foreach (AnimationState stat in anm_BO_body001)
            {
                if (stat == null) 
                {
                    Console.WriteLine(stat + " = null");
                    continue; 
                }
                stat.speed = this.speed;
            }


            for ( int i = 0; i < this.manCount; i++ )
            {
                try
                {
                    // don't know how to get a instance of a man, so I directly get man's animation then change the speed of it
                    foreach (AnimationState stat in GameMain.Instance.CharacterMgr.GetMan(i).body0.GetAnimation())
                    {
                        if (stat == null)
                        {
                            Console.WriteLine(stat + " = null");
                            continue;
                        }
                        stat.speed = this.speed;
                    }
                }
                catch
                {
                    this.manCount = i;   // once get break, i is the correct number of man 
                    break;
                }
            }
            ///

        }

        public void Update()
        {
            ///spy on input, open close by keyboard "space", change speed variable by keyboard "<", ">"
            try
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Console.WriteLine("space");
                    if (this.isChange == false)
                    {
                        this.isChange = true;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin turned on, change speed variable by keyboard ' < ', ' > '");
                        this.manCount = 6; //game always stock 6 man in yotogi scene, need to reopen plugin to reset the count of man
                    }
                    else
                    {
                        this.isChange = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin turned off");
                        this.speed = 1f;
                        this.Start();
                    }
                }

                if (this.isChange == true)
                {
                    this.Start();

                    if (Input.GetKeyDown(KeyCode.Comma))
                    {
                        //Console.WriteLine("Comma");
                        float tmp = this.speed - 0.2f;
                        //Console.WriteLine(tmp);
                        if (tmp <= 4.0f && tmp >= 0.12f)
                        {
                            this.speed = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        //Console.WriteLine("Period");
                        float tmp = this.speed + 0.2f;
                        //Console.WriteLine(tmp);
                        if (tmp <= 4.0f && tmp >= 0.12f)
                        {
                            this.speed = tmp;
                        }
                    }
                }
            
            }
            catch
            {
                this.isChange = false;
            }
            ///
            
        }

    }

}