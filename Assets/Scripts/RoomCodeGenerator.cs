using UnityEngine;

public static class RoomCodeGenerator
{
    private const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public static string Generate(int length = 5)
    {
        System.Text.StringBuilder code = new System.Text.StringBuilder();
        for (int i = 0; i < length; i++)
        {
            code.Append(chars[Random.Range(0, chars.Length)]);
        }
        return code.ToString();
    }
}
