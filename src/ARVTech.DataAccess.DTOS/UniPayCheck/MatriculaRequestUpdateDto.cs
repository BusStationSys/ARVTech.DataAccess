namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class MatriculaRequestUpdateDto
    {
        private string _formaPagamento;

        public Guid Guid { get; set; }

        public string Matricula { get; set; }

        public DateTime DataAdmissao { get; set; }

        public DateTime? DataDemissao { get; set; }

        public string DescricaoCargo { get; set; }

        public string DescricaoSetor { get; set; }

        public Guid? GuidColaborador { get; set; }

        //public PessoaFisicaRequestDto Colaborador { get; set; }

        public Guid? GuidEmpregador { get; set; }

        //public PessoaJuridicaRequestDto Empregador { get; set; }

        //public virtual ICollection<MatriculaDemonstrativoPagamentoEntity> DemonstrativosPagamento { get; set; }

        //public virtual ICollection<MatriculaEspelhoPontoEntity> EspelhosPonto { get; set; }

        public string? Banco { get; set; }

        public string? Agencia { get; set; }

        public string? Conta { get; set; }

        public string? DvConta { get; set; }

        public decimal CargaHoraria { get; set; }

        public decimal SalarioNominal { get; set; }

        public string FormaPagamento
        {
            get
            {
                return this._formaPagamento;
            }

            set
            {
                if (value.ToUpper() == "R")
                {
                    this.Banco = null;
                    this.Agencia = null;
                    this.Conta = null;
                    this.DvConta = null;
                }
                else
                {
                    this.Banco = "000";
                    this.Agencia = "000000000";
                    this.Conta = "000000000000000";
                    this.DvConta = "0";
                }

                this._formaPagamento = value;
            }
        }

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }

        public MatriculaRequestUpdateDto()
        {
            this.CargaHoraria = 220m;
            this.SalarioNominal = 0.01m;

            this.FormaPagamento = "O";
        }
    }
}