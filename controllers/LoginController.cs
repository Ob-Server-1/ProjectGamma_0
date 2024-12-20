using Microsoft.AspNetCore.Mvc;
using ProjectGamma_0.models; // Подключение пространства имен для вашей модели
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectGamma_0.controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly DBContextController _context; // Контекст базы данных
        private readonly PasswordHasher _passwordHasher; // Ваш кастомный хешер для паролей
        private readonly JwtProvider _jwtProvider; // Провайдер для генерации JWT токенов

        // Конструктор контроллера
        public AuthController(DBContextController context, PasswordHasher passwordHasher, JwtProvider jwtProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher; // Используем ваш хешер
            _jwtProvider = jwtProvider; // Используем ваш JWT провайдер
        }

        // DTO для входа
        public class LoginDto
        {
            public string Login { get; set; } // Логин пользователя
            public string Password { get; set; } // Пароль пользователя
        }

        // Метод для аутентификации пользователя
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Проверка на пустые логин и пароль
                if (string.IsNullOrEmpty(loginDto.Login) || string.IsNullOrEmpty(loginDto.Password))
                {
                    return BadRequest("Логин и пароль не могут быть пустыми.");
                }

                // Поиск пользователя в базе данных
                var user = await _context.PersonUser.FirstOrDefaultAsync(u => u.login == loginDto.Login);
                if (user == null)
                {
                    Console.WriteLine("Пользователь не найден."); // Логируем, если пользователь не найден
                    return Unauthorized("Логин не найден.");
                }

                // Проверка правильности пароля
                bool isPasswordValid = _passwordHasher.Verify(loginDto.Password, user.password);
                if (!isPasswordValid)
                {
                    Console.WriteLine("Некорректный пароль."); // Логируем информацию, если пароль неверный
                    return Unauthorized("Некорректный пароль.");
                }

                // Генерация JWT токена
                var token = _jwtProvider.GenerateToken(user);
                return Ok(new { Token = token }); // Возвращаем токен
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время логина: {ex.Message}"); // Логируем сообщение об ошибке
                return StatusCode(500, "Произошла ошибка на сервере."); // Возврат ответа об ошибке
            }
        }
    }
}
