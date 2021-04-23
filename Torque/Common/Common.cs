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
        /// 扭矩单位
        /// </summary>
        public enum TorqueUnit
        {
            Nm = 06,
            kgfcm = 02,
            kgfm=03,
            lbfin=08,
            lbfft=09
        }
    }
}
