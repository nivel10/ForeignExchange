namespace ForeignExchange.Services
{
    using ForeignExchange.Models;
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Plugin.Connectivity;
    using ForeignExchange.Helpers;

    public class ApiService
    {
        /// <summary>
        /// Metodo que optiene las tasas (Rates)
        /// </summary>
        /// <returns>Object Response</returns>
        public async Task<Response> GetRates()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://apiexchangerates.azurewebsites.net");
                var controller = "/api/Rates";
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                        Result = null,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<Rate>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString().Trim(),
                    Result = null,
                };
            }
        }

        /// <summary>
        /// Metodo generico que devuelve on objeto de tipo List()
        /// </summary>
        /// <typeparam name="T">Objeto del tipo a retornar</typeparam>
        /// <param name="urlBase">String que almacena la Url Base del API Service</param>
        /// <param name="controller">String que almacena el Url Controller del API Service</param>
        /// <returns>Objeto de tipo List()</returns>
        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = result,
                        Result = null,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.Trim(),
                    Result = null,
                };
            }
        }

        /// <summary>
        /// Metodo que verifica la conexion o acceso a internet
        /// </summary>
        /// <returns>Response()</returns>
        public async Task<Response> CheckConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var response = await CrossConnectivity.Current.IsRemoteReachable("google.com");
                if(response)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = string.Empty,
                    };
                }
                else
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Lenguages.TitleAccessInternet,
                    };
                }
            }
            else
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Lenguages.TitleSettingsInternet,
                };
            }
        }
    }
}
