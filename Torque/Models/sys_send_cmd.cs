using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torque.Models
{
    public class sys_send_cmd
    {
        /// <summary>
        /// 扭矩上限
        /// 包含小数点合计5位
        /// </summary>
        public string Torque_Hi { get; set; } = "";
        /// <summary>
        /// 扭矩下限
        /// 包含小数点合计5位
        /// </summary>
        public string Torque_Lo { get; set; } = "";
        /// <summary>
        /// 密合扭矩
        /// 设定范围为最大扭矩值的5%，可以设置为0
        /// 手动将角度功能设定为ON之后，再进行送信
        /// 包含小数点合计5位
        /// </summary>
        public string Torque_Close { get; set; } = "";
        /// <summary>
        /// 二次紧固角度
        /// 模式为“MODE-M”时无法设定
        /// “MODE-M”模式下进行角度上下限变更时，2次紧固判定角度请发送“000”
        /// 000~999的三位数角度值
        /// </summary>
        public string Ang_Second { get; set; } = "";
        /// <summary>
        /// 角度上限
        /// 000~999的三位数角度值
        /// 设定为大于角度下限和2次紧固判定角度的值，可以设置为0
        /// </summary>
        public string Ang_Hi { get; set; } = "";
        /// <summary>
        /// 角度下限
        /// 000~999的三位数角度值
        /// 设定为大于2次紧固判定角度的值，可以设置为0
        /// </summary>
        public string Ang_Lo { get; set; } = "";
        /// <summary>
        /// 单位设置
        /// N.m：06
        /// kgf•cm：02
        /// kgf•m：03
        /// lbf•in：08
        /// lbf•ft：09
        /// </summary>
        public string Unit { get; set; } = "";
    }
}
