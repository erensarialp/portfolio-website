import {
    useCallback,
    useMemo,
    useState,
} from "react";

import CategoryFilter from "./CategoryFilter";
import PortfolioGrid from "./PortfolioGrid";

import ProjectModal from "../project/ProjectModal";

import {
    getCategoryKey,
} from "../../utils/categoryUtils";

function PortfolioSection({
    projects,
    settings,
}) {
    const [
        selectedCategory,
        setSelectedCategory,
    ] = useState("all");

    const [
        selectedProject,
        setSelectedProject,
    ] = useState(null);

    const filteredProjects = useMemo(() => {
        if (selectedCategory === "all") {
            return projects;
        }

        return projects.filter(
            (project) =>
                getCategoryKey(
                    project.category
                ) === selectedCategory
        );
    }, [
        projects,
        selectedCategory,
    ]);

    function handleCategoryChange(category) {
        setSelectedCategory(category);
        setSelectedProject(null);
    }

    const handleCloseProject =
        useCallback(() => {
            setSelectedProject(null);
        }, []);

    const handlePreviousProject =
        useCallback(() => {
            if (!selectedProject) {
                return;
            }

            const currentIndex =
                filteredProjects.findIndex(
                    (project) =>
                        project.id ===
                        selectedProject.id
                );

            if (currentIndex === -1) {
                return;
            }

            const previousIndex =
                currentIndex === 0
                    ? filteredProjects.length - 1
                    : currentIndex - 1;

            setSelectedProject(
                filteredProjects[previousIndex]
            );
        }, [
            filteredProjects,
            selectedProject,
        ]);

    const handleNextProject =
        useCallback(() => {
            if (!selectedProject) {
                return;
            }

            const currentIndex =
                filteredProjects.findIndex(
                    (project) =>
                        project.id ===
                        selectedProject.id
                );

            if (currentIndex === -1) {
                return;
            }

            const nextIndex =
                currentIndex ===
                    filteredProjects.length - 1
                    ? 0
                    : currentIndex + 1;

            setSelectedProject(
                filteredProjects[nextIndex]
            );
        }, [
            filteredProjects,
            selectedProject,
        ]);

    return (
        <>
            <section
                id="work"
                className="portfolio-section section"
            >
                <div className="site-container">
                    <div className="portfolio-section__header">
                        <div>
                            <p className="section-label">
                                Selected Work
                            </p>

                            <h2 className="section-title">
                                {settings?.portfolioTitle ||
                                    "Selected Work"}
                            </h2>
                        </div>

                        {settings?.portfolioDescription && (
                            <p className="section-description portfolio-section__description">
                                {
                                    settings.portfolioDescription
                                }
                            </p>
                        )}
                    </div>

                    <CategoryFilter
                        selectedCategory={
                            selectedCategory
                        }
                        onChange={
                            handleCategoryChange
                        }
                    />

                    <PortfolioGrid
                        projects={
                            filteredProjects
                        }
                        onOpenProject={
                            setSelectedProject
                        }
                    />
                </div>
            </section>

            <ProjectModal
                project={selectedProject}
                onClose={
                    handleCloseProject
                }
                onPrevious={
                    handlePreviousProject
                }
                onNext={
                    handleNextProject
                }
            />
        </>
    );
}

export default PortfolioSection;