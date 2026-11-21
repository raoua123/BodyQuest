/* BodyQuest - progress storage, synthesized sounds and confetti (no external assets needed) */
window.bodyQuestWorlds = (function () {
    let ctx = null;
    let muted = false;
    try { muted = localStorage.getItem("bodyquest.muted") === "1"; } catch (e) { }

    function audio() {
        const AC = window.AudioContext || window.webkitAudioContext;
        if (!AC) return null;
        if (!ctx) ctx = new AC();
        if (ctx.state === "suspended") ctx.resume();
        return ctx;
    }

    // one soft note: freq, start offset (s), length (s), wave type, volume
    function note(freq, at, len, type, vol) {
        const a = audio(); if (!a || muted) return;
        const o = a.createOscillator(), g = a.createGain();
        const t = a.currentTime + at;
        o.type = type || "sine"; o.frequency.setValueAtTime(freq, t);
        g.gain.setValueAtTime(0.0001, t);
        g.gain.exponentialRampToValueAtTime(vol || 0.14, t + 0.02);
        g.gain.exponentialRampToValueAtTime(0.0001, t + len);
        o.connect(g); g.connect(a.destination);
        o.start(t); o.stop(t + len + 0.03);
    }
    function slide(from, to, at, len, type, vol) {
        const a = audio(); if (!a || muted) return;
        const o = a.createOscillator(), g = a.createGain();
        const t = a.currentTime + at;
        o.type = type || "sine";
        o.frequency.setValueAtTime(from, t);
        o.frequency.exponentialRampToValueAtTime(to, t + len);
        g.gain.setValueAtTime(0.0001, t);
        g.gain.exponentialRampToValueAtTime(vol || 0.12, t + 0.02);
        g.gain.exponentialRampToValueAtTime(0.0001, t + len);
        o.connect(g); g.connect(a.destination);
        o.start(t); o.stop(t + len + 0.03);
    }

    const sounds = {
        tap:      () => { note(660, 0, 0.09, "triangle", 0.09); },
        pop:      () => { slide(380, 760, 0, 0.12, "sine", 0.12); },
        open:     () => { note(523.25, 0, .14, "triangle", .1); note(783.99, .08, .2, "triangle", .1); },
        close:    () => { note(659.25, 0, .12, "triangle", .09); note(440, .08, .18, "triangle", .08); },
        step:     () => { note(300 + Math.random() * 60, 0, .06, "sine", .05); },
        locked:   () => { note(190, 0, .14, "square", .05); note(150, .1, .2, "square", .05); },
        lesson:   () => { [523.25, 659.25, 783.99, 1046.5].forEach((f, i) => note(f, i * .11, .3, "sine", .14)); },
        world:    () => {
            [523.25, 659.25, 783.99, 659.25, 783.99, 1046.5, 1318.5].forEach((f, i) => note(f, i * .12, .4, "triangle", .13));
            note(261.63, 0, 1.1, "sine", .08);
        },
        sparkle:  () => { [1318.5, 1568, 2093].forEach((f, i) => note(f, i * .07, .2, "sine", .06)); }
    };

    /* ---------- confetti (canvas, no library) ---------- */
    let canvas, cctx, parts = [], raf = 0;
    const COLORS = ["#ff8eaa", "#ffd66e", "#83dfcf", "#8bc9f4", "#a98ce9", "#ffb347", "#8ee08e"];
    function ensureCanvas() {
        if (canvas) return;
        canvas = document.createElement("canvas");
        canvas.className = "bq-confetti-canvas";
        canvas.style.cssText = "position:fixed;inset:0;width:100%;height:100%;pointer-events:none;z-index:9999";
        document.body.appendChild(canvas);
        const fit = () => { canvas.width = innerWidth; canvas.height = innerHeight; };
        fit(); addEventListener("resize", fit);
        cctx = canvas.getContext("2d");
    }
    function loop() {
        cctx.clearRect(0, 0, canvas.width, canvas.height);
        parts = parts.filter(p => p.life > 0 && p.y < canvas.height + 30);
        parts.forEach(p => {
            p.vy += p.g; p.vx *= 0.992; p.x += p.vx; p.y += p.vy; p.r += p.vr; p.life--;
            cctx.save(); cctx.translate(p.x, p.y); cctx.rotate(p.r);
            cctx.globalAlpha = Math.min(1, p.life / 30);
            cctx.fillStyle = p.c;
            if (p.shape === 0) cctx.fillRect(-p.s / 2, -p.s / 4, p.s, p.s / 2);
            else if (p.shape === 1) { cctx.beginPath(); cctx.arc(0, 0, p.s / 2.4, 0, 7); cctx.fill(); }
            else { cctx.beginPath(); for (let i = 0; i < 5; i++) { const a = i * 1.2566 - 1.5708, b = a + .6283; cctx.lineTo(Math.cos(a) * p.s / 2, Math.sin(a) * p.s / 2); cctx.lineTo(Math.cos(b) * p.s / 4.5, Math.sin(b) * p.s / 4.5); } cctx.closePath(); cctx.fill(); }
            cctx.restore();
        });
        if (parts.length) raf = requestAnimationFrame(loop); else { cctx.clearRect(0, 0, canvas.width, canvas.height); raf = 0; }
    }
    function burst(x, y, count, spread, power) {
        if (matchMedia("(prefers-reduced-motion: reduce)").matches) return;
        ensureCanvas();
        for (let i = 0; i < count; i++) {
            const ang = (Math.random() * spread) - spread / 2 - Math.PI / 2;
            const sp = (power || 11) * (0.45 + Math.random() * .7);
            parts.push({
                x: x, y: y, vx: Math.cos(ang) * sp, vy: Math.sin(ang) * sp, g: .26,
                r: Math.random() * 6, vr: (Math.random() - .5) * .35, s: 8 + Math.random() * 9,
                c: COLORS[(Math.random() * COLORS.length) | 0], shape: (Math.random() * 3) | 0,
                life: 110 + Math.random() * 60
            });
        }
        if (!raf) raf = requestAnimationFrame(loop);
    }

    /* ---------- soft music-box background music (off until the child turns it on) ---------- */
    let musicOn = false, musicTimer = 0, step = 0;
    const CHORDS = [[261.63, 329.63, 392], [220, 261.63, 329.63], [174.61, 220, 261.63], [196, 246.94, 293.66]];
    function musicTick() {
        if (!musicOn || muted) return;
        const chord = CHORDS[Math.floor(step / 8) % CHORDS.length];
        const f = chord[[0, 1, 2, 1, 2, 1, 0, 2][step % 8]] * 2;
        note(f, 0, 0.9, "sine", 0.045);
        if (step % 8 === 0) note(chord[0] / 2, 0, 2.2, "triangle", 0.04);
        step++;
    }

    /* ---------- Milo walking along the garden path ---------- */
    const MAP_H = 3150; // height of the garden scene (1000 x 3150), see Services/GardenLayout.cs
    const map = { path: null, milo: null, len: 0, stops: [], at: 0, say: null, sayTimer: 0, box: null };
    // LANG: was "const"; the list is replaced by setFacts() when the language changes
    let FACTS = [
        "Your heart beats about 100,000 times every day!",
        "Your brain has billions of tiny neurons.",
        "You have 206 bones when you grow up.",
        "Your lungs work all day, even while you sleep.",
        "Your tongue has thousands of tiny taste buds.",
        "Your stomach squishes food like a washing machine!",
        "Your skin is the biggest organ you have.",
        "Blood travels all around your body in about one minute."
    ];
    let factIdx = 0;
    function pos(len) { const p = map.path.getPointAtLength(len); return { x: p.x, y: p.y }; }
    function place(len) {
        const p = pos(len);
        map.milo.style.left = (p.x / 1000 * 100) + "%";
        map.milo.style.top = (p.y / MAP_H * 100) + "%";
        map.at = len;
    }
    function say(text, ms) {
        if (!map.say) return;
        map.say.textContent = text;
        map.say.classList.add("show");
        clearTimeout(map.sayTimer);
        map.sayTimer = setTimeout(() => map.say.classList.remove("show"), ms || 3600);
    }
    function followScroll(y) {
        const r = map.box.getBoundingClientRect();
        const target = window.scrollY + r.top + (y / MAP_H) * r.height - innerHeight * 0.45;
        window.scrollTo(0, window.scrollY + (target - window.scrollY) * 0.12);
    }

    const api = {
        /* nodes: [{x,y}] in 1000x3150 space; finish: {x,y}. Stops sit on the path just before each node. */
        mapInit: function (pathId, miloId, sayId, boxId, nodes, finish, startIndex) {
            map.path = document.getElementById(pathId); map.milo = document.getElementById(miloId);
            map.say = document.getElementById(sayId); map.box = document.getElementById(boxId);
            if (!map.path || !map.milo) return false;
            map.len = map.path.getTotalLength();
            const nearest = (pt) => {
                let best = 0, bd = 1e9;
                for (let l = 0; l <= map.len; l += 4) {
                    const p = pos(l), d = (p.x - pt.x) ** 2 + (p.y - pt.y) ** 2;
                    if (d < bd) { bd = d; best = l; }
                }
                return best;
            };
            map.stops = nodes.map(n => Math.max(0, nearest(n) - 170));
            map.stops.push(Math.max(0, nearest(finish) - 120));
            place(map.stops[Math.min(startIndex, map.stops.length - 1)]);
            map.milo.classList.add("ready");
            map.milo.onclick = function () { say(FACTS[factIdx++ % FACTS.length]); api.play("pop"); map.milo.classList.remove("hop"); void map.milo.offsetWidth; map.milo.classList.add("hop"); };
            return true;
        },
        miloSay: function (text, ms) { say(text, ms); },

        // LANG: Milo's facts in the chosen language
        setFacts: function (arr) { if (Array.isArray(arr) && arr.length) { FACTS = arr; factIdx = 0; } },
        // LANG: sets the page language and direction (Arabic = right-to-left)
        setLang: function (lang) { var d = document.documentElement; d.lang = lang; d.dir = lang === "ar" ? "rtl" : "ltr"; },

        scrollToMilo: function () { if (map.box && map.milo) { const r = map.milo.getBoundingClientRect(); window.scrollTo({ top: window.scrollY + r.top - innerHeight * 0.45, behavior: "smooth" }); } },
        walkTo: function (index) {
            return new Promise(resolve => {
                if (!map.path) return resolve();
                const target = map.stops[Math.min(index, map.stops.length - 1)];
                const from = map.at, dist = Math.abs(target - from);
                if (dist < 6) return resolve();
const dur = Math.min(1200, Math.max(350, dist * 0.6));                const t0 = performance.now(); let lastStep = 0;
                map.milo.classList.add("walking");
                const ease = t => t < .5 ? 2 * t * t : 1 - Math.pow(-2 * t + 2, 2) / 2;
                (function frame(now) {
                    const t = Math.min(1, (now - t0) / dur);
                    const len = from + (target - from) * ease(t);
                    place(len); followScroll(pos(len).y);
                    if (now - lastStep > 260 && t < 1) { lastStep = now; sounds.step(); }
                    if (t < 1) requestAnimationFrame(frame);
                    else { map.milo.classList.remove("walking"); api.scrollToMilo(); resolve(); }
                })(t0);
            });
        },
        speak: function (text, lang) {
            try {
                if (!("speechSynthesis" in window)) return false;
                speechSynthesis.cancel();
                const u = new SpeechSynthesisUtterance(text);
                u.lang = lang || "en-US"; u.rate = 0.92; u.pitch = 1.15;
                speechSynthesis.speak(u); return true;
            } catch (e) { return false; }
        },
        stopSpeaking: function () { try { speechSynthesis.cancel(); } catch (e) { } },
        isMusicOn: function () { return musicOn; },
        setMusic: function (on) {
            musicOn = !!on; clearInterval(musicTimer);
            if (musicOn) { audio(); step = 0; musicTick(); musicTimer = setInterval(musicTick, 480); }
            try { localStorage.setItem("bodyquest.music", musicOn ? "1" : "0"); } catch (e) { }
            return musicOn;
        },
        wasMusicOn: function () { try { return localStorage.getItem("bodyquest.music") === "1"; } catch (e) { return false; } },
        loadProgress: function (key) {
            try { const v = JSON.parse(localStorage.getItem(key) || "[]"); return Array.isArray(v) ? v : []; }
            catch (e) { return []; }
        },
        saveProgress: function (key, values) {
            try { localStorage.setItem(key, JSON.stringify(values || [])); } catch (e) { }
        },
        loadValue: function (key) { try { return localStorage.getItem(key); } catch (e) { return null; } },
        saveValue: function (key, v) { try { localStorage.setItem(key, v); } catch (e) { } },

        play: function (name) { try { (sounds[name] || sounds.tap)(); } catch (e) { } },
        lessonComplete: function () { try { sounds.lesson(); } catch (e) { } },
        isMuted: function () { return muted; },
        setMuted: function (m) {
            muted = !!m;
            try { localStorage.setItem("bodyquest.muted", muted ? "1" : "0"); } catch (e) { }
            if (!muted) sounds.pop();
            return muted;
        },

        /* confetti from the middle-bottom of the screen, or from an element */
        confetti: function (big) {
            const w = innerWidth, h = innerHeight;
            if (big) {
                burst(w * .2, h * .85, 70, 1.6, 17); burst(w * .8, h * .85, 70, 1.6, 17); burst(w * .5, h * .6, 90, 6.28, 12);
            } else burst(w * .5, h * .7, 90, 2.4, 15);
        },
        confettiAt: function (id) {
            const el = document.getElementById(id); if (!el) return this.confetti(false);
            const r = el.getBoundingClientRect();
            burst(r.left + r.width / 2, r.top + r.height / 2, 60, 6.28, 9);
        },

        confettiBurstAt: function (x, y) { burst(x, y, 22, 6.28, 6); },

        scrollToId: function (id) {
            const el = document.getElementById(id);
            if (el) el.scrollIntoView({ behavior: "smooth", block: "center", inline: "center" });
        },

        /* floating butterflies / petals are CSS only; this adds the gentle tap sound on every .bq-snd element */
        wireSounds: function () {
            if (window.__bqWired) return; window.__bqWired = true;
            document.addEventListener("pointerdown", function (e) {
                const t = e.target.closest && e.target.closest("[data-snd]");
                if (t && !t.disabled) window.bodyQuestWorlds.play(t.getAttribute("data-snd"));
                const p = e.target.closest && e.target.closest("[data-pop]");
                if (p) {
                    window.bodyQuestWorlds.play("sparkle");
                    p.classList.remove("popped"); void p.getBoundingClientRect(); p.classList.add("popped");
                    window.bodyQuestWorlds.confettiBurstAt(e.clientX, e.clientY);
                }
            }, true);
        }
    };
    return api;
})();
window.addEventListener("DOMContentLoaded", function () { window.bodyQuestWorlds.wireSounds(); });