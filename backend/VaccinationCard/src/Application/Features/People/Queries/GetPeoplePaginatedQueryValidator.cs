using Common.Resources;
using FluentValidation;

namespace Application.Features.People.Queries;

public class GetPeoplePaginatedQueryValidator : AbstractValidator<GetPeoplePaginatedQuery>
{
    public GetPeoplePaginatedQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .LessThan(100).WithMessage(string.Format(Messages.PageSizeMustBeLowerThan, 100))
            .GreaterThanOrEqualTo(0).WithMessage(Messages.PageSizeCannotBeNegative);

    }
}
