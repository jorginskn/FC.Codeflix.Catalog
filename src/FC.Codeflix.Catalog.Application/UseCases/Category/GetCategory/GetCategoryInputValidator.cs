using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentValidation;

public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty.");
    }
}