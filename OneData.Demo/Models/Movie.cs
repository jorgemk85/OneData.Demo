using OneData.Attributes;
using OneData.Demo.Enums;
using OneData.Interfaces;
using OneData.Models;
using System;
using System.ComponentModel;

namespace OneData.Demo.Models
{
    [DataTable("movies")]
    public class Movie : Cope<Movie>, IManageable
    {
        [PrimaryKeyProperty]
        public Guid Id { get; set; }
        [DateCreatedProperty]
        public DateTime DateCreated { get; set; }
        [DateModifiedProperty]
        public DateTime DateModified { get; set; }

        [DisplayName("Movie Name")]
        public string Name { get; set; }
        [DisplayName("Realese Year")]
        public int Year { get; set; }
        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        [DisplayName("Duration in Minutes")]
        public int DurationInMinutes { get; set; }
        [ForeignKey(typeof(Origin)), DisplayName("Country of Origin")]
        public Guid OriginId { get; set; }
        [ForeignData(typeof(Origin))]
        public string OriginName { get; set; }
        [DisplayName("Cover URL")]
        public string CoverUrl { get; set; }
        public float Score { get; set; }
        public MovieRatings Rating { get; set; }

        public Movie()
        {
            Id = Guid.NewGuid();
        }

        public Movie(Guid id)
        {
            Id = id;
        }
    }
}
