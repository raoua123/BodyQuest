using BodyQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace BodyQuest.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (await db.Modules.AnyAsync()) return;

        db.Modules.AddRange(
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
            });

        db.Badges.AddRange(
            new Badge { Name = "Petit explorateur", Description = "Tu as commencé ton premier module.", Emoji = "🧭" },
            new Badge { Name = "Super savant", Description = "Tu as terminé un module.", Emoji = "🎓" },
            new Badge { Name = "Maître du corps humain", Description = "Tu as terminé les six modules.", Emoji = "🏆" });

        await db.SaveChangesAsync();
    }
}
