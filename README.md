# Pathos Apocrypha

A content variant of **[Pathos](https://pathos.azurewebsites.net/)** by Callan Hodgskin, built as a
fork of [callanh/pathos-official](https://github.com/callanh/pathos-official) exactly as the upstream
README invites.

*Apocrypha*: the books that sit outside the accepted canon. Pathos calls its content the **Codex**,
and most of what follows is spellbooks — so the name fitted.

It adds **523 new definitions, about 19,400 lines of C#**, and finishes the Italian translation.
`dotnet build` is clean: 0 errors, no codex sanity messages, and the game has been launched and
played from this build.

## What it adds

| | |
|---|---|
| **Spells** | **+161** — the game goes from 62 to **222** — each with its own spellbook, unidentified appearance and cover art in all four tilesets |
| **Items** | **+121** — the mithril, adamantine, gold and rose-gold families, each now a **complete armour set** (helm, suit, shield, boots, gauntlets) as well as its weapons; khopesh, Zweihänder, chakrams, naginata, the NetHack stones (luckstone, loadstone, touchstone), dragon scales and scale mail for the four new dragon colours, and artifacts including Stormbringer, Mournblade, Gungnir, Vajra, Sudarshana Chakra, Shillelagh |
| **Monsters** | **+138** — the drow, the angelic host, the pheral, elementals and spheres of every element, golems, jellies, mimics, four complete dragon families (grey, astral, fae and mystical, each from hatchling to ancient, with their scales and scale mail), an army chain of command, and 33 palette-swapped variants |
| **Specials** | **+14** — the prestige archetypes: vampire spawn, warlock, veteran, specimen, plagued, ascetic, feral, prophet, construct, fey, drowned, duelist, tamer, graverobber. Every one is a genuine trade-off |
| **Classes** | **+5** — apothecary, nightblade, elementalist, witch, slayer |
| **Heroes** | **+17** predefined starts (13 → 30) |
| **Hordes / zoos / shrines** | **+23 / +11 / +5**, the shrines bringing 27 new boons |
| **Tricks / evolutions / recipes / companions** | **+14 / +7 / +3 / +5** |
| **Grimoires on *existing* monsters** | **120 grants across 63 shipped casters** — liches, ancient dragons, drow and greater demons now cast the new spells at you |
| **Italian translation** | **6,461 entries, 100 % complete** (6,086 in the dictionary, 375 in the in-game manual) — the only language level with English |

### Spell accessibility

Books went from 62 to 223 while keeping the same 4 % share of the loot table, which made finding a
*specific* book 2.7× harder. So: the book stock share is 4 % → 6 % (paid for out of food), the
`rare books` shop — the only place you can *choose* a book — goes from rarity 3 to 7, two paid
tuition services were added, and dedicated casters now start with a signature spell **plus two**
random rolls from their schools instead of one.

### Content the author had switched off

Three finished things were sitting commented out with unused artwork, and are now live: the **`oil`
volatile**, the **`mutation` affliction** (its code still called `Attributes.Charisma` from an older
API — corrected, and its missing icon generated), and the **halfling caveman avatars**, which were
drawn in all four tilesets and wired to nothing.

Of the 325 finished-but-unused art assets in `Atlases/*/unused/`, **310 now have content behind them**.

## Art

New spell icons are [game-icons.net](https://game-icons.net) (CC BY 3.0) rendered the way Pathos
does it — 196 px inside a 256 px canvas, flat, in the school colour. New book covers and the
palette-swapped creature variants are recolourings of the shipped Pathos art, generated per tileset
so each keeps its own style. Full credits in [CREDITS-ART.md](CREDITS-ART.md).

## Building

Exactly as upstream: install Pathos to `C:\Games\Pathos`, then `dotnet build PathosOfficial.csproj`.
The build runs `PathosMaker.exe`, which also sanity-checks the codex and fails if any glyph is
missing artwork.

To run this build rather than the shipped game, launch with the project as the working directory:

```
C:\Games\Pathos\PathosGame.exe disable-update campaign:"PathosOfficial.dll"
```

The build writes its assets into the project's `Assets/`, **not** into the installation.

## Upstream

The original project README is kept as [README-UPSTREAM.md](README-UPSTREAM.md);
building, tooling and repository layout are unchanged from it.

## Fantasy register

This variant deliberately leaves out the modern joke items Pathos inherits from NetHack. The
artwork is Callan's own and stays in his game; it simply does not belong in a fork that is trying
to read as fantasy throughout. Removed here: the credit card, stethoscope, towel, tin opener, pill,
sticking plaster, baseball bat, the plastic bat, hammer and wand, the railgun, frisbee, yoyo, the
mooshroom, the "strange object", and the sink and toilet dungeon furniture. The medical kit became
a healer's satchel, which the artwork suited better anyway. Robots and firearms stay: Pathos has
had both from the start.

## Licence

Content is Creative Commons Attribution-NonCommercial 4.0, as upstream — see `license.txt`.
This variant contains no part of the Pathos engine, which is Callan Hodgskin's proprietary software.
Unofficial, and not endorsed by the author.
