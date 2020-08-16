using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using BepInEx;

using UnityEngine;
using System.Collections;


namespace COM3D2.ChangeYotogiSpeed.Plugin
{
    [BepInPlugin("COM3D2.ChangeYotogiSpeed.Plugin", "Change Yotogi Speed", "0.0.2.0")]
    public class ChangeYotogiSpeed : BaseUnityPlugin
    {
        public Maid maid;
        public Animation anm_BO_body001;

        public bool isMode1 = false;
        public bool isMode2 = false;
        public float speed = 1.4f;
        public float speedUpTo = 2f;
        public float speedDownTo = 0.2f;
        public float abs = 0.2f;
        public int manCount = 1;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            //UnityEngine.Debug.Log("Hello, world!");
            Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin loaded, turning on by keyboard ' space ' (only works in scene with maid within)");
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

            this.maid = GameMain.Instance.CharacterMgr.GetMaid(0);
            if (this.maid == null) { Console.WriteLine("this.maid = null"); }

            this.anm_BO_body001 = this.maid.body0.GetAnimation();
            if (this.anm_BO_body001 == null) { Console.WriteLine("this.anm_BO_body001 = null"); }

            float tmp = (float)rand.NextDouble() * this.speedUpTo;
            if ( tmp > this.speedDownTo && Math.Abs(tmp - this.speed) <= this.abs )
            {
                this.speed = tmp;
            }

            for (int i = 0; i < this.manCount; i++)
            {
                try
                {
                    foreach (AnimationState stat in GameMain.Instance.CharacterMgr.GetMan(i).body0.GetAnimation())
                    {
                        if (stat == null) { continue; }

                        stat.speed = this.speed;
                    }
                }
                catch
                {
                    this.manCount = i;
                    break;
                }
            }

            foreach (AnimationState stat in anm_BO_body001)
            {
                if (stat == null) { continue; }

                stat.speed = this.speed;
            }

        }



        public void Update()
        {
            try
            {
                ///mode1, spy on input, open close by keyboard "space", change speed variable by keyboard "<", ">"
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Console.WriteLine("space");
                    if (this.isMode1 == false)
                    {
                        this.isMode1 = true;
                        this.isMode2 = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin mode1(constant mode) turned on, change speed variable by keyboard ' < ', ' > '");
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
                        Console.WriteLine("Comma this.speed " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speed = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        float tmp = this.speed + 0.2f;
                        Console.WriteLine("Period this.speed " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speed = tmp;
                        }
                    }
                }
                ///mode1



                ///mode2, spy on input, open close by keyboard "right alt", change variable by keyboard ' : ', ' " ' 
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (this.isMode2 == false)
                    {
                        this.isMode2 = true;
                        this.isMode1 = false;
                        Console.WriteLine("COM3D2.ChangeYotogiSpeed.Plugin mode2(dynamic) turned on, change speed variable by keyboard ' < > ← → ↓ ↑");
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
                        float tmp = this.abs - 0.05f;
                        Console.WriteLine("speed change range (abs) " + tmp);
                        if (tmp <= 2.0f && tmp >= 0.02f)
                        {
                            this.abs = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        float tmp = this.abs + 0.05f;
                        Console.WriteLine("speed change range (abs) " + tmp);
                        if (tmp <= 2.0f && tmp >= 0.02f)
                        {
                            this.abs = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        float tmp = this.speedUpTo - 0.2f;
                        Console.WriteLine("max speed up to " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speedUpTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        float tmp = this.speedUpTo + 0.2f;
                        Console.WriteLine("max speed up to " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speedUpTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        float tmp = this.speedDownTo - 0.1f;
                        Console.WriteLine("min peed down to " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speedDownTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        float tmp = this.speedDownTo + 0.1f;
                        Console.WriteLine("min peed down to " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
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
