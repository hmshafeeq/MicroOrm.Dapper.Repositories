
using System; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroOrm.Dapper.Repositories.Attributes.Joins;
using MicroOrm.Dapper.Repositories.Attributes;

namespace MicroOrm.Dapper.Console
{
    [Table("items")]
    public class Item  
    {
        [Column("id"), Key]
        public Guid? Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description_short")]
        public string DescriptionShort { get; set; }

        [Column("price")]
        public string Price { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("prevent_oversell")]
        public int PreventOversell { get; set; }

        [Column("age_verification")]
        public int AgeVerification { get; set; }

        [Column("button_color")]
        public string ButtonColor { get; set; }

        [Column("printer_id")]
        public Guid? PrinterId { get; set; }

        [Column("published")]
        public bool Published { get; set; }

        [Column("category_id")]
        public Guid? CategoryId { get; set; }

        [Column("image_id")]
        public Guid? ImageId { get; set; }

        [Column("default_mod_ids")]
        public string DefaultModIds { get; set; }

        [Column("default_option_ids")]
        public string DefaultOptionIds { get; set; }

        [Column("is_modifiable")]
        public bool IsModifiable { get; set; }

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
        public int Count { get; set; }

        [Ignore]
        public bool IsSelected => Count > 0;


        [LeftJoin("Users", "UserId", "Id")]
        public Category Category { get; set; }
    }



}
