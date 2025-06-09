using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PixelVrtic.Data;
using PixelVrtic.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class IzvjestajGeneratorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public IzvjestajGeneratorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var sada = DateTime.Now;
            var sljedeciMjesec = new DateTime(sada.Year, sada.Month, 1).AddMonths(1);
            var cekanje = sljedeciMjesec - sada;

            await Task.Delay(cekanje, stoppingToken);

            await KreirajIzvjestajeZaProsliMjesec();
        }
    }

    public async Task KreirajIzvjestajeZaProsliMjesec()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var prosliMjesec = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
        var pocetak = prosliMjesec;
        var kraj = prosliMjesec.AddMonths(1);

        var svaDjeca = context.Dijete.Include(d => d.grupa).ToList();

        foreach (var dijete in svaDjeca)
        {
            bool izvjestajPostoji = await context.Izvjestaj.AnyAsync(i =>
                i.DijeteId == dijete.id && i.Period == prosliMjesec);

            if (izvjestajPostoji)
                continue;

            int brojPrisustava = await context.Prisustvo
                .Where(p => p.dijeteId == dijete.id &&
                            p.datum >= pocetak &&
                            p.datum < kraj &&
                            p.prisutan)
                .CountAsync();

            var aktivnosti = await context.Aktivnost
                .Where(a => a.idGrupe == dijete.grupaId &&
                            a.datumPocetka >= pocetak &&
                            a.datumPocetka < kraj)
                .Select(a => a.nazivAktivnosti)
                .Distinct()
                .ToListAsync();

            var noviIzvjestaj = new Izvjestaj
            {
                DijeteId = dijete.id,
                Period = prosliMjesec,
                BrojDanaPrisustva = brojPrisustava,
                ListaAktivnosti = string.Join(", ", aktivnosti),
                KomentarVaspitaca = null
            };

            context.Izvjestaj.Add(noviIzvjestaj);
        }

        await context.SaveChangesAsync();
    }
}
