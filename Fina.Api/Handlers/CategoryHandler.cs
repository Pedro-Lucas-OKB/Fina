using System.Net;
using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly AppDbContext _context;

    public CategoryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description
        };

        try
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
    
            return new Response<Category?>(
                data:category, 
                code:(int)HttpStatusCode.Created, 
                message:$"Categoria \"{request.Title}\" criada com sucesso!");
        }
        catch
        {
            return new Response<Category?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível criar a categoria \"{request.Title}\"!");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
            {
                return new Response<Category?>(
                    data:null, 
                    code:(int)HttpStatusCode.NotFound, 
                    message:$"Categoria não encontrada!");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(
                data:category, 
                message:$"Categoria \"{category.Title}\" excluída com sucesso!");
        }
        catch
        {
            return new Response<Category?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível excluir a categoria!");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = _context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(
                data:categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Category>?>(
                null, 
                (int)HttpStatusCode.InternalServerError, 
                $"Não foi possível consultar as categorias!");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category == null
                ? new Response<Category?>(
                    data:null, 
                    code:(int)HttpStatusCode.NotFound, 
                    message:$"Categoria não encontrada!")
                : new Response<Category?>(data:category);
        }
        catch
        {
            return new Response<Category?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível obter a categoria solicitada!");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
            {
                return new Response<Category?>(
                data:null, 
                code:(int)HttpStatusCode.NotFound, 
                message:$"Categoria \"{request.Title}\" não encontrada!");
            }

            category.Title = request.Title;
            category.Description = request.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(
                data:category, 
                message:$"Categoria \"{request.Title}\" atualizada com sucesso!");
        }
        catch
        {
            return new Response<Category?>(
                data:null, 
                code:(int)HttpStatusCode.InternalServerError, 
                message:$"Não foi possível atualizar a categoria \"{request.Title}\"!");
        }
    }

}
