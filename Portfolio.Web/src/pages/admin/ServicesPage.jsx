import { useEffect, useState } from "react";

import {
    createService,
    deleteService,
    getAdminServices,
    updateService,
} from "../../api/servicesApi";

const initialForm = {
    title: "",
    description: "",
    displayOrder: 0,
    isActive: true,
};

function ServicesPage() {
    const [services, setServices] = useState([]);
    const [form, setForm] = useState(initialForm);

    const [editingId, setEditingId] = useState(null);

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");

    async function loadServices() {
        try {
            setLoading(true);
            setError("");

            const data = await getAdminServices();

            setServices(data);
        } catch (err) {
            console.error(err);
            setError("Hizmetler yüklenemedi.");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        loadServices();
    }, []);

    function handleChange(event) {
        const { name, value, type, checked } = event.target;

        setForm((currentForm) => ({
            ...currentForm,
            [name]: type === "checkbox" ? checked : value,
        }));
    }

    function handleEdit(service) {
        setEditingId(service.id);

        setForm({
            title: service.title ?? "",
            description: service.description ?? "",
            displayOrder: service.displayOrder ?? 0,
            isActive: service.isActive,
        });
    }

    function resetForm() {
        setEditingId(null);
        setForm(initialForm);
        setError("");
    }

    async function handleSubmit(event) {
        event.preventDefault();

        setSaving(true);
        setError("");

        const request = {
            title: form.title.trim(),
            description: form.description.trim() || null,
            displayOrder: Number(form.displayOrder),
            isActive: form.isActive,
        };

        try {
            if (editingId) {
                await updateService(editingId, request);
            } else {
                await createService(request);
            }

            resetForm();
            await loadServices();
        } catch (err) {
            console.error(err);
            setError("Hizmet kaydedilirken bir hata oluştu.");
        } finally {
            setSaving(false);
        }
    }

    async function handleDelete(service) {
        const confirmed = window.confirm(
            `"${service.title}" hizmetini silmek istediğine emin misin?`
        );

        if (!confirmed) {
            return;
        }

        try {
            await deleteService(service.id);

            if (editingId === service.id) {
                resetForm();
            }

            setServices((currentServices) =>
                currentServices.filter((item) => item.id !== service.id)
            );
        } catch (err) {
            console.error(err);
            alert("Hizmet silinirken bir hata oluştu.");
        }
    }

    if (loading) {
        return <p>Hizmetler yükleniyor...</p>;
    }

    return (
        <section>
            <div>
                <h1>Services</h1>
                <p>Frontend üzerinde gösterilecek hizmetleri buradan yönetebilirsin.</p>
            </div>

            {error && <p>{error}</p>}

            <div>
                <h2>
                    {editingId ? "Hizmeti Düzenle" : "Yeni Hizmet"}
                </h2>

                <form onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="title">
                            Hizmet Adı
                        </label>

                        <input
                            id="title"
                            name="title"
                            type="text"
                            value={form.title}
                            onChange={handleChange}
                            required
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
                            : editingId
                                ? "Güncelle"
                                : "Ekle"}
                    </button>

                    {editingId && (
                        <button
                            type="button"
                            onClick={resetForm}
                        >
                            İptal
                        </button>
                    )}
                </form>
            </div>

            <hr />

            <div>
                <h2>Mevcut Hizmetler</h2>

                {services.length === 0 ? (
                    <p>Henüz hizmet bulunmuyor.</p>
                ) : (
                    services.map((service) => (
                        <article key={service.id}>
                            <h3>{service.title}</h3>

                            <p>
                                {service.description || "Açıklama yok"}
                            </p>

                            <p>
                                Sıralama: {service.displayOrder}
                            </p>

                            <p>
                                Durum:{" "}
                                {service.isActive ? "Aktif" : "Pasif"}
                            </p>

                            <button
                                type="button"
                                onClick={() => handleEdit(service)}
                            >
                                Düzenle
                            </button>

                            <button
                                type="button"
                                onClick={() => handleDelete(service)}
                            >
                                Sil
                            </button>
                        </article>
                    ))
                )}
            </div>
        </section>
    );
}

export default ServicesPage;