using BackUpAgent.Common.Interfaces.Mapping;
using BackUpAgent.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace BackUpAgent.Common.Services.ApiRequest.DTO
{
    public class BackUpHistoryCreateDTO: IMapFrom<BackUpHistory>
    {
        [Required]
        public string BackUpName { get; set; }
        public string BackUpPath { get; set; }
        public bool IsSuccessfull { get; set; }
        public double BackUpSizeInMB { get; set; }
        [Required]
        public DateTime BuckUpDate { get; set; }
        public bool AvailableToDownload { get; set; }
        public string Description { get; set; }
        public Guid AgentId { get; set; }
    }
}
