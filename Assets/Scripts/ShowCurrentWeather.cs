using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using TMPro;

public class ShowCurrentWeather : MonoBehaviour
{
    private string apiUrl = "http://10.19.4.148:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4OTA4OTI5YzZhYTg0NjNhYmJhMGMxOWM4OGJhZmU0NSIsImlhdCI6MTcxMTQ2MDg5OSwiZXhwIjoyMDI2ODIwODk5fQ.FX1Q6EiTu6iHN8NkBwJqqk-2hTJZAiEz665BdcrRAME";
    private RootObject weatherData;
    public TMP_Text weatherText;

    public async void Start()
    {
        await SendGetRequest();
        UpdateWeatherDisplay();
    }
    public async Task SendGetRequest()
    {
        string entityId = "weather.forecast_mobility_lab";
        string url = apiUrl + "/states/" + entityId;

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            String responseBody = await response.Content.ReadAsStringAsync();
            weatherData = JsonUtility.FromJson<RootObject>(responseBody);

            weatherData.Print();
        }
    }

    public void UpdateWeatherDisplay()
    {
        if (weatherData != null)
        {
            string displayText = $"<size=25><color=#000000><b>Weather Information\n</b></color></size>";   
            displayText+= $"<size=22><b>{weatherData.attributes.friendly_name}</b></size>\n\n";
            displayText += $"<color=#000000>State:</color> {PrintState(weatherData.state)} <sprite name=\"{weatherData.state}\">\n";
            displayText += $"<color=#000000>Temperature:</color> {weatherData.attributes.temperature}°C\n";
            displayText += $"<color=#000000>Humidity:</color> {weatherData.attributes.humidity}%\n";
            displayText += $"<color=#000000>Pressure:</color> {weatherData.attributes.pressure} hPa\n";
            displayText += $"<color=#000000>Wind Speed:</color> {weatherData.attributes.wind_speed} km/h\n\n";
            displayText += $"<size=10><color=#000000>Last Updated:</color> {GetTimeAgoString(weatherData.last_updated)}</size>\n";

            weatherText.text = displayText;
        }
        else
        {
            weatherText.text = "<b>No weather data available</b>";
        }
    }

    private string GetForecastText()
    {
        string forecastText = string.Empty;

        if (weatherData.attributes.forecast != null && weatherData.attributes.forecast.days.Length > 0)
        {
            for (int i = 0; i < weatherData.attributes.forecast.days.Length; i++)
            {
                var day = weatherData.attributes.forecast.days[i];
                forecastText += $"\n{day.datetime}: {day.condition}, {day.temperature}°C";
            }
        }

        return forecastText;
    }

    string GetTimeAgoString(string datetime)
    {
        DateTime dateTime = DateTime.Parse(weatherData.last_changed);
        TimeSpan timeDiff = DateTime.Now - dateTime;

        if (timeDiff.TotalSeconds < 60)
        {
            return "Just now";
        }
        else if (timeDiff.TotalMinutes < 60)
        {
            int minutes = (int)timeDiff.TotalMinutes;
            return $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
        }
        else if (timeDiff.TotalHours < 24)
        {
            int hours = (int)timeDiff.TotalHours;
            return $"{hours} hour{(hours != 1 ? "s" : "")} ago";
        }
        else if (timeDiff.TotalDays < 30)
        {
            int days = (int)timeDiff.TotalDays;
            return $"{days} day{(days != 1 ? "s" : "")} ago";
        }
        else
        {
            return "More than 30 days ago";
        }
    }

    string PrintState(string state)
    {
        switch (state)
        {
            case "clear-night":
                return "Clear Night";
            case "cloudy":
                return "Cloudy";
            case "exceptional":
                return "Exceptional";
            case "fog":
                return "Fog";
            case "lightning":
                return "Lightning";
            case "lightning-rainy":
                return "Lightning rainy";
            case "partlycloudy":
                return "Partly cloudy";
            case "pouring":
                return "Pouring";
            case "rainy":
                return "Rainy";
            case "snowy":
                return "Snowy";
            case "snowyrainy":
                return "Snowy rainy";
            case "sunny":
                return "Sunny";
            case "windy":
                return "Windy";
            case "windy-variant":
                return "Windy";
            default:
                return "Unknown";
        }
    }


    [System.Serializable]
    public class RootObject
    {
        public string entity_id;
        public Attributes attributes;
        public string state;
        public string last_changed;
        public string last_updated;

        public void Print()
        {
            Debug.Log("Entity ID: " + entity_id);
            Debug.Log("State: " + state);
            Debug.Log("Last Changed: " + last_changed.ToString());
            Debug.Log("Last Updated: " + last_updated.ToString());

            attributes.Print();
        }
    }

    [System.Serializable]
    public class Attributes
    {
        public float temperature;
        public int humidity;
        public int pressure;
        public float wind_bearing;
        public int wind_speed;
        public Forecast forecast;
        public string friendly_name;

        public void Print()
        {
            Debug.Log("Temperature: " + temperature.ToString());
            Debug.Log("Humidity: " + humidity.ToString());
            Debug.Log("Pressure: " + pressure.ToString());
            Debug.Log("Wind Bearing: " + wind_bearing.ToString());
            Debug.Log("Wind Speed: " + wind_speed.ToString());
            Debug.Log("Friendly Name: " + friendly_name);

            if (forecast != null)
            {
                Debug.Log("Forecast:");
                forecast.Print();
            }
        }
    }

    [System.Serializable]
    public class Forecast
    {
        public ForecastDay[] days;

        public void Print()
        {
            if (days != null)
            {
                foreach (var day in days)
                {
                    day.Print();
                }
            }
        }
    }

    [System.Serializable]
    public class ForecastDay
    {
        public string condition;
        public string datetime;
        public float temperature;

        public void Print()
        {
            Debug.Log("Condition: " + condition);
            Debug.Log("Datetime: " + datetime.ToString());
            Debug.Log("Temperature: " + temperature.ToString());
        }
    }


}