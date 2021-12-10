using FluentValidation;
using PSQuest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSQuest.API.Validators
{
    public class QuestStateRequestValidator : AbstractValidator<QuestStateRequest>
    {
        public QuestStateRequestValidator()
        {
            RuleFor(p => p.PlayerId)
                .NotEmpty()
                .WithMessage("Player ID cannot be empty");
        }
    }
}
