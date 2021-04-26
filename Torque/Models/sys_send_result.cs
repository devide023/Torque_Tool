using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torque.Models
{
    /// <summary>
    /// 命令执行后返回的值
    /// </summary>
    public class sys_send_result
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string txt { get; set; }
    }
}
