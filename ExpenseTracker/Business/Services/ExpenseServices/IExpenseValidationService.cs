using Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.ExpenseServices
{
    public interface IExpenseValidationService
    {
        Task ValidateExpenseCreation(CreateExpenseDto expenseDto);
    }
}
