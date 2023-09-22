using Microsoft.AspNetCore.Mvc;
using SolarRelog.Application.Commands.Backup;

namespace SolarRelog.Controllers;

public class BackupController : Endpoint
{
    /// <summary>
    /// Download a copy of the apps database file
    /// </summary>
    [HttpGet(template:"backup")]
    public async Task<IActionResult> Get()
    {
        var backup = await Mediator.Send(new CreateBackupCommand());
        return new FileStreamResult(backup.Content, backup.ContentType){FileDownloadName = backup.FileName};
    }
}
