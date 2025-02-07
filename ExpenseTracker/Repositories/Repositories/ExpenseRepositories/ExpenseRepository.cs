using Data.DTOs;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ExpenseRepositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();  
            return expense;
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.Include(e => e.Category).Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses.Include(e => e.Category)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _context.Expenses
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Expense> GetMostExpensiveExpenseAsync(ExpenseFilterDto filter)
        {
            var query = ApplyFilter(_context.Expenses, filter);
            return await query.OrderByDescending(e => e.Amount).FirstOrDefaultAsync();
        }

        public async Task<Expense> GetLeastExpensiveExpenseAsync(ExpenseFilterDto filter)
        {
            var query = ApplyFilter(_context.Expenses, filter);
            return await query.OrderBy(e => e.Amount).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync(ExpenseFilterDto filter)
        {
            var query = ApplyFilter(_context.Expenses, filter);
            return await query.SumAsync(e => e.Amount);
        }

        public async Task<decimal> GetAverageDailyExpensesAsync(ExpenseFilterDto filter)
        {
            var query = ApplyFilter(_context.Expenses, filter);
            var totalAmount = await query.SumAsync(e => e.Amount);
            var totalDays = (filter.ToDate - filter.FromDate)?.Days ?? 1;
            return totalDays > 0 ? totalAmount / totalDays : 0;
        }

        public async Task<decimal> GetAverageMonthlyExpensesAsync(ExpenseFilterDto filter)
        {
            var query = ApplyFilter(_context.Expenses, filter);
            var totalAmount = await query.SumAsync(e => e.Amount);
            var totalMonths = ((filter.ToDate?.Year - filter.FromDate?.Year) * 12) +
                             (filter.ToDate?.Month - filter.FromDate?.Month) ?? 1;
            return totalMonths > 0 ? totalAmount / totalMonths : 0;
        }

        public async Task<decimal> GetAverageYearlyExpensesAsync(ExpenseFilterDto filter)
        {
            var query = ApplyFilter(_context.Expenses, filter);
            var totalAmount = await query.SumAsync(e => e.Amount);
            var totalYears = (filter.ToDate?.Year - filter.FromDate?.Year) ?? 1;
            return totalYears > 0 ? totalAmount / totalYears : 0;
        }

        private IQueryable<Expense> ApplyFilter(IQueryable<Expense> query, ExpenseFilterDto filter)
        {
            if (filter.CategoryId.HasValue)
                query = query.Where(e => e.CategoryId == filter.CategoryId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(e => e.Date >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(e => e.Date <= filter.ToDate.Value);

            return query;
        }

    }
}
