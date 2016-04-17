using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.DAO
{
    public class LeilaoDAO
    {
        /// <summary>
        /// Cadastra os itens no banco, não importando se o item existe ou nao
        /// </summary>
        /// <param name="itens"></param>
        /// <returns></returns>
        public static bool CadastrarItens(List<Models.ItemModel> itens)
        {
            try
            {
                var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Models.ItemModel, Models.Banco.Itens>());
                var mapper = config.CreateMapper();
                var listaItens = new List<Models.Banco.Itens>();

                foreach (var itemModel in itens)
                {
                    var item = mapper.Map<Models.Banco.Itens>(itemModel);
                    listaItens.Add(item);
                    CadastrarItem(item);
                }
                /* Adiciona a lista inteira no banco, sem verificar a existencia dos itens */
                //using (var conexao = new Models.Banco.LeilaoEntities())
                //{
                //    conexao.Itens.AddRange(listaItens);
                //    conexao.SaveChanges();
                //}

                return true;
            }
            catch(Exception erro)
            {
                return false;
            }
        }

        /// <summary>
        /// Cadastra ou Atualiza o registro no banco
        /// </summary>
        /// <param name="item"></param>
        private static void CadastrarItem(Models.Banco.Itens item)
        {
            using (var conexao = new Models.Banco.LeilaoEntities())
            {
                var busca = conexao.Itens.Where(i => i.Lote == item.Lote);
                if (busca.Any())
                {
                    var itemExistente = busca.FirstOrDefault();
                    itemExistente.MaiorLance = item.MaiorLance;
                    itemExistente.QuantidadeLances = item.QuantidadeLances;
                    itemExistente.UrlFoto = item.UrlFoto;
                    itemExistente.Descricao = item.Descricao;
                    conexao.SaveChanges();
                }
                else {
                    conexao.Itens.Add(item);
                    conexao.SaveChanges();
                }
            }
        }
    }
}

