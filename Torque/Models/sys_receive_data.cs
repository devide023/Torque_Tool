using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque.Common;

namespace Torque.Models
{
    public class sys_receive_data
    {
        /// <summary>
        /// 返回数据类型
        /// </summary>
        public Common.Common.ReturnDataType ReturnType { get; set; }
        /// <summary>
        /// 头部
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 计数位
        /// </summary>
        public string SeqNo { get; set; }
        /// <summary>
        /// 返回的扭力值
        /// </summary>
        public string Torque_data { get; set; }
        /// <summary>
        /// 返回角度值
        /// </summary>
        public string Angle_data { get; set; }
        /// <summary>
        /// 扭力值单位
        /// </summary>
        public string Torque_unit { get; set; }
        /// <summary>
        /// 角度单位
        /// </summary>
        public string Ang_unit { get; set; }
        /// <summary>
        /// 合否判定结果，第一位是扭力结果，第二位为角度结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 对判定结果分析
        /// </summary>
        public bool IsOk { get; set; } = false;
        /// <summary>
        /// 获取值的时间
        /// </summary>
        public DateTime Dttime { get {
                return DateTime.Now;
            } }
        /// <summary>
        /// 7位英数字，出产时已固化，类似于产品序列号
        /// 不能通过命令更改
        /// </summary>
        public string Sn{ get; set; }
        /// <summary>
        /// 发送命令返回的结果
        /// </summary>
        public sys_send_result Send_Result { get; set; }
    }
}
