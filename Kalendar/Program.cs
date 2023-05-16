using Kalendar;
using Spectre.Console;

// Tato proměnná je určena k tomu, aby byl program schopný říct, zda byla/nebyla nalezena událost ke smazání/prohlédnutí.
bool udalostNalezena = false;
// Tato proměnná zajišťuje opakovaný chod cyklu celého programu
bool wantsToRun = true;
DateOnly soucasneDatum = DateOnly.FromDateTime(DateTime.Now);
List<Udalost> listUdalosti = new List<Udalost>();
var kalendář = new Calendar(DateTime.Now);
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
            "Kalendář aktuálního měsíce", "Přidat událost", "Smazat událost", "Zobrazit události", "Ukončit program",
            }));
            // Program nyní rozhoduje co uživatel vybral a na základě toho operuje.
        switch (akce)
        {
            // Vypsat kalendář
            case "Kalendář aktuálního měsíce":
                Console.Clear();
                AnsiConsole.Write(kalendář);
                break;
            // Přidat událost
            case "Přidat událost":
                Console.Clear();
                // Program vyžaduje po uživateli datum, do kterého chce zapsat událost.
                var rokUdalosti = AnsiConsole.Ask<int>("Zadej rok. (YYYY)");
                var mesicUdalosti = AnsiConsole.Ask<int>("Zadej měsíc. (MM)");
                var denUdalosti = AnsiConsole.Ask<int>("Zadej den. (DD)");

                DateOnly datumUdalosti;
                try
                {
                    // Program nyní dosadí uživatelem daný datum do objektu datumUdalosti
                    datumUdalosti = new DateOnly(rokUdalosti, mesicUdalosti, denUdalosti);
                    kalendář.AddCalendarEvent(rokUdalosti, mesicUdalosti, denUdalosti);
                }
                catch (Exception e)
                {
                    // Ošetření výjimky -> Spadne na začátek programu
                    AnsiConsole.MarkupLine($"Nastala chyba! Jsi si jistý, že jsi zadal rok, měsíc či den ve správném formátu?");
                    AnsiConsole.MarkupLine(e.Message);
                    break;
                }

                var nadpisUdalosti = AnsiConsole.Ask<string>("Zadej nadpis.");
                var obsahUdalosti = AnsiConsole.Ask<string>("Zadej podrobný popis události.");

                // Zde se vytvoří objekt udalost, do kterého se dosadí parametry, které jsme vytvořili předtím.
                Udalost udalost = new Udalost(datumUdalosti, nadpisUdalosti, obsahUdalosti);
                // Nakonec se přidá událost do seznamu
                listUdalosti.Add(udalost);
                Console.Clear();
                AnsiConsole.MarkupLine("Událost byla úspěšně přidána!");
                break;
                // Smazat událost
            case "Smazat událost":
                Console.Clear();
                // Program zkontroluje, jestli není list událostí prázdný. Pokud ano, spadne na začátek.
                if (listUdalosti.Count == 0)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]V kalendáři nejsou žádné události![/]");
                    break;
                }
                // Aby uživatel věděl, které události má k dispozici a jaké mají ID, program mu to vypíše.
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
                    // Program projíždí všechny události z listu.
                    foreach (var item in listUdalosti)
                    {
                        // Pokud najde shodu v ID události a ID zadaným uživatelem, stane se následující:
                        if (item.IdUdalosti == smazanaUdalost)
                        {
                            Console.Clear();
                            // Tato proměnná se nastaví, aby program poté mohl zkontrolovat, jestli proběhlo mazání v pořádku.
                            udalostNalezena = true;
                            var tempTask = item;
                            // Jelikož jsem nenašel metodu, která by ve SpectreConsole mazala kalendářovou událost, musím to udělat ručně :)))
                            // Program projede všechny datumy v kalendáři.
                            foreach (var eventy in kalendář.CalendarEvents)
                            {
                                // Do tohoto objektu si dosadíme kvůli formátování ve třídě DateOnly Rok, Měsíc a Den z objektu kalendáře.
                                DateOnly dateTempTask = new DateOnly(eventy.Year, eventy.Month, eventy.Day);

                                // Program hledá shodu v Roku, Měsíci a Dni
                                if (dateTempTask.Year == tempTask.DatumUdalosti.Year && dateTempTask.Month == tempTask.DatumUdalosti.Month && dateTempTask.Day == tempTask.DatumUdalosti.Day)
                                {
                                    // Program maže událost z kalendáře
                                    kalendář.CalendarEvents.Remove(eventy);
                                    // Program maže událost z listu událostí
                                    listUdalosti.Remove(item);
                                    break;
                                }
                            }
                            
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
                // Zobrazení události
            case "Zobrazit události":
                Console.Clear();
                // Program zkontroluje, jestli není list událostí prázdný. Pokud ano, spadne na začátek.
                if (listUdalosti.Count == 0)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]V kalendáři nejsou žádné události![/]");
                    break;
                }
                // Aby uživatel věděl, které události má k dispozici a jaké mají ID, program mu to vypíše.
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
                // Ukočit program
            case "Ukončit program":
                Console.Clear();
                // Vypisování obrázku
                var image = new CanvasImage("logoo.png");
                image.MaxWidth(18);
                Console.Clear();
                AnsiConsole.MarkupLine("[green]Melon Taskmin[/] byl úspěšně ukončen!");
                AnsiConsole.Write(image);
                // Ukončení cyklu pro běh programu
                wantsToRun = false;
                Console.ReadKey();
                break;


        }
    }
}