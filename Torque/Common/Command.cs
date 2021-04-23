using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Torque.Common.Common;

namespace Torque.Common
{
    public static class Command
    {
        /// <summary>
        /// 设置扭矩单位
        /// cmd:N.m：06，kgf•cm：02，
        /// kgf•m：03，lbf•in：08，lbf•ft：09
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static byte[] SetUnit(TorqueUnit cmd)
        {
            var val = Convert.ToInt32(cmd).ToString().PadLeft(2, '0');
            StringBuilder sb = new StringBuilder();
            sb.Append("AT008");
            sb.Append(",");
            sb.Append( val);
            sb.Append("\r\n");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
        /// <summary>
        ///  设置扭矩上下限。
        /// </summary>
        /// <param name="Hi">扭矩上限(5位长度)</param>
        /// <param name="Lo">扭矩下限5位长度</param>
        /// <returns></returns>
        public static byte[] SetTorque_HiLo(string Hi,string Lo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AT037");
            sb.Append(",");
            sb.Append(Hi);
            sb.Append(",");
            sb.Append(Lo);
            sb.Append("\r\n");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
        /// <summary>
        /// 设置密合扭矩值
        /// 包含小数点合计5位
        /// 设定范围为最大扭矩值的5%~最大扭矩值（可以设置为0）。
        //请手动将角度功能设定为ON之后，再进行送信
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static byte[] SetTorque_MH(string cmd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AT045");
            sb.Append(",");
            sb.Append(cmd);
            sb.Append("\r\n");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
        /// <summary>
        /// 同时进行二次紧固的判定角度，角度上下限的登录。
        /// 依次分别为二次紧固的判定角度，角度下限，角度上限。
        /// </summary>
        /// <param name="ang_d">二次紧固的判定角度000~999</param>
        /// <param name="ang_lo">角度下限000~999</param>
        /// <param name="ang_hi">角度上限000~999</param>
        /// <returns></returns>
        public static byte[] SetAng_D(string ang_d,string ang_lo,string ang_hi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AT046");
            sb.Append(",");
            sb.Append(ang_d); 
            sb.Append(",");
            sb.Append(ang_lo);
            sb.Append(",");
            sb.Append(ang_hi);
            sb.Append("\r\n");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
        /// <summary>
        /// 进行7位英数字的登录。
        /// 登录后的7位英数字将追加在测试数据里进行传输
        /// </summary>
        /// <param name="cmd">7位的大写英文字母或数字</param>
        /// <returns></returns>
        public static byte[] SetDigit7(string cmd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AT046");
            sb.Append(",");
            sb.Append(cmd);
            sb.Append("\r\n");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

    }
}
