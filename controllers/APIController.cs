using ProjectGamma_0.models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.CookiePolicy;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Collections.Generic;
using FileSystem = System.IO.File;
using Microsoft.Data.Sqlite;
namespace ProjectGamma_0.controllers;



    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        private readonly DBContextController dbContext;

        public APIController(DBContextController dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Add()
        {
            try
            {
                var person = await Request.ReadFromJsonAsync<PersonUser>(); // Получаем данные в формате JSON
                if (person != null)
                {
                    DBController dbController = new DBController(person.name, person.job, person.login, person.password);

                    // Добавляем строку в json файл
                    JsonHandler json = new JsonHandler();
                    json.AddJsonReg(dbController.name!, dbController.link!);

                    await dbContext.PersonUser.AddAsync(dbController);
                    await dbContext.SaveChangesAsync();

                    // Создаем базу данных для учителя
                    string dbFileName = $"{dbController.link}"+"/DataDb"+".db"; // Используйте link как имя базы данных
                    string dbPath = Path.Combine("wwwroot", dbFileName); // Путь к новой базе данных
                    

                    // Механизм перемещения папок и создание новой
                    string sourcePath = @"wwwroot\BLANK"; // Путь к исходной директории
                    string destinationPath = Path.Combine("wwwroot", dbController.link); // Путь к новой директории
                    try
                    {
                        CallFile callFile = new CallFile();
                        callFile.CopyDirectory(sourcePath, destinationPath);
                        Console.WriteLine("Папка успешно скопирована.");
                        await CreateDatabaseAsync(dbPath); // Создание базы данных и таблиц
                }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при копировании папки: {ex.Message}");
                    }

                    return Ok(person); // Отправляем созданного пользователя в формате JSON
                }
                else
                {
                    Console.WriteLine("Некорректные данные. Проверьте, что все поля заполнены.");
                    return BadRequest("Некорректные данные.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}"); // Логируем ошибку
                return BadRequest("Произошла ошибка.");
            }
        }

        private async Task CreateDatabaseAsync(string dbPath)
        {
            // Создаем базу данных и необходимые таблицы
            using (var connection = new SqliteConnection($"Data Source={dbPath};"))
            {
                await connection.OpenAsync(); // Открываем соединение
                // Создаем таблицы
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Teachers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Position TEXT,
                        Information TEXT,
                        PathFoto TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Schedules (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        FilePath TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Disciplines (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Text TEXT
                    );

                    CREATE TABLE IF NOT EXISTS DZ (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Text TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Konsultations (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Text TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Usps (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        FilePath TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Contacts (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Text TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Competitions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Text TEXT
                    );";
                    await command.ExecuteNonQueryAsync(); // Выполняем команду
                }
            }
        }
    }


public class JsonHandler // Объявление класса JsonHandler, который будет обрабатывать JSON-файл
{
    // 1. Создаем константу jsonFilePath для хранения пути к файлу JSON, где будут храниться данные
    private readonly string jsonFilePath = "wwwroot/JsonReg.json"; // Укажите путь к вашему файлу

    // 2. Метод для добавления новой записи в JSON-файл
    public void AddJsonReg(string FullName, string Link)
    {
        // 3. Переменная для хранения списка записей, считанных из JSON-файла
        List<JsonReg> JsonRegs;

        // 4. Проверка, существует ли файл по указанному пути и не пуст ли он
        if (File.Exists(jsonFilePath) && new FileInfo(jsonFilePath).Length > 0)
        {
            // 5. Если файл существует и не пустой, считываем его содержимое
            var jsonData = File.ReadAllText(jsonFilePath);
            // 6. Десериализуем считанные данные из JSON-формата в список объектов JsonReg
            JsonRegs = JsonConvert.DeserializeObject<List<JsonReg>>(jsonData)!;
        }
        else
        {
            // 7. Если файл не существует или пуст, создаем новый пустой список для хранения записей
            JsonRegs = new List<JsonReg>();
        }

        // 8. Создаем новый объект JsonReg с переданными данными (FullName и Link)
        JsonReg jsonReg = new JsonReg
        {
            FullName = FullName,
            link = Link // Присваиваем значение поля link
        };

        // 9. Добавляем созданный объект JsonReg в список JsonRegs
        JsonRegs.Add(jsonReg);

        // 10. Сериализуем обновленный список JsonRegs в JSON-формат с использованием форматирования
        string updatedJson = JsonConvert.SerializeObject(JsonRegs, Formatting.Indented);

        // 11. Сохраняем полученный JSON обратно в файл
        File.WriteAllText(jsonFilePath, updatedJson);
    }
}
public class CallFile
{
    public void CopyDirectory(string sourceDir, string destDir) //Второй параметр отвечает за линг id
    {
        // Создаем новую директорию
        Directory.CreateDirectory(destDir);

        // Копируем файлы в новой директории
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destDir, Path.GetFileName(file));
            FileSystem.Copy(file, destFile, true); // true для перезаписи, если файл существует
        }

        // Рекурсивно копируем подкаталоги
        foreach (string directory in Directory.GetDirectories(sourceDir))
        {
            string destDirTemp = Path.Combine(destDir, Path.GetFileName(directory));
            CopyDirectory(directory, destDirTemp); // Рекурсивный вызов
        }
    }
}

