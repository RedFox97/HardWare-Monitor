using System;
using System.Drawing;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using System.Text;
using System.Net.Sockets;
using System.Xml;
using Newtonsoft.Json;

namespace HardWareMonitor
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        readonly private Computer thisComputer = new Computer();
        private TcpClient client = new TcpClient();
        public Main()
        {
            InitializeComponent();
            base.WindowState = FormWindowState.Minimized;           // Thu nhỏ của sổ ngay từ lúc đầu mở ra
            base.ShowInTaskbar = false;                             // Không hiển thị trên thanh Taskbar
            base.Visible = true;                                    // Ẩn chương trình
            base.Hide();
            AddMenu();                                              // Tạo menu
            this.thisComputer.CPUEnabled = true;
            this.thisComputer.GPUEnabled = true;
            this.thisComputer.HDDEnabled = true;
            this.thisComputer.MainboardEnabled = true;
            this.thisComputer.RAMEnabled = true;
            this.thisComputer.Open();
            XmlDocument file = new XmlDocument();                   // Đọc địa chỉ từ file xml
            file.Load("ipAddress.xml");
            XmlNode ip = file.SelectNodes("/data/Address")[0];      // Chọn node Address
            if (ip.InnerText == "")                                 // Chưa có địa chỉ IP trong file
                AppIcon.ShowBalloonTip(5000, "Lỗi kêt nối", "Không có địa chỉ IP để kết nối\r\nYêu cầu nhập địa chỉ IP của server", ToolTipIcon.Warning);
            else
            {
                IPAddr.Text = ip.InnerText;                             // Đọc địa chỉ
                client.ConnectAsync(ip.InnerText, 80).Wait(1000);       // Thử kết nối với server, timeout 5000ms
                if (client.Connected)
                {
                    IPAddr.ReadOnly = true;
                    connectButton.Text = "Disconnect";
                    timer.Enabled = true;
                }
                else
                {
                    AppIcon.ShowBalloonTip(5000, "Lỗi kêt nối", "Không tìm thấy server", ToolTipIcon.Warning);
                    client.Close();
                    client = new TcpClient();
                }
            }
        }

        /*---------------------------------------Tạo menu---------------------------------------*/
        private void AddMenu()
        {
            this.AppIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.AppIcon.ContextMenuStrip.Items.Add("Connect", null, this.OpenConnectForm);
            this.AppIcon.ContextMenuStrip.Items.Add("About", null, this.OpenAboutForm);
            this.AppIcon.ContextMenuStrip.Items.Add("Exit", null, this.Exit);
        }
        private void OpenConnectForm(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Normal;  // Mở cửa sổ Connect
            base.ShowInTaskbar = true;
            base.Visible = false;
            base.Show();
        }

        private void OpenAboutForm(object sender, EventArgs e)      // Mở cửa sổ About
        {
            new About().Show();
        }
        private void Exit(object sender, EventArgs e)               // Thoát chương trình
        {
            Application.Exit();
            AppIcon.Dispose();
        }
        /*--------------------------------------------------------------------------------------*/

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (connectButton.Text == "Connect")
            {
                status.Text = "Connecting...";
                status.ForeColor = Color.Black;
                connectButton.Enabled = false;
                client.ConnectAsync(IPAddr.Text, 80).Wait(5000);       // Thử kết nối với server, timeout 5000ms
                if (client.Connected)
                {
                    IPAddr.Enabled = false;
                    connectButton.Text = "Disconnect";
                    status.Text = "Connected";
                    status.ForeColor = Color.Green;
                    XmlDocument file = new XmlDocument();                   // Load file XML
                    file.Load("ipAddress.xml");
                    XmlNode ip = file.SelectNodes("/data/Address")[0];      // Chọn node Address
                    ip.InnerText = IPAddr.Text;                                 // Thay đổi địa chỉ
                    file.Save("ipAddress.xml");                                        // Lưu địa chỉ IP
                    timer.Enabled = true;
                }
                else
                {
                    AppIcon.ShowBalloonTip(5000, "Lỗi kêt nối", "Không tìm thấy server", ToolTipIcon.Warning);
                    status.Text = "Disconnected";
                    status.ForeColor = Color.Red;
                    client.Close();
                    client = new TcpClient();
                }
            }
            else
            {
                IPAddr.Enabled = true;
                timer.Enabled = false;
                connectButton.Text = "Connect";
                status.Text = "Disconnected";
                status.ForeColor = Color.Red;
                client.Close();
                client = new TcpClient();
            }
            connectButton.Enabled = true;
        }


        public class Infomation
        {
            public Processor CPU { get; set; }
            public double RAM { get; set; }
            public Graphic GPU { get; set; }
        }

        public class Processor
        {
            public string Name { get; set; }
            public double Load { get; set; }
            public double Temp { get; set; }
        }

        public class Graphic
        {
            public string Name { get; set; }
            public double Load { get; set; }
            public double Temp { get; set; }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            /*-------------------------------------Đọc thông số-------------------------------------*/
            String cpuName = "", gpuName = "";
            float cpuLoad = 0, cpuTemp = 0, ramLoad = 0, gpuLoad = 0, gpuTemp = 0;
            foreach (var hardware in thisComputer.Hardware)
            {
                hardware.Update();
                /*---------------------------------CPU---------------------------------*/
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    cpuName = hardware.Name;
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                            cpuLoad = sensor.Value.Value;
                        if (sensor.SensorType == SensorType.Temperature)
                            cpuTemp = sensor.Value.GetValueOrDefault();
                    }
                }

                /*---------------------------------RAM---------------------------------*/
                if (hardware.HardwareType == HardwareType.RAM)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                            ramLoad = sensor.Value.Value;
                    }
                }

                /*---------------------------------GPU---------------------------------*/
                if (hardware.HardwareType == HardwareType.GpuAti || hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    gpuName = hardware.Name;
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                            gpuLoad = sensor.Value.Value;
                        if (sensor.SensorType == SensorType.Temperature)
                            gpuTemp = sensor.Value.GetValueOrDefault();
                    }
                }
            }
            /*------------------------------Serialize------------------------------*/
            Processor dataCPU = new Processor
            {
                Name = cpuName,
                Load = Math.Round(cpuLoad, 1),
                Temp = Math.Round(cpuTemp, 1)
            };
            Graphic dataGPU = new Graphic
            {
                Name = gpuName,
                Load = Math.Round(gpuLoad, 1),
                Temp = Math.Round(gpuTemp, 1)
            };
            Infomation info = new Infomation
            {
                CPU = dataCPU,
                RAM = Math.Round(ramLoad, 1),
                GPU = dataGPU
            };
            string obj = JsonConvert.SerializeObject(info);
            /*---------------------------------------------------------------------------------------*/
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(obj + "\r\n");
                stream.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                AppIcon.ShowBalloonTip(5000, "Mất kêt nối", "Kiểm tra lại server", ToolTipIcon.Warning);
                IPAddr.Enabled = true;
                timer.Enabled = false;
                connectButton.Text = "Connect";
                status.Text = "Disconnected";
                status.ForeColor = Color.Red;
                client.Close();
                client = new TcpClient();
            }
        }
    }
}
