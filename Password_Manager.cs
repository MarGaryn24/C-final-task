using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Password_Manager
{
    private const string Salt = "DyL9TS5PalSd"; // Static salt
    private const string FilePath = "Saved_Password.txt";

    public void EncryptAndSavePassword(string password)
    {
        var encryptedPassword = EncryptPassword(password);
        File.WriteAllText(FilePath, encryptedPassword);
    }

    private string EncryptPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + Salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public string BruteForceAttack()
    {
        var encryptedPassword = File.ReadAllText(FilePath);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        string foundPassword = null;
        var possibleComponents = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Parallel.ForEach(GeneratePossiblePasswords(possibleComponents, 6), (password, state) =>
        {
            if (EncryptPassword(password) == encryptedPassword)
            {
                foundPassword = password;
                state.Stop();
                stopwatch.Stop(); // Stop the stopwatch when the password is found
            }
        });

        return foundPassword != null ? $"Password found: {foundPassword} in {stopwatch.Elapsed}" : "Password not found";
    }

    private IEnumerable<string> GeneratePossiblePasswords(string chars, int maxLength)
    {
        var charArray = chars.ToCharArray();
        for (int length = 1; length <= maxLength; length++)
        {
            foreach (var combination in GetCombinations(charArray, length))
            {
                yield return new string(combination);
            }
        }
    }

    private IEnumerable<char[]> GetCombinations(char[] chars, int length)
    {
        if (length == 1)
        {
            foreach (var c in chars)
            {
                yield return new[] { c };
            }
        }
        else
        {
            foreach (var c in chars)
            {
                foreach (var combination in GetCombinations(chars, length - 1))
                {
                    yield return (new[] { c }).Concat(combination).ToArray();
                }
            }
        }
    }
}
