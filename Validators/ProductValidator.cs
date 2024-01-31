using CodeKY_SD01.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeKY_SD01.Validators
{
	public class ProductValidator : AbstractValidator<Product>
	{
		public ProductValidator() { 
			RuleFor(product => product.Name).NotEmpty();
			RuleFor(product => product.Price).GreaterThan(0);
			RuleFor(product => product.Quantity).GreaterThan(0);
			RuleFor(product => product.Description).MinimumLength(10).When(product => product.Description !=  null);

		}
	}
}
