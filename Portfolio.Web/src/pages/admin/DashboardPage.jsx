import { useEffect, useMemo, useState } from "react";

import { getAdminPortfolio } from "../../api/portfolioApi";
import { getAdminServices } from "../../api/servicesApi";
import { getSiteSettings } from "../../api/settingsApi";

function DashboardPage() {
    const [projects, setProjects] = useState([]);
    const [services, setServices] = useState([]);
    const [settings, setSettings] = useState(null);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        async function loadDashboard() {
            try {
                setLoading(true);
                setError("");

                const [
                    projectsData,
                    servicesData,
                    settingsData,
                ] = await Promise.all([
                    getAdminPortfolio(),
                    getAdminServices(),
                    getSiteSettings(),
                ]);

                setProjects(projectsData);
                setServices(servicesData);
                setSettings(settingsData);
            } catch (err) {
                console.error(err);
                setError("Dashboard verileri yüklenemedi.");
            } finally {
                setLoading(false);
            }
        }

        loadDashboard();
    }, []);

    const summary = useMemo(() => {
        const activeProjects =
            projects.filter((project) => project.isActive).length;

        const passiveProjects =
            projects.filter((project) => !project.isActive).length;

        const totalMedia =
            projects.reduce(
                (total, project) =>
                    total + (project.media?.length ?? 0),
                0
            );

        const activeServices =
            services.filter((service) => service.isActive).length;

        return {
            totalProjects: projects.length,
            activeProjects,
            passiveProjects,
            totalMedia,
            totalServices: services.length,
            activeServices,
        };
    }, [projects, services]);

    if (loading) {
        return <p>Dashboard yükleniyor...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <section>
            <div>
                <h1>Dashboard</h1>

                <p>
                    Hoş geldin. Portfolyo sitesinin genel
                    durumunu buradan görebilirsin.
                </p>
            </div>

            <div>
                <article>
                    <h2>Toplam Proje</h2>
                    <strong>{summary.totalProjects}</strong>
                </article>

                <article>
                    <h2>Aktif Proje</h2>
                    <strong>{summary.activeProjects}</strong>
                </article>

                <article>
                    <h2>Pasif Proje</h2>
                    <strong>{summary.passiveProjects}</strong>
                </article>

                <article>
                    <h2>Toplam Medya</h2>
                    <strong>{summary.totalMedia}</strong>
                </article>

                <article>
                    <h2>Toplam Hizmet</h2>
                    <strong>{summary.totalServices}</strong>
                </article>

                <article>
                    <h2>Aktif Hizmet</h2>
                    <strong>{summary.activeServices}</strong>
                </article>
            </div>

            <hr />

            <div>
                <h2>Site Bilgisi</h2>

                <p>
                    Marka: {settings?.brandName ?? "-"}
                </p>

                <p>
                    E-posta: {settings?.email ?? "-"}
                </p>

                <p>
                    Konum: {settings?.location ?? "-"}
                </p>
            </div>

            <hr />

            <div>
                <h2>Son Projeler</h2>

                {projects.length === 0 ? (
                    <p>Henüz proje bulunmuyor.</p>
                ) : (
                    projects
                        .slice(0, 5)
                        .map((project) => (
                            <article key={project.id}>
                                <h3>{project.title}</h3>

                                <p>
                                    {project.client ||
                                        "Müşteri belirtilmemiş"}
                                </p>

                                <p>
                                    {project.isActive
                                        ? "Aktif"
                                        : "Pasif"}
                                </p>
                            </article>
                        ))
                )}
            </div>
        </section>
    );
}

export default DashboardPage;