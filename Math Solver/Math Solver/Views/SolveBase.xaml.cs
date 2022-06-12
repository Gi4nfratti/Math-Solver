using Math_Solver.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Math_Solver.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SolveBase : ContentPage
    {
        Grid gridFrame, gridInputs;
        Grid grid = new Grid();
        private string idNameAux;
        Thickness margin;
        ScrollView scrollView, scrollViewInputs;
        Frame frameFormula, frameInputs;
        bool isFirst = true;
        public List<Sequences> sequenceList = new List<Sequences>();
        List<Entry> entriesTemp;
        //Entry[] entries;
        dynamic entries;
        int time;
        string[] arrayValues;

        private string _toolbarText;
        public string ToolbarText { get => _toolbarText; set { _toolbarText = value; OnPropertyChanged(); } }

        public SolveBase(string idName, string title)
        {
            InitializeComponent();
            idNameAux = idName;

            SearchFunc(idName);
            
            ToolbarText = title;
        }

        void SearchFunc(string idName)
        {
            var idNames = new Dictionary<string, Action>()
               {
                   { "bhaskara", () => PrepareInputLine(sequenceList) },
                   { "bhaskara22", () => PrepareInputLine(sequenceList) },
                   { "bhaskara33", () => PrepareInputLine(sequenceList) },
                   { "bhaskara44", () => PrepareInputLine(sequenceList) },
                   { "soma", () => PrepareInputLine(sequenceList) },
                   { "subtraction", () => PrepareInputLine(sequenceList) },
                   { "multiplication", () => PrepareInputLine(sequenceList) },
                   { "division", () => PrepareInputLine(sequenceList) },
                   { "fraction", () => PrepareInputLine(sequenceList) }
               };

            if (idNames.ContainsKey(idName))
            {
                var r = App.mathList.Where(x => x.IdName == idName).Select(y => (y.Id, y.Data, y.Lines, y.IdExtraFormula)).First();
                sequenceList.Add(new Sequences() { FormulaId = r.Id, Sequence = r.Data, Lines = r.Lines, IdExtraFormula = r.IdExtraFormula });

                idNames[idName].Invoke();
            }
        }

        void PrepareInputLine(List<Sequences> listFormula)
        {
            arrayValues = SplitSequence(listFormula);
            int count = 0, entryQnt = 0;
            int countEntry = 0;
            string placeHolder = string.Empty;
            entriesTemp = new List<Entry>();

            if (arrayValues.Length > 0)
            {
                StandardImgExample();
                entryQnt = arrayValues.Count(e => e.Contains("entry"));
                int valueFrac = arrayValues.Count(e => e.Contains("/"));
                entryQnt += valueFrac > 0 ? valueFrac : 0;

                foreach (var item in arrayValues)
                {
                    if (String.IsNullOrEmpty(item) && valueFrac > 0)
                    {
                        continue;
                    }
                    else
                    {
                        entries = new Entry[entryQnt];

                        if (item.Contains("entry"))
                        {
                            placeHolder = GetPlaceholder(item);
                            if (item.Contains("pow"))
                            {
                                string pow = CheckPow(item);
                                placeHolder += pow + " \t";
                                entries[countEntry] = new Entry() { IsVisible = true, MaxLength = 9999999, TextColor = Color.Black, Keyboard = Keyboard.Numeric, PlaceholderColor = Color.DarkGray, Placeholder = placeHolder, HorizontalTextAlignment = TextAlignment.Center };
                                entries[countEntry].AutomationId = countEntry.ToString();
                                gridInputs.Children.Add(entries[countEntry], count, 0);
                            }
                            else if (item.Contains("/"))
                            {
                                string[] fracPLs = GetPlaceholder(item).Split('/', (char)StringSplitOptions.RemoveEmptyEntries);
                                fracPLs[0] = fracPLs[0].Replace("\t ", "");
                                StackLayout stackFrac = new StackLayout() { Spacing = 0 };
                                BoxView box = new BoxView() { BackgroundColor = Color.Black, HeightRequest = 1 };

                                entries[countEntry] = new Entry() { IsVisible = true, MaxLength = 9999999, TextColor = Color.Black, Keyboard = Keyboard.Numeric, PlaceholderColor = Color.DarkGray, Placeholder = fracPLs[0], HorizontalTextAlignment = TextAlignment.Center };
                                entries[countEntry].AutomationId = countEntry.ToString();

                                stackFrac.Children.Add(entries[countEntry]);
                                stackFrac.Children.Add(box);

                                entriesTemp.Add(entries[countEntry]);
                                CheckCompleted(placeHolder, entries[countEntry]);
                                countEntry++;

                                entries[countEntry] = new Entry() { IsVisible = true, MaxLength = 9999999, TextColor = Color.Black, Keyboard = Keyboard.Numeric, PlaceholderColor = Color.DarkGray, Placeholder = fracPLs[1], HorizontalTextAlignment = TextAlignment.Center };
                                entries[countEntry].AutomationId = countEntry.ToString();
                                stackFrac.Children.Add(entries[countEntry]);
                                gridInputs.Children.Add(stackFrac, count, 0);
                            }
                            else
                            {
                                placeHolder += " \t";
                                entries[countEntry] = new Entry() { IsVisible = true, MaxLength = 9999999, TextColor = Color.Black, Keyboard = Keyboard.Numeric, PlaceholderColor = Color.DarkGray, Placeholder = placeHolder, HorizontalTextAlignment = TextAlignment.Center };
                                entries[countEntry].AutomationId = countEntry.ToString();
                                gridInputs.Children.Add(entries[countEntry], count, 0);
                            }

                            entriesTemp.Add(entries[countEntry]);
                            CheckCompleted(placeHolder, entries[countEntry]);
                            countEntry++;
                        }
                        else
                        {
                            Label lblBase = new Label() { Text = item, HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Center };
                            gridInputs.Children.Add(lblBase, count, 0);
                        }

                        count++;
                    }
                }
            }
            PrepareButtonSolve(arrayValues);
        }

        private void PrepareButtonSolve(string[] arrayValues)
        {
            Button btnSolve = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Resolver",
            };
                btnSolve.Clicked += async (s, e) =>
                {
                    foreach (var entry in entriesTemp)
                    {
                        if (String.IsNullOrEmpty(entry.Text))
                        {
                            entry.PlaceholderColor = Color.Red;
                            return;
                        }
                    }

                    frameFormula.IsVisible = true;
                    frameFormula.IsEnabled = true;

                    if (!isFirst)
                        await frameFormula.FadeTo(0, 400);

                    grid.Children.Clear();
                    DisplayResultLines(sequenceList);
                    isFirst = false;

                    await frameFormula.FadeTo(100, 200);
                };
                stackLayoutMain.Children.Add(btnSolve);
        }

        private void DisplayResultLines(List<Sequences> sequenceList)
        {
            List<string> listTexts = new List<string>();
            List<string> listTextsAux = new List<string>();
            Entry entryAux, entryStackAux;
            Label labelAux, labelStackAux;
            StackLayout stackAux;
            List<string> lines = new List<string>();

            try {
                int qntLines = int.Parse(sequenceList[0].Lines.Substring(0, sequenceList[0].Lines.IndexOf("|")));
                string stringLines = sequenceList[0].Lines.Substring(sequenceList[0].Lines.IndexOf("|") + "|".Length);
                if (stringLines.Contains('@'))
                    lines = stringLines.Split('@', (char)StringSplitOptions.RemoveEmptyEntries).ToList();
                else
                    lines.Add(stringLines);

                foreach (var item in gridInputs.Children)
                {
                    switch (item.GetType().Name)
                    {
                        case "Entry":
                            entryAux = item as Entry;
                            listTexts.Add(entryAux.Text);
                            break;
                        case "Label":
                            labelAux = item as Label;
                            listTexts.Add(labelAux.Text);
                            break;
                        case "StackLayout":
                            stackAux = item as StackLayout;
                            foreach (var stackItem in stackAux.Children)
                            {
                                if (stackItem.GetType().Name == "Entry")
                                {
                                    entryStackAux = stackItem as Entry;
                                    listTextsAux.Add(entryStackAux.Text);
                                }
                                else if (stackItem.GetType().Name == "Label")
                                {
                                    labelStackAux = stackItem as Label;
                                    listTextsAux.Add(labelStackAux.Text);
                                }
                            }
                            listTexts.Add(String.Join(",", listTextsAux.ToArray()));
                            break;
                        default:
                            break;
                    }
                    /*
                    if (item.GetType().Name == "Entry")
                    {
                        entryAux = item as Entry;
                        listTexts.Add(entryAux.Text);
                    }
                    else if (item.GetType().Name == "Label")
                    {
                        labelAux = item as Label;
                        listTexts.Add(labelAux.Text);
                    }
                    */
                }

                grid.Children.Clear();
                
                for (int i = 0; i < qntLines; i++)
                {
                    grid = gridFrame;
                    string field = lines[i].Substring(0, lines[i].IndexOf('#'));
                    string operation = lines[i].Substring(lines[i].IndexOf('#') + "#".Length);
                    string textAux = string.Empty;

                    for (int j = 0; j < listTexts.Count; j++)
                    {
                        textAux = operation.Contains("frac") ? SolveLine(j, field, operation, listTexts, true) : SolveLine(j, field, operation, listTexts);
                        if (!string.IsNullOrEmpty(textAux))
                        {
                            grid.Children.Add(new Label()
                            {
                                Text = textAux,
                                HorizontalTextAlignment = TextAlignment.Center,
                                TextColor = GetColor(j),
                                HorizontalOptions = LayoutOptions.Center
                            }, j, i);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private string SolveLine(int j, string field, string operation, List<string> listTexts, bool isFrac = false)
        {
            string[] fields = null;
            List<string> texts = new List<string>();
            string text = string.Empty;
            dynamic textResult;
            int count = new int();

            if (field.Contains(','))
                fields = field.Split(',', (char)StringSplitOptions.RemoveEmptyEntries);

            if (fields == null && j == int.Parse(field) || fields != null || isFrac)
            {
                if (isFrac)
                {
                    foreach (var item in listTexts[0].Split(','))
                    {
                        texts.Add(item);
                    } 

                    textResult = ExecuteOperation(texts, operation);
                    if (textResult.ToString().Contains(","))
                        textResult = double.Parse(Math.Round(textResult, 1).ToString());

                    listTexts[j] = textResult.ToString();
                    return textResult.ToString();
                }
                else if (fields == null)
                {
                    text = listTexts[int.Parse(field)];
                    text = RemovePow(text);
                    texts.Add(text);
                    textResult = ExecuteOperation(texts, operation);
                    if (textResult.ToString().Contains(","))
                        textResult = double.Parse(Math.Round(textResult, 1).ToString());

                    listTexts[j] = textResult.ToString();
                    return textResult.ToString();
                }
                else
                {
                    if (fields.First().Contains(j.ToString()))
                    {
                        foreach (var item in fields)
                        {
                            string noPow = string.Empty;
                            noPow = RemovePow(listTexts[int.Parse(item)]);
                            texts.Add(noPow);
                            ++count;
                        }
                        textResult = ExecuteOperation(texts, operation);
                        if (textResult.ToString().Contains(","))
                            textResult = double.Parse(Math.Round(textResult, 1).ToString());

                        for (int i = int.Parse(fields.First()); i <= int.Parse(fields.Last()); i++)
                        {
                            listTexts[i] = "";
                        }
                        listTexts[j] = textResult.ToString();
                        return textResult.ToString();
                    }
                    else
                    {
                        return listTexts[j].ToString();
                    }
                }
            }
            else
            {
                return listTexts[j].ToString();
            }
        }

        private dynamic ExecuteOperation(List<string> text, string operation)
        {
            if (operation.Contains("pow"))
            {
                string numPow = operation.Substring(operation.IndexOf('w') + "w".Length);
                return Math.Pow(double.Parse(text[0].ToString()), double.Parse(numPow));
            }

            switch (operation)
            {
                case "+":
                    return double.Parse(text[0]) + double.Parse(text[1]);
                case "-":
                    return double.Parse(text[0]) - double.Parse(text[1]);
                case "*":
                    return double.Parse(text[0]) * double.Parse(text[1]);
                case "/":
                case "frac":
                    return double.Parse(text[0]) / double.Parse(text[1]);
                default:
                    return "";
            }
        }

        private void CheckCompleted(string item, Entry entry)
        {
            entry.Completed += (s, e) =>
            {
                var entryS = (Entry)s;
                
                if (item.Contains("\u00b2") || item.Contains("\u00b3") || item.Contains("^"))
                {
                    if (!entryS.Text.Contains("\u00b2") || !entryS.Text.Contains("\u00b3") || !entryS.Text.Contains("^"))
                    {
                        entryS.Text += "\u00b2";
                    }
                    if(Regex.Matches(entryS.Text, "\u00b2").Count == 2)
                    {
                        entryS.Text = entryS.Text.Replace("\u00b2", "");
                        entryS.Text += "\u00b2";
                    }
                }

                time = int.Parse(entryS.AutomationId) + 1;
                if (time < entries.Length)
                {
                    entries[time] = entriesTemp[time];
                    entries[time].Focus();
                }
            };
        }

        private string GetPlaceholder(string item)
        {
            int fIndex = item.IndexOf("$") + "$".Length;
            int lIndex = item.LastIndexOf("$");

            return "\t " + item.Substring(fIndex, lIndex - fIndex);
        }

        private string CheckPow(string item)
        {

            int fIndex = item.IndexOf("pow") + "pow".Length;
            int lIndex = item.LastIndexOf("endpow");

            string itemTemp = item.Substring(fIndex, lIndex - fIndex);
            switch (itemTemp)
            {
                case "2":
                    return "\u00b2";
                case "3":
                    return "\u00b3";
                default:
                    return "^" + itemTemp;
            }
        }

        private string[] SplitSequence(List<Sequences> listFormula)
        {
            try
            {
                return listFormula[0].Sequence.Split('|');
            }
            catch
            {
                throw new Exception("Erro ao dividir sequência");
            }
        }

        private Color GetColor(int j)
        {
            switch (j)
            {
                case 1:
                    return Color.Brown;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Purple;
                case 4:
                    return Color.Orange;
                case 5:
                    return Color.DeepPink;
                case 6:
                    return Color.Green;
                case 7:
                    return Color.Gold;
                case 8:
                    return Color.DarkGray;
                case 9:
                    return Color.Red;
                case 10:
                    return Color.Blue;
                default:
                    return Color.Black;
            }
        }

        private string RemovePow(string toRemovePow)
        {
            if (toRemovePow.Contains("\u00b2"))
                toRemovePow = toRemovePow.Replace("\u00b2", String.Empty);
            else if (toRemovePow.Contains("\u00b3"))
                toRemovePow = toRemovePow.Replace("\u00b3", String.Empty);
            else if (toRemovePow.Contains("^"))
                toRemovePow = toRemovePow.Replace(toRemovePow.Substring('^'), String.Empty);

            return toRemovePow;
        }

        public void StandardImgExample()
        {
            Utils.Utils utils = new Utils.Utils();
            Image imgNotation = new Image()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Source = utils.GetBase64Image(idNameAux),
                HeightRequest = 100
            };
            stackLayoutMain.Children.Add(imgNotation);

            BoxView boxView = new BoxView() { HeightRequest = 1, Color = Color.DarkGray };
            stackLayoutMain.Children.Add(boxView);

            scrollViewInputs = new ScrollView()
            {
                Orientation = ScrollOrientation.Vertical,
            };
            stackLayoutMain.Children.Add(scrollViewInputs);

            scrollView = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
            };
            stackLayoutMain.Children.Add(scrollView);

            margin = new Thickness(5, 0);

            frameInputs = new Frame() { VerticalOptions = LayoutOptions.StartAndExpand, Margin = margin, HorizontalOptions = LayoutOptions.Fill, CornerRadius = 3, BorderColor = Color.DarkGray, BackgroundColor = Color.WhiteSmoke, Padding = 2 };
            scrollViewInputs.Content = frameInputs;

            gridInputs = new Grid()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 5,
                BackgroundColor = Color.Transparent,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                }
            };
            frameInputs.Content = gridInputs;

            margin = new Thickness(10, 0);

            frameFormula = new Frame() { VerticalOptions = LayoutOptions.StartAndExpand, Margin = margin, HorizontalOptions = LayoutOptions.Fill, CornerRadius = 3, BorderColor = Color.DarkGray, BackgroundColor = Color.WhiteSmoke, Padding = 2 };
            scrollView.Content = frameFormula;

            gridFrame = new Grid()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //HorizontalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 5,
                BackgroundColor = Color.Transparent,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                }
            };
            frameFormula.Content = gridFrame;

            frameFormula.IsVisible = false;
            frameFormula.IsEnabled = false;
        }
    }
}