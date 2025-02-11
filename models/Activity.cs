using System.ComponentModel.DataAnnotations;

namespace AlpineSkiHouse.Models

{
    public class Activity
    {
        public int Id { get; set; }
        public string? Name { get; set; } //Needs to be nullable "?"
        public string? Description { get; set; } //Needs to be nullable "?"
        public string? Icon { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public Activity() { }

        public Activity(string name, string description, string icon)
        {
            Name = name;
            Description = description;
            Icon = icon;
        }
    }
}
