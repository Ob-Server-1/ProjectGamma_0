namespace ProjectGamma_0.models;

public class Teacher //Пример с автаркой и инофрмацией
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public string Information { get; set; }
    public string PathFoto { get; set; }
}

public class Schedule //Пример с документами
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FilePath { get; set; }
}
public class Disciplines // Модель только с текстом
{
    public int Id { get; set; }
    public string text { get; set; }
}
public class DZ
{
    public int Id { get; set; }
    public string text { get; set; }
}
public class Kons
{
    public int Id { get; set; }
    public string text { get; set; } 
}
public class Usp
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FilePath { get; set; }
}
public class Kontakt
{
    public int Id { get; set; }
    public string Text { get; set; }
}
public class Konkurs
{
    public int Id { get; set; }
    public string Text { get; set; }
}
