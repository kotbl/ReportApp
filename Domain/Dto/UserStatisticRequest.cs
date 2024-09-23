namespace Domain.Dto;

public class UserStatisticRequest
{
    public Guid UserId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
