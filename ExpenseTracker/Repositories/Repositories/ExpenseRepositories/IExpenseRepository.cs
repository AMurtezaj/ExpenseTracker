using Data.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ExpenseRepositories
{
    public interface IExpenseRepository
    {
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<Expense> GetExpenseByIdAsync(int id);
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
        Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId);
        Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId);
        Task<Expense> GetMostExpensiveExpenseAsync(ExpenseFilterDto filter);
        Task<Expense> GetLeastExpensiveExpenseAsync(ExpenseFilterDto filter);
        Task<decimal> GetTotalExpensesAsync(ExpenseFilterDto filter);
        Task<decimal> GetAverageDailyExpensesAsync(ExpenseFilterDto filter);
        Task<decimal> GetAverageMonthlyExpensesAsync(ExpenseFilterDto filter);
        Task<decimal> GetAverageYearlyExpensesAsync(ExpenseFilterDto filter);
    }
}
