namespace BodyQuest.Services;

/// <summary>
/// Extra learning material for each lesson: an illustration, a memory cue, a fun fact,
/// a safe "try it" activity and a one-question check. Looked up by world slug + lesson order.
/// </summary>
public static class LessonExtras
{
    public sealed record Extra(
        string Image, string Remember, string FunFact, string TryIt,
        string Question, string[] Options, int CorrectIndex, string Why);

    private static readonly Dictionary<string, Extra> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cerveau:1"] = new("/images/brain.png",
            "Ton cerveau est le chef d'orchestre de ton corps.",
            "Ton cerveau utilise autant d'énergie qu'une petite ampoule allumée toute la journée.",
            "Ferme les yeux et touche ton nez : c'est ton cerveau qui guide ta main !",
            "Que fait ton cerveau ?",
            new[] { "Il contrôle tout ce que tu fais", "Il digère les aliments", "Il fabrique les os" }, 0,
            "Le cerveau dirige tes pensées, tes mouvements et tes émotions."),
        ["cerveau:2"] = new("/images/neuron.png",
            "Les neurones sont de petits messagers électriques.",
            "Un message nerveux peut voyager plus vite qu'une voiture de course !",
            "Tape des mains puis compte jusqu'à trois : tes neurones ont envoyé le message en un éclair.",
            "Comment s'appellent les minuscules messagers du cerveau ?",
            new[] { "Les neurones", "Les poumons", "Les muscles" }, 0,
            "Les neurones envoient des signaux électriques dans tout ton corps."),

        ["coeur:1"] = new("/images/heart.png",
            "Le cœur est un muscle qui pompe le sang.",
            "Ton cœur est à peu près gros comme ton poing fermé.",
            "Pose ta main sur ta poitrine et sens les battements. Cours sur place 20 secondes, puis recommence !",
            "Que fait ton cœur ?",
            new[] { "Il pompe le sang dans tout le corps", "Il fabrique des idées", "Il goûte les aliments" }, 0,
            "Le cœur est une pompe musclée qui travaille jour et nuit."),
        ["coeur:2"] = new("/images/blood-cell.png",
            "Le sang voyage dans des tuyaux : les vaisseaux sanguins.",
            "Mis bout à bout, les vaisseaux sanguins d'un enfant feraient presque le tour de la Terre !",
            "Regarde l'intérieur de ton poignet : tu peux voir de petits vaisseaux bleutés sous la peau.",
            "Que transporte le sang ?",
            new[] { "L'oxygène et les aliments", "Uniquement de l'eau", "Des sons" }, 0,
            "Le sang livre l'oxygène et les aliments à tout ton corps."),

        ["poumons:1"] = new("/images/lungs.png",
            "Inspire : l'air entre. Expire : l'air sort.",
            "Tu respires environ 20 000 fois par jour sans même y penser.",
            "Pose les mains sur ton ventre et respire doucement : sens-le gonfler comme un ballon.",
            "Que se passe-t-il quand tu inspires ?",
            new[] { "L'air entre dans tes poumons", "L'air sort de tes poumons", "Tes poumons s'arrêtent" }, 0,
            "En inspirant, l'air entre dans tes poumons."),
        ["poumons:2"] = new("/images/lungs.png",
            "Les poumons donnent de l'oxygène au sang.",
            "Si on dépliait les petits sacs de tes poumons, ils couvriraient la moitié d'un terrain de tennis !",
            "Souffle sur une plume ou un morceau de papier léger : tu vois ton souffle travailler.",
            "Quel gaz les poumons donnent-ils au sang ?",
            new[] { "L'oxygène", "Le sucre", "Le sel" }, 0,
            "Les poumons donnent de l'oxygène au sang et reprennent le gaz carbonique."),

        ["digestion:1"] = new("/images/digestive.png",
            "Bouche, œsophage, estomac, intestin : le grand voyage des aliments.",
            "Un repas met plusieurs heures à voyager dans ton corps.",
            "Mâche un petit morceau de pain très longtemps : il devient un peu sucré dans ta bouche !",
            "Par où passent d'abord les aliments ?",
            new[] { "Par la bouche", "Par l'estomac", "Par les poumons" }, 0,
            "Tout commence dans la bouche, puis les aliments descendent dans l'œsophage."),
        ["digestion:2"] = new("/images/digestive.png",
            "L'estomac malaxe les aliments comme une machine à laver.",
            "Ton estomac peut gargouiller même quand il est vide : il te dit qu'il a faim !",
            "Écoute ton ventre quand tu as faim : tu peux entendre ton estomac gargouiller.",
            "Que fait l'estomac des aliments ?",
            new[] { "Il les malaxe en bouillie", "Il les congèle", "Il les fait grandir" }, 0,
            "L'estomac mélange les aliments avec des sucs pour en faire une bouillie."),

        ["os-muscles:1"] = new("/images/skeleton.png",
            "Le squelette soutient ton corps et protège tes organes.",
            "Les bébés ont plus d'os que les adultes : certains se soudent en grandissant !",
            "Touche ton crâne, tes côtes et tes genoux : tu sens tes os sous la peau.",
            "Combien d'os a un adulte ?",
            new[] { "206", "20", "2 000" }, 0,
            "Un adulte a 206 os, qui forment son squelette."),
        ["os-muscles:2"] = new("/images/learning-worlds/body-structure.webp",
            "Les muscles tirent sur les os pour te faire bouger.",
            "Le plus petit muscle du corps se trouve dans ton oreille !",
            "Plie ton bras et touche ton biceps : sens-le durcir quand il se contracte.",
            "Que font les muscles quand ils se contractent ?",
            new[] { "Ils tirent sur les os", "Ils fabriquent du sang", "Ils pensent" }, 0,
            "Les muscles se contractent pour tirer sur les os et te faire bouger."),

        ["cinq-sens:1"] = new("/images/learning-worlds/senses-world.webp",
            "Les yeux voient la lumière, les oreilles captent les sons.",
            "Tes yeux clignent environ 15 fois par minute pour rester propres.",
            "Ferme les yeux 10 secondes et écoute : combien de sons différents entends-tu ?",
            "Quel organe capte les sons ?",
            new[] { "Les oreilles", "Les yeux", "Le nez" }, 0,
            "Tes oreilles captent les sons et les envoient au cerveau."),
        ["cinq-sens:2"] = new("/images/learning-worlds/senses-world.webp",
            "Le nez sent, la langue goûte, la peau touche.",
            "Ta langue reconnaît cinq saveurs : sucré, salé, acide, amer et umami.",
            "Bouche-toi le nez en goûtant un aliment : le goût devient bien plus faible !",
            "Quel sens utilise la peau ?",
            new[] { "Le toucher", "L'odorat", "La vue" }, 0,
            "La peau est l'organe du toucher."),
    };

    public static Extra? For(string slug, int sortOrder) =>
        Items.TryGetValue($"{slug}:{sortOrder}", out var e) ? e : null;
}
