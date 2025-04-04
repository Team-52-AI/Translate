using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateLibrary
{



    /// <summary>
    /// Класс Translate выполняет перевод чисел между системами счисления
    /// </summary>
    public static class Translate
    {
        
        private const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Переводит число N из системы счисления p в систему счисления q.
        /// </summary>
        /// <param name="N"> Число (Число типа int)</param>
        /// <param name="p">Система счисления (Положительное число типа int) в диапазоне 2 <= q <= 36</param>
        /// <param name="q">Система счисления (Положительное число типа int) в диапазоне 2 <= q <= 36</param>
        /// <param name="m">Количество знаков после запятой (Положительное число типа int)</param>
        /// <returns>Число в новой системе исчисления</returns>
        public static string TranslateNumberFromToQ(string N, int p, int q, int m)
        {
            // Разделяем входное число на целую и дробные части
            string[] parts = N.Split(',');
            string intPart = parts[0];
            string fractionPart = parts.Length > 1 ? parts[1] : string.Empty;

            //Переводим обе части в новую систему счисления и объяединяем
            string result = TranslateInt(intPart, p, q) + TranslateFraction(fractionPart, p, q, m);
            return result;
        }


        /// <summary>
        /// Переводит целую часть числа в новую систему счисления.
        /// </summary>
        /// <param name="intPart">строка, представляющая целую часть числа в системе счисления p (Положительное число типа int)</param>
        /// <param name="p"> Основание системы счисления (Положительное число типа int в диапазоне 2 <= q <= 36)</param>
        /// <param name="q">Основание системы счисления (Положительное число типа int в диапазоне 2 <= q <= 36)</param>
        /// <returns>возвращает строку с целой частью числа в системе q.</returns>
        public static string TranslateInt(string intPart, int p, int q)
        {
            int decimalValue = 0;

            // Переводим целую часть в десятичную систему счисления
            foreach (char digit in intPart)
            {
                decimalValue = decimalValue * p + digits.IndexOf(digit); 
            }

            string result = "";

            // Переводим из десятичной системы в q-ричную, записывая остатки от деления
            do
            {
                result = digits[decimalValue % q] + result;
            }
            while ((decimalValue /= q) > 0);

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fractionPart">трока, представляющая дробную часть числа в системе счисления p (Положительное число типа int)</param>
        /// <param name="p">основание системы счисления, из которой производится перевод (Положительное число типа int диапазоне 2 <= p <= 36)</param>
        /// <param name="q">основание системы счисления, в которую нужно перевести дробную часть числа (Положительное число типа int диапазоне 2 <= p <= 36) </param>
        /// <param name="m">количество знаков после запятой в системе счисления q</param>
        /// <returns>строковое представление в новой системе счисления q с m знаками после запятой.</returns>
        private static string TranslateFraction(string fractionPart, int p, int q, int m)
        {
            if (string.IsNullOrEmpty(fractionPart)) return "";

            double decimalFraction = 0.0, power = 1.0 / p;

            // Переводим дробную часть в десятичную систему счисления
            foreach (char digit in fractionPart)
            {
                decimalFraction += digits.IndexOf(digit) * power;
                power /= p;
            }

            string result = ",";

            // Переводим дробную часть в q-ричную систему, умножая на q и выделяя целые части
            for (int i = 0; i < m; i++)
            {
                decimalFraction *= q;
                result += digits[(int)decimalFraction];
                decimalFraction -= (int)decimalFraction;
            }

            return result;
        }
    }
}
