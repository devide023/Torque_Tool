using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torque.Models
{
    public class sys_send_data
    {
        /// <summary>
        /// 扭力值上限
        /// </summary>
        public float Hight { get; set; }
        /// <summary>
        /// 扭力值下限
        /// </summary>
        public float Low { get; set; }
        /// <summary>
        /// 角度上限
        /// </summary>
        public float Ang_Hi { get; set; }
        /// <summary>
        /// 角度下限
        /// </summary>
        public float Ang_Lo { get; set; }
    }
}
