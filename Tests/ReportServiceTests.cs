using BLL.Services;
using BLL.Services.Interfaces;
using DAL;
using DAL.Models;
using Domain.Dto;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace Tests;

public class ReportServiceTests
{
    private readonly Mock<IReportService> _mockReportService = new Mock<IReportService>();
    private readonly Mock<ApiContext> _apiContext = new Mock<ApiContext>();
    private readonly Mock<BackgroundWorkerQueue> _backgroundWorkerQueue = new Mock<BackgroundWorkerQueue>();
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _reportService = new ReportService(_apiContext.Object, _backgroundWorkerQueue.Object);
    }

    [Test]
    public async Task GetReportInfo_ReturnWithoutResult()
    {
        //Arrange
        var queryInfo = new QueryInfo { Id = Guid.NewGuid(), Percentage = 50 };
        _apiContext.Setup(x => x.Queries).ReturnsDbSet([queryInfo]);

        //Act
        var result = await _reportService.GetReportInfoById(queryInfo.Id);

        //Assert
        result.Should().BeEquivalentTo(queryInfo);
    }

    [Test]
    public async Task GetReportInfo_ReturnWithResult()
    {
        //Arrange
        var queryInfo = new QueryInfo { Id = Guid.NewGuid(), Percentage = 100, Result = new QueryResult() };
        _apiContext.Setup(x => x.Queries).ReturnsDbSet([queryInfo]);

        //Act
        var result = await _reportService.GetReportInfoById(queryInfo.Id);

        //Assert
        result.Should().BeEquivalentTo(queryInfo);
    }

    [Test]
    public async Task CreateUserStatisticsReport()
    {
        //Arrange
        var queriesMockSet = new Mock<DbSet<QueryInfo>>();
        _apiContext.Setup(x => x.Queries).Returns(queriesMockSet.Object);

        //Act
        var result = await _reportService.CreateUserStatisticsReport(new UserStatisticRequest());

        //Assert
        queriesMockSet.Verify(m => m.AddAsync(It.IsAny<QueryInfo>(), CancellationToken.None), Times.Once());
        _apiContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
    }
}
