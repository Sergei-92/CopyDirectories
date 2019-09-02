using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace ConsoleApplication2
{
    class PKUMessages
    {
        public void CopyFilesCatalogMQPortal(String[] param)
        {
            //Описание параметров из файла конфигурации
            // param[0] - Дата за которую копировать папки
            // param[1] - От куда копировать папки
            // param[2] - Куда копировать папки
            // param[3] - Куда создават архив

            DirectoryInfo drInfo = new DirectoryInfo(param[2]);
            if (drInfo.Exists)
            {
                Directory.Delete(param[2], true); //Удаляем папку куда скопировали предыдущие файлы
            }

            int year = Int32.Parse(param[0]); // Год когда была создана папка
            //int coutn = 0; //Переменная для подсчета количества папок
            String logError = null;
            //List<String> deleteDirectories = new List<String>(); //Коллекция для хранения папок, которые будут удалены после копирования

            //-------------------------------Для теста на 15 сервере-------------
            File.AppendAllText("log.txt", DateTime.Now + " Копирование из каталога:" + param[1] + "\r\n");
            File.AppendAllText("log.txt", DateTime.Now + " Перенос в каталог:" + param[2] + "\r\n");
            File.AppendAllText("log.txt", DateTime.Now + " Создавать архив в каталог:" + param[3] + "\r\n");
            //--------------------------------------------------------------------
            String[] fileInDirectories = Directory.GetDirectories(param[1], "*", SearchOption.AllDirectories);

            //------------------------------Для теста на 15 сервере-------------------
            File.AppendAllText("log.txt", "Количество каталогов для копирования " + fileInDirectories.Length + "\r\n");
            //---------------------------------------------------------------------------

            if (fileInDirectories.Length != 0)
            {

                foreach (String catalogs in fileInDirectories)
                {
                    DateTime date = Directory.GetCreationTime(catalogs);

                    if (date.Year == year) //Проверяем год создания папки
                    {

                        // First create all of the directories
                        foreach (string dirPath in Directory.GetDirectories(param[1], "*", SearchOption.AllDirectories))
                        {

                            Directory.CreateDirectory(dirPath.Replace(param[1], param[2]));
                            //deleteDirectories.Add(dirPath);


                            //File.AppendAllText("log.txt", DateTime.Now+" Завершение программы. Не найдены каталоги, заданные условиями поиска в файле App.config\r\n");
                            //ClosePrgramm();
                        }
                        try
                        {
                            // Copy all the files
                            foreach (string newPath in Directory.GetFiles(param[1], "*.*", SearchOption.AllDirectories))
                                File.Copy(newPath, newPath.Replace(param[1], param[2]));
                            logError = "Копирование успешно завершино\r\n";
                            //Записываем информацию о копировании в log файл
                            File.AppendAllText("log.txt", DateTime.Now + " " + logError);

                            //После копирования файлов, удаляем ненужные каталоги
                            foreach (String deleteCatalog in fileInDirectories)
                            {
                                Console.WriteLine("Удаляю каталог " + deleteCatalog);
                                Directory.Delete(deleteCatalog, true);
                                logError = "Удалено " + fileInDirectories.Length + " каталога(ов)";

                            }

                            //Создаем архив скопированных папок
                            DirectoryInfo drInfoAr = new DirectoryInfo(param[3]);
                            if(!drInfoAr.Exists)
                            {
                                Directory.CreateDirectory(param[3]);
                            }
                            else
                            {
                                ZipFile.CreateFromDirectory(param[2], param[3] + "\\archiv_" + DateTime.Now.ToString("dd") + "_" + DateTime.Now.ToString("MM") + "_" +
                                    DateTime.Now.ToString("HH") + "_" + DateTime.Now.ToString("mm") + "_" + DateTime.Now.ToString("ss") + "_" + year.ToString() + ".zip");
                            }


                            //Записывем информацию о удалении каталогов в log файл
                            File.AppendAllText("log.txt", DateTime.Now + " " + logError);
                        }
                        catch (Exception)
                        {
                            File.AppendAllText("log.txt", DateTime.Now + " Некоторые папки уже скопированы, удалите папки по пути: " + param[2]+"\r\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет папок для копирования за "+param[0]);
                        File.AppendAllText("log.txt", "Нет папок для копирования за " + param[0]+"\r\n");
                        break;
                    }
                    //coutn = coutn + 1;
                }
            }
            else
            {
                File.AppendAllText("log.txt", DateTime.Now + " Нет папок для копирования из каталога: " + param[1]);
            }

        }
    }
}
