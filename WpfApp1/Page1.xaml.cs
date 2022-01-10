using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            NavigationWindow menu = (NavigationWindow)Window.GetWindow(this);
          

        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            NavigationWindow menu = (NavigationWindow)Window.GetWindow(this);
            MainWindow subWindow = new MainWindow();
            subWindow.Show();
            menu.Hide();
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            NavigationWindow menu = (NavigationWindow)Window.GetWindow(this);
            menu.Hide();
        }
    }
}
