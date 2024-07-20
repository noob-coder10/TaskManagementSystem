namespace TaskManagementSystem.Models.DTO.DocumentDto
{
    public class AddDocumentRequestDto
    {
        public IFormFile Document { get; set; }

        public string RequestBody { get; set; }
    }
}
