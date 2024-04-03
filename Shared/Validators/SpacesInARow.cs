using System.ComponentModel.DataAnnotations;

namespace Shared.Validators;
public sealed class SpacesInARow : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        string input = value as string;

        bool noMoreThan3SpacesInARow = true;

        for (int i = 2; i < input.Length; i++)
        {
            if (input[i] == ' ' && input[i] == input[i - 1] && input[i] == input[i - 2])
            {
                noMoreThan3SpacesInARow = false;
            }
        }

        return noMoreThan3SpacesInARow;
    }
}
