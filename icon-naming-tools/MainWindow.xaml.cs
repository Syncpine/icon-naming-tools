using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace icon_naming_tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // GlobalGrid.ShowGridLines = true;
            GlobalGrid.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));

            GlobalGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
            GlobalGrid.RowDefinitions.Add(new RowDefinition());

            GlobalGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // StackPanel
            StackPanel stackPanel = new StackPanel()
            {
                Margin = new Thickness(0, 2, 0, 2),
                Orientation = Orientation.Horizontal,
            };
            GlobalGrid.Children.Add(stackPanel);
            Grid.SetRow(stackPanel, 0);
            Grid.SetColumn(stackPanel, 0);

            // Label -> Address
            Label labelAddress = new Label()
            {
                Margin = new Thickness(10, 0, 0, 0),

                Content = "Address:",
                FontSize = 14,
                FontFamily = new FontFamily("inherit"),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            stackPanel.Children.Add(labelAddress);

            // TextBox -> Input
            _textBox = new TextBox()
            {
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(5, 0, 5, 0),

                Text = "",
                FontSize = 13,
                FontFamily = new FontFamily("inherit"),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            Border borderInput = new Border()
            {
                Width = 240,
                Height = 30,
                Background = new SolidColorBrush(Color.FromRgb(226, 226, 226)),
                Margin = new Thickness(10, 0, 0, 0),

                CornerRadius = new CornerRadius(5),

                Child = _textBox,
            };
            stackPanel.Children.Add(borderInput);

            // Button -> Open
            Button buttonOpen = new Button()
            {
                Width = 70,
                Height = 30,
                BorderThickness = new Thickness(0),
                Background = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                Margin = new Thickness(10, 0, 0, 0),


                Content = "Open",
                FontSize = 12,
                FontFamily = new FontFamily("inherit"),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            stackPanel.Children.Add(buttonOpen);

            // Button -> Transform
            Button buttonTransform = new Button()
            {
                Width = 100,
                Height = 30,
                BorderThickness = new Thickness(0),
                Background = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                Margin = new Thickness(10, 0, 0, 0),

                Content = "Transform",
                FontSize = 12,
                FontFamily = new FontFamily("inherit"),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            stackPanel.Children.Add(buttonTransform);

            // Label -> Status
            _labelStatus = new Label()
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(10, 10, 10, 10),

                Content = "",
                FontSize = 13,
                FontFamily = new FontFamily("Calibre"),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Top,
            };
            GlobalGrid.Children.Add(_labelStatus);
            Grid.SetRow(_labelStatus, 1);
            Grid.SetColumn(_labelStatus, 0);

            buttonOpen.Click += new RoutedEventHandler(ButtonOpenClick);
            buttonTransform.Click += new RoutedEventHandler(ButtonTransformClick);
        }

        private void ButtonOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = "C:\\Users\\31611\\Downloads\\",
                RestoreDirectory = true,
            };
            openFileDialog.ShowDialog();

            _textBox.Text = openFileDialog.FileName;
        }

        private void ButtonTransformClick(object sender, RoutedEventArgs e)
        {
            string path = _textBox.Text.Replace("\\", "/");
            double imageWidth = 0.0, imageHeight = 0.0;
            string newFilePath = "";

            if ("" == path)
            {
                _labelStatus.Content = "Please open a file.";
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                    imageWidth = image.Width;
                    imageHeight = image.Height;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message + exp.StackTrace);
                _labelStatus.Content = "Not image file !" + "\n\t" + path;
                return;
            }

            try
            {
                string fileName = Path.GetFileName(path);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string fileNameExtension = Path.GetExtension(fileName);

                string newFileName = FormatStr(fileNameWithoutExtension).Trim() + " " + imageWidth + "x" + imageHeight +
                                     fileNameExtension;

                newFilePath = Path.Combine(NewDirPath, newFileName).Replace("\\", "/");

                _labelStatus.Content = newFilePath;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message + exp.StackTrace);
                _labelStatus.Content = "File rename failed !" + "\n\t" + path + "\n\t" + newFilePath;
                return;
            }

            try
            {
                File.Copy(path, newFilePath, true);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message + exp.StackTrace);
                _labelStatus.Content = "File rename failed !" + "\n\t" + path + "\n\t" + newFilePath;
                return;
            }

            _labelStatus.Content = "Success copy file." + "\n\t" + path + "\n\t" + newFilePath;
        }

        // Remove "(...)" from string
        private string FormatStr(string str)
        {
            int begin = str.IndexOf("(", StringComparison.Ordinal);
            int end = str.IndexOf(")", StringComparison.Ordinal);

            if (-1 != begin && -1 != end && begin < end)
            {
                str = str.Remove(begin, end - begin + 1);
            }

            return str;
        }

        private readonly Label _labelStatus;

        private readonly TextBox _textBox;

        private const string NewDirPath = "D:/WorkSpace/Game/Material/";
    }
}