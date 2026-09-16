const categoryMap = {
    1: "Video",
    2: "Social",
    3: "Photo",
    4: "Branding",
    5: "Web",
    6: "CGI",
    7: "Other",
};

export function getCategoryLabel(category) {
    if (typeof category === "number") {
        return categoryMap[category] ?? "Other";
    }

    if (typeof category === "string") {
        const normalized = category.toLowerCase();

        const stringMap = {
            video: "Video",
            socialmedia: "Social",
            social: "Social",
            photography: "Photo",
            photo: "Photo",
            branding: "Branding",
            web: "Web",
            cgi: "CGI",
            other: "Other",
        };

        return stringMap[normalized] ?? category;
    }

    return "Other";
}

export function getCategoryKey(category) {
    if (typeof category === "number") {
        return category;
    }

    const normalized = String(category).toLowerCase();

    const stringMap = {
        video: 1,
        socialmedia: 2,
        social: 2,
        photography: 3,
        photo: 3,
        branding: 4,
        web: 5,
        cgi: 6,
        other: 7,
    };

    return stringMap[normalized] ?? 7;
}

export const portfolioCategories = [
    {
        value: "all",
        label: "All",
    },
    {
        value: 1,
        label: "Video",
    },
    {
        value: 2,
        label: "Social",
    },
    {
        value: 3,
        label: "Photo",
    },
    {
        value: 4,
        label: "Branding",
    },
    {
        value: 5,
        label: "Web",
    },
    {
        value: 6,
        label: "CGI",
    },
];