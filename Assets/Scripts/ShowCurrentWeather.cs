using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using TMPro;

public class ShowCurrentWeather : MonoBehaviour
{
    private string apiUrl = "http://10.19.128.173:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4MWU3MDQ5MzIyYTM0YWY0YTMxM2U2NmZiNDY2MWE1ZiIsImlhdCI6MTY4NDUwMjExNiwiZXhwIjoxOTk5ODYyMTE2fQ.ioN1hFsnLVj2wye_JDaymqdJ2KPisBDZpBAXCwYt04U";
    private RootObject weatherData;
    public TMP_Text weatherText;

    public async void Start()
    {
        await SendGetRequest();
        //await Task.Delay(500);
        //await GetResponseBody();
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
            string displayText = $"<size=20><color=#000000><b>Weather Information - </b></color><b>{weatherData.attributes.friendly_name}</b>\n\n";
            displayText += $"<size=18><color=#000000>State:</color> {weatherData.state}</size>\n";
            displayText += $"<size=18><color=#000000>Last Changed:</color> {GetTimeAgoString(weatherData.last_changed)}</size>\n";
            displayText += $"<size=18><color=#000000>Last Updated:</color> {GetTimeAgoString(weatherData.last_updated)}</size>\n";
            displayText += $"<size=18><color=#000000>Temperature:</color> {weatherData.attributes.temperature}°C</size>\n";
            displayText += $"<size=18><color=#000000>Humidity:</color> {weatherData.attributes.humidity}%</size>\n";
            displayText += $"<size=18><color=#000000>Pressure:</color> {weatherData.attributes.pressure} hPa</size>\n";
            displayText += $"<size=18><color=#000000>Wind Speed:</color> {weatherData.attributes.wind_speed} km/h</size>\n";
            //displayText += $"<size=26><color=#0074D9>Forecast:</color> {GetForecastText()}</size>\n";

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
                forecastText += $"\n<size=24>{day.datetime}: {day.condition}, {day.temperature}°C</size>";
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