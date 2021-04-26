using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque.Common;
namespace Torque.Models
{
    public class sys_option
    {
        /// <summary>
        /// 通行方式
        /// </summary>
        public Common.Common.ComType CommuitType { get; set; } = Common.Common.ComType.M3ID;
        /// <summary>
        /// 工作模式
        /// Torque：扭矩，Ang：角度，Both：扭矩和角度模式
        /// </summary>
        public Common.Common.WorkMode WorkType { get; set; } = Common.Common.WorkMode.Both;
        public string PortName { get; set; } = "COM1";
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 9600;
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; } = 8;
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;
        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;
        /// <summary>
        /// 读取数据超时时长
        /// </summary>
        public int ReadTimeout { get; set; }
        /// <summary>
        /// 写数超时时长
        /// </summary>
        public int WriteTimeout { get; set; }

    }
}
