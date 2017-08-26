namespace ForeignExchange.Services
{
    using ForeignExchange.Models;
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;

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
    }
}
