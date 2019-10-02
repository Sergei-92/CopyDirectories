using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;



namespace ConsoleApplication2
{
    class ReadINI
    {
        //Конструктор, принимающий путь к INI-файлу
        public ReadINI(string aPath)
        {
            if(File.Exists(aPath))
            {
                path = aPath;
                Console.WriteLine("ini файл найден");
                File.AppendAllText("log.txt", DateTime.Now + " ini файл найден\r\n");
            }
            else
            {
                Console.WriteLine("Проверьте наличие ini файла по пути "+aPath);
                File.AppendAllText("log.txt", DateTime.Now + " Проверьте наличие ini файла по пути " + aPath +"\r\n");
            }  
        }

   
        
        //Конструктор без аргументов (путь к INI-файлу нужно будет задать отдельно)
        public ReadINI() : this("") { }
       
        //Возвращает значение из INI-файла (по указанным секции и ключу) 
        public string GetPrivateString(string aSection, string aKey)
        {
            //Для получения значения
            StringBuilder buffer = new StringBuilder(SIZE);

            //Получить значение в buffer
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

            //Вернуть полученное значение
            return buffer.ToString();
        }


        //Возвращает или устанавливает путь к INI файлу
        public string Path { get { return path; } set { path = value; } }

        //Поля класса
        private const int SIZE = 1024; //Максимальный размер (для чтения значения из файла)
        private string path = null; //Для хранения пути к INI-файлу

        //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);
       

    }
 }
   


