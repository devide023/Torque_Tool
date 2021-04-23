using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torque.Models
{
    public class sys_receive_data
    {
        /// <summary>
        /// 计数位
        /// </summary>
        public string SeqNo { get; set; }
        /// <summary>
        /// 返回的扭力值
        /// </summary>
        public float Torque_data { get; set; }
        /// <summary>
        /// 返回角度值
        /// </summary>
        public float Angle_data { get; set; }
        /// <summary>
        /// 扭力值单位
        /// </summary>
        public string Torque_unit { get; set; }
        /// <summary>
        /// 角度单位
        /// </summary>
        public string Ang_unit { get; set; }
        /// <summary>
        /// 合否判定结果
        /// </summary>
        public string Result { get; set; }
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
        public string Digit7{ get; set; }
    }
}
