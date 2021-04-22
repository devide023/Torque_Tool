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
        /// 返回的扭力值
        /// </summary>
        public float Torque_data { get; set; }
        /// <summary>
        /// 角度
        /// </summary>
        public float Angle_data { get; set; }
        /// <summary>
        /// 扭力值单位
        /// </summary>
        public string Torque_unit { get; set; }
        /// <summary>
        /// 合验结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 获取值的时间
        /// </summary>
        public DateTime Dttime { get {
                return DateTime.Now;
            }  }
    }
}
