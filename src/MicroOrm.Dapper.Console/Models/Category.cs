using System;  
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroOrm.Dapper.Console
{
    [Table("categories")]
    public class Category  
    {
        [Column("id"), Key]
        public Guid? Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; }

        [Column("description_short")]
        public string DescriptionShort { get; set; }

        [Column("button_color")]
        public string ButtonColor { get; set; }

        [Column("published")]
        public int Published { get; set; }

        [Column("image_id")]
        public Guid? ImageId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; } 

        [Column("synced_at")]
        public DateTime? SyncedAt { get; set; }

        public override string ToString()
        {
            return string.Format("Id:{0} | Name:{1}", Id, Name);
        }
         
        [NotMapped]
        public bool IsSelected
        {
            get { return false; }
        }


        [NotMapped]
        public IList<Item> Items { get; set; } = new List<Item>();
    }

}
