using UnityEngine;
using System.Linq;


public static class Utility {
    private static System.Random random = new System.Random();

    public static string RandomStringOfLength(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Utility.random.Next(s.Length)]).ToArray());
    }
}
