using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentValidation;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id should not be empty");
    }
}
