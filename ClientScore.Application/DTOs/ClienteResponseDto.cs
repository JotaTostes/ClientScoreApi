using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientScore.Application.DTOs
{
    public class ClienteResponseDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
    }
}
