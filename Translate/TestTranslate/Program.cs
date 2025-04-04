using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslateLibrary;

namespace TestTranslate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            test("1201", 3, 10, 0, "46");
            test("1151", 16, 10, 0, "4433");
            test("ZZZ", 36, 11, 0, "32064");
            test("XYZ,ABC", 36, 22, 12, "42L5,66EJE4FCCII6");
            test("RD5D0E,2006", 28, 14, 5, "46B11A10,10005");
            test("404", 5, 26, 0, "40");
            test("L5E0", 17, 4, -4, "HAHA");
            test("A7G", 17, -2, 0, "2KX");
            test("L5E0", 33, 4, -4, "W9J7");
            Console.Read();
        }

        static bool Check(string n, int p, int q, int m)
        {
            if (p < 2 || p > 36 || q < 2 || q > 36)
            {
                Console.WriteLine("Недопустимая система счисления");
                return false;
            }  

            try
            {
                foreach (char d in n)
                {
                    if (Translate.digits.IndexOf(d) > p - 1)
                        throw new System.IndexOutOfRangeException();
                }    
            }
            catch (System.IndexOutOfRangeException)
            { Console.WriteLine("Цифра не соответсвует нужной системе счисления"); return false; }

            if (m < 0)
            {
                Console.WriteLine("Неверно указана точность");
                return false;
            }

            return true;
        }

        static void test(string n, int p, int q, int m, string expected_value)
        {
            string incoming_value = "";
            if (Check(n, p, q, m))
                incoming_value = Translate.TranslateNumberFromToQ(n, p, q, m);
            Console.WriteLine(expected_value==incoming_value);
        }    
    }
}
