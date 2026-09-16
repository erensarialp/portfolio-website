import {
    getAspectRatioClass,
    getMediaUrl,
} from "../../utils/mediaUtils";

function isVideo(mediaType) {
    if (typeof mediaType === "number") {
        return mediaType === 2;
    }

    return String(mediaType).toLowerCase() === "video";
}

function ProjectGallery({ media = [] }) {
    if (!media.length) {
        return (
            <div className="project-gallery__empty">
                <p>
                    Bu proje için henüz medya eklenmemiş.
                </p>
            </div>
        );
    }

    return (
        <div className="project-gallery">
            {media.map((item) => {
                const mediaUrl = getMediaUrl(item);

                const aspectClass =
                    getAspectRatioClass(item.aspectRatio);

                return (
                    <figure
                        key={item.id}
                        className={`project-gallery__item ${aspectClass}`}
                    >
                        {mediaUrl ? (
                            isVideo(item.mediaType) ? (
                                <video
                                    src={mediaUrl}
                                    controls
                                    playsInline
                                    preload="metadata"
                                />
                            ) : (
                                <img
                                    src={mediaUrl}
                                    alt={
                                        item.altText ||
                                        "Project media"
                                    }
                                    loading="lazy"
                                />
                            )
                        ) : (
                            <div className="project-gallery__placeholder">
                                <span>
                                    {isVideo(item.mediaType)
                                        ? "Video"
                                        : "Image"}
                                </span>
                            </div>
                        )}
                    </figure>
                );
            })}
        </div>
    );
}

export default ProjectGallery;