using Serilog;
using System.Security.Cryptography;
using System.Text;
using MonsterTradingCardsGame.DataLayer;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Server
{
    public static class SecurityHelper 
    {
        public static string CreateToken(string username)
        {
            DateTime validUntil = DateTime.Now.AddHours(4);
            string toEncrypt = $"{username}.{validUntil}";
            var token = EncryptString(toEncrypt);

            using (var unit = new UnitOfWork())
            {
                var user = unit.UserRepository().GetByUsername(username);
                if (user == null) throw new InvalidDataException();

                unit.TokenRepository().Add(new AuthToken(user.Id, validUntil));
                Log.Information($"Generated and saved token for username={username}!");
            }
            return token;
        }

        public static string sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                var mapper = Program.GetConfigMapper();
                if (mapper == null) throw new NullReferenceException();

                aes.Key = Encoding.UTF8.GetBytes(mapper.Secret);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                var mapper = Program.GetConfigMapper();
                if (mapper == null) throw new NullReferenceException();

                aes.Key = Encoding.UTF8.GetBytes(mapper.Secret);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static bool VerifyToken(string tokenString)
        {
            try
            {
                string decryptedToken = DecryptString(tokenString);
                var splitted = decryptedToken.Split(".");

                Guid? userGuid;
                DateTime? validityOfToken;
                using (var unit = new UnitOfWork())
                {
                    userGuid = unit?.UserRepository()?.GetByUsername(splitted[0])?.Id;
                    validityOfToken = Convert.ToDateTime(unit?.TokenRepository()?.GetById(userGuid)?.Validity);
                }
                if(userGuid == null || validityOfToken == null)
                {
                    Log.Error($"Failed to retreive token from DB for username={splitted[0]}!");
                    return false;
                }
                
                DateTime validUntil = Convert.ToDateTime($"{splitted[1]}.{splitted[2]}.{splitted[3]}");
                if(!(validityOfToken.Value.ToString() == validUntil.ToString()))
                {
                    Log.Error($"Token timestamp for username={splitted[0]} does not match DB record!");
                    return false;
                }
                if(DateTime.Now > validityOfToken)
                {
                    Log.Error($"Token for username={splitted[0]} expired!");
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Guid? GetUserIdFromToken(string token)
        {
            if (!VerifyToken(token))
            {
                return null;
            }
            string decryptedToken = DecryptString(token);
            var splitted = decryptedToken.Split(".");
            using (var unit = new UnitOfWork())
            {
                return unit?.UserRepository()?.GetByUsername(splitted[0])?.Id;
            }
        }

        public static User? GetUserFromToken(string token)
        {
            if(!VerifyToken(token))
            {
                return null;
            }
            string decryptedToken = DecryptString(token);
            var splitted = decryptedToken.Split(".");
            using (var unit = new UnitOfWork())
            {
                return unit?.UserRepository()?.GetByUsername(splitted[0]);
            }
        }

    }
}
