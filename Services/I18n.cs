using BodyQuest.Models;

namespace BodyQuest.Services;

public static class I18n
{
    private static int Ix(string lang) => lang == "en" ? 1 : lang == "ar" ? 2 : 0;

    // title, description, tagline: each packed as fr|en|ar
    private static readonly Dictionary<string, (string T, string D, string G)> Worlds = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cerveau"] = ("Le cerveau|The Brain|الدماغ", "Cerveau, neurones et système nerveux.|Brain, neurons and the nervous system.|الدماغ والخلايا العصبية والجهاز العصبي.", "Le centre de commande de ton corps|The control centre of your body|مركز التحكم في جسمك"),
        ["coeur"] = ("Le cœur|The Heart|القلب", "Battements et circulation sanguine.|Heartbeats and blood circulation.|النبضات والدورة الدموية.", "La pompe qui ne s'arrête jamais|The pump that never stops|المضخة التي لا تتوقف أبدًا"),
        ["poumons"] = ("Les poumons|The Lungs|الرئتان", "Respiration et échanges gazeux.|Breathing and gas exchange.|التنفس وتبادل الغازات.", "Inspire, expire|Breathe in, breathe out|شهيق وزفير"),
        ["digestion"] = ("La digestion|Digestion|الهضم", "Parcours des aliments et digestion.|The journey of food and digestion.|رحلة الطعام والهضم.", "De la première bouchée à l'énergie|From the first bite to energy|من اللقمة الأولى إلى الطاقة"),
        ["os-muscles"] = ("Les os et muscles|Bones and Muscles|العظام والعضلات", "Squelette, articulations et mouvements.|Skeleton, joints and movement.|الهيكل العظمي والمفاصل والحركة.", "Des os solides, des muscles puissants|Strong bones, mighty muscles|عظام قوية وعضلات مفتولة"),
        ["cinq-sens"] = ("Les cinq sens|The Five Senses|الحواس الخمس", "Vue, ouïe, odorat, goût et toucher.|Sight, hearing, smell, taste and touch.|البصر والسمع والشم والتذوق واللمس.", "Voir, entendre, sentir, goûter et toucher|See, hear, smell, taste and touch|أن ترى وتسمع وتشمّ وتتذوق وتلمس"),
        ["dents"] = ("Les dents|The Teeth|الأسنان", "Dents, mâchoire et sourire.|Teeth, jaw and smile.|الأسنان والفكّ والابتسامة.", "De petits outils pour croquer et sourire|Tiny tools for biting and smiling|أدوات صغيرة للقضم والابتسام"),
        ["peau"] = ("La peau|The Skin|الجلد", "Protection, toucher et chaleur.|Protection, touch and temperature.|الحماية واللمس والحرارة.", "Ton manteau qui te protège|Your coat that protects you|معطفك الذي يحميك"),
        ["defenses"] = ("Les défenseurs du corps|Body Defenders|حماة الجسم", "Microbes, globules blancs et défenses du corps.|Germs, white blood cells and the body's defences.|الجراثيم وكريات الدم البيضاء ودفاعات الجسم.", "Une armée minuscule qui veille sur toi|A tiny army that watches over you|جيش صغير يسهر على حمايتك"),
    };

    public static string WorldTitle(BodyModule m, string lang) =>
        lang != "fr" && Worlds.TryGetValue(m.Slug, out var w) ? w.T.Split('|')[Ix(lang)] : m.Title;

    public static string WorldDesc(BodyModule m, string lang) =>
        lang != "fr" && Worlds.TryGetValue(m.Slug, out var w) ? w.D.Split('|')[Ix(lang)] : m.Description;

    public static string Tagline(string slug, string lang) =>
        Worlds.TryGetValue(slug, out var w) ? w.G.Split('|')[Ix(lang)] : WorldArt.For(slug).Tagline;

    // ---- lesson title ~ content : [en, ar] ----
    private static readonly Dictionary<string, string[]> Lessons = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cerveau:1"] = new[] { "The Orchestra Conductor~Your brain controls everything you do: thinking, moving, feeling and even dreaming!", "قائد الأوركسترا~دماغك يتحكم في كل ما تفعله: التفكير والحركة والشعور وحتى الحلم!" },
        ["cerveau:2"] = new[] { "The Neurons~Neurons are tiny messengers that send electrical signals all through your body.", "الخلايا العصبية~الخلايا العصبية رسل صغيرة ترسل إشارات كهربائية في أنحاء جسمك." },
        ["coeur:1"] = new[] { "A Powerful Pump~Your heart is a muscle that pumps blood around your body, day and night.", "مضخة قوية~قلبك عضلة تضخ الدم في جسمك كله، ليلًا ونهارًا." },
        ["coeur:2"] = new[] { "The Journey of Blood~Blood carries oxygen and food through tubes called blood vessels.", "رحلة الدم~ينقل الدم الأكسجين والغذاء عبر أنابيب تسمى الأوعية الدموية." },
        ["poumons:1"] = new[] { "Breathing~When you breathe in, air goes into your lungs. When you breathe out, it comes back out.", "التنفس~عندما تشهق يدخل الهواء إلى رئتيك، وعندما تزفر يخرج منهما." },
        ["poumons:2"] = new[] { "Oxygen~The lungs give oxygen to the blood and take back the carbon dioxide your body does not need.", "الأكسجين~تعطي الرئتان الأكسجين للدم وتأخذان ثاني أكسيد الكربون الذي لا يحتاجه الجسم." },
        ["digestion:1"] = new[] { "The Great Journey of Food~Food passes through the mouth, the oesophagus, the stomach and then the intestine.", "الرحلة الكبرى للطعام~يمر الطعام عبر الفم ثم المريء ثم المعدة ثم الأمعاء." },
        ["digestion:2"] = new[] { "The Stomach~The stomach churns food with juices to turn it into a thick mush.", "المعدة~تخضّ المعدة الطعام مع العصارات لتحوّله إلى عجينة سائلة." },
        ["os-muscles:1"] = new[] { "The Skeleton~Your skeleton supports your body and protects your organs. An adult has 206 bones!", "الهيكل العظمي~يسند هيكلك العظمي جسمك ويحمي أعضاءك. للشخص البالغ 206 عظمات!" },
        ["os-muscles:2"] = new[] { "The Muscles~Muscles contract to pull on your bones and make you move.", "العضلات~تنقبض العضلات لتشدّ العظام وتجعلك تتحرك." },
        ["cinq-sens:1"] = new[] { "Eyes and Ears~Your eyes catch light and your ears catch sounds.", "العينان والأذنان~تلتقط عيناك الضوء وتلتقط أذناك الأصوات." },
        ["cinq-sens:2"] = new[] { "Nose, Tongue and Skin~The nose smells, the tongue tastes and the skin feels touch: they all send their information to the brain.", "الأنف واللسان والجلد~يشمّ الأنف ويتذوق اللسان ويلمس الجلد: وكلها ترسل معلوماتها إلى الدماغ." },
        ["dents:1"] = new[] { "Tools for Biting~Each tooth has a job: incisors cut, canines tear and molars crush.", "أدوات للقضم~لكل سنّ وظيفة: القواطع تقطع والأنياب تمزّق والأضراس تطحن." },
        ["dents:2"] = new[] { "Looking After Your Teeth~Brush your teeth twice a day for two minutes and eat less sugar to keep your teeth strong.", "العناية بأسنانك~نظّف أسنانك مرتين في اليوم لمدة دقيقتين، وقلّل من السكر لتبقى أسنانك قوية." },
        ["peau:1"] = new[] { "A Coat for Your Body~Your skin covers your whole body. It protects you from germs, water and little bumps.", "معطف لجسمك~يغطي جلدك جسمك كله. يحميك من الجراثيم والماء والصدمات الصغيرة." },
        ["peau:2"] = new[] { "Warm, Cold and Sweat~When you are hot, your skin sweats to cool you down. It also feels heat, cold and touch.", "الدفء والبرد والعرق~عندما تشعر بالحرّ يفرز جلدك العرق ليبرّدك. وهو يحسّ أيضًا بالحرارة والبرودة واللمس." },
        ["defenses:1"] = new[] { "Germs~Germs are tiny living things. Some can make us ill, but washing your hands with soap keeps them away.", "الجراثيم~الجراثيم كائنات حية صغيرة جدًا. بعضها قد يُمرضنا، لكن غسل اليدين بالصابون يُبعدها." },
        ["defenses:2"] = new[] { "The White Blood Cells~In your blood, white blood cells are little soldiers: they spot germs and fight them. Vaccines teach them to recognise germs.", "كريات الدم البيضاء~في دمك تعمل كريات الدم البيضاء كجنود صغار: تكتشف الجراثيم وتحاربها. وتعلّمها اللقاحات كيف تتعرّف عليها." },
    };

    private static string? Lx(string slug, int order, string lang, int part) =>
        lang != "fr" && Lessons.TryGetValue($"{slug}:{order}", out var v)
            ? v[lang == "ar" ? 1 : 0].Split('~')[part] : null;

    public static string LessonTitle(string slug, Lesson l, string lang) => Lx(slug, l.SortOrder, lang, 0) ?? l.Title;
    public static string LessonText(string slug, Lesson l, string lang) => Lx(slug, l.SortOrder, lang, 1) ?? l.Content;

    // ---- remember ~ fun fact ~ try it ~ question ~ opt1 ~ opt2 ~ opt3 ~ why : [en, ar] (option 1 is always the right one) ----
    private static readonly Dictionary<string, string[]> Extras = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cerveau:1"] = new[] {
            "Your brain is the conductor of your body.~Your brain uses as much energy as a small light bulb left on all day.~Close your eyes and touch your nose: your brain guides your hand!~What does your brain do?~It controls everything you do~It digests food~It makes bones~The brain directs your thoughts, movements and feelings.",
            "دماغك هو قائد أوركسترا جسمك.~يستهلك دماغك طاقة تعادل مصباحًا صغيرًا مضاءً طوال اليوم.~أغمض عينيك والمس أنفك: دماغك هو الذي يوجّه يدك!~ماذا يفعل دماغك؟~يتحكم في كل ما تفعله~يهضم الطعام~يصنع العظام~يوجّه الدماغ أفكارك وحركاتك ومشاعرك." },
        ["cerveau:2"] = new[] {
            "Neurons are tiny electric messengers.~A nerve message can travel faster than a race car!~Clap your hands then count to three: your neurons sent the message in a flash.~What are the brain's tiny messengers called?~Neurons~Lungs~Muscles~Neurons send electrical signals all through your body.",
            "الخلايا العصبية رسل كهربائية صغيرة.~يمكن لرسالة عصبية أن تنتقل أسرع من سيارة سباق!~صفّق بيديك ثم عُدّ إلى ثلاثة: لقد أرسلت خلاياك العصبية الرسالة في لمح البصر.~ماذا تسمى رسل الدماغ الصغيرة؟~الخلايا العصبية~الرئتان~العضلات~ترسل الخلايا العصبية إشارات كهربائية في كل جسمك." },
        ["coeur:1"] = new[] {
            "The heart is a muscle that pumps blood.~Your heart is about the size of your closed fist.~Put your hand on your chest and feel the beats. Jog on the spot for 20 seconds, then try again!~What does your heart do?~It pumps blood around the body~It makes ideas~It tastes food~The heart is a muscular pump that works day and night.",
            "القلب عضلة تضخ الدم.~قلبك بحجم قبضة يدك المغلقة تقريبًا.~ضع يدك على صدرك وأحسّ بالنبضات. اركض في مكانك 20 ثانية ثم جرّب مرة أخرى!~ماذا يفعل قلبك؟~يضخ الدم في الجسم كله~يصنع الأفكار~يتذوق الطعام~القلب مضخة عضلية تعمل ليلًا ونهارًا." },
        ["coeur:2"] = new[] {
            "Blood travels in tubes called blood vessels.~Laid end to end, a child's blood vessels would almost go around the Earth!~Look at the inside of your wrist: you can see tiny bluish vessels under the skin.~What does blood carry?~Oxygen and food~Only water~Sounds~Blood delivers oxygen and food to your whole body.",
            "ينتقل الدم عبر أنابيب تسمى الأوعية الدموية.~لو وُضعت أوعية طفل الدموية طرفًا إلى طرف لكادت تلتف حول الأرض!~انظر إلى باطن معصمك: ترى أوعية صغيرة مزرقّة تحت الجلد.~ماذا ينقل الدم؟~الأكسجين والغذاء~الماء فقط~الأصوات~يوصل الدم الأكسجين والغذاء إلى كل جسمك." },
        ["poumons:1"] = new[] {
            "Breathe in: air comes in. Breathe out: air goes out.~You breathe about 20,000 times a day without even thinking about it.~Put your hands on your belly and breathe slowly: feel it swell like a balloon.~What happens when you breathe in?~Air goes into your lungs~Air leaves your lungs~Your lungs stop~When you breathe in, air goes into your lungs.",
            "شهيق: يدخل الهواء. زفير: يخرج الهواء.~تتنفس نحو 20 ألف مرة يوميًا دون أن تفكر في ذلك.~ضع يديك على بطنك وتنفّس ببطء: أحسّ به ينتفخ كالبالون.~ماذا يحدث عندما تشهق؟~يدخل الهواء إلى رئتيك~يخرج الهواء من رئتيك~تتوقف رئتاك~عندما تشهق يدخل الهواء إلى رئتيك." },
        ["poumons:2"] = new[] {
            "The lungs give oxygen to the blood.~If you unfolded the tiny sacs in your lungs, they would cover half a tennis court!~Blow on a feather or a light piece of paper: you can see your breath at work.~Which gas do the lungs give to the blood?~Oxygen~Sugar~Salt~The lungs give oxygen to the blood and take back the carbon dioxide.",
            "تعطي الرئتان الأكسجين للدم.~لو فردنا الأكياس الصغيرة في رئتيك لغطّت نصف ملعب تنس!~انفخ على ريشة أو قطعة ورق خفيفة: سترى نَفَسك وهو يعمل.~أي غاز تعطيه الرئتان للدم؟~الأكسجين~السكر~الملح~تعطي الرئتان الأكسجين للدم وتأخذان ثاني أكسيد الكربون." },
        ["digestion:1"] = new[] {
            "Mouth, oesophagus, stomach, intestine: the great journey of food.~A meal takes several hours to travel through your body.~Chew a small piece of bread for a very long time: it becomes a little sweet in your mouth!~Where does food go first?~Through the mouth~Through the stomach~Through the lungs~It all starts in the mouth, then the food goes down the oesophagus.",
            "الفم، المريء، المعدة، الأمعاء: الرحلة الكبرى للطعام.~تستغرق الوجبة عدة ساعات لتنتقل عبر جسمك.~امضغ قطعة صغيرة من الخبز مدة طويلة جدًا: ستصبح حلوة قليلًا في فمك!~من أين يمر الطعام أولًا؟~من الفم~من المعدة~من الرئتين~يبدأ كل شيء في الفم، ثم ينزل الطعام في المريء." },
        ["digestion:2"] = new[] {
            "The stomach churns food like a washing machine.~Your stomach can rumble even when it is empty: it is telling you it is hungry!~Listen to your tummy when you are hungry: you can hear your stomach rumble.~What does the stomach do to food?~It churns it into a mush~It freezes it~It makes it grow~The stomach mixes food with juices to make a mush.",
            "تخضّ المعدة الطعام كأنها غسّالة.~قد تقرقر معدتك حتى وهي فارغة: إنها تخبرك أنها جائعة!~أنصت إلى بطنك عندما تجوع: تسمع معدتك تقرقر.~ماذا تفعل المعدة بالطعام؟~تخضّه فيصير عجينة سائلة~تجمّده~تجعله ينمو~تخلط المعدة الطعام بالعصارات لتصنع منه عجينة سائلة." },
        ["os-muscles:1"] = new[] {
            "The skeleton supports your body and protects your organs.~Babies have more bones than adults: some join together as they grow!~Touch your skull, your ribs and your knees: you can feel your bones under the skin.~How many bones does an adult have?~206~20~2,000~An adult has 206 bones, which form the skeleton.",
            "الهيكل العظمي يسند جسمك ويحمي أعضاءك.~للرضّع عظام أكثر من البالغين: فبعضها يلتحم مع النمو!~المس جمجمتك وأضلاعك وركبتيك: ستحسّ بعظامك تحت الجلد.~كم عظمة لدى الشخص البالغ؟~206~20~2000~للشخص البالغ 206 عظمات تشكّل الهيكل العظمي." },
        ["os-muscles:2"] = new[] {
            "Muscles pull on bones to make you move.~The smallest muscle in the body is in your ear!~Bend your arm and touch your biceps: feel it harden as it contracts.~What do muscles do when they contract?~They pull on bones~They make blood~They think~Muscles contract to pull on bones and make you move.",
            "تشدّ العضلات العظام لتجعلك تتحرك.~أصغر عضلة في الجسم موجودة في أذنك!~اثنِ ذراعك والمس العضلة ذات الرأسين: أحسّها تتصلّب عندما تنقبض.~ماذا تفعل العضلات عندما تنقبض؟~تشدّ العظام~تصنع الدم~تفكّر~تنقبض العضلات لتشدّ العظام وتجعلك تتحرك." },
        ["cinq-sens:1"] = new[] {
            "Eyes see light, ears catch sounds.~Your eyes blink about 15 times a minute to stay clean.~Close your eyes for 10 seconds and listen: how many different sounds can you hear?~Which organ catches sounds?~The ears~The eyes~The nose~Your ears catch sounds and send them to the brain.",
            "ترى العينان الضوء وتلتقط الأذنان الأصوات.~ترمش عيناك نحو 15 مرة في الدقيقة لتبقيا نظيفتين.~أغمض عينيك 10 ثوانٍ وأنصت: كم صوتًا مختلفًا تسمع؟~أي عضو يلتقط الأصوات؟~الأذنان~العينان~الأنف~تلتقط أذناك الأصوات وترسلها إلى الدماغ." },
        ["cinq-sens:2"] = new[] {
            "The nose smells, the tongue tastes, the skin feels.~Your tongue knows five tastes: sweet, salty, sour, bitter and umami.~Hold your nose while tasting some food: the taste becomes much weaker!~Which sense does the skin use?~Touch~Smell~Sight~The skin is the organ of touch.",
            "يشمّ الأنف ويتذوق اللسان ويلمس الجلد.~يتعرف لسانك على خمسة أطعمة: الحلو والمالح والحامض والمرّ والأومامي.~سُدّ أنفك وأنت تتذوق طعامًا: سيصبح الطعم أضعف بكثير!~أي حاسة يستخدمها الجلد؟~اللمس~الشم~البصر~الجلد هو عضو اللمس." },
        ["dents:1"] = new[] {
            "Incisors cut, canines tear, molars crush.~The enamel covering your teeth is the hardest material in your body.~Run your tongue over your front teeth, then your back teeth: feel the different shapes.~Which teeth crush food?~The molars~The incisors~The canines~The molars at the back of your mouth crush food.",
            "القواطع تقطع والأنياب تمزّق والأضراس تطحن.~المينا التي تغطي أسنانك هي أصلب مادة في جسمك.~مرّر لسانك على أسنانك الأمامية ثم على الخلفية: أحسّ باختلاف أشكالها.~أي الأسنان تطحن الطعام؟~الأضراس~القواطع~الأنياب~الأضراس في آخر الفم تطحن الطعام." },
        ["dents:2"] = new[] {
            "Brush for two minutes, twice a day.~Baby teeth fall out to make room for adult teeth, which must last a lifetime.~Set a two-minute timer the next time you brush your teeth.~How long should you brush your teeth?~Two minutes~Ten seconds~One hour~Two minutes, morning and evening, removes leftover food and protects your teeth.",
            "نظّف أسنانك دقيقتين، مرتين في اليوم.~تسقط الأسنان اللبنية لتفسح المجال لأسنان الكبار التي يجب أن تدوم طوال العمر.~اضبط مؤقّتًا على دقيقتين في المرة القادمة التي تنظّف فيها أسنانك.~كم من الوقت يجب أن تنظّف أسنانك؟~دقيقتان~عشر ثوانٍ~ساعة~دقيقتان صباحًا ومساءً تزيلان بقايا الطعام وتحميان أسنانك." },
        ["peau:1"] = new[] {
            "The skin is the biggest organ in your body.~Your skin renews itself all the time: every day tiny skin cells fall off and new ones take their place.~Look closely at the back of your hand: you can see tiny lines and little hairs.~What does your skin do for your body?~It protects it~It makes blood~It thinks for you~Skin protects your body from germs, water and bumps.",
            "الجلد هو أكبر عضو في جسمك.~يتجدد جلدك باستمرار: كل يوم تسقط خلايا جلدية صغيرة جدًا وتحلّ محلها خلايا جديدة.~انظر عن قرب إلى ظهر يدك: ترى خطوطًا دقيقة وشعيرات صغيرة.~ماذا يفعل جلدك لجسمك؟~يحميه~يصنع الدم~يفكّر بدلًا منك~يحمي الجلد جسمك من الجراثيم والماء والصدمات." },
        ["peau:2"] = new[] {
            "When you are hot, your skin sweats to cool you down.~Your skin has millions of tiny sensors that feel touch, heat and cold.~Put your hand on a glass of cool water, then on your cheek: feel the difference in temperature.~Why does your skin sweat when you are hot?~To cool you down~To make you grow~To make you hungry~Sweat dries on the skin and cools your body.",
            "عندما تشعر بالحرّ يفرز جلدك العرق ليبرّدك.~في جلدك ملايين المستشعرات الصغيرة التي تحسّ باللمس والحرارة والبرودة.~ضع يدك على كأس ماء بارد ثم على خدّك: أحسّ باختلاف الحرارة.~لماذا يتعرّق جلدك عندما تشعر بالحرّ؟~ليبرّدك~ليجعلك تكبر~ليجعلك تشعر بالجوع~يتبخّر العرق عن الجلد فيبرّد جسمك." },
        ["defenses:1"] = new[] {
            "Washing your hands with soap keeps germs away.~There are germs everywhere, but most are harmless and some even help your tummy.~Wash your hands with soap while counting to twenty: rub well between your fingers.~How can you get germs off your hands?~With water and soap~By shaking them~By hiding them~Water and soap lift germs off and wash them away.",
            "غسل اليدين بالصابون يُبعد الجراثيم.~الجراثيم موجودة في كل مكان، لكن معظمها غير مؤذٍ وبعضها يساعد بطنك أيضًا.~اغسل يديك بالصابون وعُدّ إلى عشرين: افرك جيدًا بين أصابعك.~كيف تُزيل الجراثيم عن يديك؟~بالماء والصابون~بنفضهما~بإخفائهما~يزيل الماء والصابون الجراثيم ويجرفانها بعيدًا." },
        ["defenses:2"] = new[] {
            "White blood cells are the little soldiers of your blood.~Sleeping well helps keep your defences strong.~Draw a white blood cell as a superhero and explain to someone how it protects your body.~What do white blood cells do?~They fight germs~They help you breathe~They make bones~White blood cells spot germs and destroy them.",
            "كريات الدم البيضاء هي الجنود الصغار في دمك.~النوم الجيد يساعد دفاعات جسمك على البقاء قوية.~ارسم كرية دم بيضاء على هيئة بطل خارق واشرح لشخص ما كيف تحمي جسمك.~ماذا تفعل كريات الدم البيضاء؟~تحارب الجراثيم~تساعدك على التنفس~تصنع العظام~تكتشف كريات الدم البيضاء الجراثيم وتقضي عليها." },
    };

    /// <summary>Same as LessonExtras.For, but with the text in the chosen language (image and right answer stay the same).</summary>
    public static LessonExtras.Extra? Extra(string slug, int order, string lang)
    {
        var fr = LessonExtras.For(slug, order);
        if (fr is null || lang == "fr" || !Extras.TryGetValue($"{slug}:{order}", out var v)) return fr;
        var p = v[lang == "ar" ? 1 : 0].Split('~');
        return fr with
        {
            Remember = p[0],
            FunFact = p[1],
            TryIt = p[2],
            Question = p[3],
            Options = new[] { p[4], p[5], p[6] },
            Why = p[7]
        };
    }

    // ---- Milo's facts on the map ----
    private static readonly string[] FactsFr =
    {
        "Ton cœur bat environ 100 000 fois par jour !", "Ton cerveau contient des milliards de petits neurones.",
        "Tu as 206 os quand tu seras grand.", "Tes poumons travaillent toute la journée, même quand tu dors.",
        "Ta langue a des milliers de petits bourgeons du goût.", "Ton estomac malaxe les aliments comme une machine à laver !",
        "La peau est le plus grand organe de ton corps.", "Le sang fait le tour de ton corps en environ une minute."
    };
    private static readonly string[] FactsEn =
    {
        "Your heart beats about 100,000 times every day!", "Your brain has billions of tiny neurons.",
        "You have 206 bones when you grow up.", "Your lungs work all day, even while you sleep.",
        "Your tongue has thousands of tiny taste buds.", "Your stomach squishes food like a washing machine!",
        "Your skin is the biggest organ you have.", "Blood travels all around your body in about one minute."
    };
    private static readonly string[] FactsAr =
    {
        "يخفق قلبك نحو 100 ألف مرة كل يوم!", "في دماغك مليارات الخلايا العصبية الصغيرة.",
        "لديك 206 عظمات عندما تكبر.", "تعمل رئتاك طوال اليوم، حتى وأنت نائم.",
        "في لسانك آلاف البراعم الذوقية الصغيرة.", "تخضّ معدتك الطعام كأنها غسّالة!",
        "الجلد هو أكبر عضو في جسمك.", "يدور الدم في جسمك كله في نحو دقيقة واحدة."
    };

    public static string[] Facts(string lang) => lang == "en" ? FactsEn : lang == "ar" ? FactsAr : FactsFr;
}