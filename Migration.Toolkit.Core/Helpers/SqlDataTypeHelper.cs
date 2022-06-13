namespace Migration.Toolkit.Core.Helpers;

public class SqlDataTypeHelper
{
    public static string? TruncateString(string? s, int length) => s?[..length];
    public static object? TruncateString(object? s, int length) => s is string str && str.Length > length ? str?[..length] : s; // TODO tk: 2022-06-13 check
}