using System.Net;
using Fina.Api.Data;
using Fina.Core.Enums;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Fina.Core.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly AppDbContext _context;

    public TransactionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        if (request is {Type: ETransactionType.Withdraw, Amount: >= 0 })
        {
            request.Amount *= -1;
        }

        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(
                data:transaction, 
                code:(int)HttpStatusCode.Created, 
                message:$"Transação \"{request.Title}\" criada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível criar a transação \"{request.Title}\"!");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await _context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction == null)
            {
                return new Response<Transaction?>(
                    data:null, 
                    code:(int)HttpStatusCode.NotFound, 
                    message:$"Não foi possível encontrar a transação!");
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(
                data:transaction,  
                message:$"Transação excluída com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível excluir a transação!");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await _context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return transaction == null
                ? new Response<Transaction?>(
                    data:null, 
                    code:(int)HttpStatusCode.NotFound, 
                    message:$"Transação não encontrada!")
                : new Response<Transaction?>(data:transaction);
        }
        catch
        {
            return new Response<Transaction?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível obter a transação solicitada!");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError,
                message:$"Não foi possível determinar a data de início ou término!");
        }

        try
        {
            var query = _context
                .Transactions
                .AsNoTracking()
                .Where(x =>
                    x.PaidOrReceivedAt >= request.StartDate &&
                    x.PaidOrReceivedAt <= request.EndDate &&
                    x.UserId == request.UserId)
                .OrderBy(x => x.PaidOrReceivedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(
                data:transactions,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível obter as transações!");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request is {Type: ETransactionType.Withdraw, Amount: >= 0 })
        {
            request.Amount *= -1;
        }

        try
        {
            var transaction = await _context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction == null)
            {
                return new Response<Transaction?>(
                    data:null, 
                    code:(int)HttpStatusCode.NotFound, 
                    message:$"Não foi possível encontrar a transação \"{request.Title}\"!");
            }

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(
                data:transaction,  
                message:$"Transação \"{request.Title}\" atualizada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível atualizar a transação!");
        }
    }

}
