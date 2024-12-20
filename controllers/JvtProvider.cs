using System;
using System.IdentityModel.Tokens.Jwt; // Подключение пространства имен для работы с JWT
using System.Security.Claims; // Подключение пространства имен для работы с claims
using System.Text;
using Microsoft.IdentityModel.Tokens; // Подключение пространства имен для работы с токенами
using ProjectGamma_0.models; // Подключение пространства имен для моделей

namespace ProjectGamma_0
{
    public class JwtProvider
    {
        private readonly string _secretKey; // Секретный ключ для подписи токенов
        public JwtProvider(string secretKey)
        {
            _secretKey = secretKey; // Инициализация поля секретным ключом
        }
        public string GenerateToken(PersonUserBaseDate user)
        {
            // Создание списка claims, представляющих информацию о пользователе
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.login), // Claim для имени пользователя
            };

            // Создание ключа для подписи токена с использованием секретного ключа
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)); // Используем обычный секретный ключ
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Подпись с использованием HMAC SHA-256

            // Создание JWT токена, включая claims и срок действия
            var token = new JwtSecurityToken(
                claims: claims, // Claims для токена
                expires: DateTime.Now.AddHours(240), // Установить срок действия токена на 1 час
                signingCredentials: creds // Передача ключа и алгоритма для подписи
            );

            // Возвращение токена в виде строки
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
