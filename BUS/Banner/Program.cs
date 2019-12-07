using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                char[] chars = args[1].ToArray();
                string text = String.Empty;
                int l = chars.Length;
                int num = Convert.ToInt32(args[2]);

                while (true)
                {
                    Console.Clear();
                    char[] reverse = new char[l];
                    
                    for (int i = 0; i < l; i++)
                    {
                        if(i<num)
                        text += chars[i];

                        if (i > 0)
                            reverse[i - 1] = chars[i];
                        else
                            reverse[l - 1] = chars[i];
                    }


                    Console.WriteLine(args[0] + "  " + text);
                    text = String.Empty;
                    chars = reverse;

                    Thread.Sleep(300);
                }
            }
        }
    }
}
