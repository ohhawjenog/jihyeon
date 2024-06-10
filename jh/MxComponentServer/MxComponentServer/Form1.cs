using System.Net.Sockets;
using System.Net;
using System.Text;

namespace MxComponentServer
{
    public partial class Form1 : Form
    {
        MxComopentServer server;
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        Thread thread;

        public Form1()
        {
            InitializeComponent();

            server = new MxComopentServer();

            label1.Text = "";
        }

        private void ConnectPLC_Click(object sender, EventArgs e)
        {
            string ret = server.ConnectPLC();
            label1.Text += DateTime.Now + ": " + ret + "\n";

            //StartTCPServer();

            thread = new Thread(new ThreadStart(StartTCPServer));
            thread.Start();
            
            label1.Text += DateTime.Now + ": TCP Server is running.\n";
        }

        void StartTCPServer()
        {
            listener = new TcpListener(IPAddress.Any, 7000);
            listener.Start();

            // 서버로부터 클라이언트 정보를 가져옴
            client = listener.AcceptTcpClient();

            // 클라이언트의 네트워크 스트림을 가져옴
            stream = client.GetStream();

            int bytes;
            byte[] buffer = new byte[100];

            // 클라이언트 데이터 수신
            while (((bytes = stream.Read(buffer, 0, 100)) > 0))
            {
                string output = Encoding.ASCII.GetString(buffer, 0, bytes); // D0

                // MX Component D0의 값 가져오기
                string ret = server.GetData(output);

                Byte[] newData = new byte[100];
                newData = Encoding.ASCII.GetBytes(ret);

                // 클라이언트의 데이터를 그대로 전송
                stream.Write(newData, 0, newData.Length);
            }

            stream.Close();
            client.Close();
        }


        private void DisconnectPLC_Click(object sender, EventArgs e)
        {
            if(thread!=null)
                thread.Interrupt();

            if (stream != null)
                stream.Close();

            if(client != null)
                client.Close();
    
            if(listener!= null)
                listener.Stop();

            string ret = server.DisconnectPLC();
            label1.Text += DateTime.Now + ": " + ret + "\n";
        }
    }
}
