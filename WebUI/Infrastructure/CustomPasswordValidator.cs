﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace WebUI.Infrastructure
{
    public class CustomPasswordValidator : PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            IdentityResult result = await base.ValidateAsync(pass);

            if (pass.Contains("12345"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Пароль не должен содержать последовательности чисел.");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}