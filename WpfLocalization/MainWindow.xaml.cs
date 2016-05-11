using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfLocalization {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly UserInputStorage _userInputStorage = new UserInputStorage();

        public MainWindow() {
            InitializeComponent();
            App.CultureChangeCalled += OnCultureChangeCalled;
            App.CultureChanged += OnCulcureChanged;
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e) {
            App.SwitchCulture();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show((string) FindResource("ResultMessageBoxContent"),
                (string) FindResource("ResultMessageBoxTitle"),
                MessageBoxButton.OK);
        }

        private void OnCultureChangeCalled(object sender, EventArgs eventArgs) {
            lock (App.CultureChanging) {
                // save user input
                _userInputStorage.Plane = PlaneTextBox.Text;
                _userInputStorage.JetsNumber = JetsTextBox.Text;
                if (SuccessfulAttackCheckBox.IsChecked != null) {
                    _userInputStorage.AttackSuccessIsChecked =
                        (bool) SuccessfulAttackCheckBox.IsChecked;
                }
                _userInputStorage.CommanderSelectedIndex =
                    CommanderComboBox.SelectedIndex;
                _userInputStorage.BattleshipsSelectedItems = new List<int>();
                foreach (var item in BattleshipsListBox.SelectedItems)
                    _userInputStorage.BattleshipsSelectedItems.Add(BattleshipsListBox.Items.IndexOf(item));
            }
        }

        private void OnCulcureChanged(object sender, EventArgs eventArgs) {
            // restore user input
            PlaneTextBox.Text = _userInputStorage.Plane;
            JetsTextBox.Text = _userInputStorage.JetsNumber;
            SuccessfulAttackCheckBox.IsChecked =
                _userInputStorage.AttackSuccessIsChecked;
            CommanderComboBox.SelectedIndex =
                _userInputStorage.CommanderSelectedIndex;
            for (var i = 0; i < BattleshipsListBox.Items.Count; ++i) {
                if (_userInputStorage.BattleshipsSelectedItems.Contains(i)) {
                    BattleshipsListBox.SelectedItems.Add(BattleshipsListBox.Items[i]);
                }
            }
        }

        private class UserInputStorage {
            public string Plane { get; set; }
            public string JetsNumber { get; set; }
            public bool AttackSuccessIsChecked { get; set; }
            public int CommanderSelectedIndex { get; set; }
            public IList BattleshipsSelectedItems { get; set; }
        }
    }
}