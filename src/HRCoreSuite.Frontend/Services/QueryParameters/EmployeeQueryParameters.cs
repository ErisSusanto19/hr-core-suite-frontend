namespace HRCoreSuite.Frontend.Services.QueryParameters
{
    public class EmployeeQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? PositionId { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }

        public int DaysUntilExpiry { get; set; }
    }
}