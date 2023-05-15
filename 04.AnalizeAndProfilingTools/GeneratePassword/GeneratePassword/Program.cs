using System.Security.Cryptography;

namespace GeneratePassword;

internal class Program
{
    private static void Main(string[] args)
    {
        var generator = new Generator();
        var salt = new byte[16];
        using var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(salt);

        for (var i = 0; i < 50; i++)
        {
            var password = Guid.NewGuid().ToString().Replace("-", "");
            var result = generator.GeneratePasswordHashUsingSalt(password, salt);
            Console.WriteLine($"Password {password} -> {result}");
        }


        Console.ReadLine();
    }

}