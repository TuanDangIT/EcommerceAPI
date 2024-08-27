using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Entities
{
    public class Review
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string Text { get; private set; } = string.Empty;
        public int Grade { get; private set; }
        public Product Product { get; private set; } = new();
        public Guid ProductId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set;}
        public void UpdateReview(string text, int grade, DateTime updatedAt)
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
