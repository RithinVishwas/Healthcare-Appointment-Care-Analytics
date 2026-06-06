// File: src/Healthcare.Infrastructure/Security/Pbkdf2PasswordHasher.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the password hashing service that protects stored credentials using PBKDF2 instead of plain text passwords.
// Security note: credentials and tokens are handled through hashing and JWT-based authentication practices.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Service file: contains reusable business/security logic separated from UI and controllers.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Security.Cryptography;
using Healthcare.Core.Interfaces;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Security
{
    /// <summary>Pbkdf2PasswordHasher belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        /// <summary>Creates a salted PBKDF2 password hash instead of storing the plain password.</summary>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            var salt = new byte[SaltSize];
            // Required namespaces are imported here so this file can use framework and project classes.
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Required namespaces are imported here so this file can use framework and project classes.
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var key = deriveBytes.GetBytes(KeySize);
                return string.Format("{0}:{1}:{2}", Iterations, Convert.ToBase64String(salt), Convert.ToBase64String(key));
            }
        }

        /// <summary>Verifies a user-entered password against the stored hash.</summary>
        public bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            var parts = storedHash.Split(':');
            if (parts.Length != 3)
            {
                return false;
            }

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var expectedKey = Convert.FromBase64String(parts[2]);

            // Required namespaces are imported here so this file can use framework and project classes.
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                var actualKey = deriveBytes.GetBytes(expectedKey.Length);
                return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
            }
        }
    }
}
