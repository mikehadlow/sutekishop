using System.ComponentModel.DataAnnotations;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Review : IEntity, IComment
	{
        public virtual int Id { get; set; }
        public virtual bool Approved { get; set; }

        [Required(ErrorMessage = "Review is required")]
        public virtual string Text { get; set; }

        public virtual int Rating { get; set; }

        [Required(ErrorMessage = "Reviewer Name is required")]
        public virtual string Reviewer { get; set; }

        public virtual string Answer { get; set; }

        public virtual bool HasAnswer
        {
            get { return !string.IsNullOrEmpty(Answer); }
        }

        public virtual Product Product { get; set; }
	}
}