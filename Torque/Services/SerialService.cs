using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque.InterFaces;
using Torque.Common;
using System.IO.Ports;
using Torque.Models;
using System.Threading;
namespace Torque.Services
{
    public class SerialService : ICmd
    {
        private SerialPort serialPort;
        private static SerialService _Singleton = null;
        private static object Singleton_Lock = new object();
        private sys_serial _sys_serial=new sys_serial();
        public event EventHandler<ReceiveData_EventArgs> Received_Data;
        private SerialService()
        {
            serialPort = new SerialPort();
        }

        public static SerialService CreateInstance(sys_serial serial_option)
        {
            lock (Singleton_Lock)
            {
                if (_Singleton == null)
                {
                    _Singleton = new SerialService();
                    _Singleton._sys_serial = serial_option;
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 连接参数初始化
        /// </summary>
        public void Init()
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = _sys_serial.PortName;
                    serialPort.BaudRate = _sys_serial.BaudRate;
                    serialPort.DataBits = _sys_serial.DataBits;
                    serialPort.StopBits = _sys_serial.StopBits;
                    serialPort.Parity = _sys_serial.Parity;
                    serialPort.Encoding = Encoding.UTF8;
                    serialPort.DataReceived += SerialPort_DataReceived;
                    serialPort.Open();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] m_recvBytes = new byte[serialPort.BytesToRead]; //定义缓冲区大小  
                int result = serialPort.Read(m_recvBytes, 0, m_recvBytes.Length); //从串口读取数据  
                if (result <= 0)
                    return;
                string strResult = Encoding.UTF8.GetString(m_recvBytes, 0, m_recvBytes.Length); //对数据进行转换  
                serialPort.DiscardInBuffer();
                if (Received_Data != null)
                {
                    Received_Data(sender, new ReceiveData_EventArgs { Result=strResult});
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmd"></param>
        public void SendCmd(byte[] cmd)
        {
            if (serialPort.IsOpen)
            {
                Thread.Sleep(200);
                serialPort.Write(cmd, 0, cmd.Length);
            }
        }
        /// <summary>
        /// 关闭与串口的连接
        /// </summary>
        public void Close() 
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}
