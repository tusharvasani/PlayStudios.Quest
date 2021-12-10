using FluentValidation;
using PSQuest.Data.Models;

namespace PSQuest.API.Validators
{
    public class QuestProgressRequestValidator : AbstractValidator<QuestProgressRequest>
    {
        public QuestProgressRequestValidator()
        {
            RuleFor(req => req.PlayerId)
                .NotEmpty()
                .WithMessage("Player ID cannot be empty");
            RuleFor(amt => amt.ChipBetAmount)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("ChipBetAmount must be greater than zero");
            RuleFor(pl => pl.PlayerLevel)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("PlayerLevel cannot be 0");
        }        
    }
}
