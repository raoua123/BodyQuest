namespace BodyQuest.Services;

/// <summary>
/// The quick-check questions of every lesson (3 per lesson).
/// The first question of each lesson comes from LessonExtras / I18n.Extras; the extra ones live here.
/// </summary>
public static class QuizBank
{
    public sealed record Question(string Text, string[] Options, int CorrectIndex, string Why);

    // key "world:lesson" -> [french, english, arabic]; questions separated by "||";
    // each question is  question~right answer~wrong 1~wrong 2~why  (the right answer is always written first).
    private static readonly Dictionary<string, string[]> More = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cerveau:1"] = new[]
        {
            "Où se trouve ton cerveau ?~Dans ta tête~Dans ton pied~Dans ton ventre~Ton cerveau est bien protégé dans ta tête par le crâne.||Que peux-tu faire grâce à ton cerveau ?~Penser, rêver et te souvenir~Seulement dormir~Rien du tout~Le cerveau te permet de penser, de rêver, de te souvenir et de ressentir.",
            "Where is your brain?~In your head~In your foot~In your tummy~Your brain is safely protected inside your head by the skull.||What can you do thanks to your brain?~Think, dream and remember~Only sleep~Nothing at all~Your brain lets you think, dream, remember and feel.",
            "أين يوجد دماغك؟~في رأسك~في قدمك~في بطنك~دماغك محميّ جيدًا داخل رأسك بواسطة الجمجمة.||ماذا تستطيع أن تفعل بفضل دماغك؟~أن تفكّر وتحلم وتتذكّر~أن تنام فقط~لا شيء على الإطلاق~يتيح لك دماغك أن تفكّر وتحلم وتتذكّر وتشعر."
        },
        ["cerveau:2"] = new[]
        {
            "Que transportent les neurones ?~Des messages~Du sang~De l'air~Les neurones transportent des messages électriques.||Les neurones envoient des messages…~Dans tout le corps~Seulement dans le pied~Nulle part~Les neurones relient ton cerveau à tout ton corps.",
            "What do neurons carry?~Messages~Blood~Air~Neurons carry electrical messages.||Neurons send messages…~All through the body~Only to the foot~Nowhere~Neurons connect your brain to your whole body.",
            "ماذا تنقل الخلايا العصبية؟~رسائل~الدم~الهواء~تنقل الخلايا العصبية رسائل كهربائية.||ترسل الخلايا العصبية الرسائل…~في كل أنحاء الجسم~إلى القدم فقط~إلى لا مكان~تربط الخلايا العصبية دماغك بجسمك كله."
        },
        ["coeur:1"] = new[]
        {
            "Quand tu cours, ton cœur bat…~Plus vite~Moins vite~Il s'arrête~Ton cœur accélère pour envoyer plus d'oxygène à tes muscles.||Le cœur est un…~Muscle~Os~Poumon~Le cœur est un muscle qui se serre et se relâche pour pomper le sang.",
            "When you run, your heart beats…~Faster~Slower~It stops~Your heart speeds up to send more oxygen to your muscles.||The heart is a…~Muscle~Bone~Lung~The heart is a muscle that squeezes and relaxes to pump blood.",
            "عندما تركض يخفق قلبك…~أسرع~أبطأ~يتوقف~يسرّع قلبك خفقاته ليرسل مزيدًا من الأكسجين إلى عضلاتك.||القلب هو…~عضلة~عظمة~رئة~القلب عضلة تنقبض وتنبسط لتضخّ الدم."
        },
        ["coeur:2"] = new[]
        {
            "Comment s'appellent les tuyaux où circule le sang ?~Les vaisseaux sanguins~Les os~Les dents~Le sang circule dans les vaisseaux sanguins.||Où le sang reçoit-il l'oxygène ?~Dans les poumons~Dans les oreilles~Dans les cheveux~Le sang passe dans les poumons pour y prendre de l'oxygène.",
            "What are the tubes that carry blood called?~Blood vessels~Bones~Teeth~Blood travels through blood vessels.||Where does blood pick up oxygen?~In the lungs~In the ears~In the hair~Blood passes through the lungs to pick up oxygen.",
            "ماذا تسمى الأنابيب التي يجري فيها الدم؟~الأوعية الدموية~العظام~الأسنان~يجري الدم في الأوعية الدموية.||أين يأخذ الدم الأكسجين؟~في الرئتين~في الأذنين~في الشعر~يمرّ الدم في الرئتين ليأخذ الأكسجين."
        },
        ["poumons:1"] = new[]
        {
            "Que se passe-t-il quand tu expires ?~L'air sort de tes poumons~L'air entre dans tes poumons~Tu avales ton repas~Quand tu expires, l'air ressort de tes poumons.||Par où l'air entre-t-il dans ton corps ?~Par le nez et la bouche~Par les oreilles~Par les yeux~L'air entre par le nez ou la bouche, puis descend vers les poumons.",
            "What happens when you breathe out?~Air leaves your lungs~Air goes into your lungs~You swallow your lunch~When you breathe out, air leaves your lungs.||Where does air enter your body?~Through the nose and mouth~Through the ears~Through the eyes~Air comes in through the nose or mouth, then goes down to the lungs.",
            "ماذا يحدث عندما تزفر؟~يخرج الهواء من رئتيك~يدخل الهواء إلى رئتيك~تبتلع وجبتك~عندما تزفر يخرج الهواء من رئتيك.||من أين يدخل الهواء إلى جسمك؟~من الأنف والفم~من الأذنين~من العينين~يدخل الهواء من الأنف أو الفم ثم ينزل إلى الرئتين."
        },
        ["poumons:2"] = new[]
        {
            "Que rapporte le sang aux poumons pour être rejeté ?~Le gaz carbonique~Les aliments~Les os~Le sang rapporte le gaz carbonique aux poumons, qui le rejettent quand tu expires.||Qui transporte l'oxygène dans tout le corps ?~Le sang~Les cheveux~Les ongles~Le sang transporte l'oxygène des poumons jusqu'à tout ton corps.",
            "What does blood bring back to the lungs to be breathed out?~Carbon dioxide~Food~Bones~Blood brings carbon dioxide back to the lungs, and you breathe it out.||What carries oxygen around the whole body?~Blood~Hair~Nails~Blood carries oxygen from the lungs to your whole body.",
            "ماذا يعيد الدم إلى الرئتين ليُطرَد؟~ثاني أكسيد الكربون~الطعام~العظام~يعيد الدم ثاني أكسيد الكربون إلى الرئتين فتطردانه عندما تزفر.||ما الذي ينقل الأكسجين في الجسم كله؟~الدم~الشعر~الأظافر~ينقل الدم الأكسجين من الرئتين إلى جسمك كله."
        },
        ["digestion:1"] = new[]
        {
            "Dans quel ordre passent les aliments ?~Bouche, œsophage, estomac, intestin~Estomac, bouche, intestin, œsophage~Intestin, estomac, bouche, œsophage~Les aliments descendent de la bouche vers l'intestin dans cet ordre.||Qu'est-ce que l'œsophage ?~Un tuyau qui relie la bouche à l'estomac~Un os du bras~Une dent~L'œsophage est le tuyau où glissent les aliments avant d'arriver à l'estomac.",
            "In which order does food travel?~Mouth, oesophagus, stomach, intestine~Stomach, mouth, intestine, oesophagus~Intestine, stomach, mouth, oesophagus~Food goes from the mouth down to the intestine in this order.||What is the oesophagus?~A tube that joins the mouth to the stomach~A bone in the arm~A tooth~The oesophagus is the tube food slides down before it reaches the stomach.",
            "بأي ترتيب يمرّ الطعام؟~الفم، المريء، المعدة، الأمعاء~المعدة، الفم، الأمعاء، المريء~الأمعاء، المعدة، الفم، المريء~ينزل الطعام من الفم إلى الأمعاء بهذا الترتيب.||ما المريء؟~أنبوب يصل الفم بالمعدة~عظمة في الذراع~سنّ~المريء هو الأنبوب الذي ينزلق فيه الطعام قبل أن يصل إلى المعدة."
        },
        ["digestion:2"] = new[]
        {
            "Où vont les aliments après l'estomac ?~Dans l'intestin~Dans le nez~Dans les oreilles~Après l'estomac, les aliments continuent leur voyage dans l'intestin.||À quoi sert l'intestin ?~À récupérer ce qui est bon pour le corps~À faire respirer~À faire penser~Dans l'intestin, ce qui est utile aux aliments passe dans le sang.",
            "Where does food go after the stomach?~Into the intestine~Into the nose~Into the ears~After the stomach, food continues its journey into the intestine.||What is the intestine for?~To take what is good for the body from food~To make you breathe~To make you think~In the intestine, the useful parts of food pass into the blood.",
            "إلى أين يذهب الطعام بعد المعدة؟~إلى الأمعاء~إلى الأنف~إلى الأذنين~بعد المعدة يتابع الطعام رحلته إلى الأمعاء.||ما فائدة الأمعاء؟~تأخذ من الطعام ما ينفع الجسم~تجعلك تتنفس~تجعلك تفكّر~في الأمعاء تنتقل الأجزاء المفيدة من الطعام إلى الدم."
        },
        ["os-muscles:1"] = new[]
        {
            "Que fait le squelette ?~Il soutient ton corps et protège tes organes~Il fabrique l'air~Il goûte les aliments~Le squelette est la charpente de ton corps et il protège tes organes.||Quels os protègent ton cerveau ?~Ceux du crâne~Ceux du pied~Ceux de la main~Le crâne est comme un casque solide pour ton cerveau.",
            "What does the skeleton do?~It supports your body and protects your organs~It makes air~It tastes food~The skeleton is the frame of your body and it protects your organs.||Which bones protect your brain?~The skull~The foot~The hand~The skull is like a strong helmet for your brain.",
            "ماذا يفعل الهيكل العظمي؟~يسند جسمك ويحمي أعضاءك~يصنع الهواء~يتذوق الطعام~الهيكل العظمي هو إطار جسمك وهو يحمي أعضاءك.||أي عظام تحمي دماغك؟~عظام الجمجمة~عظام القدم~عظام اليد~الجمجمة مثل خوذة قوية تحمي دماغك."
        },
        ["os-muscles:2"] = new[]
        {
            "Qu'est-ce qui fait bouger tes os ?~Les muscles~Les cheveux~Les dents~Les muscles tirent sur les os pour te faire bouger.||Comment garder des muscles forts ?~En bougeant et en jouant~En restant couché toute la journée~En ne mangeant jamais~Courir, sauter et jouer rendent tes muscles plus forts.",
            "What makes your bones move?~Muscles~Hair~Teeth~Muscles pull on your bones to make you move.||How can you keep your muscles strong?~By moving and playing~By lying down all day~By never eating~Running, jumping and playing make your muscles stronger.",
            "ما الذي يحرّك عظامك؟~العضلات~الشعر~الأسنان~تشدّ العضلات عظامك لتجعلك تتحرك.||كيف تحافظ على عضلات قوية؟~بالحركة واللعب~بالاستلقاء طوال اليوم~بعدم الأكل أبدًا~الركض والقفز واللعب تجعل عضلاتك أقوى."
        },
        ["cinq-sens:1"] = new[]
        {
            "Quel organe te permet de voir ?~Les yeux~Les oreilles~La langue~Tes yeux captent la lumière et envoient l'image au cerveau.||Qui comprend ce que captent les yeux et les oreilles ?~Le cerveau~Le genou~Le coude~Les sens envoient leurs informations au cerveau, qui les comprend.",
            "Which organ lets you see?~The eyes~The ears~The tongue~Your eyes catch light and send the picture to the brain.||What makes sense of what the eyes and ears catch?~The brain~The knee~The elbow~The senses send their information to the brain, which makes sense of it.",
            "أي عضو يتيح لك أن ترى؟~العينان~الأذنان~اللسان~تلتقط عيناك الضوء وترسل الصورة إلى الدماغ.||ما الذي يفهم ما تلتقطه العينان والأذنان؟~الدماغ~الركبة~المرفق~ترسل الحواس معلوماتها إلى الدماغ الذي يفهمها."
        },
        ["cinq-sens:2"] = new[]
        {
            "Quel organe sent les odeurs ?~Le nez~Le coude~L'oreille~C'est ton nez qui sent les odeurs.||Quel organe te fait goûter les aliments ?~La langue~Le genou~Le pied~La langue reconnaît les saveurs grâce à ses petits bourgeons du goût.",
            "Which organ smells?~The nose~The elbow~The ear~Your nose is the organ that smells.||Which organ lets you taste food?~The tongue~The knee~The foot~The tongue knows flavours thanks to its tiny taste buds.",
            "أي عضو يشمّ الروائح؟~الأنف~المرفق~الأذن~أنفك هو العضو الذي يشمّ الروائح.||أي عضو يتيح لك تذوّق الطعام؟~اللسان~الركبة~القدم~يتعرف اللسان على النكهات بفضل براعمه الذوقية الصغيرة."
        },
        ["dents:1"] = new[]
        {
            "Que font les incisives ?~Elles coupent les aliments~Elles écrasent les aliments~Elles avalent les aliments~Les incisives, devant, coupent comme de petits ciseaux.||Combien de dents de lait a un enfant ?~20~10~50~Un enfant a 20 dents de lait, qui seront remplacées par des dents d'adulte.",
            "What do incisors do?~They cut food~They crush food~They swallow food~Incisors at the front cut like tiny scissors.||How many baby teeth does a child have?~20~10~50~A child has 20 baby teeth, which are later replaced by adult teeth.",
            "ماذا تفعل القواطع؟~تقطع الطعام~تطحن الطعام~تبتلع الطعام~القواطع في الأمام تقطع مثل مقصّات صغيرة.||كم سنًّا لبنية لدى الطفل؟~20~10~50~لدى الطفل 20 سنًّا لبنية تحلّ محلها لاحقًا أسنان الكبار."
        },
        ["dents:2"] = new[]
        {
            "Qu'est-ce qui abîme le plus les dents ?~Trop de bonbons et de boissons sucrées~Boire de l'eau~Manger des légumes~Le sucre aide les microbes de la bouche à abîmer les dents.||Qui t'aide à garder des dents en bonne santé ?~Le dentiste~Le boulanger~Le facteur~Le dentiste vérifie tes dents et te donne des conseils.",
            "What harms teeth the most?~Too many sweets and sugary drinks~Drinking water~Eating vegetables~Sugar helps the germs in your mouth damage your teeth.||Who helps you keep your teeth healthy?~The dentist~The baker~The postman~The dentist checks your teeth and gives you advice.",
            "ما أكثر شيء يضرّ بالأسنان؟~الإكثار من الحلوى والمشروبات السكرية~شرب الماء~أكل الخضروات~يساعد السكر الجراثيم الموجودة في الفم على إتلاف الأسنان.||من يساعدك على إبقاء أسنانك سليمة؟~طبيب الأسنان~الخبّاز~ساعي البريد~يفحص طبيب الأسنان أسنانك ويعطيك النصائح."
        },
        ["peau:1"] = new[]
        {
            "Quel est le plus grand organe du corps ?~La peau~Le cœur~Le nez~La peau recouvre tout ton corps : c'est le plus grand de tes organes.||Que fait la peau quand tu as une petite coupure ?~Elle se répare toute seule~Elle disparaît~Elle devient un os~La peau sait se réparer : une petite croûte se forme puis tombe quand c'est guéri.",
            "What is the biggest organ in the body?~The skin~The heart~The nose~Skin covers your whole body: it is your biggest organ.||What does skin do when you get a small cut?~It heals by itself~It disappears~It turns into a bone~Skin can repair itself: a little scab forms and falls off once it has healed.",
            "ما أكبر عضو في الجسم؟~الجلد~القلب~الأنف~يغطي الجلد جسمك كله: إنه أكبر أعضائك.||ماذا يفعل الجلد عندما يُجرح جرحًا صغيرًا؟~يلتئم من تلقاء نفسه~يختفي~يتحول إلى عظمة~يعرف الجلد كيف يُصلح نفسه: تتكوّن قشرة صغيرة ثم تسقط عندما يلتئم الجرح."
        },
        ["peau:2"] = new[]
        {
            "Que mettre sur la peau quand il y a beaucoup de soleil ?~De la crème solaire~Rien du tout~De la peinture~La crème solaire et un chapeau protègent ta peau des coups de soleil.||Qu'est-ce que ta peau peut sentir ?~Le chaud, le froid et le toucher~Seulement les odeurs~Seulement les sons~Ta peau sent le chaud, le froid, le toucher et même la douleur.",
            "What should you put on your skin when it is very sunny?~Sunscreen~Nothing at all~Paint~Sunscreen and a hat protect your skin from sunburn.||What can your skin feel?~Heat, cold and touch~Only smells~Only sounds~Your skin feels heat, cold, touch and even pain.",
            "ماذا تضع على جلدك عندما تكون الشمس شديدة؟~واقيًا من الشمس~لا شيء على الإطلاق~طلاءً~الواقي من الشمس والقبعة يحميان جلدك من حروق الشمس.||ماذا يستطيع جلدك أن يحسّ؟~الحرارة والبرودة واللمس~الروائح فقط~الأصوات فقط~يحسّ جلدك بالحرارة والبرودة واللمس وحتى بالألم."
        },
        ["defenses:1"] = new[]
        {
            "Que faire quand tu éternues ?~Mettre ton coude devant ta bouche~Éternuer sur tes amis~Retenir ton souffle~Éternuer dans ton coude évite d'envoyer des microbes autour de toi.||Les microbes sont…~De très petits êtres vivants~Des morceaux de légumes~Des étoiles~Les microbes sont si petits qu'on ne les voit qu'avec un microscope.",
            "What should you do when you sneeze?~Cover your mouth with your elbow~Sneeze on your friends~Hold your breath~Sneezing into your elbow stops germs from flying around.||Germs are…~Very tiny living things~Pieces of vegetables~Stars~Germs are so small that you can only see them with a microscope.",
            "ماذا تفعل عندما تعطس؟~تضع مرفقك أمام فمك~تعطس على أصدقائك~تحبس أنفاسك~العطس في المرفق يمنع الجراثيم من الانتشار حولك.||الجراثيم هي…~كائنات حية صغيرة جدًا~قطع من الخضروات~نجوم~الجراثيم صغيرة جدًا بحيث لا نراها إلا بالمجهر."
        },
        ["defenses:2"] = new[]
        {
            "À quoi sert un vaccin ?~À apprendre au corps à reconnaître un microbe~À faire pousser les cheveux~À grandir plus vite~Un vaccin entraîne tes défenses avant que le vrai microbe arrive.||Qu'est-ce qui aide ton corps à se défendre ?~Bien dormir et bien manger~Veiller très tard~Ne jamais boire d'eau~Dormir, manger des fruits et des légumes et bouger aident tes défenses.",
            "What is a vaccine for?~To teach the body to recognise a germ~To make hair grow~To grow faster~A vaccine trains your defences before the real germ arrives.||What helps your body defend itself?~Sleeping well and eating well~Staying up very late~Never drinking water~Sleep, fruit, vegetables and exercise all help your defences.",
            "ما فائدة اللقاح؟~أن يعلّم الجسم كيف يتعرّف على جرثومة~أن ينمّي الشعر~أن تكبر أسرع~يدرّب اللقاح دفاعات جسمك قبل أن تصل الجرثومة الحقيقية.||ما الذي يساعد جسمك على الدفاع عن نفسه؟~النوم الجيد والغذاء الجيد~السهر طويلًا~عدم شرب الماء أبدًا~النوم وتناول الفواكه والخضروات والحركة كلها تساعد دفاعات جسمك."
        },
    };

    /// <summary>All the questions of a lesson in the chosen language (the first one, then the extra ones).</summary>
    public static List<Question> ForLesson(string slug, int order, string lang)
    {
        var list = new List<Question>();

        var first = I18n.Extra(slug, order, lang);
        if (first is not null)
            list.Add(new Question(first.Question, first.Options, first.CorrectIndex, first.Why));

        if (More.TryGetValue($"{slug}:{order}", out var packed))
        {
            var raw = packed[lang == "en" ? 1 : lang == "ar" ? 2 : 0];
            foreach (var block in raw.Split("||"))
            {
                var p = block.Split('~');
                if (p.Length < 5) continue;
                list.Add(new Question(p[0], new[] { p[1], p[2], p[3] }, 0, p[4]));
            }
        }

        return list;
    }
}
