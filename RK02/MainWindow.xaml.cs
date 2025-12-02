using System.Linq;
using System.Windows;
using MovieCatalogApp.Data;
using RK02.Models;

namespace RK02
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
            * Класс FakeDatabase представляет собой эмуляцию работы Entity Framework (EF) с базой данных.
            * Вместо реального подключения к серверу БД (SQL Server, SQLite и т.п.) он использует JSON-файлы,
            * расположенные в папке "Data" проекта, в качестве источника данных.
            * 
            * ❖ Принцип работы:
            *   - При запуске приложения FakeDatabase загружает данные из JSON-файлов (movies.json, genres.json, users.json, roles.json).
            *   - Все изменения с объектами (добавление, удаление, обновление) выполняются через методы Add(), Remove() или Entry().
            *   - После вызова SaveChanges() все изменения сохраняются обратно в JSON-файлы, имитируя поведение EF-контекста.
            * 
            * ❖ Поведение аналогично ADO.NET Entity Framework:
            *   - Можно выполнять LINQ-запросы: db.Movies.Where(...), db.Users.FirstOrDefault(...), и т.д.
            *   - Можно добавлять новые записи через db.Add(entity) и сохранять изменения через db.SaveChanges().
            *   - Для обновления сущностей используется объект Entry() и установка состояния Modified.
            * 
            * ❖ Пример обновления записи:
            *     var db = FakeDatabase.GetContext();
            *     var user = db.Users.FirstOrDefault(u => u.Id == 1);
            *     if (user != null)
            *     {
            *         user.Name = "Новое имя";
            *         db.Entry(user).State = FakeDatabase.FakeEntityState.Modified;
            *         db.SaveChanges();
            *     }
            * 
            * ❖ Примечание:
            *   FakeDatabase работает полностью в рамках файловой системы, без реальной БД.
            *   Это упрощённая, но функциональная модель контекста данных для тестирования или учебных целей.
        */

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
