using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithFiles4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the binary file with student data");
            string filePath = Console.ReadLine();

            try
            {
                List<Student> students = LoadStudentsFromBinaryFile(filePath);
                SaveStudentsToTextFiles(students);
                Console.WriteLine("Students saved to text files successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// метод для считывания данных о студентах из бинарного файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static List<Student> LoadStudentsFromBinaryFile(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();// создаем объект BinaryFormatter для сериализации и десериализации данных
                return (List<Student>)formatter.Deserialize(stream);// десериализуем данные из файла и возвращаем список объектов Student
            }
        }
        /// <summary>
        /// метод для сохранения студентов в текстовые файлы
        /// </summary>
        /// <param name="students"></param>
        static void SaveStudentsToTextFiles(List<Student> students)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);// получаем путь к рабочему столу
            string studentsFolderPath = Path.Combine(desktopPath, "Students");// создаем путь к директории Students на рабочем столе
            Directory.CreateDirectory(studentsFolderPath);

            var groupedStudents = students.GroupBy(s => s.Group);

            foreach (var group in groupedStudents)
            {
                string groupFileName = $"{group.Key}.txt";// создаем имя файла для текущей группы
                string groupFilePath = Path.Combine(studentsFolderPath, groupFileName);// создаем путь к файлу для текущей группы

                using (StreamWriter writer = new StreamWriter(groupFilePath))
                {
                    foreach (var student in group)
                    {
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth:dd.MM.yyyy}, {student.AverageScore}");// записываем данные о студенте в файл
                    }
                }
            }
        }
    }

    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageScore { get; set; }
    }
}

