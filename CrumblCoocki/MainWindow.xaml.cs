using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

public partial class MainWindow : Window
{
    private Dictionary<string, PlantData> plantData = new Dictionary<string, PlantData>();
    private Dictionary<string, double> mutationData = new Dictionary<string, double>();

    public MainWindow()
    {
        InitializeComponent();
        InitializeData();
    }

    private void InitializeData()
    {
        // Данные растений
        var plants = new Dictionary<string, (double minValue, double minWeight)>
        {
            {"lavender", (22563, 0.25)}, 
            {"nectarshade", (45125, 0.75)}, 
            {"nectarine", (35000, 2.807)},
            {"hive fruit", (55955, 7.59)}, 
            {"celestiberry", (9025, 1.9)}, 
            {"moon mango", (45125, 14.25)},
            {"blood banana", (5415, 1.42)}, 
            {"moon melon", (16245, 7.6)},
            {"carrot", (18, 0.24)},
            {"strawberry", (14, 0.29)}, 
            {"blueberry", (18, 0.17)}, 
            {"tomato", (27, 0.44)},
            {"cauliflower", (36, 4.74)}, 
            {"watermelon", (2708, 7.3)}, 
            {"green apple", (271, 2.85)},
            {"avocado", (91, 3.32)}, 
            {"banana", (1805, 1.42)}, 
            {"pineapple", (1805, 2.85)},
            {"kiwi", (2482, 3.5)}, 
            {"bell pepper", (4964, 7.61)}, 
            {"prickly pear", (6319, 6.65)},
            {"loquat", (7220, 6.17)}, 
            {"feijoa", (27075, 9.5)}, 
            {"sugar apple", (43320, 8.55)},
            {"crocus", (27075, 0.285)}, 
            {"succulent", (22563, 4.75)}, 
            {"violet corn", (45125, 2.85)},
            {"bendboo", (138988, 17.09)}, 
            {"cocovine", (60166, 13.3)}, 
            {"dragon pepper", (80000, 5.69)},
            {"rose", (4513, 0.95)}, 
            {"foxglove", (18050, 1.9)}, 
            {"lilac", (31588, 2.846)},
            {"pink lily", (58663, 5.699)}, 
            {"purple dahlia", (67688, 11.4)}, 
            {"sunflower", (144000, 15.65)},
            {"papaya", (903, 2.86)}, 
            {"passionfruit", (3204, 2.867)}, 
            {"soul fruit", (6994, 23.75)},
            {"cursed fruit", (15000, 22.9)}, 
            {"pear", (18050, 2.85)}, 
            {"raspberry", (90, 0.71)},
            {"peach", (271, 1.9)}, 
            {"nightshade", (3159, 0.48)}, 
            {"glowshroom", (271, 0.7)},
            {"mint", (4738, 0.95)}, 
            {"moonflower", (8574, 1.9)}, 
            {"starfruit", (13538, 2.85)},
            {"moonglow", (18050, 6.65)}, 
            {"moon blossom", (60166, 2.85)}, 
            {"cranberry", (1805, 0.95)},
            {"durian", (6317, 7.6)}, 
            {"eggplant", (6769, 4.75)}, 
            {"venus fly trap", (40612, 9.5)},
            {"lotus", (15343, 18.99)},
            {"chocolate carrot", (9960, 0.262)}, 
            {"red lollipop", (45102, 3.799)},
            {"candy sunflower", (72200, 1.428)},
            {"easter egg", (2256, 2.85)}, 
            {"candy blossom", (90250, 2.85)},
            {"manuka flower", (22563, 0.289)}, 
            {"bee balm", (16245, 0.94)}, 
            {"dandelion", (45125, 3.79)},
            {"nectar thorn", (30083, 5.76)}, 
            {"lumira", (76713, 5.69)}, 
            {"honeysuckle", (90250, 11.4)},
            {"suncoil", (72200, 9.5)}, 
            {"coconut", (400, 9.8)}, 
            {"dragon fruit", (4287, 8.4)}, 
            {"ember lily", (60166, 11.40)},
            {"bamboo", (3610, 2.8)},
            {"pepper", (7220, 3.5)}
        };

        foreach (var plant in plants)
        {
            plantData[plant.Key] = new PlantData
            {
                Name = ToTitleCase(plant.Key),
                MinValue = plant.Value.minValue,
                MinWeight = plant.Value.minWeight
            };
        }

        // Данные мутаций
        mutationData = new Dictionary<string, double>
        {
            {"gold", 20}, {"rainbow", 50}, {"wet", 2}, {"windstruck", 2}, {"moonlit", 2},
            {"chilled", 2}, {"bloodlit", 4}, {"twisted", 5}, {"frozen", 10}, {"shocked", 100},
            {"celestial", 120}, {"choc", 2}, {"pollinated", 3}, {"burnt", 4}, {"verdant", 4},
            {"honeyglazed", 5}, {"plasma", 5}, {"heavenly", 5}, {"cooked", 25}, {"zombified", 25},
            {"molten", 25}, {"sundried", 85}, {"alienlike", 100}, {"galactic", 120}, {"disco", 125},
            {"voidtouched", 135}, {"dawnbound", 150}
        };
    }

    private string ToTitleCase(string text)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
    }

    private void PlantNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var query = PlantNameTextBox.Text.ToLower();
        if (string.IsNullOrWhiteSpace(query))
        {
            PlantSuggestionsListBox.Visibility = Visibility.Collapsed;
            return;
        }

        var matches = plantData.Keys.Where(k => k.Contains(query)).Take(10).ToList();

        if (matches.Count > 0)
        {
            PlantSuggestionsListBox.Items.Clear();
            foreach (var match in matches)
            {
                PlantSuggestionsListBox.Items.Add(plantData[match].Name);
            }
            PlantSuggestionsListBox.Visibility = Visibility.Visible;
        }
        else
        {
            PlantSuggestionsListBox.Visibility = Visibility.Collapsed;
        }
    }

    private void PlantSuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PlantSuggestionsListBox.SelectedItem != null)
        {
            PlantNameTextBox.Text = PlantSuggestionsListBox.SelectedItem.ToString();
            PlantSuggestionsListBox.Visibility = Visibility.Collapsed;
        }
    }

    private void CalculateButton_Click(object sender, RoutedEventArgs e)
    {
        ClearResults();
        ClearError();

        try
        {
            // Проверка растения
            var plantName = PlantNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(plantName))
            {
                ShowError("Пожалуйста, введите название растения.");
                return;
            }

            var selectedPlant = plantData.FirstOrDefault(p =>
                p.Value.Name.Equals(plantName, StringComparison.OrdinalIgnoreCase));

            if (selectedPlant.Key == null)
            {
                ShowError($"Растение '{plantName}' не найдено в базе данных.");
                return;
            }

            var plant = selectedPlant.Value;

            // Проверка веса
            if (!double.TryParse(WeightTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double weight) || weight <= 0)
            {
                ShowError("Пожалуйста, введите корректный вес растения (положительное число).");
                return;
            }

            // Мутация роста
            var selectedGrowthMutation = (ComboBoxItem)GrowthMutationComboBox.SelectedItem;
            var growthMultiplier = double.Parse(selectedGrowthMutation.Tag.ToString());
            var growthMutationName = selectedGrowthMutation.Content.ToString().Split(' ')[0];

            // Мутации окружения
            var environmentalMutationsInput = EnvironmentalMutationsTextBox.Text.Trim();
            var selectedEnvironmentalMutations = new List<string>();
            double environmentalStackBonus = 0;

            if (!string.IsNullOrEmpty(environmentalMutationsInput))
            {
                var mutations = environmentalMutationsInput.Split(',')
                    .Select(m => m.Trim().ToLower())
                    .Where(m => !string.IsNullOrEmpty(m))
                    .ToList();

                // Проверка комбинаций
                if (!ValidateMutationCombinations(mutations, plant.Name))
                    return;

                foreach (var mutation in mutations)
                {
                    if (mutationData.ContainsKey(mutation))
                    {
                        selectedEnvironmentalMutations.Add(mutation);
                        environmentalStackBonus += (mutationData[mutation] - 1);
                    }
                    else
                    {
                        ShowError($"Мутация '{mutation}' не найдена в базе данных.");
                        return;
                    }
                }
            }

            // Расчет множителя
            var totalMultiplier = growthMultiplier * (1 + environmentalStackBonus);

            // Расчет стоимости
            double finalValue;
            string formulaUsed;

            if (weight <= plant.MinWeight)
            {
                finalValue = plant.MinValue * totalMultiplier;
                formulaUsed = "Min Value × Total Multiplier";
            }
            else
            {
                if (plant.MinWeight == 0)
                {
                    ShowError("Минимальный вес растения не может быть равен 0 для расчета по формуле с весом².");
                    return;
                }
                var kConstant = plant.MinValue / (plant.MinWeight * plant.MinWeight);
                finalValue = kConstant * (weight * weight) * totalMultiplier;
                formulaUsed = "k × Weight² × Total Multiplier";
            }

            // Вывод результатов
            DisplayResults(plant, weight, growthMutationName, growthMultiplier,
                          selectedEnvironmentalMutations, environmentalStackBonus,
                          totalMultiplier, finalValue, formulaUsed);
        }
        catch (Exception ex)
        {
            ShowError($"Произошла ошибка при расчете: {ex.Message}");
        }
    }

    private bool ValidateMutationCombinations(List<string> mutations, string plantName)
    {
        // Проверка chilled/wet/frozen
        var chilledGroup = new[] { "chilled", "wet", "frozen" };
        var chilledCount = mutations.Count(m => chilledGroup.Contains(m));
        if (chilledCount > 1)
        {
            ShowError("Можно выбрать только одну мутацию из: Chilled, Wet, Frozen.");
            return false;
        }

        // Проверка cooked/burnt
        var cookedGroup = new[] { "cooked", "burnt" };
        var cookedCount = mutations.Count(m => cookedGroup.Contains(m));
        if (cookedCount > 1)
        {
            ShowError("Можно выбрать только одну мутацию из: Cooked, Burnt.");
            return false;
        }

        // Проверка dawnbound
        if (mutations.Contains("dawnbound") && !plantName.Equals("Sunflower", StringComparison.OrdinalIgnoreCase))
        {
            ShowError("Мутация Dawnbound может быть применена только к Sunflower.");
            return false;
        }

        return true;
    }

    private void DisplayResults(PlantData plant, double weight, string growthMutationName,
                               double growthMultiplier, List<string> environmentalMutations,
                               double environmentalStackBonus, double totalMultiplier,
                               double finalValue, string formulaUsed)
    {
        ResultsPanel.Children.Clear();

        AddResultRow("Растение:", plant.Name);
        AddResultRow("Вес:", $"{weight:F2} кг");
        AddResultRow("Формула:", formulaUsed);
        AddResultRow("Мутация роста:", $"{growthMutationName} (×{growthMultiplier})");

        var envMutationsText = environmentalMutations.Count > 0
            ? string.Join(", ", environmentalMutations.Select(m => ToTitleCase(m)))
            : "Нет";
        AddResultRow("Мутации окружения:", envMutationsText);

        AddResultRow("Сумма Stack Bonuses:", $"{environmentalStackBonus:F2}");
        AddResultRow("Общий множитель:", $"{totalMultiplier:F2}");

        FinalPriceTextBlock.Text = $"💰 Итоговая стоимость: {finalValue:N2} Шекелей";
        FinalPriceBorder.Visibility = Visibility.Visible;
        ResultsBorder.Visibility = Visibility.Visible;
    }

    private void AddResultRow(string label, string value)
    {
        var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };

        var labelBlock = new TextBlock
        {
            Text = label,
            FontWeight = FontWeights.Bold,
            Width = 200,
            Foreground = System.Windows.Media.Brushes.DarkBlue
        };

        var valueBlock = new TextBlock
        {
            Text = value,
            Foreground = System.Windows.Media.Brushes.Black
        };

        panel.Children.Add(labelBlock);
        panel.Children.Add(valueBlock);
        ResultsPanel.Children.Add(panel);
    }

    private void ShowError(string message)
    {
        ErrorTextBlock.Text = message;
        ErrorTextBlock.Visibility = Visibility.Visible;
    }

    private void ClearError()
    {
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }

    private void ClearResults()
    {
        ResultsBorder.Visibility = Visibility.Collapsed;
        FinalPriceBorder.Visibility = Visibility.Collapsed;
    }

    public class PlantData
    {
        public string Name { get; set; }
        public double MinValue { get; set; }
        public double MinWeight { get; set; }
    }
}