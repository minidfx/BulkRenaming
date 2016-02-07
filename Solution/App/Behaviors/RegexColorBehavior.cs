using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI;
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
                                                                                                       typeof (IEnumerable<string>),
                                                                                                       typeof (RegexColorBehavior),
                                                                                                       new PropertyMetadata(default(IEnumerable<string>), ResultPropertyChangedCallback));

        #endregion

        #region All other members

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
            var runs = paragraph.Inlines.OfType<Run>().Select(x => new Run
                                                                   {
                                                                       Text = x.Text,
                                                                       Foreground = x.Foreground
                                                                   }).ToList();

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

            var originalTextLength = originalText.Length;
            var resultParts = GetResult(dependencyObject).ToArray();

            var newParagraph = new Paragraph();

            if (!resultParts.Any())
            {
                newParagraph.Inlines.Add(new Run {Text = originalText});
                richTextBlock.Blocks.Add(newParagraph);

                return;
            }

            // The position for splitting the string
            uint startPosition = 0;

            foreach (var part in resultParts)
            {
                var indexOf = originalText.IndexOf(part, StringComparison.Ordinal);
                if (indexOf != -1)
                {
                    var runs = new Stack<Run>();
                    var originalPartLength = (uint) indexOf - startPosition;
                    var originalPart = originalText.Substring((int) startPosition, (int) originalPartLength);
                    var partLength = (uint) part.Length;
                    var nextStartPosition = originalPartLength + partLength;

                    runs.Push(new Run
                              {
                                  Text = part,
                                  Foreground = new SolidColorBrush(Colors.Red)
                              });
                    runs.Push(new Run
                              {
                                  Text = originalPart
                              });

                    while (runs.Any())
                    {
                        var run = runs.Pop();

                        newParagraph.Inlines.Add(run);
                    }

                    startPosition += nextStartPosition;
                }
            }

            // Add the rest of the string
            newParagraph.Inlines.Add(new Run
                                     {
                                         Text = originalText.Substring((int) startPosition)
                                     });

            newParagraph.Inlines.Add(new Run
                                     {
                                         Text = originalText.Substring(originalTextLength),
                                         Foreground = new SolidColorBrush(Colors.Green)
                                     });

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

        public static void SetResult(DependencyObject element, IEnumerable<string> value)
        {
            element.SetValue(ResultProperty, value);
        }

        public static IEnumerable<string> GetResult(DependencyObject element)
        {
            return (IEnumerable<string>) element.GetValue(ResultProperty);
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