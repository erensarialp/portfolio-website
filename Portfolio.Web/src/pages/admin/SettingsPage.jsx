import { useEffect, useState } from "react";

import {
    getSiteSettings,
    updateSiteSettings,
} from "../../api/settingsApi";

const initialForm = {
    brandName: "",
    seoDescription: "",
    location: "",
    email: "",

    heroEyebrow: "",
    heroTitleLine1: "",
    heroTitleLine2: "",
    heroTitleLine3: "",
    heroDescription: "",

    aboutTitle: "",
    aboutDescription: "",

    portfolioTitle: "",
    portfolioDescription: "",

    servicesTitle: "",

    contactTitle: "",
    contactDescription: "",

    instagramUrl: "",
    whatsAppUrl: "",
    vimeoUrl: "",
};

function SettingsPage() {
    const [form, setForm] = useState(initialForm);

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    useEffect(() => {
        async function loadSettings() {
            try {
                setLoading(true);
                setError("");

                const data = await getSiteSettings();

                setForm({
                    brandName: data.brandName ?? "",
                    seoDescription: data.seoDescription ?? "",
                    location: data.location ?? "",
                    email: data.email ?? "",

                    heroEyebrow: data.heroEyebrow ?? "",
                    heroTitleLine1: data.heroTitleLine1 ?? "",
                    heroTitleLine2: data.heroTitleLine2 ?? "",
                    heroTitleLine3: data.heroTitleLine3 ?? "",
                    heroDescription: data.heroDescription ?? "",

                    aboutTitle: data.aboutTitle ?? "",
                    aboutDescription: data.aboutDescription ?? "",

                    portfolioTitle: data.portfolioTitle ?? "",
                    portfolioDescription:
                        data.portfolioDescription ?? "",

                    servicesTitle: data.servicesTitle ?? "",

                    contactTitle: data.contactTitle ?? "",
                    contactDescription:
                        data.contactDescription ?? "",

                    instagramUrl: data.instagramUrl ?? "",
                    whatsAppUrl: data.whatsAppUrl ?? "",
                    vimeoUrl: data.vimeoUrl ?? "",
                });
            } catch (err) {
                console.error(err);

                setError(
                    "Site ayarları yüklenirken bir hata oluştu."
                );
            } finally {
                setLoading(false);
            }
        }

        loadSettings();
    }, []);

    function handleChange(event) {
        const { name, value } = event.target;

        setForm((currentForm) => ({
            ...currentForm,
            [name]: value,
        }));
    }

    async function handleSubmit(event) {
        event.preventDefault();

        setSaving(true);
        setError("");
        setSuccessMessage("");

        const request = {
            brandName: form.brandName.trim(),
            seoDescription:
                form.seoDescription.trim() || null,
            location:
                form.location.trim() || null,
            email:
                form.email.trim() || null,

            heroEyebrow:
                form.heroEyebrow.trim() || null,
            heroTitleLine1:
                form.heroTitleLine1.trim(),
            heroTitleLine2:
                form.heroTitleLine2.trim() || null,
            heroTitleLine3:
                form.heroTitleLine3.trim() || null,
            heroDescription:
                form.heroDescription.trim() || null,

            aboutTitle:
                form.aboutTitle.trim() || null,
            aboutDescription:
                form.aboutDescription.trim() || null,

            portfolioTitle:
                form.portfolioTitle.trim() || null,
            portfolioDescription:
                form.portfolioDescription.trim() || null,

            servicesTitle:
                form.servicesTitle.trim() || null,

            contactTitle:
                form.contactTitle.trim() || null,
            contactDescription:
                form.contactDescription.trim() || null,

            instagramUrl:
                form.instagramUrl.trim() || null,
            whatsAppUrl:
                form.whatsAppUrl.trim() || null,
            vimeoUrl:
                form.vimeoUrl.trim() || null,
        };

        try {
            await updateSiteSettings(request);

            setSuccessMessage(
                "Site ayarları başarıyla kaydedildi."
            );
        } catch (err) {
            console.error(err);

            setError(
                "Site ayarları kaydedilirken bir hata oluştu."
            );
        } finally {
            setSaving(false);
        }
    }

    if (loading) {
        return <p>Site ayarları yükleniyor...</p>;
    }

    return (
        <section>
            <h1>Site Settings</h1>

            <p>
                Sitenin genel metinlerini ve iletişim
                bilgilerini buradan yönetebilirsin.
            </p>

            {error && <p>{error}</p>}

            {successMessage && (
                <p>{successMessage}</p>
            )}

            <form onSubmit={handleSubmit}>

                <fieldset>
                    <legend>Genel Bilgiler</legend>

                    <div>
                        <label htmlFor="brandName">
                            Marka Adı
                        </label>

                        <input
                            id="brandName"
                            name="brandName"
                            value={form.brandName}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label htmlFor="seoDescription">
                            SEO Açıklaması
                        </label>

                        <textarea
                            id="seoDescription"
                            name="seoDescription"
                            value={form.seoDescription}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="location">
                            Konum
                        </label>

                        <input
                            id="location"
                            name="location"
                            value={form.location}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="email">
                            E-posta
                        </label>

                        <input
                            id="email"
                            name="email"
                            type="email"
                            value={form.email}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Hero</legend>

                    <div>
                        <label htmlFor="heroEyebrow">
                            Küçük Üst Başlık
                        </label>

                        <input
                            id="heroEyebrow"
                            name="heroEyebrow"
                            value={form.heroEyebrow}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="heroTitleLine1">
                            Başlık 1
                        </label>

                        <input
                            id="heroTitleLine1"
                            name="heroTitleLine1"
                            value={form.heroTitleLine1}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label htmlFor="heroTitleLine2">
                            Başlık 2
                        </label>

                        <input
                            id="heroTitleLine2"
                            name="heroTitleLine2"
                            value={form.heroTitleLine2}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="heroTitleLine3">
                            Başlık 3
                        </label>

                        <input
                            id="heroTitleLine3"
                            name="heroTitleLine3"
                            value={form.heroTitleLine3}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="heroDescription">
                            Hero Açıklaması
                        </label>

                        <textarea
                            id="heroDescription"
                            name="heroDescription"
                            value={form.heroDescription}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Hakkımda</legend>

                    <div>
                        <label htmlFor="aboutTitle">
                            Başlık
                        </label>

                        <input
                            id="aboutTitle"
                            name="aboutTitle"
                            value={form.aboutTitle}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="aboutDescription">
                            Açıklama
                        </label>

                        <textarea
                            id="aboutDescription"
                            name="aboutDescription"
                            value={form.aboutDescription}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Portfolio</legend>

                    <div>
                        <label htmlFor="portfolioTitle">
                            Başlık
                        </label>

                        <input
                            id="portfolioTitle"
                            name="portfolioTitle"
                            value={form.portfolioTitle}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="portfolioDescription">
                            Açıklama
                        </label>

                        <textarea
                            id="portfolioDescription"
                            name="portfolioDescription"
                            value={form.portfolioDescription}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Hizmetler</legend>

                    <div>
                        <label htmlFor="servicesTitle">
                            Bölüm Başlığı
                        </label>

                        <input
                            id="servicesTitle"
                            name="servicesTitle"
                            value={form.servicesTitle}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <fieldset>
                    <legend>İletişim</legend>

                    <div>
                        <label htmlFor="contactTitle">
                            Başlık
                        </label>

                        <input
                            id="contactTitle"
                            name="contactTitle"
                            value={form.contactTitle}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="contactDescription">
                            Açıklama
                        </label>

                        <textarea
                            id="contactDescription"
                            name="contactDescription"
                            value={form.contactDescription}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Sosyal Medya</legend>

                    <div>
                        <label htmlFor="instagramUrl">
                            Instagram
                        </label>

                        <input
                            id="instagramUrl"
                            name="instagramUrl"
                            type="url"
                            value={form.instagramUrl}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="whatsAppUrl">
                            WhatsApp
                        </label>

                        <input
                            id="whatsAppUrl"
                            name="whatsAppUrl"
                            type="url"
                            value={form.whatsAppUrl}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label htmlFor="vimeoUrl">
                            Vimeo
                        </label>

                        <input
                            id="vimeoUrl"
                            name="vimeoUrl"
                            type="url"
                            value={form.vimeoUrl}
                            onChange={handleChange}
                        />
                    </div>
                </fieldset>

                <button
                    type="submit"
                    disabled={saving}
                >
                    {saving
                        ? "Kaydediliyor..."
                        : "Ayarları Kaydet"}
                </button>

            </form>
        </section>
    );
}

export default SettingsPage;