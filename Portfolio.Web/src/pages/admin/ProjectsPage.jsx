import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
    deletePortfolio,
    getAdminPortfolio,
} from "../../api/portfolioApi";

function ProjectsPage() {
    const [projects, setProjects] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    async function loadProjects() {
        try {
            setLoading(true);
            setError("");

            const data = await getAdminPortfolio();

            setProjects(data);
        } catch (err) {
            console.error(err);

            setError("Projeler yüklenemedi.");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        loadProjects();
    }, []);

    async function handleDelete(project) {
        const confirmed = window.confirm(
            `"${project.title}" projesini silmek istediğine emin misin?`
        );

        if (!confirmed) {
            return;
        }

        try {
            await deletePortfolio(project.id);

            setProjects((currentProjects) =>
                currentProjects.filter(
                    (item) => item.id !== project.id
                )
            );
        } catch (err) {
            console.error(err);

            alert("Proje silinirken bir hata oluştu.");
        }
    }

    if (loading) {
        return <p>Projeler yükleniyor...</p>;
    }

    if (error) {
        return (
            <div>
                <p>{error}</p>

                <button
                    type="button"
                    onClick={loadProjects}
                >
                    Tekrar Dene
                </button>
            </div>
        );
    }

    return (
        <section>
            <div>
                <h1>Projects</h1>

                <Link to="/admin/projects/new">
                    + Yeni Proje
                </Link>
            </div>

            {projects.length === 0 ? (
                <p>Henüz proje bulunmuyor.</p>
            ) : (
                <div>
                    {projects.map((project) => (
                        <article key={project.id}>
                            <div>
                                <h2>{project.title}</h2>

                                <p>
                                    {project.client || "Müşteri belirtilmemiş"}
                                </p>

                                <p>
                                    {project.year || "Yıl belirtilmemiş"}
                                </p>

                                <p>
                                    Durum:{" "}
                                    {project.isActive
                                        ? "Aktif"
                                        : "Pasif"}
                                </p>

                                <p>
                                    Medya:{" "}
                                    {project.media?.length ?? 0}
                                </p>
                            </div>

                            <div>
                                <Link
                                    to={`/admin/projects/${project.id}`}
                                >
                                    Düzenle
                                </Link>

                                <button
                                    type="button"
                                    onClick={() =>
                                        handleDelete(project)
                                    }
                                >
                                    Sil
                                </button>
                            </div>
                        </article>
                    ))}
                </div>
            )}
        </section>
    );
}

export default ProjectsPage;