using Business.CustomExceptions;
using Data.DTOs;
using Repositories.Repositories.CategoryRepositories;
using Repositories.Repositories.ExpenseRepositories;
using Repositories.Repositories.UserRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.ExpenseServices
{
    public class ExpenseValidationService : IExpenseValidationService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseValidationService(
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IExpenseRepository expenseRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task ValidateExpenseCreation(CreateExpenseDto expenseDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(expenseDto.CategoryId);
            var user = await _userRepository.GetUserByIdAsync(expenseDto.UserId);

            // Shpenzimet totale aktuale per kategorine
            var categoryExpenses = await _expenseRepository.GetExpensesByCategoryAsync(expenseDto.CategoryId);
            var currentCategoryTotal = categoryExpenses.Sum(e => e.Amount);

            // Get current total expenses for user
            var userExpenses = await _expenseRepository.GetExpensesByUserAsync(expenseDto.UserId);
            var currentUserTotal = userExpenses.Sum(e => e.Amount);

            // Validimi i buxhetitt te kategorise
            if (currentCategoryTotal + expenseDto.Amount > category.Budget)
            {
                throw new BusinessValidationException(
                    $"Adding this expense would exceed the budget for category {category.Name}");
            }

            // Validate user's total budget; Validimi i buxhetit total per perdoruesin
            if (currentUserTotal + expenseDto.Amount > user.TotalBudget)
            {
                throw new BusinessValidationException(
                    "Adding this expense would exceed your total budget");
            }
        }
    }
}
