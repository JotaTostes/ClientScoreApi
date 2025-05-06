using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientScore.Application.Validator
{
    public static class TelefoneValidator
    {
        private static readonly Regex _telefoneRegex = new(@"^\(?([1-9]{2})\)?\s?([9]?\d{4})-?(\d{4})$");

        public static bool IsValid(string telefone)
        {
            return _telefoneRegex.IsMatch(telefone);
        }

        public static string Normalize(string telefone)
        {
            var match = _telefoneRegex.Match(telefone);
            if (!match.Success)
                return telefone;

            var ddd = match.Groups[1].Value;
            var parte1 = match.Groups[2].Value;
            var parte2 = match.Groups[3].Value;

            return $"({ddd}) {parte1}-{parte2}";
        }

        public static string ObterDDD(string telefone)
        {
            var match = _telefoneRegex.Match(telefone);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
