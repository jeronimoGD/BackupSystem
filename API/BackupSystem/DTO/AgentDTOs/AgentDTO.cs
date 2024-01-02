﻿using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystem.DTO.AgentDTOs
{
    public class AgentDTO : IMapFrom<Agent>
    {

        [Required]
        public string AgentName { get; set; }
        [Required]
        public Guid ConnectionKey { get; set; }
    }
}