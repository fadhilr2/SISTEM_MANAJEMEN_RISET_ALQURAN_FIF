using Lib.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.services
{
    public static class InputReader
    {
        public static T ReadInput<T>()
        {
            Console.Write(">> ");
            string? input = Console.ReadLine();

            if (typeof(T) == typeof(int))
            {
                try
                {
                    return (T)(object)int.Parse(input ?? string.Empty);
                }
                catch (FormatException)
                {
                    Console.WriteLine(Messages.Get("error.invalid_number"));
                    return ReadInput<T>();
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine(Messages.Get("error.invalid_number"));
                    return ReadInput<T>();
                }
            }

            return (T)(object)(input ?? string.Empty);
        }
    }
}
