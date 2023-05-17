using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashFunction
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HashTextBlock.Text = HashPassword(HashTextBox.Text);
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(32);
            }
            byte[] dst = new byte[49];
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            byte[] source = Convert.FromBase64String(hashedPassword);
            if ((source.Length != 49) || (source[0] != 0))
            {
                MessageBox.Show("Exeption");
                return false;
            }
            byte[] salt = new byte[16];
            Buffer.BlockCopy(source, 1, salt, 0, 16);
            byte[] buffer3 = new byte[32];
            Buffer.BlockCopy(source, 17, buffer3, 0, 32);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                buffer4 = bytes.GetBytes(32);
            }

            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var result = VerifyHashedPassword(HashTextBlock.Text, HashTextBox.Text);
            if(result == true)
            {
                MessageBox.Show("Верно");
            }
        }
    }
}
