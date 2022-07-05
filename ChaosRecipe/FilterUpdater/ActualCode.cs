using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FilterUpdater
{
    public class ActualCode : INotifyPropertyChanged
    {
        public ActualCode()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand SaveCommand
        {
            get { return new DelegateCommand(Save); }
        }
        public ICommand ExecutionCommand
        {
            get { return new DelegateCommand(Execute); }
        }
        public ICommand BrowseCommand
        {
            get { return new DelegateCommand(Browse); }
        }

        public ObservableCollection<string> Colors { get; set; } = new ObservableCollection<string> { "Red", "Green", "Blue", "Brown", "White", "Yellow", "Cyan", "Grey", "Orange", "Pink", "Purple" };

        public ObservableCollection<string> Shapes { get; set; } = new ObservableCollection<string> { "Circle", "Diamond", "Hexagon", "Square", "Star", "Triangle", "Cross", "Moon", "Raindrop", "Kite", "Pentagon", "House" };

        public string BorderColor { get; set; } = Properties.Settings.Default.Color;
        public string SelectedShapeColor { get; set; } = Properties.Settings.Default.ShapeColor;

        private string m_shape = Properties.Settings.Default.Shape;
        public string SelectedShape
        {
            get => m_shape.Equals("UpsideDownHouse") ? "House" : m_shape;
            set
            {
                string target = value.Equals("House") ? "UpsideDownHouse" : value;
                if (!m_shape.Equals(target))
                {
                    m_shape = target;
                    OnPropertyChange(nameof(SelectedShape));
                }
            }
        }
        public string FilterPaths { get; set; } = Properties.Settings.Default.Path;
        public string TopString { get; set; } = Properties.Settings.Default.TopString;
        public string BotString { get; set; } = Properties.Settings.Default.BotString;

        private int bitmask = Properties.Settings.Default.Bitmask;
        public bool Helm
        {
            get => (bitmask & 1) == 1;
            set
            {
                if (value && (bitmask & 1) != 1)
                {
                    bitmask |= 1;
                    OnPropertyChange(nameof(Helm));
                }
                else if (!value && (bitmask & 1) == 1)
                {
                    bitmask &= ~1;
                    OnPropertyChange(nameof(Helm));
                }
            }
        }
        public bool Glove
        {
            get => (bitmask & 2) == 2;
            set
            {
                if (value && (bitmask & 2) != 2)
                {
                    bitmask |= 2;
                    OnPropertyChange(nameof(Glove));
                }
                else if (!value && (bitmask & 2) == 2)
                {
                    bitmask &= ~2;
                    OnPropertyChange(nameof(Glove));
                }
            }
        }
        public bool Boot
        {
            get => (bitmask & 4) == 4;
            set
            {
                if (value && (bitmask & 4) != 4)
                {
                    bitmask |= 4;
                    OnPropertyChange(nameof(Boot));
                }
                else if (!value && (bitmask & 4) == 4)
                {
                    bitmask &= ~4;
                    OnPropertyChange(nameof(Boot));
                }
            }
        }
        public bool Body
        {
            get => (bitmask & 8) == 8;
            set
            {
                if (value && (bitmask & 8) != 8)
                {
                    bitmask |= 8;
                    OnPropertyChange(nameof(Body));
                }
                else if (!value && (bitmask & 8) == 8)
                {
                    bitmask &= ~8;
                    OnPropertyChange(nameof(Body));
                }
            }
        }
        public bool Wep
        {
            get => (bitmask & 16) == 16;
            set
            {
                if (value && (bitmask & 16) != 16)
                {
                    bitmask |= 16;
                    OnPropertyChange(nameof(Wep));
                }
                else if (!value && (bitmask & 16) == 16)
                {
                    bitmask &= ~16;
                    OnPropertyChange(nameof(Wep));
                }
            }
        }
        public bool Ring
        {
            get => (bitmask & 32) == 32;
            set
            {
                if (value && (bitmask & 32) != 32)
                {
                    bitmask |= 32;
                    OnPropertyChange(nameof(Ring));
                }
                else if (!value && (bitmask & 32) == 32)
                {
                    bitmask &= ~32;
                    OnPropertyChange(nameof(Ring));
                }
            }
        }
        public bool Belt
        {
            get => (bitmask & 64) == 64;
            set
            {
                if (value && (bitmask & 64) != 64)
                {
                    bitmask |= 64;
                    OnPropertyChange(nameof(Belt));
                }
                else if (!value && (bitmask & 64) == 64)
                {
                    bitmask &= ~64;
                    OnPropertyChange(nameof(Belt));
                }
            }
        }
        public bool Ammy
        {
            get => (bitmask & 128) == 128;
            set
            {
                if (value && (bitmask & 128) != 128)
                {
                    bitmask |= 128;
                    OnPropertyChange(nameof(Ammy));
                }
                else if (!value && (bitmask & 128) == 128)
                {
                    bitmask &= ~128;
                    OnPropertyChange(nameof(Ammy));
                }
            }
        }


        public bool Under75
        {
            get => (bitmask & 256) == 256;
            set
            {
                if (value && (bitmask & 256) != 256)
                {
                    bitmask |= 256;
                    OnPropertyChange(nameof(Under75));
                }
                else if (!value && (bitmask & 256) == 256)
                {
                    bitmask &= ~256;
                    OnPropertyChange(nameof(Under75));
                }
            }
        }

        private void Execute()
        {
            Properties.Settings.Default.Bitmask = bitmask;
            Properties.Settings.Default.Save();
            string chaosInsert = FormatFilter();
            foreach (var untrimmedPath in FilterPaths.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var path = untrimmedPath.Trim();
                if (!File.Exists(path))
                {
                    MessageBox.Show(
                        $"Could not find file at {path}\n",
                        "Setting Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    continue;
                }
                string originalFilter = File.ReadAllText(path);
                int topIndex = originalFilter.IndexOf(TopString) + TopString.Length;
                if (topIndex < 0)
                {
                    MessageBox.Show(
                        $"Could not match the top string in {path}\n" +
                        "Please copy the line(s) preceeding where new filters should go\n\n" +
                        "top string was:\n" +
                        TopString,
                        "Setting Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    continue;
                }
                string prefix = originalFilter[..topIndex];
                string remaining = originalFilter[topIndex..];
                int botindex = remaining.IndexOf(BotString);
                if (botindex < 0)
                {
                    MessageBox.Show(
                        $"Could not match the bottom string in {path}\n" +
                        "Please copy the line(s) following where new filters should go\n\n" + 
                        "Bottom string was:\n" + 
                        BotString,
                        "Setting Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    continue;
                }
                string suffix = remaining[botindex..];
                string newFilter = prefix + chaosInsert + suffix;
                File.WriteAllText(path, newFilter);
            }
        }

        private void Save()
        {
            Properties.Settings.Default.Color = BorderColor;
            Properties.Settings.Default.ShapeColor = SelectedShapeColor;
            Properties.Settings.Default.Shape = m_shape;
            Properties.Settings.Default.Path = FilterPaths;
            Properties.Settings.Default.TopString = TopString;
            Properties.Settings.Default.BotString = BotString;
            Properties.Settings.Default.Save();
        }

        private void Browse()
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(FilterPaths.Split('\n', StringSplitOptions.RemoveEmptyEntries).First()),
                Title = "",
                DefaultExt = ".filter",
                Filter = "filter files (*.filter)|*.filter|All files (*.*)|*.*",
                Multiselect = true
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                StringBuilder sb = new();
                foreach (string s in dialog.FileNames)
                {
                    sb.AppendLine(s);
                }
                FilterPaths = sb.ToString();
            }
            OnPropertyChange(nameof(FilterPaths));
        }
        private void OnPropertyChange(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }

        private string FormatFilter()
        {
            StringBuilder sb = new StringBuilder();
            if (Wep)
            {
                SetDefault(sb);
                sb.AppendLine("\tClass == \"Daggers\" \"One Hand Axes\" \"One Hand Maces\" \"One Hand Swords\" \"Rune Daggers\" \"Sceptres\" \"Wands\"");
                sb.AppendLine("\tHeight = 3");
                sb.AppendLine("\tWidth = 1");
                SetDefault(sb);
                sb.AppendLine("\tClass == \"Bows\"");
                sb.AppendLine("\tHeight = 3");
                sb.AppendLine("\tWidth = 2");
            }
            if ((bitmask & ~16) != 0)
            {
                SetDefault(sb);
                sb.Append("\tClass ==");
                if (Helm)
                {
                    sb.Append(" \"Helmets\"");
                }
                if (Glove)
                {
                    sb.Append(" \"Gloves\"");
                }
                if (Boot)
                {
                    sb.Append(" \"Boots\"");
                }
                if (Body)
                {
                    sb.Append(" \"Body Armours\"");
                }
                if (Ring)
                {
                    sb.Append(" \"Rings\"");
                }
                if (Belt)
                {
                    sb.Append(" \"Belts\"");
                }
                if (Ammy)
                {
                    sb.Append(" \"Amulets\"");
                }
            }
            if (sb.Length > 0)
                return sb.AppendLine().AppendLine().ToString();
            else
                return "\n";
        }

        private void SetDefault(StringBuilder sb)
        {
            sb.AppendLine("\nShow");
            sb.Append("\tSetBorderColor ").AppendLine(BorderColor);
            sb.AppendLine("\tSetFontSize 38");
            sb.Append("\tMinimapIcon 2 ").Append(SelectedShapeColor).Append(' ').AppendLine(m_shape);
            sb.AppendLine("\tItemLevel >= 60");
            if (Under75)
            {
                sb.AppendLine("\tItemLevel < 75");
            }
            sb.AppendLine("\tRarity Rare");
            sb.AppendLine("\tIdentified False");
        }

        private class DelegateCommand : ICommand
        {
            private readonly Action? a = null;
            public DelegateCommand(Action action)
            {
                a = action;
            }
            public bool CanExecute(object? parameter) => true;
            public void Execute(object? parameter) => a?.Invoke();
            public event EventHandler? CanExecuteChanged;
        }

    }
}
