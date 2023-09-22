using MediatR;
using SolarRelog.Infrastructure;

namespace SolarRelog.Application.Commands.Backup;

public record CreateBackupCommand : IRequest<BackupFile>;

public class CreateBackupCommandHandler : IRequestHandler<CreateBackupCommand, BackupFile>
{
    private readonly AppDbContext _dbContext;

    public CreateBackupCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BackupFile> Handle(CreateBackupCommand request, CancellationToken cancellationToken)
    {
        return await _dbContext.CreateBackup();
    }
}