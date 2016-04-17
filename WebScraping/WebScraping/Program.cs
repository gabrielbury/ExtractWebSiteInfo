using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Subimos a dll do agility pack para a memoria
using HtmlAgilityPack;

namespace WebScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUrl = "http://www.freitasleiloesonline.com.br";

            // Componente responsável por conectar ao site
            var client = new HtmlWeb();

            var paginaMateriaisHome = client.Load(baseUrl + "/homesite/filtro.asp?q=materiais");

            // Pegar o numero de paginas
            var codigoUltimaPagina = paginaMateriaisHome.DocumentNode.SelectNodes("//*[@id='listaLotesPaginacao']/ul/li").Count;
            Console.WriteLine(codigoUltimaPagina);

            List<Models.ItemModel> listaItens = new List<Models.ItemModel>();

            // No lugar de '2' o ideal seria colocar o numero de paginas
            for (int i = 1; i <= 2; i++)
            {
                var paginaMateriais = client.Load(baseUrl + String.Format("/homesite/filtro.asp?q=materiais&pagina={0}", i));

                var materiais = paginaMateriais.DocumentNode.SelectNodes("//div[@id='listaLotes']/ul/li");

                foreach (var item in materiais)
                {
                    var urlImagem = item.SelectSingleNode("div[1]/img").Attributes["src"].Value;

                    // Remover o h1, pesquisa primeiro e remove em seguida
                    var lote = item.SelectSingleNode("div[2]/h1").InnerText;
                    item.SelectSingleNode("div[2]/h1").Remove();

                    // Com o InnerHtml, nós conseguimos pegar o texto dentro do elemento
                    var descricao = item.SelectSingleNode("div[2]").InnerText.Trim();

                    var nodeInfo = item.SelectSingleNode("div[3]");
                    var lanceInicial = Decimal.Parse(nodeInfo.SelectSingleNode("div[1]").InnerText.Replace("Lance Inicial: R$ ", ""));
                    var maiorLance = Decimal.Parse(nodeInfo.SelectSingleNode("div[2]").InnerText.Replace("Maior Lance: R$ ", ""));
                    var quantidadeLances = Int32.Parse(nodeInfo.SelectSingleNode("div[3]").InnerText.Replace("Qtd. Lances: ", ""));

                    listaItens.Add(new Models.ItemModel {
                        Descricao = descricao,
                        UrlFoto = urlImagem,
                        LanceInicial = lanceInicial,
                        MaiorLance = maiorLance,
                        Lote = lote,
                        QuantidadeLances = quantidadeLances,
                        Id = Guid.NewGuid()
                    });

                    Console.WriteLine(descricao);
                }
            }

            DAO.LeilaoDAO.CadastrarItens(listaItens);

            Console.ReadKey();
        }
    }
}
