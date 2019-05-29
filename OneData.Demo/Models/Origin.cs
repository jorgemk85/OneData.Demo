using OneData.Attributes;
using OneData.Interfaces;
using OneData.Models;
using System;

namespace OneData.Demo.Models
{
    [DataTable("origins"), CacheEnabled(30)]
    public class Origin : Cope<Origin>, IManageable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [DateCreated]
        public DateTime DateCreated { get; set; }
        [DateModified]
        public DateTime DateModified { get; set; }

        [Unique]
        public string Name { get; set; }
        [Default(16)]
        public int? IntWithoutValue { get; set; }

        public Origin()
        {
            Id = Guid.NewGuid();
        }

        public Origin(Guid id)
        {
            Id = id;
        }
    }
}
