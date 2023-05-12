using Kalendar;
using Spectre.Console;

// Tato proměnná je určena k tomu, aby byl program schopný říct, zda byla/nebyla nalezena událost ke smazání/prohlédnutí.
bool udalostNalezena = false;
bool wantsToRun = true;
DateOnly soucasneDatum = DateOnly.FromDateTime(DateTime.Now);
List<Udalost> listUdalosti = new List<Udalost>();

AnsiConsole.MarkupLine("Vítej v programu [green]Melon Taskmin[/]!");

// Program je ve smyčce, aby mohl uživatel přídávat nekonečně mnoho událostí. Smyčka se ukončí až na uživatelský vyžádání.
while (wantsToRun)
{
    // Uživatel si vybere, jakou akci chce provést.
    while (wantsToRun)
    {
        // Program si vyžádá od uživatele výběr jedné z možností.
        var akce = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .PageSize(10)
            .AddChoices(new[] {
            "Přidat událost", "Smazat událost", "Zobrazit události", "Ukončit program",
            }));
        switch (akce)
        {
            case "Přidat událost":
                Console.Clear();
                var rokUdalosti = AnsiConsole.Ask<int>("Zadej rok. (YYYY)");
                var mesicUdalosti = AnsiConsole.Ask<int>("Zadej měsíc. (MM)");
                var denUdalosti = AnsiConsole.Ask<int>("Zadej den. (DD)");

                DateOnly datumUdalosti;
                try
                {
                    datumUdalosti = new DateOnly(rokUdalosti, mesicUdalosti, denUdalosti);
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine($"Nastala chyba! Jsi si jistý, že jsi zadal rok, měsíc či den ve správném formátu?");
                    AnsiConsole.MarkupLine(e.Message);
                    break;
                }

                var nadpisUdalosti = AnsiConsole.Ask<string>("Zadej nadpis.");
                var obsahUdalosti = AnsiConsole.Ask<string>("(Volitelné) Zadej podrobný popis události.");

                Udalost udalost = new Udalost(datumUdalosti, nadpisUdalosti, obsahUdalosti);
                listUdalosti.Add(udalost);
                Console.Clear();
                AnsiConsole.MarkupLine("Událost byla úspěšně přidána!");
                break;
            case "Smazat událost":
                Console.Clear();
                if (listUdalosti.Count == 0)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]V kalendáři nejsou žádné události![/]");
                    break;
                }

                foreach (var item in listUdalosti)
                {

                    Console.WriteLine($"[ID: {item.IdUdalosti}] {item.NadpisUdalosti}");
                }
                var smazanaUdalost = AnsiConsole.Ask<int>("Zadej ID požadované události ke smazání. Pro zrušení zadej 0");
                if (smazanaUdalost == 0)
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    foreach (var item in listUdalosti)
                    {
                        if (item.IdUdalosti == smazanaUdalost)
                        {
                            Console.Clear();
                            udalostNalezena = true;
                            listUdalosti.Remove(item);
                            Udalost.Counter--;
                            break;
                        }
                    }
                    if (udalostNalezena != true)
                    {
                        AnsiConsole.MarkupLine("[red]Událost s tímto ID neexistuje![/]");
                    }
                    udalostNalezena = false;
                }


                break;
            case "Zobrazit události":
                if (listUdalosti.Count == 0)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]V kalendáři nejsou žádné události![/]");
                    break;
                }

                foreach (var item in listUdalosti)
                {

                    Console.WriteLine($"[ID: {item.IdUdalosti}] {item.NadpisUdalosti}");
                }
                var hledanaUdalost = AnsiConsole.Ask<int>("Zadej ID požadované události k prohlédnutí.");
                foreach (var item in listUdalosti)
                {
                    if (item.IdUdalosti == hledanaUdalost)
                    {
                        Console.Clear();
                        udalostNalezena = true;
                        AnsiConsole.MarkupLine($"Datum: {item.DatumUdalosti}, Nadpis: {item.NadpisUdalosti}, Obsah: {item.ObsahUdalosti}");
                        break;
                    }
                }
                if (udalostNalezena != true)
                {
                    AnsiConsole.MarkupLine("[red]Událost s tímto ID neexistuje![/]");
                }
                udalostNalezena = false;
                break;
            case "Ukončit program":
                var image = new CanvasImage("logoo.png");
                image.MaxWidth(18);
                Console.Clear();
                AnsiConsole.MarkupLine("[green]Melon Taskmin[/] byl úspěšně ukončen!");
                AnsiConsole.Write(image);
                wantsToRun = false;
                Console.ReadKey();
                break;


        }
    }
}