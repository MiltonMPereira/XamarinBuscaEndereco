using BuscaCEP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuscaCEP.Clients
{
    class ViaCepHttpClient
    {
        private static Lazy<ViaCepHttpClient> _Lazy = new Lazy<ViaCepHttpClient>(() => new ViaCepHttpClient());
        public static ViaCepHttpClient Current { get => _Lazy.Value; }

        private ViaCepHttpClient()
        {
            _HttpClient = new HttpClient();
        }

        private readonly HttpClient _HttpClient;
        public LatLong BuscarLatitudeLongitude(string cep)
        {
            try
            {
                Requisicoes req = new Requisicoes();
                string htmlResponse = req.GetLatLong("https://www.qualocep.com/busca-cep/" + cep);

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(htmlResponse);
                var nodes = Regex.Replace(doc.DocumentNode.SelectNodes("//h4").FindFirst("h4").InnerHtml, "[A-Za-z ]", "").Replace(":","").Split('/');
                LatLong latLong = new LatLong();
                latLong.latitude = nodes[0];
                latLong.longitude = nodes[1];

                return latLong;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<BuscaCepResult>> BuscarCep(string cep)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cep))
                    throw new InvalidOperationException("CEP não informado");

                if (IsCEP(cep))
                {
                    using (var response = await _HttpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Erro ao consultar o CEP");

                        var result = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(result))
                            throw new InvalidOperationException("Erro ao consultar o CEP");

                        var enderecos = JsonConvert.DeserializeObject<BuscaCepResult>(result);
                        enderecos.cidade = enderecos.localidade;
                        return new List<BuscaCepResult>() { enderecos };
                    }
                }
                else
                {
                    cep = cep.Replace(" ", "-").Trim();
                    _HttpClient.DefaultRequestHeaders.Accept.Clear();
                    _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var response = await _HttpClient.GetAsync($"http://cep.la/{cep}"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Erro ao consultar o CEP");

                        var result = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(result))
                            throw new InvalidOperationException("CEP não encontrado.");

                        var enderecos = JsonConvert.DeserializeObject<List<BuscaCepResult>>(result);

                        return enderecos;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool IsCEP(string txt)
        {
            txt = txt.Replace(".", "").Replace("-", "").Replace(" ", "");

            Regex Rgx = new Regex(@"^\d{8}$");

            if (!Rgx.IsMatch(txt))

                return false;

            else

                return true;
        }
    }

}
