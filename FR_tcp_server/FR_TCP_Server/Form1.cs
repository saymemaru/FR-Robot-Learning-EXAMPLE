using System.Net;

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
    }
}
