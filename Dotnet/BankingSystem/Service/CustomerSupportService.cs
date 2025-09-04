using AutoMapper;
using DTO;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class CustomerSupportService : ICustomerSupportService
{
    private readonly MyAppDbContext context;
    private readonly IMapper _mapper;

    public CustomerSupportService(MyAppDbContext context, IMapper mapper)
    {
        this.context = context;
        this._mapper = mapper;
    }

    private async Task<int> GetPriorityId(int queryTypeId)
    {
        var result = await context.DbQueryType.FirstAsync(q => q.QueryTypeID == queryTypeId);
        return result.PriorityId;
    }
    public async Task<bool> CreateQueryAsync(RaiseTicketDTO raiseTicketDTO)
    {

        try
        {
            var query = new CustomerQueryModel
            {
                CreatedBy = raiseTicketDTO.UserId,
                QueryTypeId = raiseTicketDTO.TicketTypeId,
                CreatedAt = IndianTime.GetIndianTime(),
                IsSolved = false,
                SolvedBy = 0,
                PriorityId = await GetPriorityId(raiseTicketDTO.TicketTypeId),
                StatusId = 1
            };

            await context.DbCustomerQuery.AddAsync(query);
            await context.SaveChangesAsync();

            var comment = new QueryComments
            {
                CustomerQueryId = query.CustomerQueryId,
                comments = raiseTicketDTO.Message,
                IsUserComment = true,
                IsStaffComment = false,
                CreatedAt = IndianTime.GetIndianTime()
            };

            await context.DbQueryComments.AddAsync(comment);
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<QueryComments> AddCommentAsync(AddCommentsDTO addCommentsDTO)
    {
        var query = await context.DbCustomerQuery.FirstOrDefaultAsync(q => q.CustomerQueryId == addCommentsDTO.QueryId);
        if (query == null)
            return null!;

        var comment = new QueryComments
        {
            CustomerQueryId = addCommentsDTO.QueryId,
            comments = addCommentsDTO.Comments,
            IsUserComment = !addCommentsDTO.isStaff,
            IsStaffComment = addCommentsDTO.isStaff,
            CreatedAt = IndianTime.GetIndianTime()
        };

        await context.DbQueryComments.AddAsync(comment);
        await context.SaveChangesAsync();

        return comment;
    }

   public async Task<List<CustomerQueryDTO>> GetAllPendingQueriesAsync()
    {
        var query = context.DbCustomerQuery
            .Include(q => q.User)
            .Include(q => q.queryType)
            .Include(q => q.QueryComments)
            .Include(q => q.QueryStatus)
            .Include(q => q.QueryPriority)
            .AsQueryable();
        query = query.Where(q => !q.IsSolved).OrderByDescending(m=> m.QueryPriority.QueryPriorityId);
        var result = await query.ToListAsync();
        return _mapper.Map<List<CustomerQueryDTO>>(result);
    }


    public async Task<bool> MarkAsSolvedAsync(int queryId, int staffId)
    {
        try
        {
            var query = await context.DbCustomerQuery.FirstOrDefaultAsync(q => q.CustomerQueryId == queryId);
            if (query == null)
                return false;

            query.IsSolved = true;
            query.SolvedBy = staffId;
            query.SolvedAt = IndianTime.GetIndianTime();

            context.DbCustomerQuery.Update(query);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
