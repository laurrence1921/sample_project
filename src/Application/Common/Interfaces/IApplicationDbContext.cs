using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.AccountsViewer> AccountsViewers { get; }

    DbSet<ApplicationUserToken> ApplicationUserTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
