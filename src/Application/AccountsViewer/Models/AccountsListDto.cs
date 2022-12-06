using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.AccountsViewer.Models;

public class AccountsListDto
{
    public string AccountName { get; set; }
    public decimal Balance { get; set; }
}

