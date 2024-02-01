using FluentValidation;
using DataLibrary;

namespace CodeKY_SD01.Validators
{
	public class ProductValidator : AbstractValidator<ProductEntity>
	{
		public ProductValidator() { 
			RuleFor(product => product.Name).MinimumLength(3);
			RuleFor(product => product.Price).GreaterThanOrEqualTo(0);
			RuleFor(product => product.Quantity).GreaterThanOrEqualTo(0);
			RuleFor(product => product.Description).MinimumLength(10).When(product => product.Description !=  null);

		}
	}
}
