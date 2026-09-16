export function getCoverMedia(project) {
    if (!project?.media?.length) {
        return null;
    }

    const cover = project.media.find(
        (media) => media.isCover
    );

    return cover ?? project.media[0];
}

export function getAspectRatioClass(aspectRatio) {
    const value =
        typeof aspectRatio === "string"
            ? aspectRatio.toLowerCase()
            : aspectRatio;

    if (
        value === 2 ||
        value === "vertical"
    ) {
        return "portfolio-card--vertical";
    }

    if (
        value === 3 ||
        value === "horizontal"
    ) {
        return "portfolio-card--horizontal";
    }

    return "portfolio-card--square";
}

export function getMediaUrl(media) {
    if (!media) {
        return null;
    }

    const possibleUrl =
        media.thumbnailStorageKey ||
        media.storageKey;

    if (!possibleUrl) {
        return null;
    }

    if (
        possibleUrl.startsWith("http://") ||
        possibleUrl.startsWith("https://")
    ) {
        return possibleUrl;
    }

    return null;
}