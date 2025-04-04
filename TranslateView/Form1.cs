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
    // Главная форма приложения (Form1)
    public partial class Form1 : Form
    {
        // Конструктор главной формы
        public Form1()
        {
            // Инициализация компонентов формы (кнопок, текстовых полей и других элементов)
            InitializeComponent();
        }

        // Обработчик нажатия кнопки button2 (Выход)
        private void button2_Click(object sender, EventArgs e)
        {
            // Завершение работы всего приложения
            Application.Exit();
        }

        // Обработчик нажатия кнопки button1 (Перевод чисел)
        private void button1_Click(object sender, EventArgs e)
        {
            // Создание экземпляра формы Translate (форма перевода чисел)
            Translate form = new Translate();
            // Открытие формы в модальном режиме (блокирует главную форму)
            form.ShowDialog();
        }

        // Обработчик нажатия кнопки button3 (Справка)
        private void button3_Click(object sender, EventArgs e)
        {
            // Создание экземпляра формы Reference (справочная информация)
            Reference form = new Reference();
            // Открытие формы в модальном режиме
            form.ShowDialog();
        }
    }
}
