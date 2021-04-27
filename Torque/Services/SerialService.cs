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
using System.Collections.Concurrent;

namespace Torque.Services
{
    public class SerialService : ICmd
    {
        private SerialPort serialPort;
        private static SerialService _Singleton = null;
        private static object Singleton_Lock = new object();
        private sys_option _sys_serial = new sys_option();
        public event Action<sys_receive_data> Receive_Result;
        public event Action<sys_message> Event_Msg;
        private sys_message msg = new sys_message();
        /// <summary>
        /// 保持读取开关
        /// </summary>
        bool _keepReading = true;
        /// <summary>
        /// 字符串队列
        /// </summary>
        ConcurrentQueue<string> _cq = new ConcurrentQueue<string>();
        
        private SerialService()
        {
            serialPort = new SerialPort();
        }

        public static SerialService CreateInstance(sys_option serial_option)
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
                    //serialPort.DataReceived += SerialPort_DataReceived;
                    serialPort.Open();
                    //开启线程读取串口数据
                    Thread trdRead = new Thread(SerialPortRead);
                    trdRead.IsBackground = true;
                    trdRead.Start();
                    //另开线程分析读取到的数据
                    Thread trdDataAnalysis = new Thread(DataAnalysis);
                    trdDataAnalysis.IsBackground = true;
                    trdDataAnalysis.Start();
                }
                catch (Exception ex)
                {
                    msg.msgtype = Common.Common.MessageType.Error;
                    msg.messageinfo = ex.Message;
                    msg.is_success = false;
                    Event_Msg?.Invoke(msg);
                }
            }
        }
        /// <summary>
        /// 设置工作模式
        /// </summary>
        public Common.Common.WorkMode WorkType
        {
            set
            {
                _Singleton._sys_serial.WorkType = value;
            }
            get
            {
                return _Singleton._sys_serial.WorkType;
            }
        }
        /// <summary>
        /// 通信方式
        /// </summary>
        public Common.Common.ComType CommuitType
        {
            get
            {
                return _Singleton._sys_serial.CommuitType;
            }
            set
            {
                _Singleton._sys_serial.CommuitType = value;
            }
        }
        /// <summary>
        /// 数据解析逻辑
        /// </summary>
        /// <param name="strResult"></param>
        private void ReceiveData_Logic(string strResult)
        {
            sys_receive_data return_data = new sys_receive_data();
            bool ok = strResult.IndexOf("RE003,OK") >= 0 ? true : false;
            bool error = strResult.IndexOf("RE004,ERROR") >= 0 ? true : false;
            bool e10 = strResult.IndexOf("E10") >= 0 ? true : false;
            bool re = strResult.IndexOf("RE,") >= 0 ? true : false;
            //接收发送指令后的返回值
            if (ok || error || e10)
            {
                sys_send_result ret = new sys_send_result();
                string[] r = strResult.Replace("\r\n", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (r.Length == 2)
                {
                    ret.code = r[0];
                    ret.msg = r[1];
                }
                switch (r[0])
                {
                    case "RE003":
                        ret.txt = "设置成功";
                        msg.msgtype = Common.Common.MessageType.Info;
                        msg.messageinfo = ret.txt;
                        msg.is_success = true;
                        Event_Msg?.Invoke(msg);
                        break;
                    case "RE004":
                        ret.txt = "设定值错误";
                        msg.msgtype = Common.Common.MessageType.Error;
                        msg.messageinfo = ret.txt;
                        msg.is_success = false;
                        Event_Msg?.Invoke(msg);
                        break;
                    case "E10":
                        ret.txt = "设定值错误,送信指令的前端没有附加AT指令";
                        msg.msgtype = Common.Common.MessageType.Error;
                        msg.messageinfo = ret.txt;
                        msg.is_success = false;
                        Event_Msg?.Invoke(msg);
                        break;
                    default:
                        ret.txt = strResult;
                        msg.msgtype = Common.Common.MessageType.Info;
                        msg.messageinfo = ret.txt;
                        msg.is_success = false;
                        Event_Msg?.Invoke(msg);
                        break;
                }
                return_data.ReturnType = Common.Common.ReturnDataType.SettingRet;
                return_data.Send_Result = ret;                
            }
            //接收仪器主动发送的值
            else if (re)
            {
                List<string> data = strResult.Replace("\r\n", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                switch (_Singleton._sys_serial.CommuitType)
                {
                    case Common.Common.ComType.M3ID:
                        return_data.Head = data[0];
                        return_data.SeqNo = data[1];
                        return_data.Torque_data = data[2];
                        return_data.Torque_unit = data[3];
                        return_data.Angle_data = data[4];
                        return_data.Ang_unit = data[5];
                        return_data.Result = data[6];
                        return_data.Sn = data[7];
                        switch (_Singleton._sys_serial.WorkType)
                        {
                            case Common.Common.WorkMode.Torque:
                                return_data.IsOk = data[6].Substring(0, 1) == "O";
                                break;
                            case Common.Common.WorkMode.Ang:
                                return_data.IsOk = data[6].Substring(1, 1) == "O";
                                break;
                            case Common.Common.WorkMode.Both:
                                return_data.IsOk = data[6] == "OO";
                                break;
                            default:
                                break;
                        }
                        break;
                    case Common.Common.ComType.M3:
                        return_data.Head = data[0];
                        return_data.SeqNo = data[1];
                        return_data.Torque_data = data[2];
                        break;
                    default:
                        break;
                }
                return_data.ReturnType = Common.Common.ReturnDataType.ProduceRet;
            }
            else
            {

            }

            Receive_Result?.Invoke(return_data);
        }
        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] m_recvBytes = new byte[serialPort.BytesToRead]; //定义缓冲区大小  
                int result = serialPort.Read(m_recvBytes, 0, m_recvBytes.Length); //从串口读取数据  
                if (result <= 0)
                    return;
                string strResult = Encoding.UTF8.GetString(m_recvBytes, 0, m_recvBytes.Length); //对数据进行转换  
                Console.WriteLine(strResult);
                Tool.Write_File(strResult);
                serialPort.DiscardInBuffer();
                
            }
            catch (Exception ex)
            {
                msg.msgtype = Common.Common.MessageType.Error;
                msg.messageinfo = ex.Message;
                msg.is_success = false;
                Event_Msg?.Invoke(msg);
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

        /// <summary>
        /// 串口读取方法
        /// </summary>
        void SerialPortRead()
        {
            while (_keepReading)
            {
                if (serialPort == null)
                {
                    msg.msgtype = Common.Common.MessageType.Error;
                    msg.messageinfo = "串口对象未实例化";
                    msg.is_success = false;
                    Event_Msg?.Invoke(msg);
                    Thread.Sleep(3000); //3秒后重新检测
                    continue;
                }
                if (!serialPort.IsOpen)
                {
                    msg.msgtype = Common.Common.MessageType.Error;
                    msg.messageinfo = "串口未打开";
                    msg.is_success = false;
                    Event_Msg?.Invoke(msg);
                    Thread.Sleep(3000);
                    continue;
                }

                try
                {
                    #region  按行读取
                    string buffer = serialPort.ReadLine();
                    if (!string.IsNullOrEmpty(buffer)) //可以不加判断，允许添加null值，数据解析时，再做判断。
                    {
                        _cq.Enqueue(buffer);
                    }
                    #endregion
                }
                catch (TimeoutException) //注意超时时间的定义
                {
                    msg.msgtype = Common.Common.MessageType.Error;
                    msg.messageinfo = "串口读取超时";
                    msg.is_success = false;
                    Event_Msg?.Invoke(msg);
                }
                catch (Exception ex) //排除隐患后可以去掉。
                {
                    msg.msgtype = Common.Common.MessageType.Error;
                    msg.messageinfo = $"串口读取异常:{ex.Message}";
                    msg.is_success = false;
                    Event_Msg?.Invoke(msg);
                }

            }
        }

        /// <summary>
        /// 数据解析
        /// </summary>
        void DataAnalysis()
        {
            while (true)
            {
                Thread.Sleep(10);
                if (_cq.IsEmpty)
                {
                    continue;
                }
                #region  按行读取
                for (int i = 0; i < _cq.Count; i++)
                {
                    string strTmp = "";
                    _cq.TryDequeue(out strTmp);
                    ReceiveData_Logic(strTmp);
                }
                #endregion
            }

        }
        /// <summary>
        /// 发送指令,设置了密合扭矩需要设置角度上下限，合符判断才会判断角度
        /// </summary>
        /// <param name="cmds"></param>
        public void SendCmd(sys_send_cmd cmd)
        {
            cmd.Torque_Hi = cmd.Torque_Hi.Trim();
            cmd.Torque_Lo = cmd.Torque_Lo.Trim();
            cmd.Unit = cmd.Unit.Trim();
            cmd.Torque_Close = cmd.Torque_Close.Trim();
            cmd.Ang_Second = cmd.Ang_Second.Trim();
            cmd.Ang_Hi = cmd.Ang_Hi.Trim();
            cmd.Ang_Lo = cmd.Ang_Lo.Trim();
            ConcurrentQueue<byte[]> cmd_list = new ConcurrentQueue<byte[]>();
            //设置扭矩上下限
            if (!string.IsNullOrEmpty(cmd.Torque_Hi) && !string.IsNullOrEmpty(cmd.Torque_Lo))
            {
                byte[] torque_cmd = Command.SetTorque_HiLo(cmd.Torque_Hi, cmd.Torque_Lo);
                cmd_list.Enqueue(torque_cmd);
            }
            //密合扭矩设置
            if (!string.IsNullOrEmpty(cmd.Torque_Close))
            {
                byte[] mh_cmd = Command.SetTorque_MH(cmd.Torque_Close);
                cmd_list.Enqueue(mh_cmd);
            }
            //角度设置
            if (
                !string.IsNullOrEmpty(cmd.Ang_Second) &&
                !string.IsNullOrEmpty(cmd.Ang_Hi) &&
                !string.IsNullOrEmpty(cmd.Ang_Lo)
               )
            {
                byte[] ang_cmd = Command.SetAng_D(cmd.Ang_Second,cmd.Ang_Lo,cmd.Ang_Hi);
                cmd_list.Enqueue(ang_cmd);
            }
            //单位设置
            if (!string.IsNullOrEmpty(cmd.Unit))
            {
                Common.Common.TorqueUnit t = (Common.Common.TorqueUnit)Enum.Parse(typeof(Common.Common.TorqueUnit), cmd.Unit);
                byte[] unit_cmd = Command.SetUnit(t);
                cmd_list.Enqueue(unit_cmd);
            }
            if (serialPort.IsOpen)
            {
                while (!cmd_list.IsEmpty)
                {
                    byte[] cmditem;
                    bool isok = cmd_list.TryDequeue(out cmditem);
                    if (isok)
                    {
                        Thread.Sleep(200);
                        serialPort.Write(cmditem, 0, cmditem.Length);
                    }
                }
            }
        }
    }
}
