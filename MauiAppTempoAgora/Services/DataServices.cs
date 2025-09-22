using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataServices
    {
        //  public static async Task<Tempo?> GetPrevisao(string cidade)
        //{
        //  Tempo? t = null;

        //string chave = "0dc3095060cd176f8559e274ff2302bd";
        //string url = $"https://api.openweathermap.org/data/2.5/weather?" +
        //  $"q={cidade}&units=metric&APPID={chave}";

        //using (HttpClient client = new HttpClient()) 
        //{
        //  HttpResponseMessage resp = await client.GetAsync(url);
        //if(resp.IsSuccessStatusCode)
        //{
        //  string json = await resp.Content.ReadAsStringAsync();

        //var rascunho = JObject.Parse(json);

        //DateTime time = new();
        //DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
        //DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();


        //t = new()
        //{
        //  lat = (double)rascunho["coord"]["lat"],
        //lon = (double)rascunho["coord"]["lon"],
        //description = (string)rascunho["weather"][0]["description"],
        //main = (string)rascunho["weather"][0]["main"],
        //temp_min = (double)rascunho["main"]["temp_min"],
        //temp_max = (double)rascunho["main"]["temp_max"],
        //speed = (double)rascunho["wind"]["speed"],                     
        //visibility = (int)rascunho["visibility"],

        //sunrise = sunrise.ToString(),
        //sunset = sunset.ToString(),
        // }; //fecha objeto do tempo
        //}//fecha if se o status do servidor foi de sucesso
        // }//fecha laço using

        // return t;
        // }

        // }
        //}

  
            public static async Task<Tempo?> GetPrevisao(string cidade)
            {
                Tempo? t = null;

                string chave = "0dc3095060cd176f8559e274ff2302bd";
                string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                             $"q={cidade}&units=metric&APPID={chave}";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage resp = await client.GetAsync(url);

                        // Cidade não encontrada
                        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            throw new Exception("Cidade não encontrada. Verifique o nome digitado.");
                        }

                        // Outros erros de servidor
                        if (!resp.IsSuccessStatusCode)
                        {
                            throw new Exception("Erro ao obter dados do servidor.");
                        }

                        // Processa os dados
                        string json = await resp.Content.ReadAsStringAsync();
                        var rascunho = JObject.Parse(json);

                        var sunrise = DateTimeOffset.FromUnixTimeSeconds((long)(rascunho["sys"]?["sunrise"] ?? 0)).ToLocalTime();
                        var sunset = DateTimeOffset.FromUnixTimeSeconds((long)(rascunho["sys"]?["sunset"] ?? 0)).ToLocalTime();

                        t = new Tempo
                        {
                            lat = (double?)rascunho["coord"]?["lat"] ?? 0,
                            lon = (double?)rascunho["coord"]?["lon"] ?? 0,
                            description = (string?)rascunho["weather"]?[0]?["description"] ?? "N/A",
                            main = (string?)rascunho["weather"]?[0]?["main"] ?? "N/A",
                            temp_min = (double?)rascunho["main"]?["temp_min"] ?? 0,
                            temp_max = (double?)rascunho["main"]?["temp_max"] ?? 0,
                            speed = (double?)rascunho["wind"]?["speed"] ?? 0,
                            visibility = (int?)rascunho["visibility"] ?? 0,
                            sunrise = sunrise.ToString("HH:mm"),
                            sunset = sunset.ToString("HH:mm"),
                        };
                    }
                    catch (HttpRequestException)
                    {
                        // Alerta de sem conexão
                        throw new Exception("Erro de conexão. Verifique sua internet.");
                    }

                    return t;
                }
            }
        }
    }
