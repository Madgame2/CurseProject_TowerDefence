using Common.Exceptions.DTO;
using Common.Exceptions.enums;
using ModestTree;
using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Common.Validation
{
    public static class Validator
    {
        public static bool ValidatePassword(string password, Action<RegisterError> onError)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                onError?.Invoke(new RegisterError(
                    RegisterErrorsEnums.passwordError,
                    "password can not be empty"));

                return false;
            }

            return true;
        }

        public static bool ValidateEmail(string email, Action<RegisterError> onError)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                onError?.Invoke(new RegisterError(
                    RegisterErrorsEnums.emailError,
                    "email can not be empty"));

                return false;
            }

            if (!IsValidEmail(email))
            {
                onError?.Invoke(new RegisterError(
                    RegisterErrorsEnums.emailError,
                    "email is not valid"));

                return false;
            }

            return true;
        }

        public static bool ValidateNickname(string nickname, Action<RegisterError> onError)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                onError?.Invoke(new RegisterError(
                    RegisterErrorsEnums.nicknameError,
                    "nickname can not be empty"));

                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            // Простая заглушка (можешь потом улучшить)
            return email.Contains("@");
        }
    }
}
