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

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FontFamilyBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontSizeBox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void RichEditorSelection(object sender, RoutedEventArgs e)
        {
            object currentValue = RichEditor.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            boldButton.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(FontWeights.Bold);
            currentValue = RichEditor.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            ItalicButton.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(FontStyles.Italic);
            currentValue = RichEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(TextDecorations.Underline);
            currentValue = RichEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontFamilyBox.SelectedItem = currentValue;
            currentValue = RichEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            FontSizeBox.SelectedItem = currentValue;
            currentValue = RichEditor.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            ColorButton.Background = this.ColorFormHexToRGB(currentValue.ToString());
        }

        private void FontFamilySelection(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyBox.SelectedItem != null)
            {
                RichEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyBox.SelectedItem);
            }
        }
        private void FontSizeSelection(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeBox.SelectedItem != null)
            {
                RichEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeBox.SelectedItem);
            }
        }

        private void ShowParamTextSelection(object sender, RoutedEventArgs e)
        {
            if (NavBarParamText.Visibility == Visibility.Hidden)
            {
                NavBarParamText.Visibility = Visibility.Visible;
                NavBarParamText.Height = 25;
            }
            else
            {
                NavBarParamText.Visibility = Visibility.Hidden;
                NavBarParamText.Height = 0;
            }
        }

        private void ColorButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                string color = colorPicker.SelectedColor.ToString() == "" ? "#fff" : colorPicker.SelectedColor.ToString();
                RichEditor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                colorPickerBlock.Visibility = Visibility.Visible;
            }
        }

        private void BackgroundButtonClick(object sender, MouseButtonEventArgs e)
        {
            /*if (e.LeftButton == MouseButtonState.Pressed)
            {
                string color = backgroundPickerBlock.SelectedColor.ToString() == "" ? "black" : colorPicker.SelectedColor.ToString();
                RichEditor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, color);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                backgroundPickerBlock.Visibility = Visibility.Visible;
            }*/
        }

        private SolidColorBrush ColorFormHexToRGB(string colorByHex)
        {
            SolidColorBrush background = new SolidColorBrush();
            try
            {
                Color colorByRGB = (Color)ColorConverter.ConvertFromString(colorByHex);
                background.Color = colorByRGB;
            }
            catch (FormatException)
            {
                background.Color = Color.FromRgb(255, 255, 255);
            }
            return background;
        }

        private void ColorPickerSelect(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            string colorByHex = colorPicker.SelectedColor.ToString() == "" ? "#fff" : colorPicker.SelectedColor.ToString();
            ColorButton.Background = this.ColorFormHexToRGB(colorByHex);
        }

        private void HiddenActionColorPicker(object sender, RoutedEventArgs e)
        {
            colorPickerBlock.Visibility = Visibility.Hidden;
        }

        private void LeftAligmentClick(object sender, RoutedEventArgs e)
        {
            RichEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }
        private void CenterAligmentClick(object sender, RoutedEventArgs e)
        {
            RichEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }
        private void RightAligmentClick(object sender, RoutedEventArgs e)
        {
            RichEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        private void JustifyAligmentClick(object sender, RoutedEventArgs e)
        {
            RichEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Justify);
        }

        private void BindLinkButtonClick(object sender, RoutedEventArgs e)
        {
            var paragraph = new Paragraph();
            var text = new Run();
            text.Text = "новый абзац";
            paragraph.Inlines.Add(text);
            RichEditor.Document.Blocks.Add(paragraph);
            RichEditor.Focus();
            RichEditor.ScrollToEnd();
        }

        private void cp_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

        }
    }
}
