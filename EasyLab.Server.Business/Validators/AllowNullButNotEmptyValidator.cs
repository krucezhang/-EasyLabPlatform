/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using FluentValidation.Resources;
using FluentValidation.Validators;
using EasyLab.Server.Resources;

namespace EasyLab.Server.Business.Validators
{
    class AllowNullButNotEmptyValidator : NotEmptyValidator
    {
        public AllowNullButNotEmptyValidator()
            : base(null)
        {
            this.ErrorMessageSource = new StaticStringSource(Rs.MUST_NOT_BE_EMPTY);
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return true;
            }
            return base.IsValid(context);
        }
    }
}
