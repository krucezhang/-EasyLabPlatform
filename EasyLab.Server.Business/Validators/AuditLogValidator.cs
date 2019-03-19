using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.Business.Validators
{
    class AuditLogValidator : AbstractValidator<EasyLab.Server.DTOs.AuditLog>
    {
        public AuditLogValidator()
        {
            RuleFor(o => o.Log).Required();
            //RuleFor(o => o.Log).SetValidator(new AuditLogValidator());
        }
    }
}
