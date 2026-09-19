using System.Security.Cryptography;
using System.Text;
using TaskScheduler.Models;

namespace TaskScheduler.Infrastructure;

public static class ETagHelper
{
    public static string Compute(ScheduledTask t)
    {
        var raw = $"{t.Id}|{t.Title}|{t.ScheduledAt:o}|{t.Status}|{t.Description}|{t.DurationMinutes}";
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(raw));
        return "\"" + Convert.ToHexString(hash).ToLowerInvariant() + "\"";
    }
}
