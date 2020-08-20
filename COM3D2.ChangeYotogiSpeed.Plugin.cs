using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using BepInEx;

using UnityEngine;
using System.Collections;


namespace COM3D2.ChangeYotogiSpeed.Plugin
{
    [BepInPlugin("COM3D2.ChangeYotogiSpeed.Plugin", "Change Yotogi Speed", "0.0.2.2")]
    public class ChangeYotogiSpeed : BaseUnityPlugin
    {
        public Maid maid;
        public Animation anm_BO_body001;

        public bool isMode1 = false;
        public bool isMode2 = false;
        public float speed = 1.4f;
        public float speedUpTo = 2f;
        public float speedDownTo = 0.2f;
        public float abs = 0.12f;
        public int manCount = 1;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            //UnityEngine.Debug.Log("Hello, world!");
            Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin loaded, turning on by keyboard  ;  or  ' (single quote) (only works in scene with maid within)");
            Update();
        }


        public void StartMode1()
        {
            ///get maid0 animation
            this.maid = GameMain.Instance.CharacterMgr.GetMaid(0);
            if (this.maid == null) { Console.WriteLine("this.maid = null"); }
            
            this.anm_BO_body001 = this.maid.body0.GetAnimation();
            if (this.anm_BO_body001 == null) { Console.WriteLine("this.anm_BO_body001 = null"); }
            ///
            
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
            
            for ( int i = 0; i < this.manCount; i++)
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


        public void StartMode2()
        {
            var rand = new System.Random();

            ///get random speed change
            float rUpTo;
            float rDownTo;
            if (this.speed + this.abs > this.speedUpTo)
            {
                rUpTo = this.speedUpTo;
            }
            else
            {
                rUpTo = this.speed + this.abs;
            }
            if (this.speed - this.abs < this.speedDownTo)
            {
                rDownTo = this.speedDownTo;
            }
            else
            {
                rDownTo = this.speed - this.abs;
            }

            float tmp = (float)rand.Next(
                (int)(rDownTo * 1000000), (int)(rUpTo * 1000000)) / 1000000;

            this.speed = tmp;
            ///

            StartMode1();
        }


        public void Update()
        {
            try
            {
                ///mode1, spy on input, open close by keyboard "Semicolon", change speed variable by keyboard "<", ">"
                if (Input.GetKeyDown(KeyCode.Semicolon))
                {
                    //Console.WriteLine("Semicolon");
                    if (this.isMode1 == false)
                    {
                        this.isMode1 = true;
                        this.isMode2 = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin mode1(constant mode) turned on, change speed variable by keyboard  <  > ");
                        this.manCount = 6; //game always stock 6 man in yotogi scene, need to reopen plugin to reset the count of man
                    }
                    else
                    {
                        this.isMode1 = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin mode1 turned off");
                        this.speed = 1f;
                        this.StartMode1();   // return to default speed
                    }
                }

                if (this.isMode1 == true)
                {
                    this.StartMode1();

                    if (Input.GetKeyDown(KeyCode.Comma))
                    {
                        float tmp = this.speed - 0.2f;
                        Console.WriteLine("current yotogi speed " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speed = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        float tmp = this.speed + 0.2f;
                        Console.WriteLine("current yotogi speed " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speed = tmp;
                        }
                    }
                }
                ///mode1



                ///mode2, spy on input, open close by keyboard " ' ", change variable by keyboard ' < > ← → ↓ ↑ ' 
                if (Input.GetKeyDown(KeyCode.Quote))
                {
                    if (this.isMode2 == false)
                    {
                        this.isMode2 = true;
                        this.isMode1 = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin mode2(dynamic) turned on, change speed variable by keyboard < > ← → ↓ ↑ ");
                        this.manCount = 6; //game always stock 6 man in yotogi scene, need to reopen plugin to reset the count of man
                    }
                    else
                    {
                        this.isMode2 = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin mode2 turned off");
                        this.speed = 1f;
                        this.StartMode1();    // return to default speed
                    }
                }


                if (this.isMode2 == true)
                {
                    this.StartMode2();

                    if (Input.GetKeyDown(KeyCode.Comma))
                    {
                        float tmp = this.abs - 0.1f;
                        Console.WriteLine("speed change range in one frame " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.01f)
                        {
                            this.abs = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        float tmp = this.abs + 0.1f;
                        Console.WriteLine("speed change range in one frame " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.01f)
                        {
                            this.abs = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        float tmp = this.speedUpTo - 0.1f;
                        Console.WriteLine("max speed up to " + tmp);
                        if (tmp <= 4.0f && tmp >= this.speedDownTo)
                        {
                            this.speedUpTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        float tmp = this.speedUpTo + 0.1f;
                        Console.WriteLine("max speed up to " + tmp);
                        if (tmp <= 4.0f && tmp >= this.speedDownTo)
                        {
                            this.speedUpTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        float tmp = this.speedDownTo - 0.1f;
                        Console.WriteLine("min speed down to " + tmp);
                        if (tmp <= this.speedUpTo && tmp >= 0.08f)
                        {
                            this.speedDownTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        float tmp = this.speedDownTo + 0.1f;
                        Console.WriteLine("min speed down to " + tmp);
                        if (tmp <= this.speedUpTo && tmp >= 0.08f)
                        {
                            this.speedDownTo = tmp;
                        }
                    }

                }
            }
            ///mode2
            catch
            {
                this.isMode1 = false;
                this.isMode2 = false;
                Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin turned off ( no maid in the scene ) ");
            }
        ///
        }

    }

}
