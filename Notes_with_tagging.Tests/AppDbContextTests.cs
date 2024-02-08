using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes_with_tagging.Constants;
using Notes_with_tagging.Data;
using Notes_with_tagging.Models;
using Notes_with_tagging.Services;

namespace Notes_with_tagging.Tests;

public class AppDbContextTests
{
    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

        var configuration = configurationBuilder.Build();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("WebApiDatabase"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IInscriptionsService, InscriptionsService>();

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task InscriptionsCRUDTest()
    {
        var serviceProvider = BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var inscriptopnService = scope.ServiceProvider.GetService<IInscriptionsService>();
        var uniqueText = Guid.NewGuid().ToString();

        //Add inscription
        await inscriptopnService.AddInscription(new Inscription { Text = uniqueText });

        //Get all inscriptions
        var inscriptions = await inscriptopnService.GetInscriptions();
        Assert.True(inscriptions.Any());

        //Select added inscription form all inscriptions
        var inscription = inscriptions.Where(x => x.Text == uniqueText).FirstOrDefault();
        Assert.True(inscription is not null);
        Assert.True(inscription.Text == uniqueText);

        //Get added inscription from db
        inscription = await inscriptopnService.GetSingleInscription(inscription.Id);
        Assert.True(inscription is not null);
        Assert.True(inscription.Text == uniqueText);

        //Update inscription
        var sampleText = "This text contains phone number 666123997 and email foo@bar.com";
        inscription.Text = sampleText;
        await inscriptopnService.UpdateInscription(inscription);

        //Get updated inscription
        inscription = await inscriptopnService.GetSingleInscription(inscription.Id);
        Assert.True(inscription.Text == sampleText);
        Assert.True(inscription.Tags.Contains(Tags.Phone));
        Assert.True(inscription.Tags.Contains(Tags.Email));

        //Delete inscription
        await inscriptopnService.DeleteInscription(inscription);
        inscription = await inscriptopnService.GetSingleInscription(inscription.Id);
        Assert.True(inscription is null);
    }
}
