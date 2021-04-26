using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Torque.Common
{
    public static class Tool
    {
        public static void Write_File(string txt)
        {
            string path = Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Close();
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"{DateTime.Now}:{txt}");
                    sw.Flush();
                }
            }
        }
    }
}
