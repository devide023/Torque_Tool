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
using static Torque.Common.Common;

namespace Torque_Tool
{
    public partial class Form1 : Form
    {
        SerialService s = null;
        public Form1()
        {
            InitializeComponent();
            sys_option option = new sys_option() { 
                PortName="COM4"
            };
            s = SerialService.CreateInstance(option);
            s.Receive_Result += S_Receive_Result;
            s.Init();

            foreach (var item in Enum.GetValues(typeof(Torque.Common.Common.TorqueUnit)))
            {
                this.ddl_unit.Items.Add(new
                {
                    name = item.ToString(),
                    code = Convert.ToInt32(item)
                }) ;
            }

            this.ddl_unit.DisplayMember = "name";
            this.ddl_unit.ValueMember = "code";

        }

        private void S_Receive_Result(sys_receive_data obj)
        {
            switch (obj.ReturnType)
            {
                case ReturnDataType.SettingRet:
                    sys_send_result ret = obj.Send_Result;
                    Console.WriteLine(ret.txt);
                    break;
                case ReturnDataType.ProduceRet:
                    this.tb_c_ang.Text = obj.Angle_data;
                    this.tb_c_ang_unit.Text = obj.Ang_unit;
                    this.tb_c_check.Text = obj.IsOk.ToString();
                    this.tb_c_no.Text = obj.SeqNo.ToString();
                    this.tb_c_result.Text = obj.Result;
                    this.tb_c_sn.Text = obj.Sn;
                    this.tb_c_torque.Text = obj.Torque_data;
                    this.tb_c_torque_unit.Text = obj.Torque_unit;
                    this.tb_c_worktype.Text = s.WorkType.ToString();
                    break;
                default:
                    break;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string up = tb_up.Text;
            string down = tb_down.Text;
            byte[] cmd = Command.SetTorque_HiLo(up, down);
            s.SendCmd(cmd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int unit = ((dynamic)ddl_unit.SelectedItem).code;
            TorqueUnit t = (TorqueUnit)Enum.Parse(typeof(Common.TorqueUnit), unit.ToString());
            byte[] cmd = Command.SetUnit(t);
            s.SendCmd(cmd);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mh = tb_mh.Text;
            s.SendCmd(Command.SetTorque_MH(mh));

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string angd = Convert.ToInt32( this.tb_angd.Text).ToString().PadLeft(3,'0');
            string hi = Convert.ToInt32(this.tb_hi.Text).ToString().PadLeft(3, '0');
            string lo = Convert.ToInt32(this.tb_lo.Text).ToString().PadLeft(3, '0');
            s.SendCmd(Command.SetAng_D(angd, lo, hi));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            s.SendCmd(Command.SetDigit7("ABC1234"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            s.WorkType = WorkMode.Torque;
        }

        private void button6_Click(object sender, EventArgs e)
        {
                s.CommuitType = ComType.M3ID;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            s.WorkType = WorkMode.Ang;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            s.WorkType = WorkMode.Both;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            s.CommuitType = ComType.M3;
        }
    }
}
