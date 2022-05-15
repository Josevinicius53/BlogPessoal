using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogPessoal.src.modelos
{
    /// <summary>
    /// <para>Resumo: Classe Responsavel por representar a Tb_Tema no banco.</para>
    /// <para>Criado por: José Vinicius </para>
    /// <para>Versão: 1.0</para>
    /// <para>Data: 29/04/2022</para>
    /// </summary>
    /// 

    [Table("tb_temas")]
    public class TemaModelo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Descricao { get; set; }

        [JsonIgnore]
        public List<PostagemModelo> PostagensRelacionadas { get; set; }
    }
}
