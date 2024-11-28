using System.ComponentModel.DataAnnotations;

namespace CognitechnaZadanie.Model.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        [MinLength(1)]
        public string Title { get; set; } = String.Empty!;
        public string? Description { get; set; }
    }
}
