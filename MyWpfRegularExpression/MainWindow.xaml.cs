using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System;

namespace MyWpfRegularExpression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Regex _regEx; 

        #region INotify Changed Properties  
        private string message;
        public string Message
        {
            get { return message; }
            set { SetField(ref message, value, nameof(Message)); }
        }

        private string stringToTest;
        public string StringToTest
        {
            get { return stringToTest; }
            set { SetField(ref stringToTest, value, nameof(StringToTest)); }
        }

        private ObservableCollection<string> itemList;
        public ObservableCollection<string> ItemList
        {
            get { return itemList; }
            set { SetField(ref this.itemList, value, nameof(ItemList)); }
        }
        private string newItem;
        public string NewItem
        {
            get
            {
                return newItem;
            }
            set
            {
                if (newItem != value)
                {
                    newItem = value;
                    var item = ItemList.SingleOrDefault(x => x == newItem);
                    if (item == null)
                        ItemList.Insert(0, newItem);
                    SelectedRegularExpression = newItem;
                }
            }
        }
        private string selectedRegularExpression;
        public string SelectedRegularExpression
        {
            get
            {
                return selectedRegularExpression;
            }
            set
            {
                if (selectedRegularExpression != value)
                {
                    selectedRegularExpression = value;
                    if (selectedRegularExpression == ItemListToXml.DELETE_COMMAND)
                    {
                        ItemList.Clear();
                        ItemList.Add(ItemListToXml.DELETE_COMMAND);
                    }
                    SetField(ref this.selectedRegularExpression, value, nameof(SelectedRegularExpression));
                }
            }
        }

        private Visibility event1Green;
        public Visibility Event1Green
        {
            get { return event1Green; }
            set
            {
                if (value != Event1Green)
                {
                    event1Green = value;
                    OnPropertyChanged("Event1Green");
                }
            }
        }
        private Visibility event1Red;
        public Visibility Event1Red
        {
            get { return event1Red; }
            set
            {
                if (value != Event1Red)
                {
                    event1Red = value;
                    OnPropertyChanged("Event1Red");
                }
            }
        }

        // Template for a new INotify Changed Property
        // for using CTRL-R-R
        private string xxx;
        public string Xxx
        {
            get { return xxx; }
            set { SetField(ref xxx, value, nameof(Xxx)); }
        }
        #endregion

        public ItemListToXml ItemListToXml { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

#if DEBUG
            Title += "    Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#else
            Title += "    Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#endif
            DataContext = this;

            ItemListToXml = new ItemListToXml();
            ItemList = ItemListToXml.Load(ref selectedRegularExpression);
            SelectedRegularExpression = selectedRegularExpression;
            Event1Green = Visibility.Hidden;
            Event1Red = Visibility.Hidden;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindow()
        {
            ItemListToXml.Save(SelectedRegularExpression, ItemList);
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_Click_Clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            StringToTest = "";
        }

        /// <summary>
        /// Button_Click_LoadDefault
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_LoadDefault(object sender, RoutedEventArgs e)
        {
            LoadDefault();
        }

        /// <summary>
        /// Button_Click_Check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Check(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(StringToTest))
            {
                Message = "Enter a String To Test";
                return;
            }
            if (String.IsNullOrWhiteSpace(SelectedRegularExpression))
            {
                Message = "Enter a Regular Expression";
                return;
            }

            _regEx = new Regex(SelectedRegularExpression);

            if (_regEx.IsMatch(StringToTest))
            {
                System.Console.Beep();
                SetEventSignal1(true);
            }
            else
            {
                System.Console.Beep();
                System.Console.Beep();
                SetEventSignal1(false);
            }
        }
        private void SetEventSignal1(bool state)
        {
            if (!state)
            {
                Event1Green = Visibility.Hidden;
                Event1Red = Visibility.Visible;
            }
            else
            {
                Event1Green = Visibility.Visible;
                Event1Red = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Button_Click_Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Lable_Message_MouseDown
        /// Clear Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lable_Message_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Message = "";
        }

        /// <summary>
        /// ComboBox_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                string newItemValue = ((System.Windows.Controls.TextBox)e.OriginalSource).Text;
                var item = ItemList.SingleOrDefault(x => x == newItemValue);
                if (item == null)
                    ItemList.Insert(0, newItemValue);
            }
        }

        /// <summary>
        /// Window_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// LoadDefault
        /// </summary>
        private void LoadDefault()
        {
            ItemList.Clear();
            ItemList.Add(@"(.){3}");                                         // more than any 3 chars
            ItemList.Add(@"^(.){3,10}$");                                    // allow 3 to 10 any chars
            ItemList.Add(@"^[a - z]{1,10}$");                                // allow 1 to 10 small letters
            ItemList.Add(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"); // valid Email id
            ItemList.Add(@"Delete All Items");
        }

        /// <summary>
        /// SetField
        /// for INotify Changed Properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
