using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.Validation
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           if (value == null || string.IsNullOrEmpty(value.ToString()))
           {
               return ValidationResult.Success;
           } 

           var primeiraLetra = value.ToString()[0].ToString();
           if (primeiraLetra != primeiraLetra.ToUpper())
           {
               return new ValidationResult("A Primeira letra do nome deve ser em maiusculo");
           }
           return ValidationResult.Success;
        }
    }
}