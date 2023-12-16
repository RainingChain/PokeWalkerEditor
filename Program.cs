namespace PokeWalkerEditor;

using System;
using PKHeX.Core;
using PokeWalkerEditor.Core;

/*

return from a stroll
    get back pokemon
    display event log

receive gift: transfer all pokemons + items + give watts



 * */



internal class Program
{
    public static byte[]? TryReadAllBytes(string path)
    {
        try
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int numBytesToRead = Convert.ToInt32(fs.Length);
                byte[] oFileBytes = new byte[numBytesToRead];
                fs.Read(oFileBytes, 0, numBytesToRead);
                return oFileBytes;
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The file {path} doesn't exist.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while reading file: " + path);
            Console.WriteLine(ex.ToString());
            return null;
        }
    }

    static void Main(string[] args)
    {
        //var savA = TryReadAllBytes("C:\\Users\\samue\\Downloads\\64k-full-rom.bin");
        var savA = TryReadAllBytes("C:\\Users\\samue\\Downloads\\POKEROM.BIN");
        
        if (savA == null)
            return;

        var savPW = new SAV4PW(savA);
        var b = savPW.Important.OT;
    }
}



