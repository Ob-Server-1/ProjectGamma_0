using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Sqlite;

using ProjectGamma_0.models;

namespace ProjectGamma_0.controllers;

public class DBContextController :DbContext
{
    public DBContextController(DbContextOptions <DBContextController> options) 
    : base(options)
    {
      Database.EnsureCreated();
    }
   public DbSet<PersonUserBaseDate> PersonUser { get; set; } = null!; // Регистрируем модель данных в бд (полная версия)
   public DbSet<Teacher> Teachers { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!; // Таблица для расписания
    public DbSet<Disciplines> Disciplines { get; set; } = null!; // Таблица для дисциплин
    public DbSet<DZ> Dzs { get; set; } = null!; // Таблица для домашнего задания
    public DbSet<Kons> Konsultations { get; set; } = null!; // Таблица для консультаций
    public DbSet<Usp> Usps { get; set; } = null!; // Таблица для успешных проектов
    public DbSet<Kontakt> Contacts { get; set; } = null!; // Таблица для контактов
    public DbSet<Konkurs> Competitions { get; set; } = null!; // Таблица для конкурсов

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Конфигурация
    }

}





