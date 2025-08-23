using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace FR_TCP_Server
{
    public partial class Form1 : Form
    {
        private TcpServer server;
        private string serverIP;
        private int serverPort;

        private string clientIP;
        private int clientPort;

        private string inputMessage;


        public Form1()
        {
            InitializeComponent();

            server = new TcpServer();
            server.LogMessage += Server_LogMessage;
            server.MessageReceived += Server_MessageReceived;

            serverIP = ServerIPBox.Text;
            int.TryParse(PortBox.Text, out serverPort);

            clientIP = ClientIPBox.Text;
            int.TryParse(ClientPortBox.Text, out clientPort);
        }

        private void Server_MessageReceived(string msg, IPEndPoint endPoint)
        {
            // ׷�ӽ��յ�����Ϣ���ı���
            //AppendTextToLog($"���� {endPoint} ����Ϣ: {msg}" + Environment.NewLine);
        }

        private void Server_LogMessage(string msg)
        {
            // ׷����־���ı���
            AppendTextToLog(msg + Environment.NewLine);
        }

        private void AppendTextToLog(string text)
        {
            if (LogBox.InvokeRequired)
            {
                LogBox.Invoke(new Action(() => AppendTextToLog(text)));
            }
            else
            {
                LogBox.AppendText(text);
                LogBox.ScrollToCaret(); // �Զ��������ײ�
            }
        }

        private void LogBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            try
            {
                server.Start(serverIP, serverPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����������ʧ��: {ex.Message}");
            }
        }

        private void ServerIPBox_TextChanged(object sender, EventArgs e)
        {
            serverIP = ServerIPBox.Text;
        }

        private void PortBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(PortBox.Text, out serverPort))
            {
                // ת���ɹ���ʹ��number���к�������
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("��������Ч������");
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                server.SendMessage(clientIP, clientPort, inputMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������Ϣʧ��: {ex.Message}");
            }
        }

        private void ClientIPBox_TextChanged(object sender, EventArgs e)
        {
            clientIP = ClientIPBox.Text;
        }

        private void textInputBox_TextChanged(object sender, EventArgs e)
        {
            inputMessage = textInputBox.Text;
        }

        private void ClientPortBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(PortBox.Text, out clientPort))
            {
                // ת���ɹ���ʹ��number���к�������
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("��������Ч������");
            }
        }

        private void StopServerButton_Click(object sender, EventArgs e)
        {
            server.Stop();
        }

        private void BoardcastButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textInputBox.Text))
            {
                MessageBox.Show("������Ҫ�㲥����Ϣ");
                return;
            }

            server.BroadcastMessage(textInputBox.Text);
            textInputBox.Clear();
        }

        private void CameraConnectionButton_Click(object sender, EventArgs e)
        {
            CameraManager.Instance
            int camer_num = 0;
            int camera_ret = -1;
            /*****************
            ��ӡ�����־
            SetLogLevel(int error, int debug, int warnning, int info)
            ��1 �ر�0
            *****************/
            DkamSDK_CSharp.SetLogLevel(1, 0, 0, 1);
            //*************************************��ѯ���************************************
            //���־������ڵ����
            camer_num = DkamSDK_CSharp.DiscoverCamera();
            AppendTextToLog("Camer num is=" + camer_num + Environment.NewLine);
            LogBox.ScrollToCaret();
            //�������
            if (camer_num < 0)
            {
                AppendTextToLog("No camera" + Environment.NewLine);
                LogBox.ScrollToCaret();
            }

            //�Ծ������ڵ������������0��IP 1:series number	
            int sort = DkamSDK_CSharp.CameraSort(0);
            AppendTextToLog("the camera sort result=" + sort + Environment.NewLine);
            LogBox.ScrollToCaret();

            for (int i = 0; i < camer_num; i++)
            {
                //��ʾ�����������IP
                AppendTextToLog("ip is=" + DkamSDK_CSharp.CameraIP(i) + Environment.NewLine);
                LogBox.ScrollToCaret();
                if (String.Compare(DkamSDK_CSharp.CameraIP(i), "192.168.58.11") == 0)
                {
                    camera_ret = i;
                }

            }
            //*************************************�������************************************
            //������������������������
            SWIGTYPE_p_CAMERA_OBJECT camera_obj1 = DkamSDK_CSharp.CreateCamera(camera_ret);
            int connect = DkamSDK_CSharp.CameraConnect(camera_obj1);
            AppendTextToLog("Connect Camera result��" + connect + Environment.NewLine);
            LogBox.ScrollToCaret();
            //�����PC���Ƿ���ͬһ��������
            LogBox.AppendText("WhetherIsSameSegment=" + DkamSDK_CSharp.WhetherIsSameSegment(camera_obj1) + Environment.NewLine);
            LogBox.ScrollToCaret();
        }
    }
}
