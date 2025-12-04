using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StoreApi.Tools
{
    public class AesCrypto
    {
        private readonly byte[] Key;

        public AesCrypto(string secretKey)
        {
            Key = SHA256.HashData(Encoding.UTF8.GetBytes(secretKey));
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                Console.WriteLine("⚠️ AesCrypto.Encrypt: plainText está vacío");
                return string.Empty;
            }

            try
            {
                using var aes = Aes.Create();
                aes.Key = Key;
                aes.GenerateIV();

                using var encryptor = aes.CreateEncryptor();
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // Combinar IV + datos cifrados
                var combined = new byte[aes.IV.Length + cipherBytes.Length];
                Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

                return Convert.ToBase64String(combined);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Encrypt: {ex.Message}");
                throw;
            }
        }

        public string Decrypt(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData))
                return string.Empty;

            try
            {
                if (!IsValidBase64(encryptedData))
                    throw new FormatException("Valor no es Base64 válido.");

                var fullBytes = Convert.FromBase64String(encryptedData);
                if (fullBytes.Length < 16)
                    throw new ArgumentException("Datos cifrados corruptos: longitud insuficiente");

                byte[] iv = new byte[16];
                byte[] cipher = new byte[fullBytes.Length - 16];

                Buffer.BlockCopy(fullBytes, 0, iv, 0, 16);
                Buffer.BlockCopy(fullBytes, 16, cipher, 0, cipher.Length);

                using var aes = Aes.Create();
                aes.Key = Key;
                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor();
                var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                throw;
            }
        }

        public string SafeDecrypt(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            try
            {
                return Decrypt(input);
            }
            catch
            {
                return input ?? "";
            }
        }

        public bool IsDataEncrypted(string data)
        {
            if (string.IsNullOrEmpty(data)) return false;
            if (IsNumericString(data) && data.Length <= 12) return false;
            if (!IsValidBase64(data)) return false;

            try
            {
                var decrypted = Decrypt(data);
                return decrypted.Any(c => !char.IsControl(c));
            }
            catch
            {
                return false;
            }
        }

        public string DiagnoseData(string data)
        {
            if (string.IsNullOrEmpty(data)) return "Datos vacíos";
            if (IsNumericString(data)) return $"PROBLEMA: Solo números - '{data}'";
            if (!IsValidBase64(data)) return $"PROBLEMA: No es Base64 - '{data}'";
            if (data.Length < 24) return $"PROBLEMA: Muy corto para ser cifrado - '{data}'";
            return $"OK: Base64 válido - '{data}'";
        }

        private bool IsValidBase64(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length % 4 != 0) return false;
            try { Convert.FromBase64String(value); return true; }
            catch { return false; }
        }

        private bool IsNumericString(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
        }
    }
}
