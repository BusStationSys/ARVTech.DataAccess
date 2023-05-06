namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIO")]
    public class UsuarioEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("APELIDO_USUARIO")]
        public virtual string ApelidoUsuario { get; set; }

        [Description("EMAIL")]
        public virtual string Email { get; set; }

        [Description("SENHA")]
        public virtual string Senha { get; set; }

        [Description("NOME")]
        public virtual string Nome { get; set; }

        [Description("SOBRENOME")]
        public virtual string Sobrenome { get; set; }

        [Description("CPF")]
        public virtual string Cpf { get; set; }

        [Description("GUIDCONTA_LOGADO")]
        public virtual Guid? GuidContaLogado { get; set; }

        [Description("GUIDCONTA")]
        public virtual Guid GuidConta { get; set; }

        public virtual ContaEntity Conta { get; set; }

        [Description("GUIDCABANHA_LOGADO")]
        public virtual Guid? GuidCabanhaLogado { get; set; }

        [Description("NOME_FANTASIA_CABANHA_LOGADO")]
        public virtual string NomeFantasiaCabanhaLogado { get; set; }

        [Description("MARCA_CABANHA_LOGADO")]
        public virtual byte[] MarcaCabanhaLogado { get; set; }

        [Description("BLOQUEADO")]
        public virtual bool Bloqueado { get; set; }

        [Description("ENVIAR_LINK_ATIVACAO")]
        public virtual bool EnviarLinkAtivacao { get; set; }

        [Description("DATA_EXPIRACAO_SENHA")]
        public virtual DateTime? DataExpiracaoSenha { get; set; }

        [Description("INTERVALO_EXPIRACAO_SENHA")]
        public virtual int? IntervaloExpiracaoSenha { get; set; }

        [Description("IDCATEGORIA_USUARIO")]
        public virtual int IdCategoriaUsuario { get; set; }

        public virtual CategoriaUsuarioEntity CategoriaUsuario { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}; Nome/Sobrenome: {this.Nome} {this.Sobrenome}.";
        }
    }
}