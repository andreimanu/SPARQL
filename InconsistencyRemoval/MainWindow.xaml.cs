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

namespace InconsistencyRemoval {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Manager man;
        Manager man2;
        Manager man3;
        Manager man4;
        int counter;
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            try {
              //  man = new Manager("Eval11", this, 1);
                man2 = new Manager("Eval22", this, 2);
                //   man3 = new Manager("Eval33", this, 3);
                //   man4 = new Manager("Eval44", this, 4);
                //   Parallel.Invoke(() => man.Run(), () => man2.Run(), () => man3.Run(), () => man4.Run());
                man2.Run();
            }
            catch(Exception ex) {
               // man.Stop();
                man2.Stop();
               // man3.Stop();
               // man4.Stop();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            man.Stop();
        }
    }
}
