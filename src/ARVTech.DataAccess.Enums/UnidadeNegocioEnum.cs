namespace ARVTech.DataAccess.Enums
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Enumm with values "Unidade de Negócio".
    /// </summary>
    public enum UnidadeNegocioEnum
    {
        [Display(Name = "MATRIZ")]
        Matriz = 1,

        [Display(Name = "MACROMIX")]
        Macromix = 2,

        [Display(Name = "RISSUL")]
        Rissul = 3,

        [Display(Name = "ATACAREJO")]
        Atacarejo = 4,

        [Display(Name = "INDÚSTRIA")]
        Industria = 5,

        [Display(Name = "LOGÍSTICA")]
        Logistica = 6,

        [Display(Name = "MANUTENÇÃO")]
        Manutencao = 7,

        [Display(Name = "TRANSPORTE")]
        Transporte = 8,
    }
}