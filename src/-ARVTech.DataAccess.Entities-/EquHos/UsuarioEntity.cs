namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIOS")]
    public class UsuarioEntity
    {
        [Key]
        public Guid? Guid { get; set; }

        public bool Bloqueado { get; set; }

        [Column("ENVIAR_LINK_ATIVACAO")]
        public bool EnviarLinkAtivacao { get; set; }

        [Column("MARCA_CABANHA_LOGADO")]
        public byte[] MarcaCabanhaLogado { get; set; }

        [Column("DATA_EXPIRACAO_SENHA")]
        public DateTime? DataExpiracaoSenha { get; set; }

        public Guid? GuidContaLogado { get; set; }

        public Guid? GuidCabanhaLogado { get; set; }

        [Column("INTERVALO_EXPIRACAO_SENHA")]
        public int? IntervaloExpiracaoSenha { get; set; }

        [Column("APELIDO_USUARIO")]
        public string ApelidoUsuario { get; set; }

        public string CPF { get; set; }

        public string Email { get; set; }

        public string Nome { get; set; }

        [Column("NOME_FANTASIA_CABANHA_LOGADO")]
        public string NomeFantasiaLogado { get; set; }

        public string Senha { get; set; }

        public string Sobrenome { get; set; }

        public virtual CategoriaUsuarioEntity CategoriaUsuario { get; set; }

        public virtual ContaEntity Conta { get; set; }
    }
}