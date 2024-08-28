using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Entities
{
    public class Review
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string Text { get; private set; } = string.Empty;
        public int Grade { get; private set; }
        public Auction Auction { get; private set; } = new();
        public Guid AuctionId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public void EditReview(string text, int grade, DateTime updatedAt)
        {
            Text = text;
            Grade = grade;
            UpdatedAt = updatedAt;
        }
        public Review(Guid id, string username, string text, int grade, DateTime createdAt)
        {
            Id = id;
            Username = username;
            Text = text;
            Grade = grade;
            CreatedAt = createdAt;
        }
        public Review()
        {

        }
    }
}
