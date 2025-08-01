namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIOS")]
    public class UsuarioEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("APELIDO_USUARIO")]
        public string ApelidoUsuario { get; set; }

        [Description("EMAIL")]
        public string Email { get; set; }

        [Description("SENHA")]
        public string Senha { get; set; }

        [Description("NOME")]
        public string Nome { get; set; }

        [Description("SOBRENOME")]
        public string Sobrenome { get; set; }

        [Description("CPF")]
        public string Cpf { get; set; }

        [Description("GUIDCONTA_LOGADO")]
        public Guid? GuidContaLogado { get; set; }

        [Description("GUIDCONTA")]
        public Guid GuidConta { get; set; }

        public ContaEntity Conta { get; set; }

        [Description("GUIDCABANHA_LOGADO")]
        public Guid? GuidCabanhaLogado { get; set; }

        [Description("NOME_FANTASIA_CABANHA_LOGADO")]
        public string NomeFantasiaCabanhaLogado { get; set; }

        [Description("MARCA_CABANHA_LOGADO")]
        public byte[] MarcaCabanhaLogado { get; set; }

        [Description("BLOQUEADO")]
        public bool Bloqueado { get; set; }

        [Description("ENVIAR_LINK_ATIVACAO")]
        public bool EnviarLinkAtivacao { get; set; }

        [Description("DATA_EXPIRACAO_SENHA")]
        public DateTime? DataExpiracaoSenha { get; set; }

        [Description("INTERVALO_EXPIRACAO_SENHA")]
        public int? IntervaloExpiracaoSenha { get; set; }

        [Description("IDCATEGORIA_USUARIO")]
        public int IdCategoriaUsuario { get; set; }

        public CategoriaUsuarioEntity CategoriaUsuario { get; set; }

        public ICollection<UsuarioCabanhaEntity> UsuariosCabanhas { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}; Nome/Sobrenome: {this.Nome} {this.Sobrenome}.";
        }
    }
}