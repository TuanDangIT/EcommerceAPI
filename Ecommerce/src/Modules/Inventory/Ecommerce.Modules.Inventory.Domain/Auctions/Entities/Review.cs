using Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Entities
{
    public class Review : BaseEntity<Guid>, IAuditable
    {
        public string Username { get; private set; } 
        public string Text { get; private set; } 
        public int Grade { get; private set; }
        public Auction Auction { get; private set; } = default!;
        public Guid AuctionId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public void Edit(string text, int grade)
        {
            if(grade < 1 && grade > 10)
            {
                throw new AuctionInvalidGradeException();
            }
            Text = text;
            Grade = grade;
        }
        public Review(Guid id, string username, string text, int grade)
        {
            Id = id;
            Username = username;
            Text = text;
            Grade = grade;
        }
    }
}
