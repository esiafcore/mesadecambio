using Microsoft.EntityFrameworkCore;
using Xanes.Models;

namespace Xanes.DataAccess.Data;

public class SeedData
{
    public static async Task SeedDataDb(ApplicationDbContext context)
    {
        try
        {
            await context.Database.EnsureCreatedAsync();

            if (context.Customers.Any())
            {
                return;   //Looks For Any Data in Customers DBSet
            }
            //Initialising Data
            await context.Customers.AddRangeAsync(GetCustomersList());
            await context.SaveChangesAsync();

        }
        catch (DbUpdateException e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.Message} - {e.InnerException?.Message}");
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.Message}");
        }
    }

    private static Customer[] GetCustomersList()
    {
        var customersList = new Customer[]
        {
            new Customer
            {
                //Id = 5809,
                CompanyId = 1,
                Code = "00803",
                CategoryId = 6, //COMERCIAL
                CategoryNumeral = 6,
                TypeId = 2, //JURIDICO
                TypeNumeral = 2,
                Identificationnumber = "J0310000122865",
                FirstName = string.Empty,
                SecondName = string.Empty,
                LastName = string.Empty,
                SecondSurname = string.Empty,
                BusinessName = "AMERICAN PHARMA",
                CommercialName = "AMERICAN PHARMA",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                //Id = 5808,
                CompanyId = 1,
                Code = "00802",
                CategoryId = 6,//COMERCIAL
                CategoryNumeral = 6,
                TypeId = 1, //NATURAL
                TypeNumeral = 1,
                Identificationnumber = "0013009870051Y",
                FirstName = "MIGUEL",
                SecondName = "FERNANDO",
                LastName = "RAMIREZ",
                SecondSurname = "OCON",
                BusinessName = "MIGUEL FERNANDO RAMIREZ OCON",
                CommercialName = "MIGUEL FERNANDO RAMIREZ OCON",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                //Id = 5807,
                CompanyId = 1,
                Code = "00801",
                CategoryId = 6,//COMERCIAL
                CategoryNumeral = 6,
                TypeId = 1, //NATURAL
                TypeNumeral = 1,
                Identificationnumber = "244686858",
                FirstName = "JIMMY",
                SecondName = "ALEXANDER",
                LastName = "SANDOVAL",
                SecondSurname = "FRANCO",
                BusinessName = "JIMMY ALEXANDER SANDOVAL FRANCO",
                CommercialName = "JIMMY ALEXANDER SANDOVAL FRANCO",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                //Id = 5806,
                CompanyId = 1,
                Code = "00800",
                CategoryId = 6, //COMERCIAL
                CategoryNumeral = 6,
                TypeId = 2, //JURIDICO
                TypeNumeral = 2,
                Identificationnumber = "J0310000441430",
                FirstName = string.Empty,
                SecondName = string.Empty,
                LastName = string.Empty,
                SecondSurname = string.Empty,
                BusinessName = "INSUMOS SMART NICARAGUA SOCIEDAD ANONIMA",
                CommercialName = "INSUMOS SMART NICARAGUA SOCIEDAD ANONIMA",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            },
            new Customer
            {
                //Id = 5805,
                CompanyId = 1,
                Code = "00799",
                CategoryId = 6,//COMERCIAL
                CategoryNumeral = 6,
                TypeId = 1, //NATURAL
                TypeNumeral = 1,
                Identificationnumber = "0012206860039E",
                FirstName = "MEYLING",
                SecondName = "RAQUEL",
                LastName = "SANCHEZ",
                SecondSurname = "ORTIZ",
                BusinessName = "MEYLING RAQUEL SANCHEZ ORTIZ",
                CommercialName = "MEYLING RAQUEL SANCHEZ ORTIZ",
                IsBank = false,
                IsSystemRow = false,
                IsActive = true
            }        
        };
        return customersList;
    }
}