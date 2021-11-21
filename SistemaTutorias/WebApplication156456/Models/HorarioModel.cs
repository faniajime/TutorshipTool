namespace WebApplication156456
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using DHTMLX.Scheduler;

    [Table("HORARIO")]
    public class HorarioModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DHXJson(Alias = "id")]
        public int id { get; set; }

        public string person_id { get; set; }

        [DHXJson(Alias = "text")]
        public string text { get; set; }

        [DHXJson(Alias = "start_date")]
        public DateTime start_date { get; set; }

        [DHXJson(Alias = "end_date")]
        public DateTime end_date { get; set; }
    }
}