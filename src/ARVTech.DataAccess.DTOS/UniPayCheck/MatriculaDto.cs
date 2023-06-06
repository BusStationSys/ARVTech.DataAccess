namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDto
    {
        public Guid? Guid { get; set; }

        public string Matricula { get; set; }

        public DateTime DataAdmissao { get; set; }

        public DateTime? DataDemissao { get; set; }

        public Guid? GuidColaborador { get; set; }

        public PessoaFisicaDto Colaborador { get; set; }

        public Guid? GuidEmpregador { get; set; }

        public PessoaJuridicaDto Empregador { get; set; }

        //public virtual ICollection<MatriculaDemonstrativoPagamentoEntity> DemonstrativosPagamento { get; set; }

        //public virtual ICollection<MatriculaEspelhoPontoEntity> EspelhosPonto { get; set; }

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }
    }
}