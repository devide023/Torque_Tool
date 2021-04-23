using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Torque.Services;
using Torque.Models;
using Torque.Common;
namespace Torque_Tool
{
    public partial class Form1 : Form
    {
        SerialService s = null;
        public Form1()
        {
            InitializeComponent();
            s = SerialService.CreateInstance();
            s.Serial_Option = new sys_serial { PortName = "COM4" };
            s.Received_Data += S_Received_Data;
            s.Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            byte[] cmd = Command.SetTorque_HiLo("15.00", "10.00");
            s.SendCmd(cmd);
        }

        private void S_Received_Data(object sender, ReceiveData_EventArgs e)
        {
            Console.WriteLine(e.Result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] cmd = Command.SetUnit(Common.TorqueUnit.kgfm);
            s.SendCmd(cmd);
        }
    }
}
