using FluentValidation;
using SimpleUploaderAPI.Models;

namespace SimpleUploaderAPI.Validators
{
    public class CreateFileDataModelValidator : AbstractValidator<CreateFileDataModel>
    {
        public CreateFileDataModelValidator()
        {
            RuleFor(x => x.FileName)
                .NotNull()
                .WithMessage("The file name is required.");
            RuleFor(x => x.FileSize)
                .NotNull()
                .WithMessage("The file size is required.");
            RuleFor(x => x.FileType)
                .NotNull()
                .WithMessage("The file type is required.");
        }

    }
}