namespace StudentManagementApplicationAPI.Models.DTOs.GradeDTOs
{
    public class GradeUpdateDTO
    {
        public int EvaluatedById { get; set; }
        public int MarksScored { get; set; }
        public string Comments { get; set; }
    }
}
