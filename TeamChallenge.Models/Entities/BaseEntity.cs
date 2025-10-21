using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Entities
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}