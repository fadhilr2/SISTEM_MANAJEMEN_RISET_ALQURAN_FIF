using System;

namespace CLI.Views
{
    public static class BaseViews
    {
        public static void PrintHeader()
        {
            Console.WriteLine("====================================");
            Console.WriteLine("SISTEM MANAJEMEN HASIL RISET AL-QURAN FIF");
            Console.WriteLine("====================================");
            Console.WriteLine();
        }

        public static void PrintSubheader(string title)
        {
            Console.WriteLine(title);
            Console.WriteLine("------------------------------------");
        }
    }
}
