using System;
using System.IO;

namespace PullJSON
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            for(int i=0; i<args.GetLength(0); i++)
            {
                int braceCount = 0; //Количество встреченных скобок. Предполагаем, что JSON находится между двумя скобками {}.
                //так могут попасться куски с обычным js, но ваять с нуля анализатор синтаксиса json не особо охота. 
                StreamWriter sw = new StreamWriter(args[0] + "PulledJSON", false,System.Text.Encoding.Default);//Записываем в на диск на лету, чтобы не тратить память
                if (File.Exists(args[0]))
                {
                    StreamReader sr = new StreamReader(args[0], System.Text.Encoding.Default);

                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            for(int k=0; k<line.Length; k++)
                            {
                                if (line[k] == '{') braceCount++;
                                else if (line[k] == '}') braceCount--;

                                if (braceCount > 0) sw.Write(line[k]);
                            }
                        }
                    sr.Close();
                    
                }
                sw.Close();
            }
        }
    }
}
