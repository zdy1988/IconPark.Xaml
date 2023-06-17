using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace IconXamlFactoryGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var wpfSettings = new WpfDrawingSettings();
            wpfSettings.CultureInfo = wpfSettings.NeutralCultureInfo;
            _fileSvgReader = new FileSvgReader(wpfSettings);
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _tsxes = System.IO.Directory.EnumerateFiles(openFileDialog.SelectedPath, "*.tsx");

                DataGrid.ItemsSource = _tsxes.Select(t => new TsxItem { Path = t, Name = Path.GetFileNameWithoutExtension(t) }).ToList();
            }
        }

        private void GenerateFileButton_Click(object sender, RoutedEventArgs e)
        {
            var length = _tsxes.Count();

            Task.Run(() =>
            {
                var index = 0;

                Generate(tsx =>
                {
                    index++;

                    this.Dispatcher.Invoke(() =>
                    {
                        this.MessageTextBlock.Text = $"{index}/{length} : {Path.GetFileNameWithoutExtension(tsx)}";
                    });
                });
            });
        }

        private void GenerateFileButton2_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                GenerateAllXamlFile(message =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.MessageTextBlock.Text = message;
                    });
                });
            });
        }

        private readonly FileSvgReader _fileSvgReader;
        private IEnumerable<string> _tsxes;
        private string outputFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "output");

        /// <summary>
        /// 处理拖拽导入的SVG文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generate(Action<string> callback)
        {
            try
            {
                if (!_tsxes.Any()) return;

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                foreach (var tsx in _tsxes)
                {
                    var sb = new StringBuilder();

                    var svgFileName = Path.GetFileNameWithoutExtension(tsx);

                    var xamlNameSpace = $"IconPark.Xaml.{svgFileName}";

                    var tsxString = System.IO.File.ReadAllText(tsx);

                    var svg = Regex.Match(tsxString, "<svg[^>]*?>[\\s\\S]*?<\\/svg>").Value;

                    svg = Regex.Replace(svg, "{props.size}", "\"48\"");
                    svg = Regex.Replace(svg, "{props.colors\\[0\\]}", "\"#000\"");
                    svg = Regex.Replace(svg, "{props.colors\\[1\\]}", "\"#111\"");
                    svg = Regex.Replace(svg, "{props.colors\\[2\\]}", "\"#222\"");
                    svg = Regex.Replace(svg, "{props.colors\\[3\\]}", "\"#333\"");
                    svg = Regex.Replace(svg, "{props.strokeWidth}", "\"2\"");
                    svg = Regex.Replace(svg, "{props.strokeLinecap}", "\"round\"");
                    svg = Regex.Replace(svg, "{props.strokeLinejoin}", "\"round\"");

                    svg = Regex.Replace(svg, "{'url\\(#' \\+ props.id \\+ '.*' \\+ '\\)'}", delegate (Match m) {
                        return $"\"url(#icon-{m.Value.Replace("{'url(#' + props.id + '", "").Replace("' + ')'}", "")})\"";
                    });
                    svg = Regex.Replace(svg, "{props.id \\+ '.*'}", delegate (Match m) {
                        return $"\"icon-{m.Value.Replace("{props.id + '", "").Replace("'}", "")}\"";
                    });
                    svg = Regex.Replace(svg, "style={{maskType: 'alpha'}}", "style=\"mask-type: alpha\"");

                    var stream = GenerateStreamFromString(svg);

                    Drawing drawing = _fileSvgReader.GetDrawingGroup(stream);

                    //去掉冗余的层次
                    while (drawing is DrawingGroup drawingGroup && (drawingGroup.Children.Count == 1))
                    {
                        var dr = drawingGroup.Children[0];
                        if (dr != null)
                        {
                            if (drawingGroup.Transform != null)
                            {
                                if (dr is DrawingGroup @group)
                                {
                                    @group.Transform = drawingGroup.Transform;
                                }
                                else if (dr is GeometryDrawing geometryDrawing)
                                {
                                    geometryDrawing.Geometry.Transform = drawingGroup.Transform;
                                }
                            }

                            drawing = dr;
                        }
                        else
                        {
                            break;
                        }
                    }

                    var drawingImage = new DrawingImage(drawing);
                    if (drawing is GeometryDrawing geometryDrawing1)
                    {
                        if (geometryDrawing1.Geometry is PathGeometry geo)
                        {
                            var pathGeometry = new PathGeometry();
                            foreach (var figure in geo.Figures)
                            {
                                pathGeometry.Figures.Add(figure);
                            }

                            var combineGeometry = Geometry.Combine(Geometry.Empty, pathGeometry,
                                GeometryCombineMode.Union, geometryDrawing1.Geometry.Transform);
                            geometryDrawing1.Geometry = combineGeometry;
                            drawing = geometryDrawing1;
                        }
                    }

                    var xaml = GetXaml(drawingImage);

                    sb.Append("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
                    sb.Append(Environment.NewLine);
                    sb.Append("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
                    sb.Append(Environment.NewLine);
                    sb.Append("                    xmlns:local=\"clr-namespace:IconPark.Xaml.Full\">");
                    sb.Append(Environment.NewLine);
                    sb.Append("<ControlTemplate x:Key=\"" + xamlNameSpace + "\" TargetType=\"{x:Type local:Icon}\">");
                    sb.Append(Environment.NewLine);
                    sb.Append("<Border>");
                    sb.Append(Environment.NewLine);
                    sb.Append("<Image Width=\"{TemplateBinding Width}\" Height=\"{TemplateBinding Height}\">");
                    sb.Append(Environment.NewLine);
                    sb.Append("<Image.Source>");
                    sb.Append(Environment.NewLine);



                    xaml = xaml.Replace(" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"", string.Empty);
                    xaml = xaml.Replace(" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"", string.Empty);
                    xaml = xaml.Replace(" xmlns:svg=\"http://sharpvectors.codeplex.com/runtime/\"", string.Empty);
                    xaml = xaml.Replace(" Pen=\"{x:Null}\"", string.Empty);

                    xaml = Regex.Replace(xaml, " xmlns:svr=\".*\"", string.Empty);
                    xaml = Regex.Replace(xaml, " svr:SvgLink.Key=\".*\"", string.Empty);
                    xaml = Regex.Replace(xaml, " svg:SvgLink.Key=\".*\"", string.Empty);
                    xaml = Regex.Replace(xaml, " svg:SvgObject.Id=\".*\"", string.Empty);
                    xaml = Regex.Replace(xaml, "<PathGeometry FillRule=\"EvenOdd\" Figures=\"([^T]*?)\" />", "<StreamGeometry>$1</StreamGeometry>");

                    xaml = Regex.Replace(xaml, "\"#FF000000\"", "\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=ColorLayer.OuterStrokeColorBrush}\"");
                    xaml = Regex.Replace(xaml, "\"#FF111111\"", "\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=ColorLayer.OuterFillColorBrush}\"");
                    xaml = Regex.Replace(xaml, "\"#FF222222\"", "\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=ColorLayer.InnerStrokeColorBrush}\"");
                    xaml = Regex.Replace(xaml, "\"#FF333333\"", "\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=ColorLayer.InnerFillColorBrush}\"");

                    xaml = Regex.Replace(xaml, "LineCap=\"Round\"", "LineCap=\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=StrokeLineCap}\"");
                    xaml = Regex.Replace(xaml, "LineJoin=\"Round\"", "LineJoin=\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=StrokeLineJoin}\"");
                    xaml = Regex.Replace(xaml, "Thickness=\"2\"", "Thickness=\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=StrokeThickness}\"");


                    sb.Append(xaml);


                    sb.Append("</Image.Source>");
                    sb.Append(Environment.NewLine);
                    sb.Append("</Image>");
                    sb.Append(Environment.NewLine);
                    sb.Append("</Border>");
                    sb.Append(Environment.NewLine);
                    sb.Append("</ControlTemplate>");
                    sb.Append(Environment.NewLine);
                    sb.Append("</ResourceDictionary>");
                    sb.Append(Environment.NewLine);

                    var tempXamlFile = Path.Combine(outputFolder, $"{xamlNameSpace}.xaml");

                    WriteToFile(sb.ToString(), tempXamlFile);

                    callback?.Invoke(tsx);
                }

                GenerateAllXamlFile(callback);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GenerateAllXamlFile(Action<string> callback)
        {
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                callback.Invoke("生成资源聚集文件中...");

                var allXamlFile = Path.Combine(outputFolder, $"IconPark.All.xaml");

                if (File.Exists(allXamlFile))
                {
                    File.Delete(allXamlFile);
                }

                var xamlFiles = System.IO.Directory.EnumerateFiles(outputFolder);

                var sb2 = new StringBuilder();
                sb2.Append("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
                sb2.Append(Environment.NewLine);
                sb2.Append("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
                sb2.Append(Environment.NewLine);
                sb2.Append("                    xmlns:local=\"clr-namespace:IconPark.Xaml.Full\">");
                sb2.Append(Environment.NewLine);
                sb2.Append("    <ResourceDictionary.MergedDictionaries>");
                sb2.Append(Environment.NewLine);

                foreach (var xamlFile in xamlFiles)
                {
                    sb2.Append($"<ResourceDictionary Source=\"./{Path.GetFileNameWithoutExtension(xamlFile)}.xaml\" />");
                    sb2.Append(Environment.NewLine);
                }

                sb2.Append("    </ResourceDictionary.MergedDictionaries>");
                sb2.Append(Environment.NewLine);
                sb2.Append("</ResourceDictionary>");
                sb2.Append(Environment.NewLine);

                var tempXamlFile2 = Path.Combine(outputFolder, $"IconPark.All.xaml");

                WriteToFile(sb2.ToString(), tempXamlFile2);

                callback.Invoke("生成资源聚集文件完毕...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        #region 辅助方法

        /// <summary>
        /// 将对象转换为XAML字符串并返回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string GetXaml(object obj)
        {
            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                Encoding = Encoding.ASCII
            };

            string xaml;
            using (var xamlStream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(xamlStream, writerSettings))
                {
                    XamlWriter.Save(obj, writer);
                }

                xaml = Encoding.ASCII.GetString(xamlStream.ToArray());
            }

            return xaml;
        }

        /// <summary>
        /// 将字符串写入指定文件中
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filePath"></param>
        private static void WriteToFile(string content, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.ASCII))
            {
                sw.Write(content);
                sw.Flush();
            }
        }

        #endregion
    }

    public class TsxItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

}
