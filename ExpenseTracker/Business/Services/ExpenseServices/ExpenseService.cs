using AutoMapper;
using Data.DTOs;
using Data.Entities;
using Repositories.Repositories.ExpenseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.ExpenseServices
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly IExpenseValidationService _validationService;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper, IExpenseValidationService validationService)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto)
        {
            await _validationService.ValidateExpenseCreation(expenseDto);
            var expense = _mapper.Map<Expense>(expenseDto);
            var createdExpense = await _expenseRepository.AddExpenseAsync(expense);
            return _mapper.Map<ExpenseDto>(createdExpense);
        }

        public async Task<ExpenseDto> GetExpenseByIdAsync(int id)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync()
        {
            var expenses = await _expenseRepository.GetAllExpensesAsync();
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseStatisticsDto> GetExpenseStatisticsAsync(ExpenseFilterDto filter)
        {
            var mostExpensive = await _expenseRepository.GetMostExpensiveExpenseAsync(filter);
            var leastExpensive = await _expenseRepository.GetLeastExpensiveExpenseAsync(filter);
            var averageDaily = await _expenseRepository.GetAverageDailyExpensesAsync(filter);
            var averageMonthly = await _expenseRepository.GetAverageMonthlyExpensesAsync(filter);
            var averageYearly = await _expenseRepository.GetAverageYearlyExpensesAsync(filter);
            var total = await _expenseRepository.GetTotalExpensesAsync(filter);

            return new ExpenseStatisticsDto
            {
                MostExpensive = MapToExpenseDto(mostExpensive),
                LeastExpensive = MapToExpenseDto(leastExpensive),
                AverageDailyExpense = averageDaily,
                AverageMonthlyExpense = averageMonthly,
                AverageYearlyExpense = averageYearly,
                TotalExpenses = total
            };
        }

        private ExpenseDto MapToExpenseDto(Expense expense)
        {
            if (expense == null) return null;

            return new ExpenseDto
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Description = expense.Description,
                Date = expense.Date,
                CategoryName = expense.Category?.Name,
                Username = expense.User?.Username
            };
        }
    }
}
