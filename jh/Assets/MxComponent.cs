using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActUtlType64Lib; // MX Component v5 Library 사용


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

        [Header("Align System")]
        public Cylinder Stopper;                // 1-1. Stopper
        public Sensor stopSensor;               // 1-1-1. Stop Sensor
        public Sensor alignSensor;              // 1-2. Align Sensor
        public Cylinder alignser;               // 1-3. Aligner
        public Sensor boxASensor;               // 1-4-1. Box A Sensor
        public Sensor BoxBSensor;               // 1-4-2. Box B Sensor

        [Space(20)]
        [Header("Palletizing System")]
        public DriveMotor yTransfer;            // 2-1. Y-Transfer
        public DriveMotor xTransfer;            // 2-2. X-Transfer
        public RotaryCylinder rotaryCylinder;   // 2-3. Rotary Cylinder
        public DriveMotor zTransfer;            // 2-4. Z-Transfer
        public Cylinder loadCylinder;           // 2-5. Load Cylinder

        [Space(20)]
        [Header("Pallet")]
        public Lamp palletALamp;                // 3-1-1. Pallet A Lamp
        public Cylinder palletAJig;             // 3-1-2. Pallet A Jig Cylinder
        public Cylinder palletAPull;            // 3-1-3. Pallet A Pull Cylinder
        public Lamp palletBLamp;                // 3-2-1. Pallet B Lamp
        public Cylinder palletBJig;             // 3-2-2. Pallet B Jig Cylinder
        public Cylinder palletBPull;            // 3-2-3. Pallet B Pull Cylinder

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
                //short[] yData = ReadDeviceBlock("Y0", 5); // Short지만, 10개의 비트를 가져옴
                //string newYData = ConvertDateIntoString(yData);

                //// print(newYData); // 데이터를 잘 받아오는지 확인

                //supplySensor.plcInputValue = newYData[4] - '0';  // Y4
                //supplyCylinder.plcInputValues[0] = newYData[10] - 48;  // Y10
                //supplyCylinder.plcInputValues[1] = newYData[11] - 48;
                //machiningCylinder.plcInputValues[0] = newYData[20] - 48;
                //deliveryCylinder.plcInputValues[0] = newYData[30] - 48;
                //deliveryCylinder.plcInputValues[1] = newYData[31] - 48;
                //objectDetector.plcInputValue = newYData[5] - 48;
                //conveyor.plcInputValue = newYData[0] - 48;
                //metalDetector.plcInputValue = newYData[6] - 48;
                //dischargeCylinder.plcInputValues[0] = newYData[40] - 48;
                //dischargeCylinder.plcInputValues[1] = newYData[41] - 48;  // 50ms * 10 = 0.5s
                //SetLampActive("Red", newYData[1] - 48);
                //SetLampActive("Yellow", newYData[2] - 48);
                //SetLampActive("Green", newYData[3] - 48);
            }
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
                print("연결 되어 있습니다.");
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
                print("연결 되어있지 않습니다.");
            }
        }

        private void OnDestroy()
        {
            OnDisconnectPLCBtnClkEvent();
        }
    }
}
