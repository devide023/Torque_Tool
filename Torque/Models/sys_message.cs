using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torque.Models
{
    public class sys_message
    {
        public Common.Common.MessageType msgtype { get; set; }
        /// <summary>
        /// true成功，false 失败
        /// </summary>
        public bool is_success { get; set; } = false;
        public string messageinfo { get; set; }
    }
}
