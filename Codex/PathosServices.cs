using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexServices : CodexPage<ManifestServices, ServiceEditor, Service>
  {
    private CodexServices() { }
#if MASTER_CODEX
    internal CodexServices(Codex Codex)
      : base(Codex.Manifest.Services)
    {
      var Strikes = Codex.Strikes;
      var Sanctities = Codex.Sanctities;
      var Attributes = Codex.Attributes;
      var Skills = Codex.Skills;
      var Spells = Codex.Spells;
      var Schools = Codex.Schools;

      Service AddService(string Name, string Description, Gold Cost, Action<ServiceEditor> Action)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Description = Description;
          S.Cost = Cost;

          CodexRecruiter.Enrol(() => Action(S));
        });
      }

      detect_traps = AddService("trap detection", "detect traps in the surrounding area.", Gold.FromCoins(100), S =>
      {
        S.SetCast().Strike(Strikes.boost, Dice.Zero);
        S.Apply.DetectTrap(Range.Sq15);
      });

      identify = AddService("identification", "identify one item in your inventory.", Gold.FromCoins(500), S =>
      {
        S.SetCast().FilterIdentified(false);
        S.Apply.IdentifyItem(All: false, Sanctity: null);
      });

      magic_mapping = AddService("magic mapping", "magic mapping for the surrounding area.", Gold.FromCoins(250), S =>
      {
        S.SetCast().Strike(Strikes.boost, Dice.Zero);
        S.Apply.Mapping(Range.Sq15, Chance.Always);
      });

      remove_curse = AddService("curse removal", "remove a curse from one item in your inventory.", Gold.FromCoins(1000), S =>
      {
        S.SetCast().FilterSanctity(Sanctities.Cursed);
        S.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
      });

      restock = AddService("restock shop", "refresh the shop with new items for sale.", Gold.FromCoins(10000), S =>
      {
        S.SetCast().Plain(Dice.Zero);
        S.Apply.Restock();
      });
      // >>> GENERATED SERVICES >>>
      // Tuition sold for gold. The engine's LearnSpell takes either one specific spell or null for
      // a random one - there is no "random spell of school X" - so this is honestly a gamble, and
      // priced as one. Targeted acquisition is what the book shops are for.
      spell_tuition = AddService("spell tuition", "learn how to cast a random spell.", Gold.FromCoins(900), S =>
      {
        S.SetCast().Plain(Dice.Zero);
        S.Apply.LearnSpell(Attributes.intelligence, Skills.literacy, Spell: null);
      });

      // A second, dearer sitting for when the first one taught you something you already knew.
      advanced_tuition = AddService("advanced tuition", "learn how to cast two random spells.", Gold.FromCoins(2000), S =>
      {
        S.SetCast().Plain(Dice.Zero);
        S.Apply.LearnSpell(Attributes.intelligence, Skills.literacy, Spell: null);
        S.Apply.LearnSpell(Attributes.intelligence, Skills.literacy, Spell: null);
      });
      // <<< GENERATED SERVICES <<<
    }
#endif

    // >>> GENERATED SERVICES-FIELDS >>>
    public readonly Service spell_tuition;
    public readonly Service advanced_tuition;
    // <<< GENERATED SERVICES-FIELDS <<<
    public readonly Service detect_traps;
    public readonly Service identify;
    public readonly Service magic_mapping;
    public readonly Service remove_curse;
    public readonly Service restock;
  }
}