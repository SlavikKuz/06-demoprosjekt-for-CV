﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TeleDronBot.DTO
{
    class StepDTO
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("UserDTO")]
        public long ChatId { get; set; }

        public UserDTO user { get; set; }
        public string NameOfStep { get; set; }
        public int CurrentStep { get; set; }
    }
}
