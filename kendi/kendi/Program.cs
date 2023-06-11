using System;
using System.Collections.Generic;

public class WeatherMarkovChain
{
    private Dictionary<string, Dictionary<string, double>> transitionMatrix;
    private Random random;

    public WeatherMarkovChain()
    {
        transitionMatrix = new Dictionary<string, Dictionary<string, double>>();
        random = new Random();
    }

    public void AddTransition(string currentState, string nextState, double probability)
    {
        if (!transitionMatrix.ContainsKey(currentState))
        {
            transitionMatrix[currentState] = new Dictionary<string, double>();
        }

        transitionMatrix[currentState][nextState] = probability;
    }

    public string GenerateWeatherForecast(int days, string initialState)
    {
        string currentWeather = initialState;
        string forecast = currentWeather;

        for (int i = 0; i < days; i++)
        {
            string nextWeather = GetNextWeather(currentWeather);
            forecast += " -> " + nextWeather;
            currentWeather = nextWeather;
        }

        return forecast;
    }

    private string GetNextWeather(string currentState)
    {
        Dictionary<string, double> transitions = transitionMatrix[currentState];

        double randomValue = random.NextDouble();
        double cumulativeProbability = 0;

        foreach (KeyValuePair<string, double> transition in transitions)
        {
            cumulativeProbability += transition.Value;
            if (randomValue < cumulativeProbability)
            {
                return transition.Key;
            }
        }

        // Eğer geçiş yapılamazsa, rastgele bir durum seç
        int randomIndex = random.Next(transitions.Count);
        return transitions.Keys.ToArray()[randomIndex];
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        WeatherMarkovChain markovChain = new WeatherMarkovChain();

        // Durumlar ve geçiş olasılıkları ekleme
        markovChain.AddTransition("güneşli", "güneşli", 0.7);
        markovChain.AddTransition("güneşli", "bulutlu", 0.2);
        markovChain.AddTransition("güneşli", "yağmurlu", 0.1);
        markovChain.AddTransition("bulutlu", "güneşli", 0.3);
        markovChain.AddTransition("bulutlu", "bulutlu", 0.4);
        markovChain.AddTransition("bulutlu", "yağmurlu", 0.3);
        markovChain.AddTransition("yağmurlu", "güneşli", 0.2);
        markovChain.AddTransition("yağmurlu", "bulutlu", 0.6);
        markovChain.AddTransition("yağmurlu", "yağmurlu", 0.2);

        // Tahmin oluşturma
        int forecastDays = 5;
        string initialState = "güneşli";

        string forecast = markovChain.GenerateWeatherForecast(forecastDays, initialState);
        Console.WriteLine("Hava Durumu Tahmini: " + forecast);
    }
}