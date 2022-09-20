using System.ComponentModel.DataAnnotations;

namespace Application.Models.Pollen
{
    public enum PollenRiskLevel
    {
        [Display(Name = "Low")]
        Low,
        [Display(Name = "Moderate")]
        Moderate,
        [Display(Name = "High")]
        High,
        [Display(Name = "Very high")]
        VeryHigh
    }
}
