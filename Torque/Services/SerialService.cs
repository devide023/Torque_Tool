﻿using System;
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
        public sys_serial Serial_Option { get; set; } = new sys_serial();
        public event EventHandler<ReceiveData_EventArgs> Received_Data;
        private SerialService()
        {
            serialPort = new SerialPort();
        }

        public static SerialService CreateInstance()
        {
            lock (Singleton_Lock)
            {
                if (_Singleton == null)
                {
                    _Singleton = new SerialService();
                }
            }
            return _Singleton;
        }
        public void Init()
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = Serial_Option.PortName;
                    serialPort.BaudRate = Serial_Option.BaudRate;
                    serialPort.DataBits = Serial_Option.DataBits;
                    serialPort.StopBits = Serial_Option.StopBits;
                    serialPort.Parity = Serial_Option.Parity;
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
                string strResult = Encoding.ASCII.GetString(m_recvBytes, 0, m_recvBytes.Length); //对数据进行转换  
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

        public void SendCmd(byte[] cmd)
        {
            if (serialPort.IsOpen)
            {
                Thread.Sleep(200);
                serialPort.Write(cmd, 0, cmd.Length);
            }
        }

        public void Close() 
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}