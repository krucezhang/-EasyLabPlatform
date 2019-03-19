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
    class MaxLengthValidator : LengthValidator
    {
        public MaxLengthValidator(int max)
            : base(0, max)
        {
            this.ErrorMessageSource = new StaticStringSource(string.Format(Rs.VALUE_TOO_LONG, max));
        }
    }
}
