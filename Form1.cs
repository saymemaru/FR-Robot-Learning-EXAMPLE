using System.Net;
using System.Threading;
using System.Windows.Forms;
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
            int.TryParse(ServerPortBox.Text, out serverPort);

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

        //�̰߳�ȫ��׷���ı�����־�ı���
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

        //��־�ı���仯
        private void LogBox_TextChanged(object sender, EventArgs e)
        {

        }

        //����������
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

        //������IP
        private void ServerIPBox_TextChanged(object sender, EventArgs e)
        {
            serverIP = ServerIPBox.Text;
        }

        //�������˿�
        private void ServerPortBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ServerPortBox.Text, out serverPort))
            {
                // ת���ɹ���ʹ��number���к�������
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("��������Ч������");
            }
        }

        //������Ϣ
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

        //�ͻ���IP
        private void ClientIPBox_TextChanged(object sender, EventArgs e)
        {
            clientIP = ClientIPBox.Text;
        }

        //������Ϣ
        private void TextInputBox_TextChanged(object sender, EventArgs e)
        {
            inputMessage = TextInputBox.Text;
        }

        //�ͻ��˶˿�
        private void ClientPortBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ServerPortBox.Text, out clientPort))
            {
                // ת���ɹ���ʹ��number���к�������
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("��������Ч������");
            }
        }

        //ֹͣ������
        private void StopServerButton_Click(object sender, EventArgs e)
        {
            server.Stop();
        }

        //�㲥
        private void BoardcastButton_Click(object sender, EventArgs e)
        {
            //����Ϊ��
            if (string.IsNullOrWhiteSpace(TextInputBox.Text))
            {
                MessageBox.Show("������Ҫ�㲥����Ϣ");
                return;
            }
            //��������
            if (!TextInputBox.Text.StartsWith("/"))
            {
                server.BroadcastMessage(TextInputBox.Text);
                TextInputBox.Clear();
                return;
            }
            //������
            // ����UI�̶߳�ȡ�ı�
            string commandText = TextInputBox.Text;
            ThreadPool.QueueUserWorkItem(_ => server.ExecuteServerCommand(commandText));
            TextInputBox.Clear();
        }

        //�������
        private void CameraConnectionButton_Click(object sender, EventArgs e)
        {
            string cameraLog;

            if (CameraManager.Instance.isCameraConnected)
            {
                AppendTextToLog($"[{DateTime.Now:HH:mm:ss}] ���������" + Environment.NewLine);
                LogBox.ScrollToCaret();
                return;
            }
            else if (CameraManager.Instance.InitializeCamera(out cameraLog))
            {
                AppendTextToLog($"[{DateTime.Now:HH:mm:ss}] �����ʼ���ɹ�: "
                    + cameraLog + Environment.NewLine);
                LogBox.ScrollToCaret();
            }
            else
            {
                AppendTextToLog($"[{DateTime.Now:HH:mm:ss}] �����ʼ��ʧ��: "
                    + cameraLog + Environment.NewLine);
                LogBox.ScrollToCaret();
            }
        }

        //�������ͼ
        private void SaveCloudPointButton_Click(object sender, EventArgs e)
        {
            if (!CameraManager.Instance.isCameraConnected)
            {
                MessageBox.Show("�����������");
                return;
            }
            CameraManager.Instance.SaveCloudPointFile();
        }

        //����RGBͼ��
        private void SaveRGBButton_Click(object sender, EventArgs e)
        {
            if (!CameraManager.Instance.isCameraConnected)
            {
                MessageBox.Show("�����������");
                return;
            }
            CameraManager.Instance.SaveRGBFile();
        }
    }
}
