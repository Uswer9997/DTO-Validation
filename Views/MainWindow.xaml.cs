using DTO_Validation.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace DTO_Validation.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public static partial class CommandHelper
    {
        private static RoutedUICommand _addPersonWindow;
        public static RoutedUICommand AddPersonWindow
        {
            get
            {
                if (_addPersonWindow is null)
                {
                    _addPersonWindow = new RoutedUICommand("Открыть окно добавления Person", nameof(AddPersonWindow), typeof(CommandHelper));
                    CommandManager.RegisterClassCommandBinding(typeof(Window), new CommandBinding(_addPersonWindow, OnAddPersonWindow, OnCanAddPersonWindow));
                }
                return _addPersonWindow;
            }
        }

        private static void OnCanAddPersonWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Source is FrameworkElement fe && fe.DataContext is MainWindowViewModel;
        }

        private static void OnAddPersonWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Source is FrameworkElement fe && fe.DataContext is MainWindowViewModel vm))
                throw new NotImplementedException("Команда должна вызываться для контекста данных MainWindowViewModel");

            var wind = new AddPersonWindow()
            {
                DataContext = vm.CreatePersonVM(),
            };
            wind.Show();
        }

    }
}
