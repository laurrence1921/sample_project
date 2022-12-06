using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.AccountsViewer.Models;
public class AccountsViewerDto
{
    public string AccountName { get; set; }
    public decimal Balance { get; set; }
    public string Month { get; set; }
    public int Year { get; set; }
}

