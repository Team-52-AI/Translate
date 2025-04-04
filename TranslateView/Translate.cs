using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslateView
{
    // Главная форма приложения для перевода чисел между системами счисления
    public partial class Translate : Form
    {
        // Конструктор формы
        public Translate()
        {
            InitializeComponent(); // Инициализация компонентов формы
        }

        // Обработчик нажатия кнопки перевода
        private void button1_Click(object sender, EventArgs e)
        {
            // Очищаем предыдущий результат
            richTextBox1.Text = "";

            // Получаем значения из текстовых полей с удалением пробелов
            string p_1 = textBox1.Text.Trim();  // Исходная система счисления
            string q_1 = textBox2.Text.Trim();  // Целевая система счисления
            string n_1 = textBox3.Text.Trim();  // Число для перевода

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(p_1))
            {
                richTextBox1.Text = "Введите исходную систему!";
                return;
            }
            if (string.IsNullOrEmpty(q_1))
            {
                richTextBox1.Text = "Введите итоговую систему!";
                return;
            }
            if (string.IsNullOrEmpty(n_1))
            {
                richTextBox1.Text = "Введите число!";
                return;
            }

            // Проверка корректности систем счисления (от 2 до 36)
            int p, q;
            if (!int.TryParse(p_1, out p) || p <= 1 || p > 36)
            {
                richTextBox1.Text = "Исходная система должна быть от 2 до 36!";
                return;
            }
            if (!int.TryParse(q_1, out q) || q <= 1 || q > 36)
            {
                richTextBox1.Text = "Итоговая система должна быть от 2 до 36!";
                return;
            }

            // Проверка на наличие пробелов в числе
            if (n_1.Contains(" "))
            {
                richTextBox1.Text = "Число не должно содержать пробелов!";
                return;
            }

            // Проверка соответствия числа исходной системе счисления
            if (!IsValidNumber(n_1, p))
            {
                richTextBox1.Text = $"Число не соответствует системе {p}!";
                return;
            }

            // Выполняем перевод числа с заданной точностью (10 знаков после запятой)
            string result = TranslateLibrary.TranslateNumberFromToQ(n_1, p, q, 10);
            richTextBox1.Text = result;
        }

        // Метод проверки корректности числа для заданной системы счисления
        private bool IsValidNumber(string number, int baseSystem)
        {
            // Допустимые цифры для указанной системы счисления
            string allowedDigits = TranslateLibrary.digits.Substring(0, baseSystem);
            string[] parts = number.Split(','); // Разделяем на целую и дробную части

            // Проверка целой части числа
            foreach (char c in parts[0])
            {
                if (allowedDigits.IndexOf(char.ToUpper(c)) == -1)
                    return false;
            }

            // Проверка дробной части (если есть)
            if (parts.Length > 1)
            {
                foreach (char c in parts[1])
                {
                    if (allowedDigits.IndexOf(char.ToUpper(c)) == -1)
                        return false;
                }
            }

            return true;
        }

        // Обработчик нажатия кнопки закрытия формы
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close(); // Закрываем текущую форму
        }
    }

    // Статический класс для выполнения перевода чисел между системами счисления
    public static class TranslateLibrary
    {
        // Строка допустимых цифр (до 36-ричной системы)
        public const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // Основной метод перевода числа между системами счисления
        public static string TranslateNumberFromToQ(string N, int p, int q, int m)
        {
            try
            {
                // Обработка отрицательных чисел
                bool isNegative = N.StartsWith("-");
                if (isNegative) N = N.Substring(1); // Удаляем знак минус для обработки

                // Разделяем число на целую и дробную части
                string[] parts = N.Split(',');
                string intPart = parts[0];          // Целая часть
                string fractionPart = parts.Length > 1 ? parts[1] : string.Empty; // Дробная часть

                // Выполняем перевод и объединяем результаты
                string result = TranslateInt(intPart, p, q) + TranslateFraction(fractionPart, p, q, m);
                return isNegative ? "-" + result : result; // Возвращаем знак минуса при необходимости
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}"; // Обработка и вывод ошибок
            }
        }

        // Метод перевода целой части числа
        public static string TranslateInt(string intPart, int p, int q)
        {
            if (string.IsNullOrEmpty(intPart)) return "0"; // Обработка пустой строки

            // Переводим в десятичную систему
            long decimalValue = 0;
            foreach (char digit in intPart.ToUpper()) // Обрабатываем цифры в верхнем регистре
            {
                int value = digits.IndexOf(digit);
                if (value == -1 || value >= p)
                    throw new ArgumentException($"Недопустимая цифра {digit} для системы {p}");

                decimalValue = decimalValue * p + value; // Накопление десятичного значения
            }

            // Переводим из десятичной в целевую систему
            if (decimalValue == 0) return "0"; // Ноль в любой системе - ноль

            string result = "";
            while (decimalValue > 0)
            {
                result = digits[(int)(decimalValue % q)] + result; // Формируем результат справа налево
                decimalValue /= q;
            }

            return result;
        }

        // Метод перевода дробной части числа
        private static string TranslateFraction(string fractionPart, int p, int q, int m)
        {
            if (string.IsNullOrEmpty(fractionPart) || m <= 0) return ""; // Проверка на пустую дробную часть

            // Переводим в десятичную систему
            double decimalFraction = 0.0;
            double power = 1.0 / p; // Начальный вес разряда
            foreach (char digit in fractionPart.ToUpper())
            {
                int value = digits.IndexOf(digit);
                if (value == -1 || value >= p)
                    throw new ArgumentException($"Недопустимая цифра {digit} для системы {p}");

                decimalFraction += value * power; // Накопление дробного значения
                power /= p; // Уменьшение веса следующего разряда
            }

            // Переводим из десятичной в целевую систему
            string result = ".";
            for (int i = 0; i < m; i++) // m - точность (количество знаков после запятой)
            {
                decimalFraction *= q;
                int digit = (int)decimalFraction; // Целая часть после умножения
                result += digits[digit];         // Добавляем соответствующую цифру
                decimalFraction -= digit;        // Оставляем только дробную часть
                if (decimalFraction == 0) break; // Прекращаем если дробная часть нулевая
            }

            return result;
        }
    }
}