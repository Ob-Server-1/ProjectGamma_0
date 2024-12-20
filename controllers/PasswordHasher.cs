

namespace ProjectGamma_0.controllers
{
    public class PasswordHasher : IPasswordHasher
    {
        //Генерация шифрованного пароля
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        //Првоерка зашифрованного пароля
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashedPassword);
}