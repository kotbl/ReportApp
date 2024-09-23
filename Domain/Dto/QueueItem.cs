namespace Domain.Dto;

public class QueueItem
{
    public Guid QueryId { get; set; }
    public UserStatisticRequest? Request { get; set; }
}
