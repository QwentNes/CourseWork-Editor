using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Xceed.Wpf.Toolkit;

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        private string fontColor = "#fff";
        private string backgroundColor = "#fff";
        private string colorSelection = "font";
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
            //currentValue = RichEditor.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            //ColorButton.Background = this.ColorFormHexToRGB(currentValue.ToString());
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
        private void Save_Executed(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(RichEditor.Document.ContentStart, RichEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }
        private void СhooseСolorClick(object sender, MouseButtonEventArgs e)
        {
            bool type = (sender as Button).Tag.ToString() == "font";
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //string color = colorPicker.SelectedColor.ToString() == "" ? "#fff" : colorPicker.SelectedColor.ToString();
                RichEditor.Selection.ApplyPropertyValue(type ? TextElement.ForegroundProperty : TextElement.BackgroundProperty, type ? this.fontColor : this.backgroundColor);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            { 
                colorPickerBlock.Margin = type ? new Thickness(-65, 25, 0, 0) : new Thickness(-25, 25, 0, 0);
                this.colorSelection = type ? "font" : "background";
                colorPickerBlock.Visibility = Visibility.Visible;
                colorPicker.SelectedColor = type ? (Color)ColorConverter.ConvertFromString(this.fontColor) : (Color)ColorConverter.ConvertFromString(this.backgroundColor);
            }
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
            if(this.colorSelection == "font")
            {
                this.fontColor = colorByHex;
                fontColorButton.Background = this.ColorFormHexToRGB(colorByHex);
            }
            if (this.colorSelection == "background")
            {
                this.backgroundColor = colorByHex;
                backgroundColorButton.Background = this.ColorFormHexToRGB(colorByHex);
            }
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
        private static List RenderList(string type = "none", string[] content = null)
        {
            List listWarp = new List();
            listWarp.MarkerStyle = (type == "Disc") ? TextMarkerStyle.Disc : (type == "Decimal") ? TextMarkerStyle.Decimal : TextMarkerStyle.None;
            foreach (string line in content)
            {
                listWarp.ListItems.Add(new ListItem(new Paragraph(new Run(line))));
            }
            return listWarp;
        }
        private void CreateListClick(object sender, RoutedEventArgs e)
        {
            string Type = (sender as Button).Tag.ToString();
            string content = RichEditor.Selection.Text ?? "...";
            string[] selectedText = content.Split('\n');
            try
            {
                RichEditor.Selection.Text = "";
                TextRange range = new TextRange(RichEditor.Selection.Start, RichEditor.Selection.End);
                Paragraph refPara = range.Start.Paragraph;
                if(refPara == null)
                {
                    RichEditor.Document.Blocks.Add(new Paragraph());
                    RichEditor.Document.Blocks.InsertAfter(RichEditor.Document.Blocks.FirstBlock, RenderList(Type, selectedText));
                    return;
                }
                RichEditor.Document.Blocks.InsertAfter(refPara, RenderList(Type, selectedText)); //error ?
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Действие не поддерживается", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BindLinkButtonClick(object sender, RoutedEventArgs e)
        {
            UrlWindow getLink = new UrlWindow();
            getLink.ShowDialog();
            System.Windows.MessageBox.Show(getLink.Url);
            if (!string.IsNullOrEmpty(getLink.Url))
            {
                TextRange range = new TextRange(RichEditor.Selection.Start, RichEditor.Selection.End);
                this.ReplaceTextWithLink(RichEditor, range, new Uri(getLink.Url));
            }
        }
        private Hyperlink RenderHyperlink(string namelink, Uri linkUrl)
        {
            Hyperlink link = new Hyperlink(new Run(namelink));
            link.RequestNavigate += Hyperlink_RequestNavigate;
            link.NavigateUri = linkUrl;
            link.ToolTip = "Ссылка (кликните для перехода)";
            return link;
        }
        private void ReplaceTextWithLink(Xceed.Wpf.Toolkit.RichTextBox textBox, TextRange textRange, Uri link)
        {
            if (textRange.Start.Parent is Run)
            {
                var run = textRange.Start.Paragraph;
                var runBefore = new Run(new TextRange(run.ContentStart, textRange.Start).Text);
                var runAfter = new Run(new TextRange(textRange.End, run.ContentEnd).Text);
                var runBody = textBox.Selection.Text;
                textRange.Start.Paragraph.Inlines.Clear();
                textRange.Start.Paragraph.Inlines.Add(runBefore);
                textRange.Start.Paragraph.Inlines.Add(RenderHyperlink(runBody, link));
                textRange.Start.Paragraph.Inlines.Add(runAfter);
                textBox.CaretPosition = runAfter.ContentEnd;
            }
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start("explorer.exe", e.Uri.AbsoluteUri);
        }
    }
}