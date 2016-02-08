using System;
using System.Linq;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

using WinRTXamlToolkit.Controls.Extensions;

namespace App.Behaviors
{
    public static class RegexColorBehavior
    {
        #region Fields and Constants

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.RegisterAttached("FileName",
                                                                                                         typeof (string),
                                                                                                         typeof (RegexColorBehavior),
                                                                                                         new PropertyMetadata(default(string), FileNamePropertyChangedCallback));

        public static readonly DependencyProperty FuturResultProperty = DependencyProperty.RegisterAttached("FuturResult",
                                                                                                            typeof (string),
                                                                                                            typeof (RegexColorBehavior),
                                                                                                            new PropertyMetadata(default(string), FuturResultPropertyChangedCallback));

        public static readonly DependencyProperty ResultProperty = DependencyProperty.RegisterAttached("Result",
                                                                                                       typeof (string),
                                                                                                       typeof (RegexColorBehavior),
                                                                                                       new PropertyMetadata(default(string), ResultPropertyChangedCallback));

        #endregion

        #region All other members

        private static Run CreateRun(string text)
        {
            return new Run
                   {
                       Text = text
                   };
        }

        private static Run CreateRun(string text, Brush foreground, FontWeight fontWeight)
        {
            return new Run
                   {
                       Text = text,
                       Foreground = foreground,
                       FontWeight = fontWeight
                   };
        }

        private static Run CloneRun(Run x)
        {
            return CreateRun(x.Text, x.Foreground, x.FontWeight);
        }

        private static void FuturResultPropertyChangedCallback(DependencyObject dependencyObject,
                                                               DependencyPropertyChangedEventArgs e)
        {
            var richTextBlock = (RichTextBlock) dependencyObject;
            var futurResult = (string) e.NewValue;

            if (string.IsNullOrEmpty(futurResult))
            {
                return;
            }

            var paragraph = richTextBlock.Blocks.OfType<Paragraph>().Single();

            // Filter and create new run blocks with its configurations
            var runs = paragraph.Inlines.OfType<Run>().Select(CloneRun).ToList();

            // Clear old previous run blocks
            richTextBlock.Blocks.Clear();

            // Create a new paragrah with its blocks
            var newParagraph = new Paragraph();
            runs.ForEach(x => newParagraph.Inlines.Add(x));

            // Add the custom block containing the regex result
            newParagraph.Inlines.Add(new Span
                                     {
                                         Inlines = {new Run {Text = futurResult}},
                                         Foreground = new SolidColorBrush(Colors.Green)
                                     });

            richTextBlock.Blocks.Add(newParagraph);
        }

        private static void ResultPropertyChangedCallback(DependencyObject dependencyObject,
                                                          DependencyPropertyChangedEventArgs e)
        {
            var richTextBlock = (RichTextBlock) dependencyObject;

            var originalText = GetFileName(dependencyObject);
            if (originalText == null)
            {
                return;
            }

            richTextBlock.Blocks.Clear();

            var result = GetResult(dependencyObject);
            var newParagraph = new Paragraph();

            if (result == null)
            {
                newParagraph.Inlines.Add(CreateRun(originalText));
                richTextBlock.Blocks.Add(newParagraph);

                return;
            }

            var indexOf = originalText.IndexOf(result, StringComparison.Ordinal);
            if (indexOf != -1)
            {
                newParagraph.Inlines.Add(CreateRun(originalText.Substring(0, indexOf)));
                newParagraph.Inlines.Add(CreateRun(originalText.Substring(indexOf, result.Length), new SolidColorBrush(Colors.Red), FontWeights.Bold));
                newParagraph.Inlines.Add(CreateRun(originalText.Substring(indexOf + result.Length)));
            }

            richTextBlock.Blocks.Add(newParagraph);
        }

        private static void FileNamePropertyChangedCallback(DependencyObject dependencyObject,
                                                            DependencyPropertyChangedEventArgs e)
        {
            var richTextBlock = (RichTextBlock) dependencyObject;

            if (richTextBlock.Blocks.Any())
            {
                return;
            }

            var fileName = e.NewValue as string;
            if (fileName == null)
            {
                return;
            }

            richTextBlock.AppendText(fileName);
        }

        public static void SetFuturResult(DependencyObject element, string value)
        {
            element.SetValue(FuturResultProperty, value);
        }

        public static string GetFuturResult(DependencyObject element)
        {
            return (string) element.GetValue(FuturResultProperty);
        }

        public static void SetResult(DependencyObject element, string value)
        {
            element.SetValue(ResultProperty, value);
        }

        public static string GetResult(DependencyObject element)
        {
            return (string) element.GetValue(ResultProperty);
        }

        public static string GetFileName(DependencyObject obj)
        {
            return (string) obj.GetValue(FileNameProperty);
        }

        public static void SetFileName(DependencyObject obj, string value)
        {
            obj.SetValue(FileNameProperty, value);
        }

        #endregion
    }
}