using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

internal static class Pura
{
    public static void PuraIt(string filePath)
    {
        FileInfo fileInfo = new(filePath);

        if (fileInfo.Extension.Equals(".cs", StringComparison.CurrentCultureIgnoreCase))
        {
            PuraCs(filePath);
        }
    }

    /// <summary>
    /// Main Task:
    /// #if NET8_0
    ///    [SuppressUnmanagedCodeSecurityAttribute]
    ///    internal sealed partial class LibWebp
    ///    {
    ///        [LibraryImport("kernel32.dll", EntryPoint = "RtlCopyMemory", SetLastError = false)]
    ///        internal static partial void CopyMemory(IntPtr dest, IntPtr src, uint count);
    ///    }
    /// #else
    ///    [SuppressUnmanagedCodeSecurityAttribute]
    ///    internal sealed partial class LibWebp
    ///    {
    ///        [DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory", SetLastError = false)]
    ///        internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
    ///    }
    /// #endif
    /// </summary>
    /// <param name="filePath"></param>
    public static void PuraCs(string filePath)
    {
        bool isEdited = false;
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string lineTrimmed = line.Trim();

            if (line.Contains("[LibraryImport(\"libwebp.dll\", EntryPoint = \"WebPF"))
            {
            }

            // Ex. [LibraryImport("libwebp.dll", EntryPoint = "WebPFreeDecBuffer")]
            if (lineTrimmed.StartsWith("[LibraryImport("))
            {
                isEdited = true;

                lines[i] = line.Replace("[LibraryImport(", "[DllImport(");

                // Ex. [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
                if (lines[i + 1].Trim().Contains("UnmanagedCallConv"))
                {
                    lines[i] = line.Replace(")]", ", CallingConvention = CallingConvention.Cdecl)]");
                }
            }
            else if (lineTrimmed.StartsWith("[UnmanagedCallConv("))
            {
                isEdited = true;
                lines[i] = null!;
            }
            else if (lineTrimmed.Contains(" static partial "))
            {
                if ((lines[i - 1]?.Trim().StartsWith("[DllImport(") ?? false) || (lines[i - 2]?.Trim().StartsWith("[DllImport(") ?? false))
                {
                    isEdited = true;
                    lines[i] = line.Replace(" static partial ", " static extern ");
                }
            }
        }

        if (isEdited)
        {
            File.WriteAllLines(filePath, lines.Where(line => line != null));
        }
    }

    private static string IndentStart(this string line)
    {
        if (line.Contains('\t'))
        {
            return new string('\t', line.TakeWhile(c => c == '\t').Count());
        }
        else
        {
            return new string(' ', line.TakeWhile(c => c == ' ').Count());
        }
    }
}
