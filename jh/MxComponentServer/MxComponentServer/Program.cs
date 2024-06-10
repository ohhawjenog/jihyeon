using ActUtlType64Lib;
using static System.Windows.Forms.AxHost;

namespace MxComponentServer
{
    enum State
    {
        Disconnected,
        Connected
    }

    public class MxComopentServer
    {
        State state = State.Disconnected;
        ActUtlType64 mxComponent;

        public MxComopentServer()
        {
            mxComponent = new ActUtlType64();
            mxComponent.ActLogicalStationNumber = 1;
        }

        public string ConnectPLC()
        {
            int ret = mxComponent.Open();
            if(ret == 0)
            {
                return "Connection succeded!";
            }
            else
            {
                return "Connection failed...";
            }
        }

        public string DisconnectPLC()
        {
            int ret = mxComponent.Close();
            if (ret == 0)
            {
                return "Disconnection succeded!";
            }
            else
            {
                return "Disconnection failed...";
            }
        }

        public string GetData(string device)
        {
            int value;
            int ret = mxComponent.GetDevice(device, out value);
            // GetDevice -> ReadDeviceBlock
            if(ret == 0)
            {
                return value.ToString();
            }
            else
                return ret.ToString("X");
        }

        public string SetData(string device, int value)
        {
            int ret = mxComponent.SetDevice(device, value);
            if (ret == 0)
            {
                return "데이터 전송 성공";
            }
            else
                return ret.ToString("X");
        }
    }

    internal class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}