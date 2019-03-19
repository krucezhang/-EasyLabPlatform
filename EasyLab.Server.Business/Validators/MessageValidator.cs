/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/10/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using FluentValidation;

namespace EasyLab.Server.Business.Validators
{
    class MessageValidator : AbstractValidator<EasyLab.Server.DTOs.Message>
    {
        public MessageValidator()
        {
            RuleFor(o => o.RecordId).Required(true);
            RuleFor(o => o.RecordId).MaxLength(250);
            RuleFor(o => o.InstrumentName).MaxLength(50);
            RuleFor(o => o.InstrumentIpV4).IpV4();
            RuleFor(o => o.Tag).MaxLength(1024);
        }
    }
}
