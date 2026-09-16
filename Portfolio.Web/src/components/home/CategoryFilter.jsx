import { portfolioCategories } from "../../utils/categoryUtils";

function CategoryFilter({
    selectedCategory,
    onChange,
}) {
    return (
        <div
            className="category-filter"
            aria-label="Portfolio categories"
        >
            {portfolioCategories.map((category) => {
                const isActive =
                    selectedCategory === category.value;

                return (
                    <button
                        key={category.value}
                        type="button"
                        className={`category-filter__button ${isActive
                                ? "category-filter__button--active"
                                : ""
                            }`}
                        onClick={() =>
                            onChange(category.value)
                        }
                    >
                        {category.label}
                    </button>
                );
            })}
        </div>
    );
}

export default CategoryFilter;