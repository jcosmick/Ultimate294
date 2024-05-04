using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace SCP294.Types.Config
{
    public class DrinkList
    {
        public static List<CustomDrink> DefaultDrinks = new List<CustomDrink>()
    {
      new()
      {
        DrinkNames =
          {
          "SCP-207 (coca-cola)"
          },
        AntiColaModel = false,
        HealAmount = 30f,
        BackfireChance = 0.0f,
        DrinkMessage = "è fredda e rinfrescante.",
        DrinkEffects =
          {
          new DrinkEffect()
          {
            EffectType = (EffectType) 18,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 0.0f
          }
          }
      },
      new()
      {
        DrinkNames = new List<string>()
        {
          "anti-SCP-207 (pepsi)"
        },
        AntiColaModel = true,
        BackfireChance = 0.0f,
        DrinkMessage = "è estremamente calda",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = (EffectType) 34,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 0.0f
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Anti-materia"
        },
        AntiColaModel = false,
        Explode = true,
        BackfireChance = 0.0f,
        DrinkMessage = "BOOM BOOM!!!",
        DrinkEffects = new List<DrinkEffect>()
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Cocaina"
        },
        AntiColaModel = true,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti strano, ma pieno di energie.",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 4,
            Time = 5f,
            ShouldAddIfPresent = true
          },
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 18,
            Time = 200f,
            ShouldAddIfPresent = true
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Caffè"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ha un sapore che ricorda il caffè del bar sotto casa.",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 4,
            Time = 2f,
            ShouldAddIfPresent = true
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Coraggio",
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "All'improvviso ti senti come se potessi fare quello che vuoi",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 15,
            Time = 180f,
            ShouldAddIfPresent = true
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Morte"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Cosa ti aspettavi",
        KillPlayer = true,
        KillPlayerString = "Hai bevuto la morte, cosa ti aspettavi?",
        DrinkEffects = new List<DrinkEffect>()
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Red Bull"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Red Bull ti mette le aliiii.",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 15,
            Time = 180f,
            ShouldAddIfPresent = true
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>() { "Estus" },
        HealAmount = 100f,
        HealStatusEffects = true,
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Il sapore è difficile da descrivere, ti senti meglio però",
        DrinkEffects = new List<DrinkEffect>()
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Felicità"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Una forte sensazione di gioia ti pervade il cuore",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 29,
            Time = 30f,
            ShouldAddIfPresent = true
          },
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 4,
            Time = 30f,
            ShouldAddIfPresent = true
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Sborra"
        },
        Tantrum = true,
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Perchè?...",
        DrinkEffects = new List<DrinkEffect>()
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Velocità",
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti molto nervoso.",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 4,
            Time = 15f,
            ShouldAddIfPresent = true
          },
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 15,
            Time = 15f,
            ShouldAddIfPresent = true
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Erba"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "QUESTO DRINK SPACCA!",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 4,
            Time = 60f,
            ShouldAddIfPresent = true
          },
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 15,
            Time = 60f,
            ShouldAddIfPresent = true
          }
        }
      }
    };
        public static List<CustomDrink> CommunityDrinks = new List<CustomDrink>()
    {
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Caramella rosa"
        },
        AntiColaModel = true,
        BackfireChance = 0.25f,
        ExplodeOnBackfire = true,
        DrinkMessage = "Ha un forte sapore di fragola, ti fa male la pancia...",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = (EffectType) 18,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 0.0f
          },
          new DrinkEffect()
          {
            EffectType = (EffectType) 34,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 0.0f
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "SCP-173"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più fragile, e più solido allo stesso tempo.",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = (EffectType) 21,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 3f
          },
          new DrinkEffect()
          {
            EffectType = (EffectType) 0,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 3f
          },
          new DrinkEffect()
          {
            EffectType = (EffectType) 4,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 3f
          },
          new DrinkEffect()
          {
            EffectType = (EffectType) 11,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 3f
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Omosessuale"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkCallback = new Action<Player>(DrinkCallbacks.Boykisser),
        DrinkMessage = "",
        DrinkEffects = new List<DrinkEffect>()
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "SCP-268 (invisibilità)"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti invisibile",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = (EffectType) 19,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 15f
          }
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "SCP-500 (panacea)"
        },
        HealAmount = 100f,
        HealStatusEffects = true,
        AntiColaModel = true,
        BackfireChance = 0.25f,
        DrinkMessage = "Ha il sapore di uno sciroppo per la tosse. Ti senti già meglio stranamente.",
        DrinkEffects = new List<DrinkEffect>()
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "SCP-999 (mostro degli abbracci)"
        },
        HealAmount = 100f,
        HealStatusEffects = true,
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più felice, e le tue ferite cominciano a curarsi.",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = (EffectType) 15,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 45f
          },
        }
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "SCP-018 (pallina rossa)"
        },
        AntiColaModel = true,
        BackfireChance = 0.99f,
        DrinkMessage = "Subito, come hai bevuto il drink, varie SCP-018 escono dalla tua bocca!!!",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.BallSpam)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Pirots"
        },
        AntiColaModel = true,
        BackfireChance = 0.0f,
        DrinkMessage = "ECCOLOOOO!!!",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Pirots)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Nanismo"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più piccolo...",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Shrink)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Gigantismo"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più grande...",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Grow)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Anoressia"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più magro...",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Thin)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Obesità"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più obeso...",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Wide)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Enderman"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = EffectType.SinkHole,
            EffectAmount = (byte) 1,
            ShouldAddIfPresent = true,
            Time = 0.3f
          }
        },
        DrinkCallback = new Action<Player>(DrinkCallbacks.Enderman)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Flashbang"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "now do the flashbang dance",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Flash)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Tradimento"
        },
        AntiColaModel = true,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti in dovere di tradire il tuo team",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Tradimento)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Elio"
        },
        AntiColaModel = true,
        BackfireChance = 0.0f,
        DrinkMessage = "(La tua voce è a tono alto per 1.5 minuti)",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(player => DrinkCallbacks.Helium(player, 90f))
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Zombie nano"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "Ti senti più piccolo e più assetato di sangue",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Zombie)
        
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Zolfo"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "(la tua voce è a tono basso per 1.5 minuti)",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Sulfur)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "SCP-106 (uomo nero)"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "per 15 secondi sarai nascosto nella dimensione dell'uomo nero \n non provare a scappare",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectType = EffectType.SinkHole,
            EffectAmount = (byte) 255,
            ShouldAddIfPresent = true,
            Time = 15f
          },
          new DrinkEffect()
          {
            EffectType = EffectType.Stained,
            EffectAmount = (byte) 255,
            ShouldAddIfPresent = true,
            Time = 15f
          }
        },
        DrinkCallback = new Action<Player>(DrinkCallbacks.uomonero)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "DeadEye"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "I guess we got to pay for our sins",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.DeadEye)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Verstappen",
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "TU TU TURU",
        DrinkEffects = new List<DrinkEffect>()
        {
          new DrinkEffect()
          {
            EffectAmount = (byte) 175,
            EffectType = (EffectType) 22,
            Time = 10f,
            ShouldAddIfPresent = true
          },
          new DrinkEffect()
          {
            EffectAmount = (byte) 1,
            EffectType = (EffectType) 40,
            Time = 10f,
            ShouldAddIfPresent = true
          }
        },
        DrinkCallback = new Action<Player>(player => SoundHandler.PlayAudio("verstappen.ogg", 50, true, "RB19", player.Position, 10f, player))
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Juggernog"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "<color=#C91010> you need to feel big and strong</color>\n<color=#FFFFFF>reach for Juggernog tonight!</color>",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Juggernog)
      },
      new CustomDrink()
      {
        DrinkNames = new List<string>()
        {
          "Max ammo"
        },
        AntiColaModel = false,
        BackfireChance = 0.0f,
        DrinkMessage = "<color=#C91010> MAX AMMO!</color>",
        DrinkEffects = new List<DrinkEffect>(),
        DrinkCallback = new Action<Player>(DrinkCallbacks.Maxammo)
      }
    };
    }
}
