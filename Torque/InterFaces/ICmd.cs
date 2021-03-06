using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Torque.Models;

namespace Torque.InterFaces
{
    public interface ICmd
    {
        /// <summary>
        /// 发送指令到串口
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        void SendCmd(byte[] cmd);
        /// <summary>
        /// 发送指令到串口
        /// </summary>
        /// <param name="cmds"></param>
        void SendCmd(sys_send_cmd cmd);
    }
}
