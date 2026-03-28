using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.Entities
{
    public class FinancialProfile
    {
        [BsonElement("monthlyExpenses")]
        public decimal MonthlyExpenses { get; set; }

        [BsonElement("existingDebt")]
        public decimal ExistingDebt { get; set; }

        [BsonElement("creditScore")]
        public int CreditScore { get; set; }

        [BsonElement("savingsAmount")]
        public decimal SavingsAmount { get; set; }

        [BsonElement("bankName")]
        public string BankName { get; set; }

        [BsonElement("accountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("accountType")]
        public string AccountType { get; set; } // Checking, Savings, Both

        [BsonElement("otherAssets")]
        public decimal OtherAssets { get; set; }

        [BsonElement("liabilities")]
        public decimal Liabilities { get; set; }

        [BsonElement("monthlyDebtPayments")]
        public decimal MonthlyDebtPayments { get; set; }

        [BsonElement("creditCards")]
        public List<CreditCard> CreditCards { get; set; }

        [BsonElement("loans")]
        public List<Loan> Loans { get; set; }

        [BsonElement("bankStatementsProvided")]
        public bool BankStatementsProvided { get; set; }

        [BsonElement("lastCreditCheckDate")]
        public DateTime? LastCreditCheckDate { get; set; }

        public FinancialProfile()
        {
            CreditCards = new List<CreditCard>();
            Loans = new List<Loan>();
            MonthlyExpenses = 0;
            ExistingDebt = 0;
            SavingsAmount = 0;
            OtherAssets = 0;
            Liabilities = 0;
            MonthlyDebtPayments = 0;
            BankStatementsProvided = false;
        }

        public decimal GetNetWorth()
        {
            return (SavingsAmount + OtherAssets) - (ExistingDebt + Liabilities);
        }

        public decimal GetDebtToIncomeRatio(decimal monthlyIncome)
        {
            if (monthlyIncome <= 0) return 0;
            return (MonthlyDebtPayments / monthlyIncome) * 100;
        }

        public decimal GetExpenseToIncomeRatio(decimal monthlyIncome)
        {
            if (monthlyIncome <= 0) return 0;
            return (MonthlyExpenses / monthlyIncome) * 100;
        }

        public decimal GetDisposableIncome(decimal monthlyIncome)
        {
            return monthlyIncome - MonthlyExpenses - MonthlyDebtPayments;
        }

        public string GetCreditScoreRating()
        {
            if (CreditScore >= 750) return "Excellent";
            if (CreditScore >= 700) return "Good";
            if (CreditScore >= 650) return "Fair";
            if (CreditScore >= 600) return "Poor";
            return "Very Poor";
        }

        public bool IsCreditScoreAcceptable(int minimumScore = 600)
        {
            return CreditScore >= minimumScore;
        }
    }
}
