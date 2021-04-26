using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torque.Common
{
    public static class Common
    {
        /// <summary>
        /// 通信类型
        /// M3+ID
        /// M-3
        /// </summary>
        public  enum ComType
        {
            M3ID=1,
            M3=2,
        }
        /// <summary>
        /// 工作模式
        /// Torque：扭矩，Ang：角度，Both：扭矩和角度模式
        /// </summary>
        public enum WorkMode
        { 
            Torque=1,
            Ang = 2,
            Both= 3,
        }
        /// <summary>
        /// 扭矩单位
        /// </summary>
        public enum TorqueUnit
        {
            Nm = 06,
            Kgfcm = 02,
            Kgfm=03,
            Lbfin=08,
            Lbfft=09
        }

        public enum ReturnDataType
        { 
            /// <summary>
            /// 设置返回
            /// </summary>
            SettingRet=1,
            /// <summary>
            /// 生产制造返回
            /// </summary>
            ProduceRet=2,
        }
    }
}
