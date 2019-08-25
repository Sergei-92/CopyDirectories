using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Пространство имен для файла конфигурации
using System.Configuration;
using System.Collections.Specialized;

using System.Diagnostics;


namespace ConsoleApplication2
{
    class Program
    {
        //Метод для принудителного завершения программы
        public static void  ClosePrgramm()
        {
            Process.GetCurrentProcess().Kill();
        }

      /*  private static void configFile()
        {
            //Определяем путь где находится программа
            String pathConfigure = Environment.CurrentDirectory;

            //Найдем config файл в каталоге программы
            String[] file = Directory.GetFiles(pathConfigure, "*.txt");

            foreach (string path_nameFile in file)
            {
                string nameFile = Path.GetFileName(path_nameFile);

                if (nameFile == "config.txt")
                {
                    string[] lines = File.ReadAllLines(path_nameFile);

                    foreach (string line in lines)
                    { 
                    
                    }

                }
                else
                {
                    //Пишем лог с не успешным запуском копирования
                }
            }
        }*/

        private static void configXML()
        {
            NameValueCollection sAll;
            sAll = ConfigurationManager.AppSettings;
            String[] param = new String[sAll.Count];
            int i = 0;

            //Считываем параметры из config файла
            foreach(string s in sAll.AllKeys)
            {
                string str = ConfigurationManager.AppSettings.Get(s);
                param[i] = str;
                i = i + 1;
            }

            File.AppendAllText("log.txt", DateTime.Now+" Считывание пареметров из App.config: Успех\r\n");


            PKUMessages mqportal = new PKUMessages();
            //Вызываем метод для копирования папок из mq_portal
            mqportal.CopyFilesCatalogMQPortal(param);
        }
        
        static void Main(string[] args)
        {
           
            String pathProgramm = Environment.CurrentDirectory; //Определяем путь где находится программа
            String log = pathProgramm + @"\log.txt";
            File.Delete(log);
            File.AppendAllText("log.txt", DateTime.Now+" Запуск программы\r\n");
            

            configXML();
            
        }


    }
}
