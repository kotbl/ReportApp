namespace DAL.Models;

public class QueryInfo
{
    public Guid Id { get; set; }
    public double Percentage { get; set; }
    public QueryResult? Result { get; set; }
}
