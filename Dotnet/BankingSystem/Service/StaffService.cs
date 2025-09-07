using AutoMapper;
using interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using MyDbContext;

namespace Service;

public class StaffService : IStaffService
{
    private readonly MyAppDbContext context;

    private readonly IMapper mapper;
    public StaffService(MyAppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<string> ReviewAccountTypeChangeAsync(int ticketId, int staffId, int action,string RejectionReason)
    {
        var ticket = await context.DbAccountUpdateTickets
            .Include(t => t.Account)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);

        if (ticket == null)
            return "Ticket not found.";

        if (ticket.IsProcessed)
            return "Ticket already processed.";

        if (action == 1)
        {
            var parts = ticket.RequestedChange.Split(" to ");
            if (parts.Length != 2)
                return "Invalid ticket format.";

            var newAccountType = parts[1];
            var accountType = await context.DbAccountType
                .FirstOrDefaultAsync(a => a.AccountType.ToLower() == newAccountType.ToLower());

            if (accountType == null)
                return "Invalid account type in ticket.";

            ticket.Account!.AccountTypeId = accountType.AccountTypeID;
            ticket.Account.LastTransactionAt = IndianTime.GetIndianTime();

            ticket.IsApproved = true;
            ticket.IsProcessed = true;
            ticket.RejectionReason = "";
            ticket.ApprovedBy = staffId.ToString();
            ticket.ApprovedAt = IndianTime.GetIndianTime();

            context.DbAccount.Update(ticket.Account);
            context.DbAccountUpdateTickets.Update(ticket);
            await context.SaveChangesAsync();

            return "Account type updated successfully after staff approval.";
        }
        else if (action == 2)
        {
            ticket.IsApproved = false;
            ticket.IsProcessed = true;
            ticket.RejectionReason = RejectionReason;
            ticket.ApprovedBy = staffId.ToString();
            ticket.ApprovedAt = IndianTime.GetIndianTime();

            context.DbAccountUpdateTickets.Update(ticket);
            await context.SaveChangesAsync();

            return "Ticket rejected by staff.";
        }

        return "Invalid action. Use 1 for approve, 2 for reject.";
    }

    public async Task<List<AccountUpdateTicketDTO>> GetAllAccountUpdateTickesAsync()
    {
        var ticket = await context.DbAccountUpdateTickets.ToListAsync();

        if (ticket == null)
        {
            return null;
        }
        return mapper.Map<List<AccountUpdateTicketDTO>>(ticket);
    }

    public async Task<List<AccountUpdateTicketDTO>> GetALlPendingAccountUpdateTicketAsync()
    {
        var ticket = await context.DbAccountUpdateTickets.Where(x => x.IsProcessed == false).ToListAsync();
        if (ticket == null)
        {
            return null;
        }
        return mapper.Map<List<AccountUpdateTicketDTO>>(ticket);
    }

}