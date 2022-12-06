using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.AccountsViewer.Models;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AccountsViewer.Queries;


public record QueryCurrentBalances : IRequest<List<AccountsListDto>>
{
    public string Month { get; set; }
}

public class QueryCurrentBalancesHandler : IRequestHandler<QueryCurrentBalances, List<AccountsListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public QueryCurrentBalancesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountsListDto>> Handle(QueryCurrentBalances request, CancellationToken cancellationToken)
    {
        var accounts = new List <string> { "R&D", "Canteen", "CEO’s car", "Marketing", "Parking fines" };

        var month = MonthToNumber(request.Month);
        var records = await _context.AccountsViewers.Where(x => x.Year <= DateTime.Now.Year).ToListAsync();

        foreach (var item in records)
        {
            var tmp = new DateTime(item.Year, item.Month, 1);
            if (tmp.CompareTo(DateTime.Now) > 0)
            {
                records.Remove(item);
            }
        }

        var response = new List<AccountsListDto>();

        foreach (var item in accounts)
        {
            response.Add(new AccountsListDto
            {
                Balance = records.Where(x => x.AccountName == item).Select(x => x.Balance).Sum(),
                AccountName = item
            });
        }
        
        return response;
    }

private int MonthToNumber(string month)
    {
        switch (month.ToLower())
        {
            case "january": return 1;
            case "february": return 2;
            case "march": return 3;
            case "april": return 4;
            case "may": return 5;
            case "june": return 6;
            case "july": return 7;
            case "august": return 8;
            case "september": return 9;
            case "october": return 10;
            case "november": return 11;
            case "december": return 12;
            default: return 0;
        }
    }
}
