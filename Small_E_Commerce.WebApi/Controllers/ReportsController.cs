using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Small_E_Commerce.Application.Reports;

namespace Small_E_Commerce.WebApi.Controllers;

[ApiController]
[Route("api/v1/reports")]
public class ReportsController(ISender sender) : ControllerBase
{
    [HttpGet("orders-weekly")]
    [Authorize (Policy = "Report.OrdersWeekly")]
    public async Task<ActionResult<IFormFile>> GlobalReport(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new OrdersWeeklyReportQuery(), cancellationToken);
        return File(result, MediaTypeNames.Application.Octet, "Orders_Weekly_Report.csv");
    }
    
}