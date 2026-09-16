import { useEffect } from "react";

import ProjectGallery from "./ProjectGallery";

import {
    getCategoryLabel,
} from "../../utils/categoryUtils";

function ProjectModal({
    project,
    onClose,
    onPrevious,
    onNext,
}) {
    useEffect(() => {
        if (!project) {
            return;
        }

        const previousOverflow =
            document.body.style.overflow;

        document.body.style.overflow = "hidden";

        function handleKeyDown(event) {
            if (event.key === "Escape") {
                onClose();
            }

            if (event.key === "ArrowLeft") {
                onPrevious?.();
            }

            if (event.key === "ArrowRight") {
                onNext?.();
            }
        }

        window.addEventListener(
            "keydown",
            handleKeyDown
        );

        return () => {
            document.body.style.overflow =
                previousOverflow;

            window.removeEventListener(
                "keydown",
                handleKeyDown
            );
        };
    }, [
        project,
        onClose,
        onPrevious,
        onNext,
    ]);

    if (!project) {
        return null;
    }

    return (
        <div
            className="project-modal"
            role="dialog"
            aria-modal="true"
            aria-labelledby="project-modal-title"
        >
            <div className="project-modal__topbar">
                <button
                    type="button"
                    className="project-modal__close"
                    onClick={onClose}
                    aria-label="Projeyi kapat"
                >
                    Close
                    <span aria-hidden="true">×</span>
                </button>

                <div className="project-modal__navigation">
                    <button
                        type="button"
                        onClick={onPrevious}
                    >
                        ← Prev
                    </button>

                    <button
                        type="button"
                        onClick={onNext}
                    >
                        Next →
                    </button>
                </div>
            </div>

            <div className="project-modal__content">
                <header className="project-modal__header">
                    <div className="project-modal__title-area">
                        <p className="section-label">
                            {getCategoryLabel(
                                project.category
                            )}
                        </p>

                        <h2
                            id="project-modal-title"
                            className="project-modal__title"
                        >
                            {project.title}
                        </h2>
                    </div>

                    <div className="project-modal__meta">
                        {project.client && (
                            <div>
                                <span>Client</span>
                                <strong>
                                    {project.client}
                                </strong>
                            </div>
                        )}

                        {project.year && (
                            <div>
                                <span>Year</span>
                                <strong>
                                    {project.year}
                                </strong>
                            </div>
                        )}

                        {project.format && (
                            <div>
                                <span>Format</span>
                                <strong>
                                    {project.format}
                                </strong>
                            </div>
                        )}

                        <div>
                            <span>Category</span>

                            <strong>
                                {getCategoryLabel(
                                    project.category
                                )}
                            </strong>
                        </div>
                    </div>
                </header>

                {(project.description ||
                    project.result) && (
                        <div className="project-modal__story">
                            {project.description && (
                                <div>
                                    <p className="project-modal__story-label">
                                        Project
                                    </p>

                                    <p className="project-modal__description">
                                        {project.description}
                                    </p>
                                </div>
                            )}

                            {project.result && (
                                <div>
                                    <p className="project-modal__story-label">
                                        Result
                                    </p>

                                    <p className="project-modal__description">
                                        {project.result}
                                    </p>
                                </div>
                            )}
                        </div>
                    )}

                <ProjectGallery
                    media={project.media}
                />
            </div>
        </div>
    );
}

export default ProjectModal;