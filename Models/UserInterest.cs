namespace Guariba.Models
{
    public class UserInterest
    {
        // Chaves estrangeiras que formarão a chave primária composta
        public int UserId { get; set; }
        public Interest Interest { get; set; } // O próprio enum

        // Propriedades de navegação
        public User User { get; set; }
    }
}

