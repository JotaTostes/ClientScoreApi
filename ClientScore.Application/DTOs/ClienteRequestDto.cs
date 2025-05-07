namespace ClientScore.Application.DTOs
{
    public class ClienteRequestDto
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public decimal RendimentoAnual { get; set; }
        public string Estado { get; set; }
        public string Telefone { get; set; }
    }
}
