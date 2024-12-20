using Microsoft.AspNetCore.Mvc;

namespace ProjectGamma_0.models;

    public class PersonUserBaseDate //Модель для регистрации пользователей в бд
    {
        public Guid Id { get; set; }
        public string? name { get; set; }
        public string? job { get; set; }
        public string? login { get; set; }
        public string? password { get; set; }
        public string? link { get; set; }  
    }
    public class PersonUser //Это модель нужна для 1рвого шага, затем идет преобразование
    {
        public string? name { get; set; }
        public string? job { get; set; }
        public string? login { get; set; }
        public string? password { get; set;}
    }
