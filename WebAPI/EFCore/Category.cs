using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Column(TypeName ="varchar(50)")]
        public string Name { get; set; }
    }
}
