using FluentValidation;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mime;

namespace Space.Application.Handlers;

public class CreateSupportCommandValidation : AbstractValidator<CreateSupportCommand>
{
    public CreateSupportCommandValidation()
    {
        RuleForEach(c => c.Images)
            .NotEmpty().WithMessage("At least one image is required.")
            .ChildRules(image =>
            {
                image.RuleFor(i => i.ContentType).Must(type => type.Contains("image/"))
                    .WithMessage("Only images in image format are supported.");

                image.RuleFor(i => i.Length).Must(length => length < 2 * 1024 * 1024)
                    .WithMessage("Image size must be less than 2MB.");
            });
    }
}





