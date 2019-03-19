/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using FluentValidation;

namespace EasyLab.Server.Business.Validators
{
    public class CreateUserValidator : AbstractValidator<EasyLab.Server.DTOs.User>
    {
        public CreateUserValidator()
        {
            RuleFor(o => o.LoginName).Required();
            RuleFor(o => o.LoginName).MaxLength(100);
            RuleFor(o => o.Password).Required();
            RuleFor(o => o.Password).MaxLength(100);
            RuleFor(o => o.LoginDateTime).Required();
            RuleFor(o => o.CreateDateTime).Required();
        }
    }
}
