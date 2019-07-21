using Domain.Enum;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class CreditCard
    {

        [Column("Type")] public CreditCardType Type { get; set; }

        [Column("ExpirationDate")] public DateTime ExpirationDate { get; set; }

        [Column("OwnerName")] public string OwnerName { get; set; }

        [Column("CardNumber")] public string CardNumber { get; set; }

        [Column("SecurityNumber")] public string SecurityNumber { get; set; }

      
        public CreditCard(CreditCardType cardType, string cardNumber, string securityNumber, string ownerName, DateTime expiration)
        {

            if (!VerificarTarjeta(cardNumber))
            {
                throw new Exception("Numero Tarjeta invalido");
            }

          
            
           

            Type = cardType;
            CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : "";
            SecurityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : "";
            OwnerName = !string.IsNullOrWhiteSpace(ownerName) ? ownerName : "";
            ExpirationDate = expiration;
        }

        protected CreditCard()
        {
            Type = CreditCardType.Unknown;
            ExpirationDate = DateTime.UtcNow;
            CardNumber = string.Empty;
            OwnerName = string.Empty;
            SecurityNumber = string.Empty;
        }


        
        public static bool VerificarTarjeta(string creditCardNumber)
        {
            StringBuilder digitsOnly = new StringBuilder();
            creditCardNumber.ToList().ForEach(c =>
            {
                if (Char.IsDigit(c)) digitsOnly.Append(c);
            });

            if (digitsOnly.Length > 18 || digitsOnly.Length < 15) return false;

            int sum = 0;
            int digit = 0;
            int addend = 0;
            bool timesTwo = false;

            for (int i = digitsOnly.Length - 1; i >= 0; i--)
            {
                digit = Int32.Parse(digitsOnly.ToString(i, 1));
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9)
                    {
                        addend -= 9;
                    }
                }
                else
                {
                    addend = digit;
                }
                sum += addend;
                timesTwo = !timesTwo;
            }
            return (sum % 10) == 0;
        }

    }
}
