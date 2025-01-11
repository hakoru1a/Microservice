using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.DTOs.Customer;
using Customer.API.Services.Interface;

namespace Customer.API.Controllers
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customers")
                          .WithTags("Customers");

            group.MapGet("/", async (ICustomerService customerService) =>
            {
                var result = await customerService.GetCustomers(0);
                return Results.Ok(result);
            })
            .WithName("GetCustomers")
            .Produces<List<CustomerDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapGet("/{id}", async (long id, ICustomerService customerService) =>
            {
                var customer = await customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                    return Results.NotFound($"Customer with ID {id} not found.");

                return Results.Ok(customer);
            })
            .WithName("GetCustomerById")
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapGet("/by-email/{email}", async (string email, ICustomerService customerService) =>
            {
                var customer = await customerService.GetCustomerByEmailAddressAsync(email);
                if (customer == null)
                    return Results.NotFound($"Customer with email {email} not found.");

                return Results.Ok(customer);
            })
            .WithName("GetCustomerByEmail")
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPost("/", async (CreateCustomerDto customerDto, ICustomerService customerService) =>
            {
                try
                {
                    var result = await customerService.CreateCustomerAsync(customerDto);
                    return Results.Created($"/api/customers/{result.Id}", result);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CreateCustomer")
            .Produces<CustomerDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id}", async (long id, UpdateCustomerDto customerDto, ICustomerService customerService) =>
            {
                try
                {
                    var result = await customerService.UpdateCustomerAsync(id, customerDto);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("not found"))
                        return Results.NotFound(ex.Message);

                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("UpdateCustomer")
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id}", async (long id, ICustomerService customerService) =>
            {
                var result = await customerService.DeleteCustomerAsync(id);
                if (!result)
                    return Results.NotFound($"Customer with ID {id} not found.");

                return Results.NoContent();
            })
            .WithName("DeleteCustomer")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}