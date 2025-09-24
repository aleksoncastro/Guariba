namespace Guariba.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }

        // Relationships
        //remetente da mensagem
        public int SenderId { get; set; }
        public User Sender { get; set; }

        //destinatario da mensagem
        public int RecipientId { get; set; }
        public User Recipient { get; set; }


    }
}
