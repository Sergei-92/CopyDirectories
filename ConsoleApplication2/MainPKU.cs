using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

//Пространство имен для файла конфигурации
using System.Configuration;
using System.Collections.Specialized;

using System.Diagnostics;


namespace ConsoleApplication2
{
    class MainPKU
    {
        //Метод для принудителного завершения программы
        public static void  ClosePrgramm()
        {
            Process.GetCurrentProcess().Kill();
        }

        //Считываю параметры из Config.ini файла
        private static void configINI()
        {
            ReadINI read = new ReadINI(Environment.CurrentDirectory+@"\Config.ini");

            if (read.Path != null)
            {
                String[] param = new String[4];

                String year = read.GetPrivateString("messagePKU", "year");
                param[0] = year;

                String path_from = read.GetPrivateString("messagePKU", "path_from");
                param[1] = path_from;

                String path_to = read.GetPrivateString("messagePKU", "path_to");
                param[2] = path_to;

                String path_archive = read.GetPrivateString("messagePKU", "path_archive");
                param[3] = path_archive;

                Console.WriteLine("Считываение параметров из Config.ini : Успех");
                File.AppendAllText("log.txt", DateTime.Now + " Считывание пареметров из Config.ini : Успех\r\n");

                PKUMessages mqportal = new PKUMessages();
                //Вызываем метод для копирования папок из mq_portal
                mqportal.CopyFilesCatalogMQPortal(param);
            }
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

        //Считываю параметры из App.config файла
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

            bool flag = true; //Флаг отвечающий за файл конфигурации true-Config.ini false-App.config


            String pathProgramm = Environment.CurrentDirectory; //Определяем путь где находится программа
            String log = pathProgramm + @"\log.txt";
            File.Delete(log);
            File.AppendAllText("log.txt", DateTime.Now+" Запуск программы\r\n");


            if (flag)
            {
                configINI();
            }
            else
            {
                configXML();
            }
            
            
        }

    }

}
