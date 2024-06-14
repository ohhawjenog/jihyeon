using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActUtlType64Lib; // MX Component v5 Library 사용
using System;
using System.Linq;


namespace MPS
{
    public class MxComponent : MonoBehaviour
    {
        public static MxComponent instance;
        public enum Connection
        {
            Connected,
            Disconnected,
        }

        ActUtlType64Lib.ActUtlType64 mxComponent;
        public Connection connection = Connection.Disconnected;
        public int boxACount;
        public int boxBCount;

        [Header("Align System")]
        //public Conveyor conveyor;               // 1-1. Conveyor
        public Cylinder Stopper;                // 1-2. Stopper
        public Sensor stopSensor;               // 1-2-1. Stop Sensor
        public Cylinder aligner;               // 1-3. Aligner
        public Sensor alignSensor;              // 1-4. Align Sensor
        public Sensor boxASensor;               // 1-5-1. Box A Sensor
        public Sensor boxBSensor;               // 1-5-2. Box B Sensor

        [Space(20)]
        [Header("Palletizing System")]
        public DriveMotor yTransfer;            // 2-1. Y-Transfer
        public DriveMotor xTransfer;            // 2-2. X-Transfer
        //public RotaryCylinder rotaryCylinder;   // 2-3. Rotary Cylinder
        public DriveMotor zTransfer;            // 2-4. Z-Transfer
        public Cylinder loadCylinder;           // 2-5. Load Cylinder

        [Space(20)]
        [Header("Pallet")]
        //public Lamp palletALamp;                // 3-1-1. Pallet A Lamp
        public Cylinder palletAJigCylinder;             // 3-1-2. Pallet A Jig Cylinder
        public Cylinder palletAPullCylinder;            // 3-1-3. Pallet A Pull Cylinder
        //public Lamp palletBLamp;                // 3-2-1. Pallet B Lamp
        public Cylinder palletBJigCylinder;             // 3-2-2. Pallet B Jig Cylinder
        public Cylinder palletBPullCylinder;            // 3-2-3. Pallet B Pull Cylinder

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            mxComponent = new ActUtlType64Lib.ActUtlType64();
            mxComponent.ActLogicalStationNumber = 1;

            StartCoroutine(CoStart());
        }

        IEnumerator CoStart()
        {
            while (true)
            {
                GetTotalDeviceData();

                yield return new WaitForSeconds(0.2f);
            }
        }

        private void GetTotalDeviceData()
        {
            if (connection == Connection.Connected)
            {
                short[] dData = ReadDeviceBlock("D0", 1);
                short[] xData = ReadDeviceBlock("X0", 1);
                short[] yData = ReadDeviceBlock("Y0", 5);
                string newDData = ConvertDataIntoString(dData);
                string newXData = ConvertDataIntoString(xData);
                string newYData = ConvertDataIntoString(yData);

                boxACount                               = newDData[0 ] - 48;
                boxBCount                               = newXData[1 ] - 48;

                //stopSensor.plcInputValue                = newXData[1 ] - 48;
                //alignSensor.plcInputValue               = newXData[2 ] - 48;
                //boxASensor.plcInputValue                = newXData[3 ] - 48;
                //boxBSensor.plcInputValue                = newXData[4 ] - 48;

                //conveyor.plcInputValue                  = newYData[0 ] - 48;
                //Stopper.plcInputValues[0]               = newYData[10] - 48;
                //Stopper.plcInputValues[1]               = newYData[11] - 48;
                //aligner.plcInputValues[0]               = newYData[12] - 48;
                //aligner.plcInputValues[1]               = newYData[13] - 48;
                yTransfer.plcInputValues[0]             = newYData[42] - 48;
                yTransfer.plcInputValues[1]             = newYData[43] - 48;
                xTransfer.plcInputValues[0]             = newYData[40] - 48;
                xTransfer.plcInputValues[1]             = newYData[41] - 48;
                //rotaryCylinder.plcInputValues[0]        = newYData[20] - 48;
                //rotaryCylinder.plcInputValues[1]        = newYData[21] - 48;
                zTransfer.plcInputValues[0]             = newYData[44] - 48;
                zTransfer.plcInputValues[1]             = newYData[45] - 48;
                //loadCylinder.plcInputValues[0]          = newYData[22] - 48;
                //loadCylinder.plcInputValues[1]          = newYData[23] - 48;
                //palletAJigCylinder.plcInputValues[0]    = newYData[30] - 48;
                //palletAJigCylinder.plcInputValues[1]    = newYData[31] - 48;
                //palletAPullCylinder.plcInputValues[0]   = newYData[32] - 48;
                //palletAPullCylinder.plcInputValues[1]   = newYData[33] - 48;
                //palletBJigCylinder.plcInputValues[0]    = newYData[34] - 48;
                //palletBJigCylinder.plcInputValues[1]    = newYData[35] - 48;
                //palletBPullCylinder.plcInputValues[0]   = newYData[36] - 48;
                //palletBPullCylinder.plcInputValues[1]   = newYData[37] - 48;
            }
        }

        short[] ReadDeviceBlock(string startDeviceName, int _blockSize)
        {
            if (connection == Connection.Connected)
            {
                short[] devices = new short[_blockSize];
                int returnValue = mxComponent.ReadDeviceBlock2(startDeviceName, _blockSize, out devices[0]);

                if (returnValue != 0)
                    print(returnValue.ToString("X"));

                return devices;
            }
            else
                return null;
        }

        string ConvertDataIntoString(short[] data)
        {
            string newData = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    newData += "0000000000";
                    continue;
                }

                string temp = Convert.ToString(data[i], 2);
                string temp2 = new string(temp.Reverse().ToArray());
                newData += temp2;

                if (temp2.Length < 10)
                {
                    int zeroCount = 10 - temp2.Length;
                    for (int j = 0; j < zeroCount; j++)
                        newData += "0";
                }
            }

            return newData;
        }

        int GetDevice(string device)
        {
            if (connection == Connection.Connected)
            {
                int data = 0;
                int returnValue = mxComponent.GetDevice(device, out data);

                if (returnValue != 0)
                    print(returnValue.ToString("X"));

                return data;
            }
            else
                return 0;
        }

        public bool SetDevice(string device, int value)
        {
            if (connection == Connection.Connected)
            {
                int returnValue = mxComponent.SetDevice(device, value);

                if (returnValue != 0)
                    print(returnValue.ToString("X"));

                return true;
            }
            else
                return false;
        }

        public void OnConnectPLCBtnClkEvent()
        {
            if (connection == Connection.Disconnected)
            {
                int returnValue = mxComponent.Open();

                if (returnValue == 0)
                {
                    print("연결에 성공하였습니다.");

                    connection = Connection.Connected;
                }
                else
                {
                    print("연결에 실패했습니다. returnValue: 0x" + returnValue.ToString("X")); // 16진수로 변경
                }
            }
            else
            {
                print("연결되어 있습니다.");
            }
        }

        public void OnDisconnectPLCBtnClkEvent()
        {
            if (connection == Connection.Connected)
            {
                int returnValue = mxComponent.Close();

                if (returnValue == 0)
                {
                    print("연결 해제에 성공했습니다.");

                    connection = Connection.Disconnected;
                }
                else
                {
                    print("연결 해제에 실패했습니다. returnValue: 0x" + returnValue.ToString("X")); // 16진수로 변경
                }
            }
            else
            {
                print("연결되어 있지 않습니다.");
            }
        }

        bool isStarted = false;
        public void OnStartPLCButtonClkEvent()
        {
            if (connection == Connection.Connected)
            {
                isStarted = !isStarted;

                if (isStarted)
                    SetDevice("X0", 1);
                else
                    SetDevice("X0", 0);
            }
        }

        bool isStoped = false;
        public void OnStopPLCButtonClkEvent()
        {
            if (connection == Connection.Connected)
            {
                isStoped = !isStoped;

                if (isStoped)
                    SetDevice("X1", 1);
                else
                    SetDevice("X1", 0);
            }
        }

        bool isEmmergency = false;
        //public void OnEmmergencyStopBtnClkEvent()
        //{
        //    if (connection == Connection.Connected)
        //    {
        //        isEmmergency = !isEmmergency;

        //        if (isEmmergency)
        //            SetDevice("X2", 1);
        //        else
        //            SetDevice("X2", 0);
        //    }
        //}
        private void OnDestroy()
        {
            OnDisconnectPLCBtnClkEvent();
        }
    }
}
