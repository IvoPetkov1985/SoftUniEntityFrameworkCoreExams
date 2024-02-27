using Newtonsoft.Json;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class ExportTaskJsonDTO
    {
        [JsonProperty(nameof(TaskName))]
        public string TaskName { get; set; } = null!;

        [JsonProperty(nameof(OpenDate))]
        public string OpenDate { get; set; } = null!;

        [JsonProperty(nameof(DueDate))]
        public string DueDate { get; set; } = null!;

        [JsonProperty(nameof(LabelType))]
        public string LabelType { get; set; } = null!;

        [JsonProperty(nameof(ExecutionType))]
        public string ExecutionType { get; set; } = null!;
    }

    public class ExportExployeeDTO
    {
        [JsonProperty(nameof(Username))]
        public string Username { get; set; } = null!;

        [JsonProperty(nameof(Tasks))]
        public ExportTaskJsonDTO[] Tasks { get; set; } = null!;
    }
}
