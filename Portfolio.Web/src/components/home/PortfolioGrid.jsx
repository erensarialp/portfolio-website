import PortfolioCard from "./PortfolioCard";

function PortfolioGrid({
    projects,
    onOpenProject,
}) {
    if (!projects.length) {
        return (
            <div className="portfolio-empty">
                <p>
                    Bu kategoride henüz proje bulunmuyor.
                </p>
            </div>
        );
    }

    return (
        <div className="portfolio-grid">
            {projects.map((project) => (
                <PortfolioCard
                    key={project.id}
                    project={project}
                    onOpen={onOpenProject}
                />
            ))}
        </div>
    );
}

export default PortfolioGrid;