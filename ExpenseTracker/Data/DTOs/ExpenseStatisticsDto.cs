using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ExpenseStatisticsDto
    {
        public ExpenseDto MostExpensive { get; set; }
        public ExpenseDto LeastExpensive { get; set; }
        public decimal AverageDailyExpense { get; set; }
        public decimal AverageMonthlyExpense { get; set; }
        public decimal AverageYearlyExpense { get; set; }
        public decimal TotalExpenses { get; set; }
    }
}
