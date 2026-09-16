import {
    getAspectRatioClass,
    getCoverMedia,
    getMediaUrl,
} from "../../utils/mediaUtils";

import {
    getCategoryLabel,
} from "../../utils/categoryUtils";

function PortfolioCard({
    project,
    onOpen,
}) {
    const coverMedia =
        getCoverMedia(project);

    const mediaUrl =
        getMediaUrl(coverMedia);

    const aspectClass =
        getAspectRatioClass(
            coverMedia?.aspectRatio
        );

    function handleClick() {
        onOpen(project);
    }

    function handleKeyDown(event) {
        if (
            event.key === "Enter" ||
            event.key === " "
        ) {
            event.preventDefault();
            onOpen(project);
        }
    }

    return (
        <article
            className={`portfolio-card ${aspectClass}`}
            role="button"
            tabIndex={0}
            onClick={handleClick}
            onKeyDown={handleKeyDown}
            aria-label={`${project.title} projesini görüntüle`}
        >
            <div className="portfolio-card__media">
                {mediaUrl ? (
                    <img
                        src={mediaUrl}
                        alt={
                            coverMedia?.altText ||
                            project.title
                        }
                        loading="lazy"
                    />
                ) : (
                    <div className="portfolio-card__placeholder">
                        <span>
                            {getCategoryLabel(
                                project.category
                            )}
                        </span>
                    </div>
                )}

                <div className="portfolio-card__overlay">
                    <span>
                        View Project
                    </span>
                </div>
            </div>

            <div className="portfolio-card__info">
                <div>
                    <h3 className="portfolio-card__title">
                        {project.title}
                    </h3>

                    <p className="portfolio-card__category">
                        {getCategoryLabel(
                            project.category
                        )}
                    </p>
                </div>

                {project.year && (
                    <span className="portfolio-card__year">
                        {project.year}
                    </span>
                )}
            </div>
        </article>
    );
}

export default PortfolioCard;