using Clientes.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Clientes.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public TipoCliente TipoCliente { get; set; }
        [Required]
        [CustomValidation(typeof(Cliente), nameof(ValidarDocumento))]
        public string Documento { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Cadastro { get; set; }
        [Required]
        public string Telefone { get; set; }
        public bool IsDeleted { get; set; } // Soft Delete

        public static ValidationResult ValidarDocumento(string documento, ValidationContext context)
        {
            var instance = context.ObjectInstance as Cliente;
            if (instance.TipoCliente == TipoCliente.PF && !ValidarCPF(documento)) 
            {
                return new ValidationResult("CPF inválido.");
            }

            if (instance.TipoCliente == TipoCliente.PJ && !ValidarCNPJ(documento))
            {
                return new ValidationResult("CNPJ inválido.");
            }

            return ValidationResult.Success;
        }

        private static bool ValidarCPF(string documento)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            documento = documento.Trim();
            documento = documento.Replace(".", "").Replace("-", "");
            if (documento.Length != 11)
                return false;
            tempCpf = documento.Substring(0, 9);
            soma = 0;

            for (int i = 0; i<9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i<10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return documento.EndsWith(digito);
        }

        private static bool ValidarCNPJ(string documento)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            documento = documento.Trim();
            documento = documento.Replace(".", "").Replace("-", "").Replace("/", "");
            if (documento.Length != 14)
                return false;
            tempCnpj = documento.Substring(0, 12);
            soma = 0;
            for (int i = 0; i<12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return documento.EndsWith(digito);
        }

    }
}
