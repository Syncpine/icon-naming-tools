using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

            GridGlobal.ShowGridLines = false;

            GridGlobal.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            GridGlobal.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            GridGlobal.RowDefinitions.Add(new RowDefinition());

            GridGlobal.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
            GridGlobal.ColumnDefinitions.Add(new ColumnDefinition());
            GridGlobal.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });

            // Label Address
            Label labelAddress = new Label()
            {
                Name = "Address",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,

                Content = "Address:",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            GridGlobal.Children.Add(labelAddress);

            Grid.SetRow(labelAddress, 0);
            Grid.SetColumn(labelAddress, 0);

            // Input Address
            _textAddress = new TextBox()
            {
                Name = "Address",
                Width = 200,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,

                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            GridGlobal.Children.Add(_textAddress);

            Grid.SetRow(_textAddress, 0);
            Grid.SetColumn(_textAddress, 1);

            // Btn Open
            Button btnOpen = new Button()
            {
                Name = "Open",
                Width = 80,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,

                Content = "Open",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            GridGlobal.Children.Add(btnOpen);

            Grid.SetRow(btnOpen, 0);
            Grid.SetColumn(btnOpen, 2);

            // Btn Transform
            Button btnTransform = new Button()
            {
                Name = "Transform",
                Width = 80,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,

                Content = "Transform",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            GridGlobal.Children.Add(btnTransform);

            Grid.SetRow(btnTransform, 1);
            Grid.SetColumn(btnTransform, 1);

            // Label Status
            _labelStatus = new Label()
            {
                Name = "Status",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,

                Content = "",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            GridGlobal.Children.Add(_labelStatus);

            Grid.SetRow(_labelStatus, 2);
            Grid.SetColumn(_labelStatus, 0);

            Grid.SetColumnSpan(_labelStatus, 3);

            btnOpen.Click += new RoutedEventHandler(BtnOpenClick);
            btnTransform.Click += new RoutedEventHandler(BtnTransformClick);
        }

        // Open Folder
        private void BtnOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\Users\\31611\\Downloads",
                RestoreDirectory = true,
            };
            openFileDialog.ShowDialog();

            _textAddress.Width = double.NaN;

            _textAddress.Text = openFileDialog.FileName;

            _labelStatus.Content = "Open " + openFileDialog.FileName + " success.";
        }

        // Transform
        private void BtnTransformClick(object sender, RoutedEventArgs e)
        {
            if ("" == _textAddress.Text)
            {
                _labelStatus.Content = "Please input icon path.";
                _textAddress.Width = 200;
                return;
            }

            if (!File.Exists(_textAddress.Text))
            {
                _labelStatus.Content = _textAddress.Text + " is not exist.";
                _textAddress.Width = 200;
                return;
            }

            string sourceIcon = _textAddress.Text;
            string sourceIconWithoutExtension = Path.GetFileNameWithoutExtension(sourceIcon);

            _iconInfoGroup.DirectoryPath = Path.GetDirectoryName(sourceIcon);
            _iconInfoGroup.UpdateDirectoryPath = _updateDirectoryPath;
            _iconInfoGroup.Extension = Path.GetExtension(sourceIcon);

            string statusStr = "";
            bool isExist = true;
            foreach (var name in _nameMap)
            {
                string iconFilePath = Path.Combine(_iconInfoGroup.DirectoryPath,
                    sourceIconWithoutExtension + name.Key + _iconInfoGroup.Extension);
                if (!File.Exists(iconFilePath))
                {
                    statusStr += iconFilePath + "\n";
                    isExist = false;
                }

                _iconInfoGroup.IconPathList.Add(iconFilePath);

                string updateIconFilePath = Path.Combine(_iconInfoGroup.UpdateDirectoryPath,
                    sourceIconWithoutExtension + name.Value + _iconInfoGroup.Extension);
                _iconInfoGroup.UpdateIconPathList.Add(updateIconFilePath);
            }

            if (!isExist)
            {
                _labelStatus.Content = statusStr + " is not exist\n";
                _textAddress.Width = 200;
                return;
            }

            try
            {
                for (int index = 0; index < _iconInfoGroup.IconPathList.Count; index++)
                {
                    // File.Move(_iconInfoGroup.IconPathList[index], _iconInfoGroup.UpdateIconPathList[index]);
                    File.Copy(_iconInfoGroup.IconPathList[index], _iconInfoGroup.UpdateIconPathList[index]);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message + exception.StackTrace);
                _labelStatus.Content = "Transform error!\n";
                return;
            }

            _labelStatus.Content = "Transform succeed!\n";
        }

        private readonly TextBox _textAddress;

        private readonly Label _labelStatus;

        private readonly IconInfo _iconInfoGroup = new IconInfo();

        private readonly string _updateDirectoryPath = "D:\\WorkSpace\\Game\\Material";

        private readonly Dictionary<string, string> _nameMap = new Dictionary<string, string>()
        {
            { "", " 16x16" },
            { " (1)", " 32x32" },
            { " (2)", " 48x48" },
            { " (3)", " 64x64" },
            { " (4)", " 128x128" },
        };
    }
}