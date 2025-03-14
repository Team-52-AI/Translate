using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TranslateView
{
    public partial class Translate : Form
    {
        public Translate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string p_1 = textBox1.Text;
            int p;
            string q_1 = textBox2.Text;
            int q;
            string n_1 = textBox3.Text;
            double n;
            int m = 3;

            if ((textBox1.Text).Length == 0)
            {
                textBox4.Text = "Введите исходную сис.!".ToString();
                return;
            }

            else if ((textBox2.Text).Length == 0)
            {
                textBox4.Text = "Введите итоговую сис.!".ToString();
                return;
            }

            else if ((textBox3.Text).Length == 0)
            {
                textBox4.Text = "Введите число!".ToString();
                return;
            }

            try
            {
                p = Convert.ToInt32(p_1);
            }
            catch (Exception)
            {
                textBox4.Text = "Введите исходную сис.!".ToString();
                return;
            }

            try
            {
                q = Convert.ToInt32(q_1);
            }
            catch (Exception)
            {
                textBox4.Text = "Введите итоговую сис.!".ToString();
                return;
            }

            try
            {
                n = Convert.ToDouble(n_1);
            }
            catch (Exception)
            {
                textBox4.Text = "Введите число!".ToString();
                return;
            }

            // Используем класс Translate для перевода числа
            string result = TranslateLibrary.TranslateNumberFromToQ(n_1, p, q, m);
            textBox4.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public static class TranslateLibrary
    {
        public const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string TranslateNumberFromToQ(string N, int p, int q, int m)
        {
            string[] parts = N.Split(',');
            string intPart = parts[0];
            string fractionPart = parts.Length > 1 ? parts[1] : string.Empty;
            return TranslateInt(intPart, p, q) + TranslateFraction(fractionPart, p, q, m);
        }

        public static string TranslateInt(string intPart, int p, int q)
        {
            int decimalValue = 0;
            foreach (char digit in intPart)
            {
                decimalValue = decimalValue * p + digits.IndexOf(digit);
            }

            string result = "";
            do
            {
                result = digits[decimalValue % q] + result;
            }
            while ((decimalValue /= q) > 0);

            return result;
        }

        private static string TranslateFraction(string fractionPart, int p, int q, int m)
        {
            if (string.IsNullOrEmpty(fractionPart)) return "";

            double decimalFraction = 0.0, power = 1.0 / p;
            foreach (char digit in fractionPart)
            {
                decimalFraction += digits.IndexOf(digit) * power;
                power /= p;
            }

            string result = ",";
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
