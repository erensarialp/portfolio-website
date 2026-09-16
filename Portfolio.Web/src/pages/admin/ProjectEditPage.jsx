import { useEffect, useState } from "react";

import {
    useNavigate,
    useParams,
} from "react-router-dom";

import {
    createPortfolio,
    getAdminPortfolioById,
    updatePortfolio,
} from "../../api/portfolioApi";

const initialForm = {
    title: "",
    description: "",
    category: 2,
    client: "",
    year: "",
    format: "",
    result: "",
    displayOrder: 0,
    isActive: true,
};

function ProjectEditPage() {
    const { id } = useParams();
    const navigate = useNavigate();

    const isNew = !id;

    const [form, setForm] = useState(initialForm);
    const [loading, setLoading] = useState(!isNew);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");

    useEffect(() => {
        if (isNew) {
            return;
        }

        async function loadProject() {
            try {
                const project =
                    await getAdminPortfolioById(id);

                setForm({
                    title: project.title ?? "",
                    description: project.description ?? "",
                    category: project.category,
                    client: project.client ?? "",
                    year: project.year ?? "",
                    format: project.format ?? "",
                    result: project.result ?? "",
                    displayOrder: project.displayOrder ?? 0,
                    isActive: project.isActive,
                });
            } catch (err) {
                console.error(err);

                setError("Proje yüklenemedi.");
            } finally {
                setLoading(false);
            }
        }

        loadProject();
    }, [id, isNew]);

    function handleChange(event) {
        const {
            name,
            value,
            type,
            checked,
        } = event.target;

        setForm((currentForm) => ({
            ...currentForm,

            [name]:
                type === "checkbox"
                    ? checked
                    : value,
        }));
    }

    async function handleSubmit(event) {
        event.preventDefault();

        setSaving(true);
        setError("");

        const request = {
            title: form.title,
            description:
                form.description.trim() || null,
            category: Number(form.category),
            client:
                form.client.trim() || null,
            year:
                form.year === ""
                    ? null
                    : Number(form.year),
            format:
                form.format.trim() || null,
            result:
                form.result.trim() || null,
            displayOrder:
                Number(form.displayOrder),
            isActive: form.isActive,
        };

        try {
            if (isNew) {
                await createPortfolio(request);
            } else {
                await updatePortfolio(id, request);
            }

            navigate("/admin/projects");
        } catch (err) {
            console.error(err);

            setError(
                "Proje kaydedilirken bir hata oluştu."
            );
        } finally {
            setSaving(false);
        }
    }

    if (loading) {
        return <p>Proje yükleniyor...</p>;
    }

    return (
        <section>
            <h1>
                {isNew
                    ? "Yeni Proje"
                    : "Projeyi Düzenle"}
            </h1>

            {error && <p>{error}</p>}

            <form onSubmit={handleSubmit}>

                <div>
                    <label htmlFor="title">
                        Proje Adı
                    </label>

                    <input
                        id="title"
                        name="title"
                        value={form.title}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label htmlFor="client">
                        Müşteri
                    </label>

                    <input
                        id="client"
                        name="client"
                        value={form.client}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="category">
                        Kategori
                    </label>

                    <select
                        id="category"
                        name="category"
                        value={form.category}
                        onChange={handleChange}
                    >
                        <option value={1}>
                            Video
                        </option>

                        <option value={2}>
                            Social Media
                        </option>

                        <option value={3}>
                            Photography
                        </option>

                        <option value={4}>
                            Branding
                        </option>

                        <option value={5}>
                            Web
                        </option>

                        <option value={6}>
                            CGI
                        </option>

                        <option value={7}>
                            Other
                        </option>
                    </select>
                </div>

                <div>
                    <label htmlFor="year">
                        Yıl
                    </label>

                    <input
                        id="year"
                        name="year"
                        type="number"
                        value={form.year}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="format">
                        Format
                    </label>

                    <input
                        id="format"
                        name="format"
                        value={form.format}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="description">
                        Açıklama
                    </label>

                    <textarea
                        id="description"
                        name="description"
                        value={form.description}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="result">
                        Sonuç
                    </label>

                    <textarea
                        id="result"
                        name="result"
                        value={form.result}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label htmlFor="displayOrder">
                        Sıralama
                    </label>

                    <input
                        id="displayOrder"
                        name="displayOrder"
                        type="number"
                        value={form.displayOrder}
                        onChange={handleChange}
                    />
                </div>

                <div>
                    <label>
                        <input
                            name="isActive"
                            type="checkbox"
                            checked={form.isActive}
                            onChange={handleChange}
                        />

                        Aktif
                    </label>
                </div>

                <button
                    type="submit"
                    disabled={saving}
                >
                    {saving
                        ? "Kaydediliyor..."
                        : "Kaydet"}
                </button>

            </form>
        </section>
    );
}

export default ProjectEditPage;