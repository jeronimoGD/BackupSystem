using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.GenericDTOs;

namespace BackupSystem.DTO.BackUpConfigurationDTOs
{
    public class BackUpConfigurationDTO  : GenericDTO, IMapFrom<Agent>
    {
    }
}
