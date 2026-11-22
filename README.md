# LifeManagementApp

Mun projekti Client-Side Programming -kurssille (TT00BI73, VAMK, syksy 2026).  
Tekijä: Elton Zekiqi

Tein .NET MAUI -sovelluksen, jossa on to-do-lista ja Vaasan sää samassa paikassa. Lisäksi appi kertoo, jos tuuli riittää leijasurffaukseen.

## Mitä sovellus tekee

**Etusivu**
- Näyttää 3 tärkeintä tekemätöntä tehtävää. Ensin korkein prioriteetti, ja jos menee tasan, niin aikaisempi eräpäivä voittaa.
- Hakee Vaasan sään Open-Meteosta (lämpötila ja tuulen nopeus m/s).
- Jos tuulee yli 5 m/s, ruudulle tulee iso sininen "🪁 Leijasurffikeli!" -laatikko.

**Tehtävät**
- Listaa kaikki tehtävät.
- Tehtävät voi järjestää prioriteetin tai eräpäivän mukaan.
- "+ Lisää tehtävä" -napista avautuu uuden tehtävän näkymä.
- Tehtävää napauttamalla pääsee muokkaamaan sitä.

**Lisää / muokkaa**
- Kentät: otsikko, prioriteetti (1–5), eräpäivä, kategoria, muistiinpanot ja tehty-ruksi.
- Ilman otsikkoa ei voi tallentaa, vaan tulee punainen virheilmoitus.
- Muokatessa näkyy myös Poista-nappi.

**Muuta**
- Tehtävät tallentuvat SQLiteen (EF Core Code First), joten ne säilyvät sovelluksen sulkemisen jälkeen.
- Kategoriat (Koulu, Työ, Koti, Urheilu) ovat omassa taulussa (1–N-suhde: yhdellä kategorialla voi olla monta tehtävää).
- Jos verkkoyhteys puuttuu, säästä näytetään virheilmoitus eikä sovellus kaadu. Latauksen aikana näkyy latausympyrä.

## Käynnistäminen

Tarvitset .NET 9 SDK:n ja MAUI-workloadin (`dotnet workload install maui`). Olen testannut sovelluksen Windowsilla.

```bash
git clone <repo-url>
cd LifeManagementApp
dotnet build LifeManagementApp/LifeManagementApp.csproj -f net9.0-windows10.0.19041.0
dotnet run --project LifeManagementApp/LifeManagementApp.csproj -f net9.0-windows10.0.19041.0
