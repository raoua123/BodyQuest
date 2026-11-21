using BodyQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace BodyQuest.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        // Worlds are added by slug, so a database created with the first six worlds
        // receives the newer ones on the next start without losing anything.
        var existing = await db.Modules.Select(m => m.Slug).ToListAsync();

        var worlds = new List<BodyModule>
        {
            new BodyModule
            {
                Slug = "cerveau", Title = "Le cerveau", Emoji = "🧠", ColorHex = "#ff8fb1", SortOrder = 1,
                Description = "Cerveau, neurones et système nerveux.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Le chef d'orchestre",
                        Content = "Ton cerveau contrôle tout ce que tu fais : penser, bouger, ressentir et même rêver !" },
                    new Lesson { SortOrder = 2, Title = "Les neurones",
                        Content = "Les neurones sont de minuscules messagers qui envoient des signaux électriques dans tout ton corps." }
                },
                Organs = { new Organ { Name = "Cerveau", SvgId = "organ-brain",
                    Description = "Il pèse environ 1,4 kg et contient des milliards de neurones." } }
            },
            new BodyModule
            {
                Slug = "coeur", Title = "Le cœur", Emoji = "❤️", ColorHex = "#ff5a5f", SortOrder = 2,
                Description = "Battements et circulation sanguine.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Une pompe puissante",
                        Content = "Ton cœur est un muscle qui pompe le sang dans tout ton corps, jour et nuit." },
                    new Lesson { SortOrder = 2, Title = "Le voyage du sang",
                        Content = "Le sang transporte l'oxygène et les aliments grâce à des tuyaux appelés vaisseaux sanguins." }
                },
                Organs = { new Organ { Name = "Cœur", SvgId = "organ-heart",
                    Description = "Il bat environ 100 000 fois par jour." } }
            },
            new BodyModule
            {
                Slug = "poumons", Title = "Les poumons", Emoji = "🫁", ColorHex = "#5ec8e5", SortOrder = 3,
                Description = "Respiration et échanges gazeux.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Respirer",
                        Content = "Quand tu inspires, l'air entre dans tes poumons. Quand tu expires, il ressort." },
                    new Lesson { SortOrder = 2, Title = "L'oxygène",
                        Content = "Les poumons donnent de l'oxygène au sang et reprennent le gaz carbonique dont le corps n'a pas besoin." }
                },
                Organs = { new Organ { Name = "Poumons", SvgId = "organ-lungs",
                    Description = "Ils sont au nombre de deux et se gonflent comme des ballons." } }
            },
            new BodyModule
            {
                Slug = "digestion", Title = "La digestion", Emoji = "🍎", ColorHex = "#ffb347", SortOrder = 4,
                Description = "Parcours des aliments et digestion.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Le grand voyage des aliments",
                        Content = "Les aliments passent par la bouche, l'œsophage, l'estomac puis l'intestin." },
                    new Lesson { SortOrder = 2, Title = "L'estomac",
                        Content = "L'estomac malaxe les aliments avec des sucs pour les transformer en une bouillie." }
                },
                Organs = { new Organ { Name = "Estomac", SvgId = "organ-stomach",
                    Description = "Une poche musclée qui brasse les aliments." } }
            },
            new BodyModule
            {
                Slug = "os-muscles", Title = "Les os et muscles", Emoji = "🦴", ColorHex = "#c9b8a3", SortOrder = 5,
                Description = "Squelette, articulations et mouvements.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Le squelette",
                        Content = "Ton squelette soutient ton corps et protège tes organes. Un adulte a 206 os !" },
                    new Lesson { SortOrder = 2, Title = "Les muscles",
                        Content = "Les muscles se contractent pour tirer sur les os et te faire bouger." }
                },
                Organs = { new Organ { Name = "Squelette", SvgId = "organ-skeleton",
                    Description = "La charpente solide de ton corps." } }
            },
            new BodyModule
            {
                Slug = "cinq-sens", Title = "Les cinq sens", Emoji = "👀", ColorHex = "#8ee08e", SortOrder = 6,
                Description = "Vue, ouïe, odorat, goût et toucher.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Les yeux et les oreilles",
                        Content = "Tes yeux captent la lumière et tes oreilles captent les sons." },
                    new Lesson { SortOrder = 2, Title = "Nez, langue et peau",
                        Content = "Le nez sent, la langue goûte et la peau touche : tous envoient leurs infos au cerveau." }
                },
                Organs = { new Organ { Name = "Yeux", SvgId = "organ-eyes",
                    Description = "Ils transforment la lumière en images pour ton cerveau." } }
            },
            new BodyModule
            {
                Slug = "dents", Title = "Les dents", Emoji = "", ColorHex = "#ffd9a0", SortOrder = 7,
                Description = "Dents, mâchoire et sourire.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Des outils pour croquer",
                        Content = "Tes dents ont chacune un travail : les incisives coupent, les canines déchirent et les molaires écrasent." },
                    new Lesson { SortOrder = 2, Title = "Prendre soin de ses dents",
                        Content = "Brosse-toi les dents deux fois par jour, pendant deux minutes, et mange moins de sucre : tes dents resteront solides." }
                },
                Organs = { new Organ { Name = "Dents", SvgId = "organ-teeth",
                    Description = "Des outils solides pour croquer, couper et écraser." } }
            },
            new BodyModule
            {
                Slug = "peau", Title = "La peau", Emoji = "", ColorHex = "#f4b6a0", SortOrder = 8,
                Description = "Protection, toucher et chaleur.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Un manteau pour ton corps",
                        Content = "Ta peau recouvre tout ton corps. Elle te protège des microbes, de l'eau et des petits chocs." },
                    new Lesson { SortOrder = 2, Title = "Chaud, froid et transpiration",
                        Content = "Quand tu as chaud, ta peau transpire pour te rafraîchir. Elle sent aussi le chaud, le froid et le toucher." }
                },
                Organs = { new Organ { Name = "Peau", SvgId = "organ-skin",
                    Description = "Le manteau souple qui recouvre ton corps." } }
            },
            new BodyModule
            {
                Slug = "defenses", Title = "Les défenseurs du corps", Emoji = "", ColorHex = "#b9c9ff", SortOrder = 9,
                Description = "Microbes, globules blancs et défenses du corps.",
                Lessons =
                {
                    new Lesson { SortOrder = 1, Title = "Les microbes",
                        Content = "Les microbes sont de tout petits êtres vivants. Certains peuvent nous rendre malades, mais se laver les mains avec du savon les éloigne." },
                    new Lesson { SortOrder = 2, Title = "Les globules blancs",
                        Content = "Dans ton sang, les globules blancs sont de petits soldats : ils repèrent les microbes et les combattent. Les vaccins leur apprennent à les reconnaître." }
                },
                Organs = { new Organ { Name = "Globules blancs", SvgId = "organ-defense",
                    Description = "De petits soldats qui protègent ton sang." } }
            }
        };

        db.Modules.AddRange(worlds.Where(w => !existing.Contains(w.Slug)));

        if (!await db.Badges.AnyAsync())
            db.Badges.AddRange(
            new Badge { Name = "Petit explorateur", Description = "Tu as commencé ton premier module.", Emoji = "🧭" },
            new Badge { Name = "Super savant", Description = "Tu as terminé un module.", Emoji = "🎓" },
            new Badge { Name = "Maître du corps humain", Description = "Tu as terminé les six modules.", Emoji = "🏆" });

        await db.SaveChangesAsync();
    }
}
