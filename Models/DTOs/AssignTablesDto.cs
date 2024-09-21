namespace Ordernary.Models.DTOs
{
    public class AssignTablesDto
    {
        public int WaiterId { get; set; }
        public List<int> TableIds { get; set; }
    }
}
