using System.ComponentModel.DataAnnotations;

namespace Shared.Validators;
public sealed class NoPeriods : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        string input = value as string;

        bool noPeriods = !input.Contains('.');

        return noPeriods;
    }
}
