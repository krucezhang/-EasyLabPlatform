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
    class IpV4AddressValidator : RegularExpressionValidator
    {
        public IpV4AddressValidator()
            : base(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$")
        {
            this.ErrorMessageSource = new StaticStringSource(Rs.INVALID_IP_V4);
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (string.IsNullOrEmpty(context.PropertyValue as string))
            {
                return true;
            }
            return base.IsValid(context);
        }
    }
}
