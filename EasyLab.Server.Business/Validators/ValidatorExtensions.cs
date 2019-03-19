/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using FluentValidation;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Common.ExceptionExtensions;
using EasyLab.Server.Common.Extensions;

namespace EasyLab.Server.Business.Validators
{
    static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IpV4<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IpV4AddressValidator());
        }

        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int max)
        {
            return ruleBuilder.SetValidator(new MaxLengthValidator(max));
        }

        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, bool allowEmptyStrings = false)
        {
            return ruleBuilder.SetValidator(new RequiredValidator() { AllowEmptyStrings = allowEmptyStrings });
        }

        public static IRuleBuilderOptions<T, string> AllowNullButNotEmpty<T>(this IRuleBuilder<T, string> ruleBuild)
        {
            return ruleBuild.SetValidator(new AllowNullButNotEmptyValidator());
        }
        /// <summary>
        /// Throw the BadRequest EasyLabException if the validation failed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="instance"></param>
        public static void ValidateAndThrowEasyLabException<T>(this IValidator<T> validator, T instance)
        {
            var results = validator.Validate(instance);

            if (!results.IsValid)
            {
                string message = results.Errors[0].ErrorMessage;
                string subcode = results.Errors[0].PropertyName.ToCamelCase();

                throw new EasyLabException(System.Net.HttpStatusCode.BadRequest, message) { SubCode = subcode };
            }
        }
    }
}
