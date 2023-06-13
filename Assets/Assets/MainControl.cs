/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Globalization;
using System.Threading;

namespace M2MqttUnity
{
    public class MainControl : MonoBehaviour
    {
        public M2MqttUnityClient myM2MqttUnityClient;
        
        InputField outputArea;
        InputField outputArea_debug;
        public Joystick JoystickV;
        public Joystick JoystickH;

        // Start is called before the first frame update
        void Start()
        {
            outputArea = GameObject.Find("OutputArea").GetComponent<InputField>();
            JoystickV = GameObject.Find("JoystickV").GetComponent<Joystick>();
            JoystickH = GameObject.Find("JoystickH").GetComponent<Joystick>();
            JoystickV.AxisOptions = AxisOptions.Vertical;
            JoystickH.AxisOptions = AxisOptions.Horizontal;
        }
        
        // Update is called once per frame
        void Update()
        {
            if(myM2MqttUnityClient.IsConnected())
            {
                publishMotor(JoystickV.Vertical,1,2);
                publishMotor(JoystickH.Horizontal,3,4);
                String txt = "HC04-A: " + myM2MqttUnityClient.HC04_A.ToString() + "   |   " + "HC04-B: " + myM2MqttUnityClient.HC04_B.ToString() + "   |   " + "HC04-C: " + myM2MqttUnityClient.HC04_C.ToString() + "\n" + "VL53L0X-A: " + myM2MqttUnityClient.VL53L0X_A.ToString() + " | " + "VL53L0X-B: " + myM2MqttUnityClient.VL53L0X_B.ToString() + " | " + "VL53L0X-C: " + myM2MqttUnityClient.VL53L0X_C.ToString() + "\n";
                outputArea.text = txt;
            }
            else
            { 
                Debug.Log("Connecting..."); 
            }
        }

        //Motorcontroller
        void publishMotor(float j,int ma,int mb)
        {
            if (j > 0)
            {
                myM2MqttUnityClient.publish("L298N/M" + ma.ToString(), "1");
                myM2MqttUnityClient.publish("L298N/M" + mb.ToString(), "0");
            }
            else if (j < 0)
            {
                myM2MqttUnityClient.publish("L298N/M" + ma.ToString(), "0");
                myM2MqttUnityClient.publish("L298N/M" + mb.ToString(), "1");
            }
            else 
            {
                myM2MqttUnityClient.publish("L298N/M" + ma.ToString(), "0");
                myM2MqttUnityClient.publish("L298N/M" + mb.ToString(), "0");
            }
            
        }
    }
}


