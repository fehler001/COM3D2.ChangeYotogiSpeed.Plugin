using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using BepInEx;

using UnityEngine;
using System.Collections;


namespace COM3D2.ChangeYotogiSpeed.Plugin
{
    [BepInPlugin("COM3D2.ChangeYotogiSpeed.Plugin", "Change Yotogi Speed", "0.0.3.1")]
    public class ChangeYotogiSpeed : BaseUnityPlugin
    {
        public Maid maid;
        public Animation anm_BO_body001;
        public Camera camera;
        public bool IsPespective = true;

        public bool leftControlDown = false;
        public bool leftAltDown = false;
        public bool rightControlDown = false;
        public bool rightAltDown = false;

        public bool isMode1 = false;
        public bool isMode2 = false;
        public float speed = 1.4f;
        public float speedUpTo = 2.0f;
        public float speedDownTo = 1.21f;
        public float abs = 0.12f;
        public int manCount = 1;

        public Vector3 cameraPosition;
        public float orthoSize = 0.3f;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            //UnityEngine.Debug.Log("Hello, world!");
            UnityEngine.Debug.Log("COM3D2.ChangeYotogiSpeed.Plugin loaded, turning on by keyboard  ;  or  ' (single quote) (only works in scene with maid within)");
            UnityEngine.Debug.Log("'Right Alt' + 'O', 'Right Alt' + 'P' to toggle orthographic or Perspective of camera view");
            UnityEngine.Debug.Log("'Right Alt'+ '[', 'Right Alt' + ']' to adjust field of view, 'Right Alt' + 'backslash' to reset field of view to 35");
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

            for (int i = 0; i < this.manCount; i++)
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

            ///get random speed change value
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

            float tmp = (float)rand.Next( (int)(rDownTo * 1000000), (int)(rUpTo * 1000000) )  / 1000000;

            this.speed = tmp;
            ///

            StartMode1();
        }


        public void Update()
        {
            ///detect left control down or up
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //Console.WriteLine("left control down");
                this.leftControlDown = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                //Console.WriteLine("left control up");
                this.leftControlDown = false;
            }
            ///

            ///detect right control down or up
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                //Console.WriteLine("right control down");
                this.rightControlDown = true;
            }
            if (Input.GetKeyUp(KeyCode.RightControl))
            {
                //Console.WriteLine("right control up");
                this.rightControlDown = false;
            }
            ///

            ///detect left alt down or up
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                //Console.WriteLine("left alt down");
                this.leftAltDown = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                //Console.WriteLine("left alt up");
                this.leftAltDown = false;
            }
            ///

            ///detect right alt down or up
            if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                //Console.WriteLine("right alt down");
                this.rightAltDown = true;
            }
            if (Input.GetKeyUp(KeyCode.RightAlt))
            {
                //Console.WriteLine("right alt up");
                this.rightAltDown = false;
            }
            ///



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
                        Console.WriteLine("current yotogi speed = " + tmp);
                        if (tmp <= 4.0f && tmp >= 0.08f)
                        {
                            this.speed = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        float tmp = this.speed + 0.2f;
                        Console.WriteLine("current yotogi speed = " + tmp);
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
                        this.speed = (this.speedUpTo + this.speedDownTo) / 2; // in case float rUpto small than float rDownTo
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
                        Console.WriteLine("speed change range in one frame = " + tmp);
                        if (tmp > 0.01f)
                        {
                            this.abs = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Period))
                    {
                        float tmp = this.abs + 0.1f;
                        Console.WriteLine("speed change range in one frame = " + tmp);
                        if (tmp < 1.0f)
                        {
                            this.abs = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        float tmp = this.speedUpTo + 0.1f;
                        Console.WriteLine("max speed = " + tmp);
                        if (tmp < 4.0f && tmp > this.speedDownTo)
                        {
                            this.speedUpTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        float tmp = this.speedUpTo - 0.1f;
                        Console.WriteLine("max speed = " + tmp);
                        if (tmp > this.speedDownTo)
                        {
                            this.speedUpTo = tmp;
                            this.speed = (this.speedUpTo + this.speedDownTo) / 2;
                            this.StartMode1();  // in case float rUpto small than float rDownTo
                        }
                    }                        
                        
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        float tmp = this.speedDownTo - 0.1f;
                        Console.WriteLine("min speed = " + tmp);
                        if (tmp > 0.08f)
                        {
                            this.speedDownTo = tmp;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        float tmp = this.speedDownTo + 0.1f;
                        Console.WriteLine("min speed = " + tmp);
                        if (tmp < this.speedUpTo)
                        {
                            this.speedDownTo = tmp;
                            this.speed = (this.speedUpTo + this.speedDownTo) / 2;
                            this.StartMode1();  // in case float rUpto small than float rDownTo
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



            ///toggle Parallel ( orthographic called in unity3d ) or Perspective view
            try
            {
                if(this.rightAltDown == true)
                {
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        this.camera = GameMain.Instance.MainCamera.camera;
                        this.camera.orthographic = true;
                        this.cameraPosition = this.camera.transform.position;   // lock camera.transform.position ( I think this should not be changed in orthographic)
                        this.camera.orthographicSize = this.orthoSize;
                        this.IsPespective = false;
                        //Console.WriteLine("camera.orthographic = true");
                        //Console.WriteLine("'Right Alt' + 'O', 'Right Alt' + 'P' to toggle orthographic or Perspective of camera view");
                    }
                }
                if (this.rightAltDown == true)
                {
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        this.camera = GameMain.Instance.MainCamera.camera;
                        this.camera.orthographic = false;
                        this.IsPespective = true;
                        //Console.WriteLine("camera.orthographic = false");
                        //Console.WriteLine("'Right Alt'+ '[', 'Right Alt' + ']' to adjust field of view, 'Right Alt' + 'backslash' to reset field of view to 35");
                    }
                }
                
                
                if (this.IsPespective == false)
                {
                    if( Input.mouseScrollDelta != new Vector2(0.0f, 0.0f) )
                    {
                        this.camera.transform.position = this.cameraPosition;
                        //Console.WriteLine("camera.transform.position = " + this.camera.transform.position);
                
                        float tmp = this.orthoSize - Input.mouseScrollDelta.y * (float)0.01;
                        if (tmp >= 0.1f && tmp <= 2f)   // just from my opinion
                        {
                            this.orthoSize = tmp;
                            this.camera.orthographicSize = this.orthoSize;
                            //Console.WriteLine("camera.orthographicSize = " + this.orthoSize);
                        }
                    }
                }
                

                //adjust field of view
                if (this.IsPespective == true)
                {
                    if (this.rightAltDown == true)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftBracket))
                        {
                            this.camera = GameMain.Instance.MainCamera.camera;
                            if (this.camera.fieldOfView >= 10)
                            {
                                this.camera.fieldOfView = this.camera.fieldOfView - 1f;
                                //Console.WriteLine("camera.fieldOfView = " + this.camera.fieldOfView + "   ( default is 35 )");
                            }
                        }
                
                        if (Input.GetKeyDown(KeyCode.RightBracket))
                        {
                            this.camera = GameMain.Instance.MainCamera.camera;
                            if (this.camera.fieldOfView <= 60)
                            {
                                this.camera.fieldOfView = this.camera.fieldOfView + 1f;
                                //Console.WriteLine("camera.fieldOfView = " + this.camera.fieldOfView + "   ( default is 35 )");
                            }
                        }
                
                        if (Input.GetKeyDown(KeyCode.Backslash))
                        {
                            this.camera = GameMain.Instance.MainCamera.camera;
                            this.camera.fieldOfView = 35f;
                            //Console.WriteLine("camera.fieldOfView = " + this.camera.fieldOfView + "   ( default is 35 )");
                        }
                
                    }
                }
                ///
                
                
                //debug
                //if (Input.GetKeyDown(KeyCode.Q))
                //{
                //    if(this.IsPespective == false)
                //    {
                //        Console.WriteLine(this.camera.orthographicSize);
                //    }
                //    else
                //    {
                //        Console.WriteLine(this.camera.fieldOfView);
                //    }
                //}


            }
            catch
            {
                Console.WriteLine("Toggle perspective view got wrong");
            }


        }

    }

}
