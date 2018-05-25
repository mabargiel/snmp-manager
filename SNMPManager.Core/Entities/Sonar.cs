using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNMPManager.Core.Entities
{
    public class Sonar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string IpAddress { get; set; }
        public string Mib { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
