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
    class RequiredValidator : PropertyValidator, IPropertyValidator
    {
        private bool allowEmpty;

        public RequiredValidator()
            : base(Rs.REQUIRED_FIELD_MISSING_OR_EMPTY)
        {
            allowEmpty = false;
        }

        public bool AllowEmptyStrings
        {
            get
            {
                return allowEmpty;
            }
            set
            {
                if (allowEmpty != value)
                {
                    allowEmpty = true;
                    this.ErrorMessageSource = new StaticStringSource(allowEmpty ? Rs.REQUIRED_FIELD_MISSING : Rs.REQUIRED_FIELD_MISSING_OR_EMPTY);
                }
            }
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return false;
            }
            string text = context.PropertyValue as string;

            return text == null || this.AllowEmptyStrings || text.Trim().Length != 0;
        }
    }
}
