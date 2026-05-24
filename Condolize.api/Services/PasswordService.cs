namespace Condolize.api.Services
{
    public class PasswordService
    {

        public string HashPassword(string password)
        {
            // Implement a secure hashing algorithm, e.g., using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            // Verify the password against the hash
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

    }
}
