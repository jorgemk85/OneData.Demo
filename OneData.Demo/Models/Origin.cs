using OneData.Attributes;
using OneData.Interfaces;
using OneData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneData.Demo.Models
{
    [DataTable("origins")]
    public class Origin : Cope<Origin>, IManageable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [DateCreated]
        public DateTime DateCreated { get; set; }
        [DateModified]
        public DateTime DateModified { get; set; }

        public string Name { get; set; }

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
