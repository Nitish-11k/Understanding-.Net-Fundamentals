using System.Security.Cryptography;
using System.Text;

namespace TodoApi.Infrastructure.Security
{
    public class LicenseKeyGenerator
    {
        // MOVE THESE TO appsettings.json IN PRODUCTION!
        private readonly byte[] _signKey = Encoding.UTF8.GetBytes("YOUR_VERY_LONG_SECURE_SIGNING_KEY_32_BYTES_REQUIRED_DSECURE");
        private readonly byte[] _encKey = Encoding.UTF8.GetBytes("YOUR_16_BYTE_KEY_DSECURE"); // 16 chars for simple XOR/AES

        public string GenerateKey(DateTime expiry, int type)
        {
            // 1. Pack Payload (Simplified for 24 chars)
            byte[] payload = new byte[7];
            payload[0] = 1; // Version
            BitConverter.GetBytes((int)(expiry - DateTime.UnixEpoch).TotalDays).CopyTo(payload, 1); // 4 bytes date
            payload[5] = (byte)type;
            payload[6] = (byte)RandomNumberGenerator.GetInt32(0, 255); // Entropy

            // 2. Sign (HMAC)
            using var hmac = new HMACSHA256(_signKey);
            byte[] hash = hmac.ComputeHash(payload);
            byte[] signature = hash[0..8]; // Truncate to 8 bytes

            // 3. Combine & Obfuscate
            byte[] token = [.. payload, .. signature]; // 15 Bytes
            
            // Simple XOR for obfuscation (use AES in real prod if needed)
            for(int i=0; i<token.Length; i++) token[i] ^= _encKey[i % _encKey.Length];

            // 4. Encode to Base32 (Custom implementation or library needed for strictly Crockford)
            // For now, using Base64Url to keep it simple for you to run
            return Convert.ToBase64String(token).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }
    }
}