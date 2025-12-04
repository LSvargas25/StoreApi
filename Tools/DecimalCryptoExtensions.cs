using StoreApi.Tools;

public static class DecimalCryptoExtensions
{
    public static byte[] EncryptDecimalToBytes(this AesCrypto crypto, decimal value)
    {
        string decimalStr = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string encrypted = crypto.Encrypt(decimalStr);
        return Convert.FromBase64String(encrypted);
    }

    public static decimal DecryptDecimalFromBytes(this AesCrypto crypto, byte[] encryptedBytes)
    {
        if (encryptedBytes == null || encryptedBytes.Length == 0) return 0;
        string base64 = Convert.ToBase64String(encryptedBytes);
        string decrypted = crypto.Decrypt(base64);
        return decimal.Parse(decrypted, System.Globalization.CultureInfo.InvariantCulture);
    }
}
