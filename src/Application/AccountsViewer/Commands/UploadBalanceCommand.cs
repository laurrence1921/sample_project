using System.Threading;
using CleanArchitecture.Application.AccountsViewer.Models;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CleanArchitecture.Application.AccountsViewer.Commands;

public record UploadBalanceCommand : IRequest<bool>
{
    public IFormFile File { get; set; }
}

public class UploadBalanceCommandHandler : IRequestHandler<UploadBalanceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UploadBalanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UploadBalanceCommand request, CancellationToken cancellationToken)
    {
        if (request.File.FileName.ToLower().Contains(".xlsx") || request.File.FileName.ToLower().Contains(".xls"))
        {
            await using var stream = request.File.OpenReadStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var p = new ExcelPackage(stream);
            var ws = p.Workbook.Worksheets[0];
            var rowCount = ws.Dimension.End.Row;

            var tmp = new List<AccountsViewerDto>();

            for (int i = 2; i <= rowCount; i++)
            {
                var month = ws.Cells[1, 1].Text.Split(" ")[3];

                tmp.Add(new AccountsViewerDto
                {
                    Month = month,
                    AccountName = ws.Cells[i, 1].Text.Trim(),
                    Balance = Convert.ToDecimal(ws.Cells[i, 2].Text),
                    Year = DateTime.UtcNow.Year
                });
                
                UpsertData(tmp, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        else if(request.File.FileName.ToLower().Contains(".txt"))
        {
            // process as text
            await using var stream = request.File.OpenReadStream();
            using var sr = new StreamReader(stream);
            List<String> lines = new List<String>();
            String line;
            var tmp = new List<AccountsViewerDto>();

            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }

            var month = lines[0].Split(" ")[3];

            for (int i = 1; i < lines.Count; i++)
            {
                var tmpLine = lines[i].Split('\t');
                var balance = Convert.ToDecimal(tmpLine[1]);
                var accountName = tmpLine[0];

                tmp.Add(new AccountsViewerDto
                {
                    AccountName = accountName,
                    Balance = balance,
                    Month = month,
                    Year = DateTime.UtcNow.Year
                });
            }

            UpsertData(tmp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new Exception("File format not recognized!");
        }
        
        return true;
    }

    private void UpsertData (List<AccountsViewerDto> data, CancellationToken cancellationToken)
    {
        foreach (var item in data)
        {
            var tmpRecord =  _context.AccountsViewers
                    .Where(x => x.Month == MonthToNumber(item.Month.ToLower()) && x.Year == item.Year && x.AccountName == item.AccountName)
                    .FirstOrDefault();

            if (tmpRecord is null)
            {
                // add new 
                _context.AccountsViewers.Add(new Domain.Entities.AccountsViewer
                {
                    AccountName = item.AccountName,
                    Balance = item.Balance,
                    Month = MonthToNumber(item.Month.ToLower()),
                    Year = item.Year,
                });
            }
            else
            {
                // update
                tmpRecord.Balance = item.Balance;
            }
        }
    }

    private int MonthToNumber(string month)
    {
        switch (month)
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
