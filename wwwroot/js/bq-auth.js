/* Sign up / sign in / sign out calls used by the login page and the header badge.
   The login cookie can only be set by a normal HTTP response, so these go through fetch()
   (Blazor Server components talk over a WebSocket). Always resolves, never throws. */
window.bodyQuestAuth = {
    post: async function (url, body) {
        try {
            const response = await fetch(url, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                credentials: "same-origin",
                body: JSON.stringify(body || {})
            });

            let data = {};
            try { data = await response.json(); } catch (e) { }

            return { ok: response.ok, status: response.status, error: data && data.error ? data.error : null };
        } catch (e) {
            return { ok: false, status: 0, error: "network" };
        }
    }
};
