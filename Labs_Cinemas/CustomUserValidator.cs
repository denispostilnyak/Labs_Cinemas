using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Labs_Cinemas.Models;
using System.Text.RegularExpressions;

namespace Labs_Cinemas
{
    public class CustomUserValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user) {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.Email.ToLower().EndsWith("@spam.com")) {
                errors.Add(new IdentityError {
                    Description = "Данный домен находится в спам-базе. Выберите другой почтовый сервис"
                });
            }
            if (user.UserName.Contains("admin")) {
                errors.Add(new IdentityError {
                    Description = "Ник пользователя не должен содержать слово 'admin'"
                });
            }
           
            if (Regex.IsMatch(user.Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")) {
                errors.Add(new IdentityError {
                    Description = "Format name@mail.com"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
