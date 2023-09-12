using System.Data.SQLite;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace SolarRelog.Infrastructure;

public record BackupFile(FileStream Content, string ContentType, string FileName);

public static class DbContextExtensions
{
    private const string ConnStrPrefix = "Data Source=";
    
    public static async Task<BackupFile> CreateBackup(this DbContext context)
    {
        var sourceConnString = context.Database.GetConnectionString() ?? string.Empty;
        
        var filePath = GetFilePathFromConnectionString(sourceConnString);
        var filename = filePath.Replace(".db", string.Empty);
        
        var backupFileName = filename + "_backup_" + DateTime.Now.ToString("yy-MM-dd_HH-mm-ss") + ".db";
        var backupConnString = ConnStrPrefix + backupFileName;
        
        await using var location = new SQLiteConnection(sourceConnString);
        await using var destination = new SQLiteConnection(backupConnString);
        
        await location.OpenAsync();
        await destination.OpenAsync();
        
        location.BackupDatabase(destination, "main", "main", -1, null, 0);

        var fullBackupPath = destination.FileName;
        
        await location.CloseAsync();
        await destination.CloseAsync();
        
        var fs = new FileStream(fullBackupPath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
        
        return new BackupFile(
            Content: fs,
            ContentType: MediaTypeNames.Application.Octet,
            FileName: backupFileName);
    }

    private static string GetFilePathFromConnectionString(string connectionString)
    {
        var path = connectionString.Replace(ConnStrPrefix, string.Empty);
        return path.Trim();
    }
}